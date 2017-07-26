using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanMovement : MonoBehaviour {

    public GameObject CameraRig;
    public GameObject CentreTracker;
    public Transform VRHeadset;
    public Vector3 walkingVector = Vector3.zero;
    public ControllerEvents LcontrollerEvents;
    public ControllerEvents RcontrollerEvents;
    [Range(0.1f, 0.5f)]
    public float movementThreshold = 0.2f;
    [Range(0.1f, 10.0f)]
    public float movementSpeed = 1.0f;
    private bool walkingSwitch;
    private float VRmovementSpeed;



    void Start()
    {
        if(CameraRig == null)
        {
            CameraRig = GameObject.Find("[CameraRig]");
        }

        if(CentreTracker == null)
        {
            CentreTracker = new GameObject();
            CentreTracker.name = "CentreTracker";
            CentreTracker.transform.parent = CameraRig.transform;
            CentreTracker.transform.localPosition = new Vector3(0, 0, 0);
        }

        if(VRHeadset == null)
        {
            VRHeadset = GameObject.Find("Camera (eye)").transform;
        }

        calculateMovementSpeed();
    }

    void FixedUpdate()
    {
        positionFinder();
        setThreshold();
        move();
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

    private Vector3 positionFinder()
    {
        float z = VRHeadset.transform.position.z - CentreTracker.transform.position.z;
        float x = VRHeadset.transform.position.x - CentreTracker.transform.position.x;

        walkingVector.z = Mathf.Clamp((float)System.Math.Round(z, 1), -1, 1);
        walkingVector.x = Mathf.Clamp((float)System.Math.Round(x, 1), -1, 1);

        return walkingVector;
    }

    private void setThreshold()
    {
        if(walkingVector.z <= movementThreshold && walkingVector.z >= -movementThreshold)
        {
            walkingVector.z = 0.0f;
        }

        if(walkingVector.x <= movementThreshold && walkingVector.x >= -movementThreshold)
        {
            walkingVector.x = 0.0f;
        }
    }

    private void move()
    {
        if (walkingSwitch)
        {
            Vector3 FBMovement = CameraRig.transform.forward * walkingVector.z * VRmovementSpeed * Time.deltaTime;
            Vector3 LRstrafe = CameraRig.transform.right * walkingVector.x * VRmovementSpeed * Time.deltaTime;
            float Upheight = CameraRig.transform.position.y;
            CameraRig.transform.position += (FBMovement + LRstrafe);
            CameraRig.transform.position = new Vector3(CameraRig.transform.position.x,
                                                       Upheight,
                                                       CameraRig.transform.position.z);
        }
    }
}
