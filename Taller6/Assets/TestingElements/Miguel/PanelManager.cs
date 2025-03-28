using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    InputManager inputManager;

    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject panelVictoria;
    [SerializeField] GameObject panelDerrota;
    [SerializeField] float posicionFinalPaneles;
    [SerializeField] float tiempoBotonActivo = 0.5f;
    [SerializeField] GameObject botonPausa;
    [SerializeField] GameObject panelPausa;
    [SerializeField] float tiempoconteo = .3f;
    [SerializeField] TextMeshProUGUI conteoDespausar;
    [SerializeField] Image barraProgreso;

    bool isPaused;
    AudioClip cancion;

    private void Awake()
    {
        isPaused = false;
        inputManager = InputManager.Instanciar;
        cancion = audioSource.clip;
    }

    private void OnEnable()
    {
        inputManager.EnTap += Tap;
    }

    private void OnDisable()
    {
        inputManager.EnTap -= Tap;
    }

    private void Update()
    {
        if(!isPaused)
        {
            if (barraProgreso.fillAmount > 0.95f) GanarNivel();
            ActualizarProgreso();
        }
    }

    private void ActualizarProgreso()
    {
        float tiempoMax = cancion.length;
        float tiempoActual = audioSource.time;

        float progreso = tiempoActual / tiempoMax;
        barraProgreso.fillAmount = progreso;
    }

    private void GanarNivel()
    {
        panelVictoria.SetActive(true);
        RectTransform rectTrans = panelVictoria.GetComponent<RectTransform>();
        Image image = panelVictoria.GetComponent<Image>();

        rectTrans.DOAnchorPos(new Vector2(0, 0), 1f, true);
        image.DOFade(1f, .5f);
    }

    public void PerderNivel()
    {
        panelDerrota.SetActive(true);
        RectTransform rectTrans = panelDerrota.GetComponent<RectTransform>();

        rectTrans.DOAnchorPos(new Vector2(0, 0), 1f, true);
    }

    public void Tap()
    {
        botonPausa.SetActive(true);

        StartCoroutine(IDesaparecerBoton(tiempoBotonActivo));
    }

    public void Pausar()
    {
        
        isPaused = true;
        audioSource.Pause();
        panelPausa.SetActive(true);
        RectTransform rectTrans = panelPausa.GetComponent<RectTransform>();

        rectTrans.DOAnchorPos(new Vector2(0, 0), 1f, true).onComplete = () => Time.timeScale = 0;
    }

    public void Despausar()
    {
        Time.timeScale = 1;
        RectTransform rectTrans = panelPausa.GetComponent<RectTransform>();
        rectTrans.DOAnchorPos(new Vector2(0, posicionFinalPaneles), 1f, true);
        conteoDespausar.gameObject.SetActive(true);

        StartCoroutine(IDespauasar(tiempoconteo));
    }

    public void Continuar()
    {
        Debug.Log(SceneManager.GetSceneByName("Prueba UI").buildIndex);
        SceneManager.LoadScene(0); //Index de la escena de UI
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator IDesaparecerBoton(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        botonPausa.SetActive(false);
    }

    IEnumerator IDespauasar(float tiempo)
    {
        RectTransform numeroConteo = conteoDespausar.gameObject.GetComponent<RectTransform>();

        for (int i = 2; i > 0; i--)
        {
            conteoDespausar.DOFade(0, tiempo);
            yield return new WaitForSeconds(tiempo);
            conteoDespausar.text = i.ToString();
            conteoDespausar.DOFade(1, 0f);
        }
        conteoDespausar.DOFade(0, tiempo);
        yield return new WaitForSeconds(tiempo);
        conteoDespausar.gameObject.SetActive(false);

        isPaused = false;
        audioSource.UnPause();
        panelPausa.SetActive(false);
    }
}
