using UnityEngine.UI;

public class Miscellaneous
{
    public static void SetButtonEnabled(Button button, bool isEnable, HexEnum activeColor, HexEnum disableColor)
    {
        button.interactable = isEnable;

        button.GetComponent<Image>().color = isEnable
            ? ImageUtility.GetColorFromHexEnum(activeColor)
            : ImageUtility.GetColorFromHexEnum(disableColor);
    }
}