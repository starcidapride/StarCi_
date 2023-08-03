using Newtonsoft.Json;
using Unity.Netcode;
using UnityEngine;

public class GameCentralManager : SingletonNetwork<GameCentralManager>
{
    public NetworkVariable<NetworkGameCentral> NetworkGameCentral = new();
    public static bool IsFinishLoad {get; set; } = false;
    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            NetworkGameCentral.Value = new()
            {
                TurnId = 0,
                // host start first
                IsHostTurn = true,
                TurnPhase = TurnPhase.PrePhase
            };
        }

        IsFinishLoad = true;
    }

    public override void OnNetworkDespawn()
    {
        IsFinishLoad = false;
    }

    private void Update()
    {
        if (NetworkManager.Singleton.IsListening)
        {
            Debug.Log("Network Game Central : " + JsonConvert.SerializeObject(NetworkGameCentral.Value));
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateNetworkGameCentralServerRpc(NetworkGameCentral networkGameCentral)
    {
        NetworkGameCentral.Value = networkGameCentral;
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndTurnServerRpc(bool isHostTurn)
    {
        EndTurnClientRpc(isHostTurn);
    }

    [ClientRpc]
    public void EndTurnClientRpc(bool isHostTurn)
    {
        PlayRoomManager.Instance.UpdateActionField(IsHost == isHostTurn, TurnPhase.PrePhase);
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextPhaseServerRpc(bool isHostTurn, TurnPhase phase)
    {
        NextPhaseClientRpc(isHostTurn, phase);
    }

    [ClientRpc]
    public void NextPhaseClientRpc(bool isHostTurn, TurnPhase phase)
    {
        PlayRoomManager.Instance.UpdateActionField(IsHost == isHostTurn, phase);
    }
}