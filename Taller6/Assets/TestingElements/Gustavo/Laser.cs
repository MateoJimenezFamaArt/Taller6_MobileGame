using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Laser : MonoBehaviour
{
    public float scaleIncreasePerBeat = 0.3f; // Cuánto crece el láser por beat
    public int explosionDelayBeats = 3; // Retardo en beats antes de hacer algo
    public float fadeOutTime = 1f; // Tiempo antes de regresar a la piscina

    private LineRenderer objectRenderer;
    private Vector3 initialScale;
    private int beatCounter = 0;
    private bool hasExploded = false;
    private LaserSpawner laserSpawner; // Referencia al LaserSpawner para retornar a la piscina
    private BoxCollider boxCollider; // Referencia al BoxCollider del láser

    void OnEnable()
    {
        if (objectRenderer == null) objectRenderer = GetComponent<LineRenderer>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider>();

        // Reset values when reused from pool
        beatCounter = 0;
        hasExploded = false;
        transform.localScale = initialScale;

        // Disable BoxCollider initially
        boxCollider.enabled = false;

        SingletonBeatManager.Instance.OnBeat += OnBeat;
    }

    void Start()
    {
        objectRenderer = GetComponent<LineRenderer>();
        initialScale = transform.localScale;
        objectRenderer.startWidth = 0.1f;
        objectRenderer.endWidth = 0.1f;

        // Find LaserSpawner to return to pool
        laserSpawner = FindObjectOfType<LaserSpawner>();
        if (laserSpawner == null)
        {
            Debug.LogError("Laser: No LaserSpawner found in scene!");
            return;
        }
    }

    void OnBeat()
    {
        if (hasExploded) return;

        beatCounter++;

        objectRenderer.startWidth = objectRenderer.startWidth + scaleIncreasePerBeat;
        objectRenderer.endWidth = objectRenderer.endWidth + scaleIncreasePerBeat;

        if (beatCounter >= explosionDelayBeats)
        {
            StartCoroutine(ActivateLaser());
        }
    }

    IEnumerator ActivateLaser()
    {
        if (hasExploded) yield break;

        hasExploded = true;

        // Activa el BoxCollider
        boxCollider.enabled = true;

        // Espera 3 segundos antes de continuar
        yield return new WaitForSeconds(3f);

        // Llamada a la corutina FadeOutAndReturn después de 3 segundos
        StartCoroutine(FadeOutAndReturn());
    }

    IEnumerator FadeOutAndReturn()
    {
        float elapsedTime = 0;
        Vector3 startScale = transform.localScale;

        while (elapsedTime < fadeOutTime)
        {
            float t = elapsedTime / fadeOutTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = initialScale; // Reset scale
        laserSpawner.ReturnToLaserPool(gameObject); // Return to object pool
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SingletonBeatManager.Instance.OnBeat -= OnBeat;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("has sido contagiado con sida");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}