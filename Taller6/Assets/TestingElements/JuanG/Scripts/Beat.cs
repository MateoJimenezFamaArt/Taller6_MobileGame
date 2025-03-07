using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Pool;

public class Beat : MonoBehaviour
{
    [SerializeField] RectTransform _transform;
    [SerializeField] Image _image;

    private IObjectPool<Beat> _beatPool;

    public IObjectPool<Beat> BeatPool
    {
        set => _beatPool = value;
    }

    public void Deactive()
    {
        DOTween.Sequence()
            .Join(_transform.DOAnchorPosX(0, 1).SetEase(Ease.InOutSine))
            .Join(_image.DOFade(1, 1).SetEase(Ease.Linear))
            .OnComplete(() => _beatPool.Release(this));

    }

}
