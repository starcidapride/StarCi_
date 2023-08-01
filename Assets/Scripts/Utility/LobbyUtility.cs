using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using static EnumUtility;
public class LobbyUtility
{
    public static async Task<Lobby> CreateLobby(string lobbyName, string lobbyHost, string lobbyDescription, string relayCode, bool isPrivate)
    {
        try
        {
            return await LobbyService.Instance.CreateLobbyAsync(
                lobbyName,
                2,
                new CreateLobbyOptions()
                {
                    Data = new Dictionary<string, DataObject>()
                    {
                        { GetDescription(LobbyKey.Host), new DataObject(
                            DataObject.VisibilityOptions.Public,
                            lobbyHost
                            ) },

                        { GetDescription(LobbyKey.Description), new DataObject(
                            DataObject.VisibilityOptions.Public,
                           lobbyDescription
                            ) },
                         { GetDescription(LobbyKey.Status), new DataObject(
                            DataObject.VisibilityOptions.Public,
                            true.ToString()
                            ) },
                        { GetDescription(LobbyKey.RelayCode), new DataObject(
                            DataObject.VisibilityOptions.Public,
                            relayCode
                            ) }
                    }
                }
                );

        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex.Message);
            ShowLobbyExceptionAlert(ex.Reason);
            return null;
        }
    }

    public static async Task<Lobby> JoinLobbyByCode(string code)
    {
        try
        {
            return await LobbyService.Instance.JoinLobbyByCodeAsync(code);
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex.Message);
            ShowLobbyExceptionAlert(ex.Reason);
            return null;
        }
    }

    public static async Task<Lobby> QuickJoinLobby()
    {
        try
        {
            return await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex.Message);
            ShowLobbyExceptionAlert(ex.Reason);
            return null;
        }
    }

    public static bool StopHeartbeat { get; set; } = true;
    public static async void MaintainLobbyHeartbeat(string lobbyId)
    {
        try
        {
            while (StopHeartbeat)
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(lobbyId); ;

                await Task.Delay(TimeSpan.FromSeconds(25));
            }
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex.Message);
            ShowLobbyExceptionAlert(ex.Reason);
        }
    }

    public static async Task<List<Lobby>> GetLobbies()
    {
        try
        {
            return (await Lobbies.Instance.QueryLobbiesAsync()).Results;
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex.Message);
            ShowLobbyExceptionAlert(ex.Reason);
            return null;
        }
    }

    public static async Task<bool> LeaveLobby(string lobbyId)
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(lobbyId, AuthenticationService.Instance.PlayerId);

            return true;
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex.Message);
            ShowLobbyExceptionAlert(ex.Reason);

            return false;
        }
    }

    public static async Task<bool> DeleteLobby(string lobbyId)
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(lobbyId);

            return true;
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex.Message);
            ShowLobbyExceptionAlert(ex.Reason);

            return false;
        }
    }

    public static async Task<bool> RemovePlayer(Lobby lobby, ulong clientId)
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(lobby.Id, lobby.Players[(int)clientId].Id);

            return true;
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex.Message);
            ShowLobbyExceptionAlert(ex.Reason);

            return false;
        }
    }


    private static void ShowLobbyExceptionAlert(LobbyExceptionReason reason)
    {
        var message = reason switch
        {
            LobbyExceptionReason.RequestTimeOut => "The request to the lobby has timed out.",
            LobbyExceptionReason.LobbyFull => "The lobby is currently full. Please try again later or join another lobby.",
            LobbyExceptionReason.LobbyNotFound => "The lobby you are trying to access could not be found.",
            LobbyExceptionReason.RateLimited => "You have been rate-limited for accessing the lobby. Please try again later.",
            LobbyExceptionReason.NetworkError => "A network error has occurred. Please check your internet connection and try again.",
            LobbyExceptionReason.NoOpenLobbies => "There are no open lobbies available at the moment. Please try again later or create a new lobby.",
            _ => reason.ToString()
        };

        AlertManager.Instance.Show(
            AlertCaption.Failure,
            message,
            new List<AlertButton>()
                {
                new AlertButton()
                {
                    ButtonText = ButtonText.Cancel,
                    HandleOnClick = () => AlertManager.Instance.Hide()
                }
            }
        );
    }
}

public enum LobbyKey
{
    [Description("Host")]
    Host,

    [Description("Description")]
    Description,

    [Description("Relay Code")]
    RelayCode,

    [Description("Status")]
    Status
}