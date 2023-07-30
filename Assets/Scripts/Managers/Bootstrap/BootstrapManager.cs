using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootstrapManager : Singleton<BootstrapManager>
{
    private IEnumerator Start()
    {   
        yield return new WaitUntil(() => LoadingSceneManager.Instance != null);

        LoadingSceneManager.Instance.LoadScene(SceneName.Home, false);
    }
}
