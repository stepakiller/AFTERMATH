using UnityEngine;
using DG.Tweening;

public class GearRotator : MonoBehaviour
{
    [SerializeField] float _duration = 2f;
    [SerializeField] RotateMode _rotateMode = RotateMode.FastBeyond360;

    void Start() => Spin();

    void Spin() =>  transform.DORotate(new Vector3(0, 0, 360), _duration, _rotateMode).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetRelative();

    void OnDestroy() => transform.DOKill();
}      
