using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyRoomManager : Singleton<LobbyRoomManager>
{
    [SerializeField]
    private Transform ui;
    
    [SerializeField]
    private Transform tableBody;

    [SerializeField]
    private Button createLobbyButton;

    [SerializeField] 
    private Button joinWithCodeButton;

    [SerializeField]
    private Button joinLobbyButton;

    [SerializeField]
    private Transform bodyRowPrefab;

    [SerializeField]
    private Transform createLobbyModalPrefab;

    [SerializeField]
    private Transform joinWithCodePrefab;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => ui.gameObject.activeSelf);

        createLobbyButton.onClick.AddListener(() => ModalManager.Instance.CreateModal(createLobbyModalPrefab));
        joinWithCodeButton.onClick.AddListener(() => ModalManager.Instance.CreateModal(joinWithCodePrefab));

        joinLobbyButton.onClick.AddListener(OnJoinLobbyButtonClick);
    }

    private void OnJoinLobbyButtonClick()
    {

    }

    public void SetActiveUI(bool isActive)
    {
        StartCoroutine(SetActiveUICoroutine(isActive));
    }

    private IEnumerator SetActiveUICoroutine(bool isActive)
    {
        if (isActive)
        {
            ui.gameObject.SetActive(true);

            yield return AnimationUtility.WaitForAnimationCompletion(ui);
        }
        else
        {
            ui.gameObject.SetActive(false);
        }
    }
}
