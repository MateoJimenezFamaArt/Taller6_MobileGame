using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Laser : MonoBehaviour
{
    public float scaleIncreasePerBeat = 0.3f; // Cuánto crece el láser por beat
    public int explosionDelayBeats = 3; // Retardo en beats antes de hacer algo
    public Color warningColor = Color.blue; // Color de advertencia antes de la expansión
    public Color activeColor = Color.red; // Color cuando se activa
    public float fadeOutTime = 1f; // Tiempo antes de regresar a la piscina

    private Renderer objectRenderer;
    private Vector3 initialScale;
    private int beatCounter = 0;
    private bool hasExploded = false;
    private LaserSpawner laserSpawner; // Referencia al LaserSpawner para retornar a la piscina
    private BoxCollider boxCollider; // Referencia al BoxCollider del láser

    void OnEnable()
    {
        if (objectRenderer == null) objectRenderer = GetComponent<Renderer>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider>();

        // Reset values when reused from pool
        beatCounter = 0;
        hasExploded = false;
        transform.localScale = initialScale;
        objectRenderer.material.color = warningColor; // Start as warning color

        // Disable BoxCollider initially
        boxCollider.enabled = false;

        BeatManager.OnBeat += OnBeat;
    }

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        initialScale = transform.localScale;

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
        Debug.Log("xd");
        transform.localScale += new Vector3(scaleIncreasePerBeat, scaleIncreasePerBeat, 0);

        if (beatCounter >= explosionDelayBeats)
        {
            StartCoroutine(ActivateLaser());
        }
    }

    IEnumerator ActivateLaser()
    {
        if (hasExploded) yield break;

        hasExploded = true;
        objectRenderer.material.color = activeColor;

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
        Color startColor = objectRenderer.material.color;
        Vector3 startScale = transform.localScale;

        while (elapsedTime < fadeOutTime)
        {
            float t = elapsedTime / fadeOutTime;
            objectRenderer.material.color = Color.Lerp(startColor, Color.clear, t);
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
        BeatManager.OnBeat -= OnBeat;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("has sido contagiado con sida");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}