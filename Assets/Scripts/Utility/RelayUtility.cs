using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Services.Relay;

using static EnumUtility;
public class RelayUtility
{
    public static async Task<string> CreateRelay()
    {
        try
        {
            var allocation = await RelayService.Instance.CreateAllocationAsync(2);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
                );

            return joinCode;
        }
        catch (RelayServiceException ex)
        {
            ShowRelayExceptionAlert(ex.Reason);

            return null;
        }
    }

    public static async Task<bool> JoinRelay(string joinCode)
    {
        try
        {
            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
                );

            return true;
        }
        catch (RelayServiceException ex)
        {
            ShowRelayExceptionAlert(ex.Reason);

            return false;
        }
    }

    private static void ShowRelayExceptionAlert(RelayExceptionReason reason)
    {
        var message = reason switch
        {
            RelayExceptionReason.RequestTimeOut => "The request to the relay has timed out.",
            RelayExceptionReason.JoinCodeNotFound => "The join code for the relay could not be found.",
            RelayExceptionReason.AllocationNotFound => "The relay you are trying to access could not be found.",
            RelayExceptionReason.RegionNotFound => "The specified region for the relay could not be found.",
            RelayExceptionReason.NetworkError => "A network error has occurred. Please check your internet connection and try again.",
            _ => "An unknown error has occurred in the relay."
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