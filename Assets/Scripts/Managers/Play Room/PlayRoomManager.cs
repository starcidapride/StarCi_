using DG.Tweening;
using System.Collections;
using System.ComponentModel;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayRoomManager : Singleton<PlayRoomManager>
{
    [SerializeField]
    private Transform ui;
    
    [SerializeField]
    private Transform yourPlayDeck;

    [SerializeField]
    private Transform yourCharacterDeck;

    [SerializeField]
    private Transform yourHand;

    [SerializeField]
    private Transform opponentsPlayDeck;

    [SerializeField]
    private Transform opponentsCharacterDeck;

    [SerializeField]
    private Transform opponentsHand;

    [SerializeField]
    private TMP_Text currentTurnText;

    [SerializeField]
    private Button mainPhase1Button;

    [SerializeField]
    private Button combatPhaseButton;

    [SerializeField]
    private Button mainPhase2Button;

    [SerializeField]
    private Button endTurnButton;

    public void SetActiveUI(bool isActive)
    {
        StartCoroutine(SetActiveUICoroutine(isActive));
    }

    private IEnumerator SetActiveUICoroutine(bool isActive)
    {
        if (isActive)
        {
            ui.gameObject.SetActive(true);

            yield return AnimationUtility.WaitForAnimationCompletion(ui);
        }
        else
        {
            ui.gameObject.SetActive(false);
        }
    }
    private IEnumerator Start()
    {
        yield return new WaitUntil(() =>
        NetworkSessionManager.IsFinishLoad
        && ui.gameObject.activeSelf);

        var isHost = NetworkManager.Singleton.IsHost;

        PlaceComponentDeck(ComponentDeckType.Play, Participant.You);
        PlaceComponentDeck(ComponentDeckType.Character, Participant.You);
        PlaceComponentDeck(ComponentDeckType.Play, Participant.Opponent);
        PlaceComponentDeck(ComponentDeckType.Character, Participant.Opponent);

        if (isHost)
        {   
            for (int i =  0; i < 5; i++)
            {

                BothPlayerDrawACard();
                yield return new WaitForSeconds(1.1f);
            }
        }

        yield return new WaitUntil(() => GameCentralManager.IsFinishLoad);

        // host start first
        
    }

    public void PlaceComponentDeck(ComponentDeckType componentDeckType, Participant participant)
    {
        var playDeck = participant == Participant.You ? yourPlayDeck : opponentsPlayDeck;

        var characterDeck = participant == Participant.You ? yourCharacterDeck : opponentsCharacterDeck;

        var componentDeck = componentDeckType == ComponentDeckType.Play ? playDeck : characterDeck;

        var networkPlayerDatas = NetworkSessionManager.Instance.NetworkPlayerDatas.Value;

        var clientId = NetworkSessionManager.Instance.ClientId;

        var networkPlayerData = participant == Participant.You
            ? networkPlayerDatas.GetByClientId(clientId)
            : networkPlayerDatas.GetOtherByClientId(clientId);

        var playDeckCards = networkPlayerData.PlayDeck.Cards;

        var characterDeckCards = networkPlayerData.CharacterDeck.Cards;

        var cardNames = componentDeckType == ComponentDeckType.Play ? playDeckCards : characterDeckCards;

        foreach (var cardName in cardNames)
        {
            CardUtility.InstantiateAndSetupCardWithSleeve(cardName,
                componentDeck, Vector2.zero, Vector2.one / 2,
                networkPlayerData.Player.CardSleeveIndex);
        }
    }

    public void BothPlayerDrawACard()
    {
       
        var networkPlayerDatas = NetworkSessionManager.Instance.NetworkPlayerDatas.Value;

        var clientId = NetworkSessionManager.Instance.ClientId;

        var you = networkPlayerDatas.GetByClientId(clientId);

        var opponent = networkPlayerDatas.GetOtherByClientId(clientId);

        var yourNumCards = you.Hand.Cards.Count;

        var opponentsNumCards = opponent.Hand.Cards.Count;

        NetworkSessionManager.Instance.PlayBothPlayerDrawACardClientRpc(yourNumCards, opponentsNumCards);


        var yourLastCard = you.PlayDeck.Cards[^1];

        you.PlayDeck.Cards.Remove(yourLastCard);

        you.Hand.Cards.Add(yourLastCard);


        var opponentsLastCard = opponent.PlayDeck.Cards[^1]; 

        opponent.PlayDeck.Cards.Remove(opponentsLastCard);

        opponent.Hand.Cards.Add(opponentsLastCard);

        networkPlayerDatas.Update(you);

        networkPlayerDatas.Update(opponent);

        NetworkSessionManager.Instance.UpdatePlayerDataServerRpc(networkPlayerDatas);
    }

    public void PlayBothPlayerDrawACard(int yourNumCards, int opponentsNumCards)
    {
        PlayDrawACard(Participant.You, yourNumCards);
        PlayDrawACard(Participant.Opponent, opponentsNumCards);
    }


    public void PlayDrawACard(Participant participant, int handNumCards)
    {
        var grids = Grid.SplitSpriteIntoIndexedGridsWithMaxWidth(yourHand, 1, handNumCards + 1, 400, true);

        var playDeck = participant == Participant.You
            ? yourPlayDeck : opponentsPlayDeck;

        var hand = participant == Participant.You 
            ? yourHand : opponentsHand;

        var lastCard = playDeck.GetChild(playDeck.childCount - 1);

        var showCardSleeve = participant != Participant.You;


        lastCard.SetParent(hand);
        lastCard.DOLocalMove(grids[grids.Count - 1].Center, 1f).
            OnStart(() =>
            {
                FlipCard(lastCard, showCardSleeve);

                if (grids.Count - 1 > 0)
                {
                    for (int i = 0; i < grids.Count - 1; i++)
                    {
                        hand.GetChild(i).DOLocalMove(grids[i].Center, 1f);
                    }
                }
            });
    }

    private void FlipCard(Transform card, bool showCardSleeve)
    {
        StartCoroutine(FlipCardCoroutine(card, showCardSleeve));
    }

    private IEnumerator FlipCardCoroutine(Transform card, bool showCardSleeve)
    {
        yield return AnimationUtility.ExecuteTriggerThenWait(card, TriggerName.FlipIn);

        card.GetComponent<CardSleeveManager>().SetVisibility(showCardSleeve);


        yield return AnimationUtility.ExecuteTriggerThenWait(card, TriggerName.FlipOut);
    }

    public void UpdateActionField(bool isYourTurn)
    {
            currentTurnText.text = isYourTurn ? EnumUtility.GetDescription(CurrentTurn.YourTurn) 
                : EnumUtility.GetDescription(CurrentTurn.OpponentsTurn);

            Miscellaneous.SetButtonEnabled(mainPhase1Button, isYourTurn);
            Miscellaneous.SetButtonEnabled(combatPhaseButton, isYourTurn);
            Miscellaneous.SetButtonEnabled(mainPhase2Button, isYourTurn);
            Miscellaneous.SetButtonEnabled(endTurnButton, isYourTurn);
        }
    }


public enum CurrentTurn
{
    [Description("Your Turn")]
    YourTurn,

    [Description("Opponent's Turn")]
    OpponentsTurn
}

