using UnityEngine;

[ExecuteAlways]
public class HumanBodyPart : MonoBehaviour
{
    public Transform boneStart;   // e.g. shoulder
    public Transform boneEnd;     // e.g. elbow — leave null for point parts like the head
    public Camera targetCamera;

    void Awake() => targetCamera = Camera.main;

    void Update()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        if (targetCamera == null || boneStart == null) return;

        // Position at the bone (or midpoint, for a limb segment)
        transform.position = boneEnd != null
            ? (boneStart.position + boneEnd.position) * 0.5f
            : boneStart.position;
    }
}
