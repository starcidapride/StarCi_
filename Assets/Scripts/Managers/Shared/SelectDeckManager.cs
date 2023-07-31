using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectDeckManager : Singleton<SelectDeckManager>
{
    [SerializeField]
    private TMP_Dropdown selectDeckDropdown;

    public static SceneName ActiveScene { get; set; }

    private User user;
    private IEnumerator Start()
    {   
        switch (ActiveScene)
        {
            case SceneName.CardWarehouse:
                yield return new WaitUntil(() => CardWarehouseManager.IsFinishLoad);
                break;

            //case SceneName.WaitingRoom:
            //    yield return new WaitUntil(() => WaitingRoomManager.IsFinishLoad);
            //    break;
        }

        user = LocalSessionManager.Instance.User;

        UpdateDeckDropdownOptions();

        selectDeckDropdown.onValueChanged.AddListener(OnSelectDeckDropdownValueChanged);

        InvokeOnSelectedDeckChanged();
    }

    public void UpdateDeckDropdownOptions()
    {
        var options = new List<TMP_Dropdown.OptionData>();

        foreach (var deck in user.DeckCollection.Decks)
        {
            options.Add(new TMP_Dropdown.OptionData()
            {
                text = deck.DeckName
            });
        }
        
        selectDeckDropdown.options = options;

        selectDeckDropdown.value = user.DeckCollection.SelectedDeckIndex;
    }

    private void OnSelectDeckDropdownValueChanged(int value)
    {
        user.DeckCollection.SelectedDeckIndex = value;

        LocalSessionManager.Instance.SaveToPlayerPrefs();

        InvokeOnSelectedDeckChanged();
    }

    public delegate void OnSelectedDeckChangedDelegate();

    public event OnSelectedDeckChangedDelegate OnSelectedDeckChanged;
    public void InvokeOnSelectedDeckChanged()
    {
        OnSelectedDeckChanged?.Invoke();
    }

}
