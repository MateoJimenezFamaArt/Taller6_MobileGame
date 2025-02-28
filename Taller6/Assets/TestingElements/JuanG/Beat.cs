using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Beat : MonoBehaviour
{
    [SerializeField] RectTransform _transform;
    [SerializeField] Image _image;

    void Start()
    {
        
        DOTween.Sequence()
            .Join(_transform.DOAnchorPosX(0, 0.5f).SetEase(Ease.InOutSine))
            .Join(_image.DOFade(1, 1).SetEase(Ease.Linear)).OnComplete(() => Destroy(gameObject));
    }

}
