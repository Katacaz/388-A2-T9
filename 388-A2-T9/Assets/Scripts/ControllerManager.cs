using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  static class ControllerManager
{
    public enum Buttons
    {
        A,
        B,
        X,
        Y,
        LTrigger,
        LButton,
        RTrigger,
        RButton
    }

    //Check if button is being held down
    public static bool ButtonPressCheck(Buttons button)
    {
        bool status = false;
        switch (button)
        {
            case Buttons.A:
                status = OVRInput.Get(OVRInput.RawButton.A);
                break;
            case Buttons.B:
                status = OVRInput.Get(OVRInput.RawButton.B);
                break;
            case Buttons.X:
                status = OVRInput.Get(OVRInput.RawButton.X);
                break;
            case Buttons.Y:
                status = OVRInput.Get(OVRInput.RawButton.Y);
                break;
            case Buttons.LTrigger:
                status = OVRInput.Get(OVRInput.RawButton.LIndexTrigger);
                break;
            case Buttons.LButton:
                status = OVRInput.Get(OVRInput.RawButton.LHandTrigger);
                break;
            case Buttons.RTrigger:
                status = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
                break;
            case Buttons.RButton:
                status = OVRInput.Get(OVRInput.RawButton.RHandTrigger);
                break;
        }
        return status;
    }
    //Check if button was let go
    public static bool ButtonUpCheck(Buttons button)
    {
        bool status = false;
        switch (button)
        {
            case Buttons.A:
                status = OVRInput.GetUp(OVRInput.RawButton.A);
                break;
            case Buttons.B:
                status = OVRInput.GetUp(OVRInput.RawButton.B);
                break;
            case Buttons.X:
                status = OVRInput.GetUp(OVRInput.RawButton.X);
                break;
            case Buttons.Y:
                status = OVRInput.GetUp(OVRInput.RawButton.Y);
                break;
            case Buttons.LTrigger:
                status = OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger);
                break;
            case Buttons.LButton:
                status = OVRInput.GetUp(OVRInput.RawButton.LHandTrigger);
                break;
            case Buttons.RTrigger:
                status = OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger);
                break;
            case Buttons.RButton:
                status = OVRInput.GetUp(OVRInput.RawButton.RHandTrigger);
                break;
        }
        return status;
    }
    //Check if button was just pressed
    public static bool ButtonDownCheck(Buttons button)
    {
        bool status = false;
        switch (button)
        {
            case Buttons.A:
                status = OVRInput.GetDown(OVRInput.RawButton.A);
                break;
            case Buttons.B:
                status = OVRInput.GetDown(OVRInput.RawButton.B);
                break;
            case Buttons.X:
                status = OVRInput.GetDown(OVRInput.RawButton.X);
                break;
            case Buttons.Y:
                status = OVRInput.GetDown(OVRInput.RawButton.Y);
                break;
            case Buttons.LTrigger:
                status = OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger);
                break;
            case Buttons.LButton:
                status = OVRInput.GetDown(OVRInput.RawButton.LHandTrigger);
                break;
            case Buttons.RTrigger:
                status = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);
                break;
            case Buttons.RButton:
                status = OVRInput.GetDown(OVRInput.RawButton.RHandTrigger);
                break;
        }
        return status;
    }
    //Check if button is being touched
    public static bool ButtonTouchCheck(Buttons button)
    {
        OVRInput.Update();
        bool status = false;
        switch (button)
        {
            case Buttons.A:
                status = OVRInput.Get(OVRInput.RawTouch.A);
                break;
            case Buttons.B:
                status = OVRInput.Get(OVRInput.RawTouch.B);
                break;
            case Buttons.X:
                status = OVRInput.Get(OVRInput.RawTouch.X);
                break;
            case Buttons.Y:
                status = OVRInput.Get(OVRInput.RawTouch.Y);
                break;
            case Buttons.LTrigger:
                status = OVRInput.Get(OVRInput.RawTouch.LIndexTrigger);
                break;
            case Buttons.LButton:
                status = OVRInput.Get(OVRInput.RawTouch.LTouchpad);
                break;
            case Buttons.RTrigger:
                status = OVRInput.Get(OVRInput.RawTouch.RIndexTrigger);
                break;
            case Buttons.RButton:
                status = OVRInput.Get(OVRInput.RawTouch.RTouchpad);
                break;
        }
        return status;
    }
}
