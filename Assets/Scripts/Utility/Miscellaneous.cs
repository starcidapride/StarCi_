using UnityEngine.UI;

public class Miscellaneous
{
    public static void SetButtonEnabled(Button button, bool isEnable)
    {
        button.interactable = isEnable;

        button.GetComponent<Image>().color = isEnable
            ? ImageUtility.GetColorFromHexEnum(HexEnum.Beige)
            : ImageUtility.GetColorFromHexEnum(HexEnum.Gray);
    }
}