using Newtonsoft.Json;
using System;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class NetworkSessionManager : SingletonNetworkPersistent<NetworkSessionManager>
{
    public Lobby Lobby { get; set; }

    public NetworkVariable<NetworkLobby> NetworkLobby = new();
    public NetworkVariable<NetworkPlayerDatas> NetworkPlayerDatas = new();

    public FixedString64Bytes PlayerId { get; set; }

    public ulong ClientId { get; set; }

    public static bool IsFinishLoad { get; set; } = false;

    public async void KickPlayer()
    {   
        //await LobbyUtility.RemovePlayer(
        //   Lobby,
        //    NetworkPlayerDatas.Value.GetOtherIndexByClientId(
        //        ClientId
        //       ));
    }

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
        Lobby = PlayerPrefsUtility.LoadFromPlayerPrefs<Lobby>(PlayerPrefsKey.Lobby);

        LobbyUtility.StopHeartbeat = true;

        await LobbyUtility.DeleteLobby(Lobby.Id);

        PlayerPrefsUtility.RemoveFromPlayPrefs(PlayerPrefsKey.Lobby);

        Lobby = null;
    }

    private async void OnClientDisconnectCallback(ulong clientId)
    {
        if (IsHost)
        {
            await LobbyUtility.RemovePlayer(Lobby, NetworkPlayerDatas.Value.GetIndexByClientId(clientId));
        
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
            Lobby  = PlayerPrefsUtility.LoadFromPlayerPrefs<Lobby>(PlayerPrefsKey.Lobby);

            NetworkLobby.Value = new()
            {
                LobbyId = Lobby.Id,
                LobbyName = Lobby.Name,
                LobbyCode = Lobby.LobbyCode,
                IsPrivate = Lobby.IsPrivate
            };

            NetworkPlayerDatas.Value = new()
            {
                PlayerDatas = new()
            };

            LobbyUtility.StopHeartbeat = false;

            LobbyUtility.MaintainLobbyHeartbeat(Lobby.Id);    
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

        var networkPlayerDatas = NetworkPlayerDatas.Value;

        networkPlayerDatas.Add(connectedPlayer);

        NetworkPlayerDatas.Value = networkPlayerDatas;

        UpdatePlayerDataServerRpc(networkPlayerDatas);

        IsFinishLoad = true;
    }


    [ServerRpc(RequireOwnership = false)]    
    public void UpdatePlayerDataServerRpc(NetworkPlayerDatas networkPlayerDatas, ServerRpcParams serverRpcParams = default)
    {
        var senderClientId = serverRpcParams.Receive.SenderClientId;

        NetworkPlayerDatas.Value = networkPlayerDatas;
        
        if (ClientId != senderClientId)
        {
            UpdatePlayerDataClientRpc();
        }
    }

    [ClientRpc]
    private void UpdatePlayerDataClientRpc()
    {   
        WaitingRoomManager.Instance.SetPlayerCard(Participant.Opponent);

    }

    [ServerRpc(RequireOwnership = false)]
    public void ToggleReadyStatusServerRpc(bool status, ServerRpcParams serverRpcParams = default)
    {   
        var senderClientId = serverRpcParams.Receive.SenderClientId;

        ToggleReadyStatusClientRpc(status, senderClientId);
    }


    [ClientRpc]
    private void ToggleReadyStatusClientRpc(bool status, ulong clientId)
    {
            WaitingRoomManager.Instance.SetReadyStatus(
                ClientId == clientId ? Participant.You : Participant.Opponent, 
                status);

        var networkPlayerDatas = NetworkPlayerDatas.Value;

        WaitingRoomManager.Instance.SetStartButtonEnabled(
             networkPlayerDatas.PlayerDatas.Count == 2
        && IsHost
        && networkPlayerDatas.GetByClientId(clientId).Player.IsReady
        && networkPlayerDatas.GetOtherByClientId(clientId).Player.IsReady
            );
    }

    [ClientRpc]
    public void StartGameClientRpc()
    {   
        if (IsHost)
        {
            LoadingSceneManager.Instance.LoadScene(SceneName.PlayRoom, true);
        } else
        {
            LoadingFadeEffectManager.Instance.FadeAll();
        }
    }

    [ClientRpc]
    public void PlayBothPlayerDrawACardClientRpc(int yourNumCards, int opponentsNumCards)
    {
        PlayRoomManager.Instance.PlayBothPlayerDrawACard(yourNumCards, opponentsNumCards);
    }




    public override void OnNetworkDespawn()
    {
        IsFinishLoad = false;
    }
}