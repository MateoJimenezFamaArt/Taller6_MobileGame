using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;  // Assign the object to follow
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10); // Adjust for desired positioning
    [SerializeField] private float smoothSpeed = 5f; // Adjust for smooth movement

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate the desired position (relative to world space)
        Vector3 desiredPosition = target.position + offset;
        
        // Smoothly interpolate the camera's position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Always look at the target's position, but NOT its rotation
        transform.LookAt(target.position);
    }
}