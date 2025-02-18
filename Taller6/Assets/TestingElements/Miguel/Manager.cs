using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] AudioSource cancionNivel;
    [SerializeField] GameObject panelVictoria;

    bool isPaused;

    private void Awake()
    {
        isPaused = false;
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
        RectTransform rectTransVictoria = panelVictoria.GetComponent<RectTransform>();
        Image imageVictoria = panelVictoria.GetComponent<Image>();

        rectTransVictoria.DOAnchorPos(new Vector2(0, 0), 1f, true);
        imageVictoria.DOFade(1f, .5f);
    }
}
