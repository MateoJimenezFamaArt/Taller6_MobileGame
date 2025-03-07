using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    public float explosionRadius = 3f; // Radius of the explosion
    public int explosionDelayBeats = 3; // Explodes on the third beat
    public Color warningColor = Color.blue; // Warning color before explosion
    public Color explodeColor = Color.red; // Color at explosion
    public float scaleIncreasePerBeat = 0.5f; // How much the object grows each beat
    public float fadeOutTime = 0.5f; // Time before returning to pool

    private Renderer objectRenderer;
    private Vector3 initialScale;
    private int beatCounter = 0;
    private bool hasExploded = false;
    private bool isFadingOut = false; // Nueva variable para evitar múltiples corutinas
    private ObjectSpawner objectSpawner; // Reference to return to pool
    private PlayerScore playerScore;

    void OnEnable()
    {
        if (objectRenderer == null) objectRenderer = GetComponent<Renderer>();

        // Reset values when reused from pool
        beatCounter = 0;
        hasExploded = false;
        isFadingOut = false; // Reiniciar flag cuando se reactive el objeto
        transform.localScale = initialScale;
        objectRenderer.material.color = warningColor; // Start as warning color

        // Subscribe to BeatManager events
        BeatManager.OnBeat += OnBeat;
    }

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        initialScale = transform.localScale;

        // Find ObjectSpawner to return to pool
        objectSpawner = FindObjectOfType<ObjectSpawner>();
        playerScore = FindObjectOfType<PlayerScore>();
        
        if (objectSpawner == null)
        {
            Debug.LogError("BeatExploder: No ObjectSpawner found in scene!");
            return;
        }
        if (playerScore == null)
        {
            Debug.LogError("PlayerScore: No PlayerScore found in scene!");
            return;
        }
    

    StartCoroutine(ScoreOverTime());
}

IEnumerator ScoreOverTime()
{
    while (true) // Se repite indefinidamente
    {
        yield return new WaitForSeconds(1f);
        playerScore.AddScore(1);
    }
}

    void OnBeat()
    {
        if (hasExploded) return; // Evita múltiples explosiones

        beatCounter++;
        transform.localScale += Vector3.one * scaleIncreasePerBeat; // Increase size

        if (beatCounter >= explosionDelayBeats)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded) return; // Verificación adicional por seguridad
        hasExploded = true; // Marcar como explotado para evitar múltiples activaciones

        // Fade out & return to pool
        StartCoroutine(FadeOutAndReturn());
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("+score");
            playerScore.AddScore(2);
            StartCoroutine(FadeOutAndReturn()); // Ahora está protegido contra múltiples ejecuciones
        }
    }

    IEnumerator FadeOutAndReturn()
    {
        if (isFadingOut) yield break; // Evita múltiples activaciones de la corutina
        isFadingOut = true;

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
        objectSpawner.ReturnToPool(gameObject); // Return to object pool
        isFadingOut = false; // Reset flag para futuras activaciones
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        BeatManager.OnBeat -= OnBeat;
    }

    void OnDrawGizmos()
    {
        // Draw explosion radius in Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
