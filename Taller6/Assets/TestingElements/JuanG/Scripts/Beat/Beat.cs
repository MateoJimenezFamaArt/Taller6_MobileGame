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

    public void Active()
    {
        _image.DOFade(1, 0).SetEase(Ease.Linear).OnComplete(() => Deactive());
    }

    public void Deactive()
    {
        DOTween.Sequence()
            .Join(_transform.DOAnchorPosX(0, SingletonBeatManager.Instance.GetBeatInterval()).SetEase(Ease.InOutSine))
            .Join(_image.DOFade(0, SingletonBeatManager.Instance.GetBeatInterval()).SetEase(Ease.Linear))
            .OnComplete(() => _beatPool.Release(this));
    }

}
