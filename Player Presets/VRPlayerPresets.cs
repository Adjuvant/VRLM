using UnityEngine;

public class VRPlayerPresets : MonoBehaviour
{
    [Header("Headset Information", order = 1)]
    [Tooltip("Optionally insert SteamVR 'Camera (eye)' here or allow the script to auto find the 'Camera (eye)'.")]
    public Transform VRHeadset; //Use the VRHeadSet's eye

    private CapsuleCollider PlayerCollider;
    private float headsetYOffset = 0.2f;
    private Rigidbody VRrigidbody;
    private bool falling = false;

    void OnEnable()
    {
        createPresets();
    }

    void FixedUpdate()
    {
        updatePresets();
    }

    private void createPresets()
    {
        VRrigidbody = gameObject.GetComponent<Rigidbody>();
        if (VRrigidbody == null)
        {
            VRrigidbody = gameObject.AddComponent<Rigidbody>();
            VRrigidbody.mass = 1;
            VRrigidbody.freezeRotation = true;
        }

        PlayerCollider = gameObject.GetComponent<CapsuleCollider>();
        if (PlayerCollider == null)
        {
            PlayerCollider = gameObject.AddComponent<CapsuleCollider>();
            PlayerCollider.center = new Vector3(0f, 1.0f, 0f);
            PlayerCollider.height = 1f;
            PlayerCollider.radius = 0.15f;
        }

        if (VRHeadset == null)
        {
            VRHeadset = GameObject.Find("Camera (eye)").transform;
        }
    }

    private void updatePresets()
    {
        var playAreaHeightAdjustment = 0.009f;
        var newColliderYSize = (VRHeadset.transform.localPosition.y - headsetYOffset);
        var newColliderYCenter = (newColliderYSize != 0 ? (newColliderYSize / 2) + playAreaHeightAdjustment : 0);
        
        if (PlayerCollider)
        {
            PlayerCollider.height = newColliderYSize;
            PlayerCollider.center = new Vector3(VRHeadset.localPosition.x, newColliderYCenter, VRHeadset.localPosition.z);
        }
    }
}