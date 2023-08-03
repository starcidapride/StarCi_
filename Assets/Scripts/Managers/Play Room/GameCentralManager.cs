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
                IsYourTurn = true
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
}