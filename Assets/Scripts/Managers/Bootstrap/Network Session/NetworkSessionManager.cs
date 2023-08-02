using System;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class NetworkSessionManager : SingletonNetworkPersistent<NetworkSessionManager>
{
    public NetworkVariable<NetworkLobby> NetworkLobby = new();
    public NetworkVariable<NetworkPlayerDatas> NetworkPlayerDatas = new();

    private Lobby lobby;
    public FixedString64Bytes PlayerId { get; set; }

    private ulong clientId;

    public static bool IsFinishLoad { get; set; } = false;
    public override void OnNetworkSpawn()
    {
        clientId = NetworkManager.LocalClientId;

        PlayerId = Guid.NewGuid().ToString();

        var user = LocalSessionManager.Instance.User;

        var selectedDeck = LocalSessionManager.Instance.GetSelectedDeck();

        if (IsHost)
        {
            lobby = PlayerPrefsUtility.LoadFromPlayerPrefs<Lobby>(PlayerPrefsKey.Lobby);

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
                ClientId = clientId,
                PlayerId = PlayerId
            },

            Player = new()
            {
                Username = user.Username,
                Picture = user.PictureIndex,
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

    public async override void OnNetworkDespawn()
    {
        if (IsHost)
        {
            lobby = PlayerPrefsUtility.LoadFromPlayerPrefs<Lobby>(PlayerPrefsKey.Lobby);

            LobbyUtility.StopHeartbeat = true;

            lobby = null;

            NetworkLobby = new();

            NetworkPlayerDatas = new();

            await LobbyUtility.DeleteLobby(lobby.Id);

        }

        IsFinishLoad = false;
    }
}