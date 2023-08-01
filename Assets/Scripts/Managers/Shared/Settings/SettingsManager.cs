
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{
    [SerializeField]
    private Button soundButton;

    [SerializeField]
    private Button settingsButton;

    [SerializeField]
    private Transform soundModalPrefab;

    [SerializeField]
    private Transform settingsModalPrefab;

    private void Start()
    {
        soundButton.onClick.AddListener(() => ModalManager.Instance.CreateModal(soundModalPrefab));

        settingsButton.onClick.AddListener(() => ModalManager.Instance.CreateModal(settingsModalPrefab));
    }
}
