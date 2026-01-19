using UnityEngine;

public class AnomalyController : MonoBehaviour
{
    public enum Mode
    {
        EnableGameObject,  // enable a hidden object
        TransformTarget    // rotate / move / scale an object
    }

    [Header("Setup")]
    public Mode mode = Mode.EnableGameObject;

    [Tooltip("Used for TransformTarget mode. Leave null to use this transform.")]
    public Transform target;

    [Header("TransformTarget - Active State (Local)")]
    public Vector3 activeLocalPosition;
    public Vector3 activeLocalEuler;
    public Vector3 activeLocalScale = Vector3.one;

    // captured defaults
    private bool defaultActive;
    private Transform t;
    private Vector3 defaultLocalPos;
    private Quaternion defaultLocalRot;
    private Vector3 defaultLocalScale;

    void Awake()
    {
        defaultActive = gameObject.activeSelf;

        t = (target != null) ? target : transform;
        defaultLocalPos = t.localPosition;
        defaultLocalRot = t.localRotation;
        defaultLocalScale = t.localScale;
    }

    public void ResetToDefault()
    {
        // reset transform target
        if (t != null)
        {
            t.localPosition = defaultLocalPos;
            t.localRotation = defaultLocalRot;
            t.localScale = defaultLocalScale;
        }

        // reset enable/disable anomaly object
        gameObject.SetActive(defaultActive);
    }

    public void Activate()
    {
        if (mode == Mode.EnableGameObject)
        {
            gameObject.SetActive(true);
            return;
        }

        // TransformTarget mode
        if (t == null) t = transform;
        t.localPosition = activeLocalPosition;
        t.localRotation = Quaternion.Euler(activeLocalEuler);
        t.localScale = activeLocalScale;
    }
}
