using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityHoverManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string abilitySymbol;

    private string abilityName;

    private string abilityDescription;

    public void SetProperties(AbilitySymbol abilitySymbol, string abilityName, string abilityDescription)
    {
        this.abilitySymbol = EnumUtility.GetDescription(abilitySymbol);
        this.abilityName = abilityName;
        this.abilityDescription = abilityDescription;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CardWarehouseTooltipManager.Instance.Show(abilitySymbol, abilityName, abilityDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CardWarehouseTooltipManager.Instance.Hide();
    }

    public void Update()
    {
        CardWarehouseTooltipManager.Instance.MoveContainerToMousePointer();
    }
}
