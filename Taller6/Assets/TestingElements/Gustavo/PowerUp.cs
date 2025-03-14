using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private int explosionDelayBeats = 12;
    [SerializeField] private Color warningColor = Color.blue;
    [SerializeField] private Color explodeColor = Color.red;
    [SerializeField] private float scaleIncreasePerBeat = 0.1f;
    [SerializeField] private float fadeOutTime = 0.5f;

    [SerializeField] private GameObject explosionEffectPrefab; // NUEVO: Prefab de explosión

    private Renderer objectRenderer;
    private Vector3 initialScale; // NUEVO: Escala inicial como referencia
    private int beatCounter = 0;
    private bool hasExploded = false;
    private bool isFadingOut = false;
    private PowerUpSpawner powerUpSpawner;
    private PlayerScore playerScore;

    // NUEVO: Límites de etapas
    private int initialStageLimit;
    private int middleStageLimit;
    private int finalStageLimit;

    void OnEnable()
    {
        if (objectRenderer == null) objectRenderer = GetComponent<Renderer>();

        beatCounter = 0;
        hasExploded = false;
        isFadingOut = false;

        // NUEVO: Guardar escala inicial y calcular límites de etapas
        initialScale = transform.localScale;

        initialStageLimit = explosionDelayBeats / 3;
        middleStageLimit = initialStageLimit * 2;
        finalStageLimit = explosionDelayBeats - 1;

        transform.localScale = initialScale; // Empieza con su escala inicial
        objectRenderer.material.color = warningColor;

        BeatManager.OnBeat += OnBeat;
    }

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        powerUpSpawner = FindObjectOfType<PowerUpSpawner>();
        playerScore = FindObjectOfType<PlayerScore>();

        if (powerUpSpawner == null)
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
        while (true)
        {
            yield return new WaitForSeconds(1f);
            //playerScore.AddScore(1);
        }
    }

    void OnBeat()
    {
        if (hasExploded) return;

        beatCounter++;

        // Reducción de escala según la etapa
        if (beatCounter == initialStageLimit)
        {
            transform.localScale = initialScale - (Vector3.one * scaleIncreasePerBeat);
        }
        else if (beatCounter == middleStageLimit)
        {
            transform.localScale = initialScale - (Vector3.one * scaleIncreasePerBeat * 2);
        }

        if (beatCounter >= explosionDelayBeats)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // NUEVO: Instanciar la explosión visual
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        powerUpSpawner.ReturnToPool(gameObject); // Return to pool sin fade
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int scoreToAdd = 2; // Puntaje por defecto

            if (beatCounter <= initialStageLimit)
            {
                scoreToAdd = 6;
            }
            else if (beatCounter <= middleStageLimit)
            {
                scoreToAdd = 4;
            }

            playerScore.AddScore(scoreToAdd);
            Debug.Log($"+{scoreToAdd} puntos");

            Explode(); // Ahora usa Explode() en lugar del FadeOutAndReturn
        }
    }

    void OnDisable()
    {
        BeatManager.OnBeat -= OnBeat;
    }
}
