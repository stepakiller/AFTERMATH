using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class ObjectMover : MonoBehaviour
{
    Transform _targetObject;
    [SerializeField] float _distance = 5f;
    [SerializeField] float _duration = 1.5f;
    [SerializeField] Ease _easeType = Ease.OutQuad;

    private Tween _currentTween;

    void Start() => _targetObject = transform;

    public void MoveObjectRelativeX()
    {
        if (_targetObject == null || !_targetObject.gameObject.activeSelf) return;
        _currentTween?.Kill();
        
        float endValue = _targetObject.localPosition.x + _distance;
        
        _currentTween = _targetObject.DOLocalMoveX(endValue, _duration)
            .SetEase(_easeType)
            .SetLink(_targetObject.gameObject);
    }
}