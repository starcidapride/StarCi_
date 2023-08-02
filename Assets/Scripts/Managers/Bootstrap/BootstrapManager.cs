using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootstrapManager : Singleton<BootstrapManager>
{
    private IEnumerator Start()
    {   
        yield return new WaitUntil(() => LoadingSceneManager.Instance != null);

        var authenticationTask = AuthenticationUtility.InitiateAnonymousSignIn();

        yield return new WaitUntil(() => authenticationTask.IsCompleted);

        LoadingSceneManager.Instance.LoadScene(SceneName.Home, false);
    }
}
