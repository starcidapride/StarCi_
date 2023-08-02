using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.ComponentModel;

public delegate void OnClickDelegate();

public class AlertManager : SingletonPersistent<AlertManager>
{
    [SerializeField]
    private Image backdropImage;

    [SerializeField]
    private Transform messageBox;

    private readonly float a = 0.5f;

    private void Start()
    {
        var color = backdropImage.color;
        color.a = a;

        backdropImage.color = color;
    }

    public void Hide()
    {
        GameObjectUtility.RemoveAllChildGameObjects(backdropImage.transform);

        backdropImage.gameObject.SetActive(false);

        LoadingSceneManager.IsInputBlocked = false;
    }

    public void Show(AlertCaption caption, string message, List<AlertButton> buttons = null)
    {
        backdropImage.gameObject.SetActive(true);

        LoadingSceneManager.IsInputBlocked = true;

        GameObjectUtility.RemoveAllChildGameObjects(backdropImage.transform);

        AlertMessageBoxManager.Caption = caption;

        AlertMessageBoxManager.Message = message;

        AlertMessageBoxManager.Buttons = buttons;

        Instantiate(messageBox, backdropImage.transform);
    }

}

public class AlertButton
{
    public ButtonText ButtonText { get; set; }

    public OnClickDelegate HandleOnClick;
}

public enum AlertCaption
{
    [Description("Success")]
    Success,
    [Description("Failure")]
    Failure,
    [Description("Confirmation")]
    Confirmation
}

public enum ButtonText
{
    [Description("Cancel")]
    Cancel,

    [Description("Quit")]
    Quit,

    [Description("Confirm")]
    Confirm,

    [Description("Yes")]
    Yes,

    [Description("No")]
    No
}
