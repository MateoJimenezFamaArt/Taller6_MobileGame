using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BeatExploder : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticles;
    private ParticleSystem explosionParticlesInstance;

    public float explosionRadius = 3f; // Radius of the explosion
    public int explosionDelayBeats = 3; // Explodes on the third beat
    public Color warningColor = Color.yellow; // Warning color before explosion
    public Color explodeColor = Color.red; // Color at explosion
    public float scaleIncreasePerBeat = 0.5f; // How much the object grows each beat
    public float fadeOutTime = 0.5f; // Time before returning to pool

    private Renderer objectRenderer;
    private Vector3 initialScale;
    private int beatCounter = 0;
    private bool hasExploded = false;
    private ObjectSpawner objectSpawner; // Reference to return to pool
    private PanelManager panelManager;

    void OnEnable()
    {
        if (objectRenderer == null) objectRenderer = GetComponent<Renderer>();

        // Reset values when reused from pool
        beatCounter = 0;
        hasExploded = false;
        transform.localScale = initialScale;
        objectRenderer.material.color = warningColor; // Start as warning color

        if (explosionParticlesInstance == null)
        {
            explosionParticlesInstance = Instantiate(explosionParticles, transform.position, Quaternion.identity, transform);
        }
        explosionParticlesInstance.Stop();

        // Subscribe to BeatManager events
        SingletonBeatManager.Instance.OnBeat += OnBeat;
    }

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        initialScale = transform.localScale;

        // Find ObjectSpawner to return to pool
        objectSpawner = FindObjectOfType<ObjectSpawner>();
        if (objectSpawner == null)
        {
            Debug.LogError("BeatExploder: No ObjectSpawner found in scene!");
            return;
        }
        panelManager = FindFirstObjectByType<PanelManager>();
    }

    void OnBeat()
    {
        if (hasExploded) return;

        beatCounter++;
        transform.localScale += Vector3.one * scaleIncreasePerBeat; // Increase size

        if (beatCounter >= explosionDelayBeats)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;
        objectRenderer.material.color = explodeColor;

        if (explosionParticlesInstance != null)
        {
            explosionParticlesInstance.transform.position = transform.position;
            explosionParticlesInstance.Play();
        }
        else
        {
            Debug.LogWarning("destrui las particulas");
            explosionParticlesInstance = Instantiate(explosionParticles, transform.position, Quaternion.identity, transform);
            explosionParticlesInstance.Play();
        }

        // Check if player is within explosion range
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= explosionRadius)
            {
                Debug.Log("Player hit by explosion! Restarting scene...");
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

                //Panel de derrota
                panelManager.PerderNivel();
            }
        }

        // Fade out & return to pool
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
        if (explosionParticlesInstance != null)
        {
            explosionParticlesInstance.Stop();
            explosionParticlesInstance.Clear();
        }
        objectSpawner.ReturnToPool(gameObject); // Return to object pool
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SingletonBeatManager.Instance.OnBeat -= OnBeat;
    }

    void OnDrawGizmos()
    {
        // Draw explosion radius in Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
