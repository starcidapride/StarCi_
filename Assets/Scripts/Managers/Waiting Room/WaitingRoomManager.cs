using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomManager : Singleton<WaitingRoomManager>
{
    [SerializeField]
    private TMP_Text lobbyNameText;

    [SerializeField]
    private TMP_Text lobbyCodeText;

    [SerializeField]
    private Button leaveLobbyButton;

    [SerializeField] 
    private Button editLobbyButton;

    [SerializeField]
    private Image yourPictureImage;

    [SerializeField]
    private TMP_Text yourUsernameText;

    [SerializeField]
    private Transform yourHost;

    [SerializeField]
    private Transform yourReady;

    [SerializeField]
    private Transform yourIdle;

    [SerializeField]
    private Image opponentsCard;

    [SerializeField] 
    private Transform opponentsCardContainer;

    [SerializeField]
    private Image opponentsPictureImage;

    [SerializeField]
    private TMP_Text opponentsUsernameText;

    [SerializeField]
    private Transform opponentsReady;

    [SerializeField]
    private Transform opponentsIdle;

    [SerializeField]
    private Transform opponentsHost;

    [SerializeField]
    private Button kickButton;

    [SerializeField]
    private Button readyButton;

    [SerializeField]
    private Button startButton;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => NetworkSessionManager.IsFinishLoad 
        && LoadingFadeEffectManager.IsEndFadeOut);

        var isHost = NetworkManager.Singleton.IsHost;

        SetPlayerCard(Participant.You);

        if (isHost)
        {
            DisableOpponentsCard();
        } else
        {
            SetPlayerCard(Participant.Opponent);
        }
    }

    public void SetPlayerCard(Participant participant)
    {
        var user = LocalSessionManager.Instance.User;

        var isHost = NetworkManager.Singleton.IsHost;

       if (participant == Participant.You)
        {

            var you = NetworkSessionManager.Instance.NetworkPlayerDatas.Value.GetByPlayerId(
                NetworkSessionManager.Instance.PlayerId);

            yourPictureImage.sprite = ImageUtility.CreateSpriteFromTexture(PathUtility.LoadIndexedPicture(you.Player.PictureIndex));
        
            yourUsernameText.text = user.Username;

            yourHost.gameObject.SetActive(isHost);

            yourReady.gameObject.SetActive(you.Player.IsReady);

            yourIdle.gameObject.SetActive(!you.Player.IsReady);
        } else
        {
            var opponent = NetworkSessionManager.Instance.NetworkPlayerDatas.Value.GetOtherPlayerByPlayerId(
               NetworkSessionManager.Instance.PlayerId);

            opponentsCard.color = Color.white; 

            opponentsPictureImage.sprite = ImageUtility.CreateSpriteFromTexture(PathUtility.LoadIndexedPicture(opponent.Player.PictureIndex));

            opponentsUsernameText.text = user.Username;

            opponentsHost.gameObject.SetActive(!isHost);

            opponentsReady.gameObject.SetActive(opponent.Player.IsReady);

            opponentsIdle.gameObject.SetActive(!opponent.Player.IsReady);

            kickButton.gameObject.SetActive(isHost);
        }
    }

    public void SetReadyStatus(Participant participant, bool status)
    {
        if (participant == Participant.You)
        {
            yourReady.gameObject.SetActive(status);

            yourIdle.gameObject.SetActive(!status);
        }
        else
        {
            opponentsReady.gameObject.SetActive(status);

            opponentsIdle.gameObject.SetActive(!status);
        }
    }

    public void DisableOpponentsCard()
    {
        opponentsCard.color = ImageUtility.GetColorFromHexEnum(HexEnum.Gray);

        opponentsCardContainer.gameObject.SetActive(false);
    }

}