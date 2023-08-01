using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardWarehouseTooltipManager : Singleton<CardWarehouseTooltipManager>
{
    [SerializeField]
    private Transform container;

    [SerializeField]
    private TMP_Text abilityTitleText;

    [SerializeField]
    private TMP_Text abilityDescriptionText;

    public bool IsContainerHasActived()
    {
        return container.gameObject.activeSelf;
    }
    public void Show(string abilitySymbol, string abilityName, string abilityDescription)
    {

        abilityTitleText.text = $"{abilitySymbol} - {abilityName}";

        abilityDescriptionText.text = abilityDescription;

        container.gameObject.SetActive(true);

        MoveContainerToMousePointer();
    }

    public void MoveContainerToMousePointer()
    {
        var mousePos = GameObjectUtility.GetMousePos();

        var containerSize = ((RectTransform) container.transform).sizeDelta;

        container.transform.position = mousePos + containerSize / 2 ;
    }
    public void Hide()
    {
        container.gameObject.SetActive(false);
    }
}
