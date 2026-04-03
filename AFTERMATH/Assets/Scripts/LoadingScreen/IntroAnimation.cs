using UnityEngine;
using DG.Tweening;
public class IntroAnimation : MonoBehaviour
{
    [SerializeField] Transform _camera;
    [SerializeField] Transform[] _doors;
    [SerializeField] float _moveDuration = 1.5f;
    [SerializeField] float _rotateDuration = 1.0f;
    [SerializeField] Ease _animationEase = Ease.OutQuad; 
    Transform currentdoor;

    void Awake() => currentdoor = _doors[PlayerPrefs.GetInt("currentLocation")];
    void Start() => PlayAnimations();

    void PlayAnimations()
    {
        Vector3 firstObjLocalPos = _camera.localPosition;
        _camera.localPosition = new Vector3(firstObjLocalPos.x, firstObjLocalPos.y, 10f);

        Vector3 secondObjLocalRot = currentdoor.localEulerAngles;
        currentdoor.localEulerAngles = new Vector3(secondObjLocalRot.x, -100f, secondObjLocalRot.z);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_camera.DOLocalMoveZ(0f, _moveDuration).SetEase(_animationEase));
        sequence.Append(currentdoor.DOLocalRotate(new Vector3(secondObjLocalRot.x, 0f, secondObjLocalRot.z), _rotateDuration).SetEase(_animationEase));
        sequence.SetLink(gameObject);
    }
}