using UnityEngine;
using DG.Tweening;

public class UIGearRotator : MonoBehaviour
{
    [SerializeField] float _duration;
    [SerializeField] RotateMode _rotateMode = RotateMode.FastBeyond360;
    [SerializeField] Ease _easeType = Ease.Linear;

    RectTransform _rectTransform;
    Tween _rotationTween;

    void Awake() =>  _rectTransform = GetComponent<RectTransform>();

    void Start() => StartSpinning();

    void StartSpinning()
    {
        _rotationTween?.Kill();
        _rotationTween = _rectTransform.DORotate(new Vector3(0, 0, -360), _duration, _rotateMode)
            .SetEase(_easeType)
            .SetLoops(-1, LoopType.Incremental)
            .SetRelative()
            .SetUpdate(true);
    }

    void OnEnable()
    {
        if (_rotationTween == null || !_rotationTween.IsActive())  StartSpinning();
        else _rotationTween.Play();
    }

    void OnDisable() => _rotationTween?.Pause();
    void OnDestroy() =>  _rotationTween?.Kill();
}
