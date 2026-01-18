using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDoorGrabDrive : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HingeJoint hinge;
    [SerializeField] private XRGrabInteractable handleGrab;

    [Header("Tuning")]
    [Tooltip("How strongly the door follows the target angle.")]
    [SerializeField] private float angularDrive = 25f;

    [Tooltip("Extra damping to reduce overshoot.")]
    [SerializeField] private float angularDamping = 8f;

    [Tooltip("Clamp angular velocity for stability.")]
    [SerializeField] private float maxAngularVelocity = 6f;

    private Rigidbody doorRb;

    // Cached hinge configuration
    private Vector3 hingeWorldAxis;
    private Vector3 hingeWorldAnchor;
    private float minLimit;
    private float maxLimit;

    // Grab state
    private bool isGrabbed;
    private Transform interactorTransform;

    // Offset captured on grab to avoid snapping
    private float grabAngleOffset;

    private void Awake()
    {
        if (!hinge) hinge = GetComponent<HingeJoint>();
        doorRb = GetComponent<Rigidbody>();

        if (!hinge || !doorRb)
        {
            Debug.LogError($"{name}: Missing HingeJoint or Rigidbody.");
            enabled = false;
            return;
        }

        // Cache limits in degrees (HingeJoint limits are relative to joint's reference)
        var limits = hinge.limits;
        minLimit = limits.min;
        maxLimit = limits.max;

        // Subscribe to grab events
        if (!handleGrab)
        {
            Debug.LogError($"{name}: handleGrab not assigned.");
            enabled = false;
            return;
        }

        handleGrab.selectEntered.AddListener(OnGrab);
        handleGrab.selectExited.AddListener(OnRelease);
    }

    private void FixedUpdate()
    {
        // Update hinge world data each frame (in case door is part of moving hierarchy)
        hingeWorldAxis = transform.TransformDirection(hinge.axis).normalized;
        hingeWorldAnchor = hinge.transform.TransformPoint(hinge.anchor);

        if (!isGrabbed || interactorTransform == null)
            return;

        // Compute desired angle from hand position around hinge axis
        float desiredAngle = ComputeTargetAngleFromHand(interactorTransform.position);

        // Apply offset so we don't snap at grab time
        desiredAngle += grabAngleOffset;

        // Clamp to hinge limits
        desiredAngle = Mathf.Clamp(desiredAngle, minLimit, maxLimit);

        // Drive door towards desired angle (PD controller)
        float currentAngle = hinge.angle; // degrees
        float error = Mathf.DeltaAngle(currentAngle, desiredAngle);

        // Convert error to angular velocity target
        float targetAngVel = (error * angularDrive) - (doorRb.angularVelocity.magnitude * angularDamping);
        targetAngVel = Mathf.Clamp(targetAngVel, -maxAngularVelocity, maxAngularVelocity);

        // Apply torque around hinge axis
        Vector3 torque = hingeWorldAxis * targetAngVel * doorRb.inertiaTensor.magnitude;
        doorRb.AddTorque(torque, ForceMode.Acceleration);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        interactorTransform = args.interactorObject.transform;

        // Capture offset so the door doesn't snap to a computed angle immediately
        float target = ComputeTargetAngleFromHand(interactorTransform.position);
        grabAngleOffset = hinge.angle - target;

        // Stabilize
        doorRb.maxAngularVelocity = Mathf.Max(doorRb.maxAngularVelocity, maxAngularVelocity);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        interactorTransform = null;
    }

    private float ComputeTargetAngleFromHand(Vector3 handWorldPos)
    {
        // Vector from hinge anchor to hand projected onto plane perpendicular to hinge axis
        Vector3 toHand = handWorldPos - hingeWorldAnchor;
        Vector3 planar = Vector3.ProjectOnPlane(toHand, hingeWorldAxis);

        if (planar.sqrMagnitude < 0.0001f)
            return hinge.angle;

        // Reference direction = door's "closed" forward projected onto plane
        // Choose a stable reference: door pivot's forward projected
        Vector3 refDir = Vector3.ProjectOnPlane(transform.forward, hingeWorldAxis).normalized;
        Vector3 handDir = planar.normalized;

        // Signed angle around hinge axis
        float angle = Vector3.SignedAngle(refDir, handDir, hingeWorldAxis);

        // This gives a relative angle; hinge.angle is also relative but may have different zero depending on setup.
        // That’s why we capture grabAngleOffset on grab.
        return angle;
    }

    private void OnDestroy()
    {
        if (handleGrab)
        {
            handleGrab.selectEntered.RemoveListener(OnGrab);
            handleGrab.selectExited.RemoveListener(OnRelease);
        }
    }
}
