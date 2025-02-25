using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class PanelManager : MonoBehaviour
{
    InputManager inputManager;

    [SerializeField] AudioSource cancionNivel;
    [SerializeField] GameObject panelVictoria;
    [SerializeField] GameObject panelDerrota;
    [SerializeField] float tiempoBotonActivo = 0.5f;
    [SerializeField] GameObject botonPausa;
    [SerializeField] GameObject panelPausa;
    [SerializeField] float tiempoconteo = .3f;
    [SerializeField] TextMeshProUGUI conteoDespausar;

    bool isPaused;

    private void Awake()
    {
        isPaused = false;
        inputManager = InputManager.Instanciar;
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
        if(!cancionNivel.isPlaying && !isPaused)
        {
            panelVictoria.SetActive(true);
            AcabarNivel();
        }
    }

    private void AcabarNivel()
    {
        RectTransform rectTrans = panelVictoria.GetComponent<RectTransform>();
        Image image = panelVictoria.GetComponent<Image>();

        rectTrans.DOAnchorPos(new Vector2(0, 0), 1f, true);
        image.DOFade(1f, .5f);
    }

    public void Tap()
    {
        botonPausa.SetActive(true);

        StartCoroutine(IDesaparecerBoton(tiempoBotonActivo));
    }

    public void Pausar()
    {
        isPaused = true;
        cancionNivel.Pause();
        panelPausa.SetActive(true);
        RectTransform rectTrans = panelPausa.GetComponent<RectTransform>();

        rectTrans.DOAnchorPos(new Vector2(0, 0), 1f, true);
    }

    public void Despausar()
    {
        RectTransform rectTrans = panelPausa.GetComponent<RectTransform>();
        rectTrans.DOAnchorPos(new Vector2(0, 360), 1f, true);
        conteoDespausar.gameObject.SetActive(true);

        StartCoroutine(IDespauasar(tiempoconteo));
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
        cancionNivel.UnPause();
    }
}
