

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinWithCodeModalManager : Singleton<JoinWithCodeModalManager>
{
    [SerializeField]
    private TMP_InputField lobbyCodeTextInput;

    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private Button confirmButton;

    private string lobbyCode;

    private void Start()
    {
        lobbyCode = lobbyCodeTextInput.text;

        lobbyCodeTextInput.onEndEdit.AddListener(value => lobbyCode = value);

        cancelButton.onClick.AddListener(ModalManager.Instance.CloseNearestModal);

        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    private async void OnConfirmButtonClick()
    {
        var lobby = await LobbyUtility.JoinLobbyByCode(lobbyCode);

        LoadingSceneManager.Instance.JoinRelayAndStartClient(lobby);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnConfirmButtonClick();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ModalManager.Instance.CloseNearestModal();
        }
    }
}