﻿using UnityEngine;

public class JogOnSpotMovement : MonoBehaviour
{
    [Header("Headset Information", order = 1)]
    [Tooltip("Optionally insert SteamVR '[CameraRig]' here or allow the script to auto find the '[CameraRig]''.")]
    public GameObject cameraRig;
    [Tooltip("Optionally insert SteamVR 'Camera (eye)' here or allow the script to auto find the 'Camera (eye)'.")]
    public Transform VRHeadset;
    [Tooltip("Optionally insert a GameObject that is used to track the headset's movement.")]
    public GameObject zeroTracker;
    [Tooltip("Insert left controller here.")]
    public ControllerEvents LcontrollerEvents;
    [Tooltip("Insert right controller here.")]
    public ControllerEvents RcontrollerEvents;

    [Header("Movement Settings", order = 2)]
    [Tooltip("Height threshold before movement is detected.")]
    [Range(1.0f, 5.0f)]
    public int headThreshold = 1;
    [Tooltip("Player movement Speed.")]
    [Range(1.0f, 20.0f)]
    public float movementSpeed = 1.0f;

    private Vector3 walkingVector = Vector3.zero;
    private float VRHeadThreshold;
    private float VRmovementSpeed;
    private float playerHeight;
    private bool walkingSwitch;

    void Start ()
    {
        if (cameraRig == null)
        {
            cameraRig = GameObject.Find("[CameraRig]");
        }

        if (VRHeadset == null)
        {
            VRHeadset = GameObject.Find("Camera (eye)").transform;
        }

        if (zeroTracker == null)
        {
            zeroTracker = new GameObject("Zero Tracker");
        }

        setTrackerObj(zeroTracker);
        walkingSwitch = false;
        setMovementInformation();
    }

    void FixedUpdate ()
    {
        jogFinder();
        move();
        walkingVector.z = 0;
    }

    void OnEnable()
    {
        LcontrollerEvents.TouchpadTouched += HandlerTouchPadPressed;
        LcontrollerEvents.TouchpadTouchReleased += HandleTouchpadReleased;
        RcontrollerEvents.TouchpadTouched += HandlerTouchPadPressed;
        RcontrollerEvents.TouchpadTouchReleased += HandleTouchpadReleased;
    }

    void OnDisable()
    {
        LcontrollerEvents.TouchpadTouched -= HandlerTouchPadPressed;
        LcontrollerEvents.TouchpadTouchReleased -= HandleTouchpadReleased;
        RcontrollerEvents.TouchpadTouched -= HandlerTouchPadPressed;
        RcontrollerEvents.TouchpadTouchReleased -= HandleTouchpadReleased;
    }

    public void HandlerTouchPadPressed(object sender, ControllerEvents.ControllerInteractionEventArgs e)
    {
        walkingSwitch = true;
    }

    private void HandleTouchpadReleased(object sender, ControllerEvents.ControllerInteractionEventArgs e)
    {
        walkingSwitch = false;
    }

    private void setTrackerObj(GameObject trackingObject)
    {
        trackingObject.transform.parent = cameraRig.transform;
        trackingObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void setToggleWalking()
    {
        walkingSwitch = !walkingSwitch;
    }

    public void setMovementInformation()
    {
        VRHeadThreshold = (float) headThreshold / 1000;
        VRmovementSpeed = movementSpeed * 10;
    }
    
    private void jogFinder()
    {
        if(VRHeadset.position.y - zeroTracker.transform.position.y >= playerHeight + VRHeadThreshold || 
           VRHeadset.position.y - zeroTracker.transform.position.y <= playerHeight - VRHeadThreshold)
        {
            walkingVector.z = 10f;
            playerHeight = VRHeadset.position.y - zeroTracker.transform.position.y;
        }
    }

    private void move()
    {
        if(walkingSwitch)
        {
            Vector3 FBMovement = VRHeadset.forward * walkingVector.z * VRmovementSpeed * Time.deltaTime;
            float Upheight = cameraRig.transform.position.y;
            cameraRig.transform.position += (FBMovement * Time.deltaTime);
            cameraRig.transform.position = new Vector3(cameraRig.transform.position.x,
                                                       Upheight,
                                                       cameraRig.transform.position.z);
        }
    }
}
