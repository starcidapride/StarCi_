using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyModalManager : Singleton<CreateLobbyModalManager>
{
    [SerializeField]
    private TMP_InputField lobbyNameTextInput;

    [SerializeField]
    private Toggle privateToggleInput;

    [SerializeField]
    private TMP_InputField descriptionTextInput;

    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private Button confirmButton;

    private string lobbyName, description;

    private bool isPrivate;

    private void Start()
    {
        lobbyName = lobbyNameTextInput.text;
        isPrivate = privateToggleInput.isOn;
        description = descriptionTextInput.text;

        lobbyNameTextInput.onEndEdit.AddListener(value => lobbyName = value);
        privateToggleInput.onValueChanged.AddListener(value => isPrivate = value);
        descriptionTextInput.onEndEdit.AddListener(value => description = value);

        cancelButton.onClick.AddListener(ModalManager.Instance.CloseNearestModal);

        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    private void OnConfirmButtonClick()
    {
        LoadingSceneManager.Instance.CreateRelayAndStartHostCoroutine(lobbyName, description);
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
