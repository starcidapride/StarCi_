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

        mainPhase1Button.onClick.AddListener(() => OnPhaseButtonClick(TurnPhase.MainPhase1));
        combatPhaseButton.onClick.AddListener(() => OnPhaseButtonClick(TurnPhase.CombatPhase));
        mainPhase2Button.onClick.AddListener(() => OnPhaseButtonClick(TurnPhase.MainPhase2));

        endTurnButton.onClick.AddListener(OnEndTurnButtonClick);


        var isHost = NetworkManager.Singleton.IsHost;

        PlaceComponentDeck(ComponentDeckType.Play, Participant.You);
        PlaceComponentDeck(ComponentDeckType.Character, Participant.You);
        PlaceComponentDeck(ComponentDeckType.Play, Participant.Opponent);
        PlaceComponentDeck(ComponentDeckType.Character, Participant.Opponent);

        yield return new WaitUntil(() => GameCentralManager.IsFinishLoad);

        var networkGameCentral = GameCentralManager.Instance.NetworkGameCentral.Value;

        var isYourTurn = isHost == networkGameCentral.IsHostTurn;

        currentTurnText.text = isYourTurn
            ? EnumUtility.GetDescription(CurrentTurn.YourTurn)
                : EnumUtility.GetDescription(CurrentTurn.OpponentsTurn);


        if (isHost)
        {   
            for (int i =  0; i < 5; i++)
            {

                BothPlayerDrawACard();
                yield return new WaitForSeconds(1.1f);
            }
        }

        UpdateActionField(isYourTurn, TurnPhase.PrePhase);
    }

    private void OnPhaseButtonClick(TurnPhase turnPhase)
    {
        var networkGameCentral = GameCentralManager.Instance.NetworkGameCentral;

        var networkGameCentralValue = networkGameCentral.Value;

        networkGameCentralValue.TurnPhase = turnPhase;

        GameCentralManager.Instance.UpdateNetworkGameCentralServerRpc(networkGameCentralValue);

        GameCentralManager.Instance.NextPhaseServerRpc(networkGameCentralValue.IsHostTurn, turnPhase);
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

    private void OnEndTurnButtonClick()
    {
        var networkGameCentral = GameCentralManager.Instance.NetworkGameCentral;

        var networkGameCentralValue = networkGameCentral.Value;

        networkGameCentralValue.GoNextTurn();

        GameCentralManager.Instance.UpdateNetworkGameCentralServerRpc(networkGameCentralValue);

        GameCentralManager.Instance.EndTurnServerRpc(networkGameCentralValue.IsHostTurn);
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

    public void UpdateActionField(bool isYourTurn, TurnPhase currentPhase)
    {
            currentTurnText.text = isYourTurn ? EnumUtility.GetDescription(CurrentTurn.YourTurn) 
                : EnumUtility.GetDescription(CurrentTurn.OpponentsTurn);

        Miscellaneous.SetButtonEnabled(mainPhase1Button,
            isYourTurn
            && currentPhase < TurnPhase.MainPhase1,
            HexEnum.Ivory,
            currentPhase == TurnPhase.MainPhase1
            ? HexEnum.Green
            : HexEnum.Gray
            );
            
        Miscellaneous.SetButtonEnabled(combatPhaseButton, 
            isYourTurn
            && currentPhase < TurnPhase.CombatPhase,
            HexEnum.Ivory,
             currentPhase == TurnPhase.CombatPhase
            ? HexEnum.Green
            : HexEnum.Gray
            );
            
        Miscellaneous.SetButtonEnabled(mainPhase2Button, 
            isYourTurn
            && currentPhase < TurnPhase.MainPhase2
            ,
             HexEnum.Ivory,
             currentPhase == TurnPhase.MainPhase2
            ? HexEnum.Green
            : HexEnum.Gray
            );
            
        Miscellaneous.SetButtonEnabled(endTurnButton, isYourTurn, 
            HexEnum.Ivory,
            HexEnum.Gray);
        }
    }


public enum CurrentTurn
{
    [Description("Your\nTurn")]
    YourTurn,

    [Description("Opponent's\nTurn")]
    OpponentsTurn
}

