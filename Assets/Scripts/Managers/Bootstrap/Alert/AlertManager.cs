using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.ComponentModel;

public delegate void OnClickDelegate();

public class AlertManager : SingletonPersistent<AlertManager>
{
    [SerializeField]
    private Image backdrop;

    [SerializeField]
    private Transform messageBox;

    private readonly float a = 0.5f;
    public void Hide()
    {
        GameObjectUtility.RemoveAllChildGameObjects(backdrop.transform);

        backdrop.gameObject.SetActive(false);

        LoadingSceneManager.IsInputBlocked = false;
    }

    public void Show(AlertCaption caption, string message, List<AlertButton> buttons = null)
    {
        backdrop.gameObject.SetActive(true);

        var color = backdrop.color;
        
        color.a = a;

        backdrop.color = color;

        LoadingSceneManager.IsInputBlocked = true;

        GameObjectUtility.RemoveAllChildGameObjects(backdrop.transform);

        AlertMessageBoxManager.Caption = caption;

        AlertMessageBoxManager.Message = message;

        AlertMessageBoxManager.Buttons = buttons;

        Instantiate(messageBox, backdrop.transform);
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
