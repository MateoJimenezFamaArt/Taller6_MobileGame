using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 100, 0); // Set speed in degrees per second

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
