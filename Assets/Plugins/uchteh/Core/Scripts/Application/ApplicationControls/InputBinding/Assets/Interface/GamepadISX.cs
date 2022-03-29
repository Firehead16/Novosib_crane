using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

//---------------------------------------------------------------------------
// Parent Class for All Gamepad/Controller Input from New Input System.
//---------------------------------------------------------------------------

// ReSharper disable once InconsistentNaming
public class GamepadISX : MonoBehaviour
{

    protected InputAction MButtonAction;
    protected InputAction MdPadAction;
    protected InputAction MStickMoveAction;

    // Callback funtion when a button in a dpad is pressed.
    protected virtual void OnDpadPress(DpadControl control)
    {
        string dpadName = FirstLetterToUpper(control.name);
        OnControllerButtonPress(control.up, dpadName);
        OnControllerButtonPress(control.down, dpadName);
        OnControllerButtonPress(control.left, dpadName);
        OnControllerButtonPress(control.right, dpadName);
    }

    // Callback function when a stick is moved.
    protected virtual void StickMove(StickControl control)
    {
    }


    // If the one of the controller button is pressed
    protected virtual string OnControllerButtonPress(ButtonControl control, string dpadName = null, bool isXbox = false, bool isPs = false)
    {
        string buttonName = control.name;

        // If the button input is from pressing a stick
        if (buttonName.Contains("StickPress"))
        {
            buttonName = buttonName.Replace("Press", "");
        }
        else
        {
            if (control.aliases.Count > 0)
            {
                if (isXbox)    buttonName = control.aliases[0];
                else if (isPs) buttonName = control.aliases[1];
                else buttonName = control.name.Replace("button", "");
            } 
        }
        return buttonName;
    }

    

    protected void StartHighlight(Transform controlTrans)
    {
        SpriteRenderer sr = controlTrans.Find("Highlight_Input_System").GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = true;
    }

    protected void StopHighlight(Transform controlTrans)
    {
        SpriteRenderer sr = controlTrans.Find("Highlight_Input_System").GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;
    }

    protected string FirstLetterToUpper(string str)
    {
        if (String.IsNullOrEmpty(str))
            return null;
        if (str.Length == 1)
            return str.ToUpper();
        return char.ToUpper(str[0]) + str.Substring(1);
    }
 
}
