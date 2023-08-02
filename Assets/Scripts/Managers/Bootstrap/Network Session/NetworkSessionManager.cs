using Newtonsoft.Json;
using System;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class NetworkSessionManager : SingletonNetworkPersistent<NetworkSessionManager>
{
    private Lobby lobby;

    public NetworkVariable<NetworkLobby> NetworkLobby = new();
    public NetworkVariable<NetworkPlayerDatas> NetworkPlayerDatas = new();

    public FixedString64Bytes PlayerId { get; set; }

    public ulong ClientId { get; set; }

    public static bool IsFinishLoad { get; set; } = false;

    public void Start()
    {   
        NetworkManager.Singleton.OnServerStopped += OnServerStopped;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }

    public void Update()
    {
        if (NetworkManager.Singleton.IsListening)
        {
            Debug.Log("Network Lobby : " + JsonConvert.SerializeObject(NetworkLobby.Value));
            Debug.Log("Network Player Datas : " + JsonConvert.SerializeObject(NetworkPlayerDatas.Value));
        }
    }

    private async void OnServerStopped(bool obj)
    {
        lobby = PlayerPrefsUtility.LoadFromPlayerPrefs<Lobby>(PlayerPrefsKey.Lobby);

        LobbyUtility.StopHeartbeat = true;

        await LobbyUtility.DeleteLobby(lobby.Id);

        PlayerPrefsUtility.RemoveFromPlayPrefs(PlayerPrefsKey.Lobby);

        lobby = null;
    }

    private async void OnClientDisconnectCallback(ulong clientId)
    {
        if (IsHost)
        {
            await LobbyUtility.RemovePlayer(lobby, NetworkPlayerDatas.Value.GetIndexByClientId(clientId));
        
            WaitingRoomManager.Instance.DisableOpponentsCard();
        } else
        {
            LoadingSceneManager.Instance.LoadScene(SceneName.LobbyRoom, false);
        }
    }

    public override void OnNetworkSpawn()
    {
        ClientId = NetworkManager.LocalClientId;

        PlayerId = Guid.NewGuid().ToString();

        var user = LocalSessionManager.Instance.User;

        var selectedDeck = LocalSessionManager.Instance.GetSelectedDeck();

        if (IsHost)
        {
            lobby = PlayerPrefsUtility.LoadFromPlayerPrefs<Lobby>(PlayerPrefsKey.Lobby);

            NetworkLobby.Value = new()
            {
                LobbyId = lobby.Id,
                LobbyName = lobby.Name,
                LobbyCode = lobby.LobbyCode,
                IsPrivate = lobby.IsPrivate
            };

            NetworkPlayerDatas.Value = new()
            {
                PlayerDatas = new()
            };

            LobbyUtility.StopHeartbeat = false;

            LobbyUtility.MaintainLobbyHeartbeat(lobby.Id);    
        }

       
        var connectedPlayer = new NetworkPlayerData()
        {
            PlayerSession = new()
            {
                ClientId = ClientId,
                PlayerId = PlayerId
            },

            Player = new()
            {
                Username = user.Username,
                PictureIndex = user.PictureIndex,
                CardSleeveIndex = user.CardSleeveIndex,
                IsReady = false
            },

            PlayDeck = new()
            {
                Cards = selectedDeck.PlayDeck
            },

            CharacterDeck = new()
            {
                Cards = selectedDeck.CharacterDeck
            },

            Hand = new()
            {
                Cards = new()
            }
        };

        AddPlayerDataServerRpc(connectedPlayer);

        IsFinishLoad = true;
    }


    [ServerRpc(RequireOwnership = false)]    
    private void AddPlayerDataServerRpc(NetworkPlayerData networkPlayerData)
    {   
        var networkPlayerDatas = NetworkPlayerDatas.Value;

        networkPlayerDatas.Add(networkPlayerData);

        NetworkPlayerDatas.Value = networkPlayerDatas;
    }

    public override void OnNetworkDespawn()
    {
        IsFinishLoad = false;
    }
}