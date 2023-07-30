using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class LoadingSceneManager : SingletonPersistent<LoadingSceneManager>
{
    [SerializeField]
    private Transform createUserModalPrefab;

    public static bool InputBlocked { get; set; } = false;
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

                    CardWarehouseManager.Instance.SetActiveUI(true);
                break;

            //case SceneName.LobbyRoom:
              //  LobbyRoomManager.Instance.SetActiveUI(true);
             //   break;
        }
    }

    public void LoadScene(SceneName sceneToLoad, bool isNetworkSessionAction)
    {
        StartCoroutine(LoadSceneCoroutine(sceneToLoad, isNetworkSessionAction));
    }

    private IEnumerator LoadSceneCoroutine(SceneName sceneToLoad, bool isNetworkSessionActive)
    {
        InputBlocked = true;

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

        InputBlocked = false;
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
}

public enum SceneName
{
    [Description("Bootstrap")]
    Bootstrap,

    [Description("Home")]
    Home,

    [Description("Card Warehouse")]
    CardWarehouse
}