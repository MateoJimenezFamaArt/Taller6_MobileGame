using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;       // Velocidad del movimiento
    [SerializeField] private float amplitude = 0.5f; // Qué tanto sube y baja

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Guarda la posición inicial
    }

    void Update()
    {
        // Movimiento senoidal para hacer que suba y baje suavemente
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
