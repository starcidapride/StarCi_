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
    private Button joinLobbyByCodeButton;

    [SerializeField]
    private Button joinLobbyButton;

    [SerializeField]
    private Transform bodyRowPrefab;

}
