using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ControllerEvents : MonoBehaviour {

    public struct ControllerInteractionEventArgs
    {
        public uint controllerIndex;
    }

    public delegate void ControllerInteractionEventHandler(object sender, ControllerInteractionEventArgs e);

    public event ControllerInteractionEventHandler MenuPressed;
    public event ControllerInteractionEventHandler MenuReleased;

    public event ControllerInteractionEventHandler TouchpadTouched;
    public event ControllerInteractionEventHandler TouchpadTouchReleased;

    [HideInInspector]
    public bool menuPressed = false;
    [HideInInspector]
    public bool touchpadTouched = false;

    private uint controllerIndex;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Update()
    {
        controllerIndex = (uint)trackedObj.index;
        device = SteamVR_Controller.Input((int)controllerIndex);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            OnMenuPressed(SetButtonEvent(ref menuPressed, true));
        }
        else if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            OnMenuReleased(SetButtonEvent(ref menuPressed, false));
        }

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            OnTouchpadTouchStart(SetButtonEvent(ref touchpadTouched, true));
        }
        else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            OnTouchpadTouchEnd(SetButtonEvent(ref touchpadTouched, false));
        }
    }

    private ControllerInteractionEventArgs SetButtonEvent(ref bool buttonBool, bool value)
    {
        buttonBool = value;
        ControllerInteractionEventArgs e;
        e.controllerIndex = controllerIndex;
        return e;
    }

    public virtual void OnMenuPressed(ControllerInteractionEventArgs e)
    {
        if (MenuPressed != null)
        {
            MenuPressed(this, e);
        }
    }

    public virtual void OnMenuReleased(ControllerInteractionEventArgs e)
    {
        if (MenuReleased != null)
        {
            MenuReleased(this, e);
        }
    }

    public virtual void OnTouchpadTouchStart(ControllerInteractionEventArgs e)
    {
        if (TouchpadTouched != null)
        {
            TouchpadTouched(this, e);
        }
    }

    public virtual void OnTouchpadTouchEnd(ControllerInteractionEventArgs e)
    {
        if (TouchpadTouchReleased != null)
        {
            TouchpadTouchReleased(this, e);
        }
    }
}
