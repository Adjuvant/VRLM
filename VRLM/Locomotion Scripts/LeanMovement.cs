using UnityEngine;

public class LeanMovement : MonoBehaviour
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
    [Tooltip("Threshold before movement is detected.")]
    [Range(1, 5)]
    public int movementThreshold = 2;
    [Tooltip("Player movement Speed.")]
    [Range(0.1f, 10.0f)]
    public float movementSpeed = 1.0f;

    private Vector3 walkingVector = Vector3.zero;
    private float VRHeadThreshold;
    private float VRmovementSpeed;
    private bool walkingSwitch;

    void Start()
    {
        if(cameraRig == null)
        {
            cameraRig = GameObject.Find("[CameraRig]");
        }

        if(VRHeadset == null)
        {
            VRHeadset = GameObject.Find("Camera (eye)").transform;
        }

        if (zeroTracker == null)
        {
            zeroTracker = new GameObject("Zero Tracker");
        }

        setTrackerObj(zeroTracker);
        setMovementInformation();
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

    public void setMovementInformation()
    {
        VRHeadThreshold = (float) movementThreshold / 10;
        VRmovementSpeed = movementSpeed * 10;
    }

    private void setTrackerObj(GameObject trackingObject)
    {
        trackingObject.transform.parent = cameraRig.transform;
        trackingObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    private Vector3 positionFinder()
    {
        float z = VRHeadset.transform.position.z - zeroTracker.transform.position.z;
        float x = VRHeadset.transform.position.x - zeroTracker.transform.position.x;

        walkingVector.z = Mathf.Clamp((float)System.Math.Round(z, 1), -1, 1);
        walkingVector.x = Mathf.Clamp((float)System.Math.Round(x, 1), -1, 1);

        return walkingVector;
    }

    private void setThreshold()
    {
        if(walkingVector.z <= VRHeadThreshold && walkingVector.z >= -VRHeadThreshold)
        {
            walkingVector.z = 0.0f;
        }

        if(walkingVector.x <= VRHeadThreshold && walkingVector.x >= -VRHeadThreshold)
        {
            walkingVector.x = 0.0f;
        }
    }

    private void move()
    {
        if (walkingSwitch)
        {
            Vector3 FBMovement = cameraRig.transform.forward * walkingVector.z * VRmovementSpeed * Time.deltaTime;
            Vector3 LRstrafe = cameraRig.transform.right * walkingVector.x * VRmovementSpeed * Time.deltaTime;
            float Upheight = cameraRig.transform.position.y;
            cameraRig.transform.position += (FBMovement + LRstrafe);
            cameraRig.transform.position = new Vector3(cameraRig.transform.position.x,
                                                       Upheight,
                                                       cameraRig.transform.position.z);
        }
    }
}
