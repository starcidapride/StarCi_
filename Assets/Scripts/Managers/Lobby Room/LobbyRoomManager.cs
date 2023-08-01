using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyRoomManager : Singleton<LobbyRoomManager>
{
    [SerializeField]
    private Transform tableBody;

    [SerializeField]
    private Button createLobbyButton;

    [SerializeField] 
    private Button joinWithCodeButton;

    [SerializeField]
    private Button joinLobbyButton;

    [SerializeField]
    private Transform bodyRowPrefab;

    [SerializeField]
    private Transform createLobbyModalPrefab;

    [SerializeField]
    private Transform joinWithCodePrefab;

    private void Start()
    {
        createLobbyButton.onClick.AddListener(() => ModalManager.Instance.CreateModal(createLobbyModalPrefab));
        joinWithCodeButton.onClick.AddListener(() => ModalManager.Instance.CreateModal(joinWithCodePrefab));

        joinLobbyButton.onClick.AddListener(OnJoinLobbyButtonClick);
    }

    private void OnJoinLobbyButtonClick()
    {

    }
}
