using UnityEngine;

[ExecuteAlways]
public class HumanBodyPart : MonoBehaviour
{
    public Transform boneStart;   // e.g. shoulder
    public Transform boneEnd;     // e.g. elbow — leave null for point parts like the head
    public Camera targetCamera;
    public bool lockUpright = true; // Y-axis-only billboard vs full spherical

    void Reset() => targetCamera = Camera.main;

    void LateUpdate()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        if (targetCamera == null || boneStart == null) return;

        // Position at the bone (or midpoint, for a limb segment)
        transform.position = boneEnd != null
            ? (boneStart.position + boneEnd.position) * 0.5f
            : boneStart.position;

        // Face the camera
        Vector3 toCamera = targetCamera.transform.position - transform.position;
        if (lockUpright) toCamera.y = 0f;
        if (toCamera.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(-toCamera.normalized, Vector3.up);

        // Roll around the facing axis to match the limb's on-screen angle
        if (boneEnd != null)
        {
            Vector3 limbDir = boneEnd.position - boneStart.position;
            Vector3 projected = Vector3.ProjectOnPlane(limbDir, transform.forward);
            if (projected.sqrMagnitude > 0.0001f)
                transform.rotation = Quaternion.LookRotation(transform.forward, projected.normalized);
        }
    }
}
