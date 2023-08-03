using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.ComponentModel;
using Unity.Services.Lobbies.Models;

public class LoadingSceneManager : SingletonPersistent<LoadingSceneManager>
{
    [SerializeField]
    private Transform createUserModalPrefab;

    [SerializeField]
    private Transform createFirstDeckModalPrefab;
    public static bool IsInputBlocked { get; set; } = false;
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(OnSceneLoadedCoroutine(scene));
    }

    private IEnumerator OnSceneLoadedCoroutine(Scene scene)
    {
        yield return new WaitUntil(() => LoadingFadeEffectManager.IsEndFadeOut);

        var user = LocalSessionManager.Instance.User;

        var loadedScene = EnumUtility.GetEnumValueByDescription<SceneName>(scene.name);

        switch (loadedScene)
        {
            case SceneName.Home:
                if (user == null)
                {
                    ModalManager.Instance.CreateModal(createUserModalPrefab);
                }
                else
                {
                    HomeManager.Instance.SetActiveUI(true);
                }

                break;

            case SceneName.CardWarehouse:
                if (user.DeckCollection.SelectedDeckIndex == -1)
                {
                    ModalManager.Instance.CreateModal(createFirstDeckModalPrefab);
                } else
                {
                    CardWarehouseManager.Instance.SetActiveUI(true);
                }
                break;

            case SceneName.LobbyRoom:
                LobbyRoomManager.Instance.SetActiveUI(true);
                break;

            case SceneName.WaitingRoom:
                WaitingRoomManager.Instance.SetActiveUI(true);
                break;

            case SceneName.PlayRoom:
                PlayRoomManager.Instance.SetActiveUI(true);
                break;
        }
    }

    public void LoadScene(SceneName sceneToLoad, bool isNetworkSessionAction)
    {
        StartCoroutine(LoadSceneCoroutine(sceneToLoad, isNetworkSessionAction));
    }

    private IEnumerator LoadSceneCoroutine(SceneName sceneToLoad, bool isNetworkSessionActive)
    {
        IsInputBlocked = true;

        LoadingFadeEffectManager.Instance.FadeIn();

        yield return new WaitUntil(() => LoadingFadeEffectManager.IsEndFadeIn);

        if (isNetworkSessionActive)
        {
            if (NetworkManager.Singleton.IsServer)
                LoadSceneNetwork(sceneToLoad);
        }
        else
        {
            LoadSceneLocal(sceneToLoad);
        }

        yield return new WaitForSeconds(1f);

        LoadingFadeEffectManager.Instance.FadeOut();

        yield return new WaitUntil(() => LoadingFadeEffectManager.IsEndFadeOut);

        IsInputBlocked = false;
    }

    private void LoadSceneLocal(SceneName sceneToLoad)
    {
        SceneManager.LoadScene(EnumUtility.GetDescription(sceneToLoad));
    }

    public void LoadSceneNetwork(SceneName sceneToLoad)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(
            EnumUtility.GetDescription(sceneToLoad),
            LoadSceneMode.Single);
    }

    public void CreateRelayAndStartHost(string lobbyName, string lobbyDescription, bool lobbyPrivate)
    {
        StartCoroutine(CreateRelayAndStartHostCoroutine(lobbyName, lobbyDescription, lobbyPrivate));
    }

    public IEnumerator CreateRelayAndStartHostCoroutine(string lobbyName, string lobbyDescription, bool lobbyPrivate)
    {
        LoadingManager.Instance.Show();
        try
        {
            var createRelayTask = RelayUtility.CreateRelay();

            yield return new WaitUntil(() => createRelayTask.IsCompleted);

            var relayCode = createRelayTask.Result;

            if (relayCode == null) yield break;

            var createLobbyTask = LobbyUtility.CreateLobby(lobbyName,
                LocalSessionManager.Instance.User.Username,
                lobbyDescription,
                relayCode,
                lobbyPrivate);

            yield return new WaitUntil(() => createLobbyTask.IsCompleted);

            var lobby = createLobbyTask.Result;

            if (lobby == null) yield break;

            PlayerPrefsUtility.SaveToPlayerPrefs(PlayerPrefsKey.Lobby, lobby);

            LoadScene(SceneName.WaitingRoom, true);

            NetworkManager.Singleton.StartHost();

        }
        finally
        {
            LoadingManager.Instance.Hide();
        }
    }

    public void JoinRelayAndStartClient(Lobby lobby)
    {
        StartCoroutine(JoinRelayAndStartClientCoroutine(lobby));
    }

    private IEnumerator JoinRelayAndStartClientCoroutine(Lobby lobby)
    {
        LoadingManager.Instance.Show();
        try
        {
            var joinCode = lobby.Data[EnumUtility.GetDescription(LobbyKey.RelayCode)].Value;

            Debug.Log(joinCode);

            var resultTask = RelayUtility.JoinRelay(joinCode);

            yield return new WaitUntil(() => resultTask.IsCompleted);

            if (!resultTask.Result) yield break;

            LoadingFadeEffectManager.Instance.FadeIn();

            yield return new WaitUntil(() => LoadingFadeEffectManager.IsEndFadeIn);

            NetworkManager.Singleton.StartClient();

            yield return new WaitForSeconds(1f);

            LoadingFadeEffectManager.Instance.FadeOut();
        }
        finally
        {
            LoadingManager.Instance.Hide();
        }
    }


}

public enum SceneName
{
    [Description("Bootstrap")]
    Bootstrap,

    [Description("Home")]
    Home,

    [Description("Card Warehouse")]
    CardWarehouse,

    [Description("Lobby Room")]
    LobbyRoom,

    [Description("Waiting Room")]
    WaitingRoom,

    [Description("Play Room")]
    PlayRoom
}