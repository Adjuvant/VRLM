using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMovement : MonoBehaviour {

    public GameObject CameraRig;
    public Transform VRHeadset;
    public Vector3 walkingVector = Vector3.zero;
    public Transform leftController;
    public Transform rightController;
    public ControllerEvents LcontrollerEvents;
    public ControllerEvents RcontrollerEvents;
    public float swingThreshold = 0.01f;

    private float leftControllerHeight;
    private float rightControllerHeight;
    private float VRmovementSpeed;
    private bool walkingSwitch;

    [Range(1.0f, 10.0f)]
    public float movementSpeed = 1.0f;


    void Start ()
    {
        if (CameraRig == null)
        {
            CameraRig = GameObject.Find("[CameraRig]");
        }

        if (VRHeadset == null)
        {
            VRHeadset = GameObject.Find("Camera (eye)").transform;
        }

        walkingSwitch = false;
        calculateMovementSpeed();
    }

    void FixedUpdate ()
    {
        armSwingFinder();
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

    public void setToggleWalking()
    {
        walkingSwitch = !walkingSwitch;
    }

    private void calculateMovementSpeed()
    {
        VRmovementSpeed = movementSpeed * 10;
    }

    private void armSwingFinder()
    {
        if (rightController.position.y >= rightControllerHeight + swingThreshold || rightController.position.y <= rightControllerHeight - swingThreshold ||
            leftController.position.y >= leftControllerHeight + swingThreshold || leftController.position.y <= leftControllerHeight - swingThreshold)
        {
            walkingVector.z = 10f;
            rightControllerHeight = rightController.position.y;
            leftControllerHeight = leftController.position.y;
        }
    }

    private void move()
    {
        if(walkingSwitch)
        {
            Vector3 FBMovement = VRHeadset.forward * walkingVector.z * VRmovementSpeed * Time.deltaTime;
            float Upheight = CameraRig.transform.position.y;
            CameraRig.transform.position += (FBMovement * Time.deltaTime);
            CameraRig.transform.position = new Vector3(CameraRig.transform.position.x,
                                                       Upheight,
                                                       CameraRig.transform.position.z);
        }
    }
}
