using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateUserModalManager : Singleton<CreateUserModalManager>
{
    [SerializeField]
    private TMP_InputField usernameTextInput;

    [SerializeField]
    private Button confirmButton;

    private string username;
    private void Start()
    {
        username = usernameTextInput.text;

        usernameTextInput.onEndEdit.AddListener(value => username = value);

        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    private void OnConfirmButtonClick()
    {
        try
        {
            LocalSessionManager.Instance.Initialize(username);

            HomeManager.Instance.SetActiveUI(true);
        } 
        finally
        {
            ModalManager.Instance.CloseNearestModal();
        }
 
    }
}
