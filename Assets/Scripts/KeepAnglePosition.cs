using UnityEngine;

public class KeepAnglePosition : MonoBehaviour
{
    private Vector3 startRotation;

    [SerializeField] private Quaternion oldRotation;

    [SerializeField] private float toleranceAngle = 80;

    [SerializeField] private float eps = 10.0f;

    [SerializeField] private Axis keepAxisAngle = Axis.X;

    private float rotationFromStart;
    
    public enum Axis
    {
        X,
        Y,
        Z
    }
    
    
    private void Start()
    {
        startRotation = keepAxisAngle switch
        {
            Axis.X => transform.forward,
            Axis.Y => transform.forward,
            Axis.Z => transform.up,
            _ => startRotation
        };

        oldRotation = transform.localRotation;
        
        rotationFromStart = keepAxisAngle switch
        {
            Axis.X => transform.localEulerAngles.x,
            Axis.Y => transform.localEulerAngles.y,
            Axis.Z => transform.localEulerAngles.z,
            _ => rotationFromStart
        };
    }

    private void LateUpdate()
    {
        if (Angle() > toleranceAngle) transform.localRotation = oldRotation;
        else oldRotation = transform.localRotation;

        Vector3 locRot = transform.localEulerAngles;
        transform.localEulerAngles = keepAxisAngle switch
        {
            Axis.X => new Vector3(Mathf.Clamp(locRot.x, rotationFromStart - eps, rotationFromStart + eps), locRot.y,
                locRot.z),
            Axis.Y => new Vector3(locRot.x, Mathf.Clamp(locRot.y, rotationFromStart - eps, rotationFromStart + eps),
                locRot.z),
            Axis.Z => new Vector3(locRot.x, locRot.y,
                Mathf.Clamp(locRot.z, rotationFromStart - eps, rotationFromStart + eps)),
            _ => transform.localEulerAngles
        };
    }

    private float Angle()
    {
        return Vector3.Angle(transform.forward, startRotation);
    }
}
