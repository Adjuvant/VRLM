using UnityEngine;

public class ArmMovement : MonoBehaviour
{
    [Header("Headset Information", order = 1)]
    [Tooltip("Optionally insert SteamVR '[CameraRig]' here or allow the script to auto find the '[CameraRig]''.")]
    public GameObject cameraRig;
    [Tooltip("Optionally insert SteamVR 'Camera (eye)' here or allow the script to auto find the 'Camera (eye)'.")]
    public Transform VRHeadset;
    [Tooltip("Insert left controller here.")]
    public Transform leftController;
    [Tooltip("Insert right controller here.")]
    public Transform rightController;
    [Tooltip("Insert left controller here.")]
    public ControllerEvents LcontrollerEvents;
    [Tooltip("Insert right controller here.")]
    public ControllerEvents RcontrollerEvents;

    [Header("Movement Settings", order = 2)]
    [Tooltip("Controller threshold before movement is detected.")]
    [Range(1, 5)]
    public int swingThreshold = 1;
    [Tooltip("Player movement Speed.")]
    [Range(1.0f, 10.0f)]
    public float movementSpeed = 1.0f;

    private Vector3 walkingVector = Vector3.zero;
    private float VRControllerThreshold;
    private float leftControllerHeight;
    private float rightControllerHeight;
    private float VRmovementSpeed;
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

        walkingSwitch = false;
        setMovementInformation();
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

    public void setMovementInformation()
    {
        VRControllerThreshold = (float) swingThreshold / 100;
        VRmovementSpeed = movementSpeed * 10;
    }

    private void armSwingFinder()
    {
        if (rightController.position.y >= rightControllerHeight + VRControllerThreshold || rightController.position.y <= rightControllerHeight - VRControllerThreshold ||
            leftController.position.y >= leftControllerHeight + VRControllerThreshold || leftController.position.y <= leftControllerHeight - VRControllerThreshold)
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
            float Upheight = cameraRig.transform.position.y;
            cameraRig.transform.position += (FBMovement * Time.deltaTime);
            cameraRig.transform.position = new Vector3(cameraRig.transform.position.x,
                                                       Upheight,
                                                       cameraRig.transform.position.z);
        }
    }
}
