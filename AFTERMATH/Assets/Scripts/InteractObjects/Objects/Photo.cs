using UnityEngine;
using DG.Tweening;

public class Photo : MonoBehaviour, Equippable
{
    [SerializeField] Door _targetDoor;
    [SerializeField] float _inspectDistance = 0.4f; 
    [SerializeField] Vector3 _inspectRotation = new Vector3(0f, 180f, 0f);
    [SerializeField] float _transitionDuration = 0.6f;
    [SerializeField] Ease _easeType = Ease.OutCubic;

    bool _isAnimating = false;

    public void Equip()
    {
        if (InputManager.Instance != null) InputManager.Instance.OnInteractPressed += StartInspectSequence;
    }

    public void Unequip()
    {
        if (InputManager.Instance != null) InputManager.Instance.OnInteractPressed -= StartInspectSequence;
    }

    void StartInspectSequence()
    {
        if (_isAnimating) return;
        _isAnimating = true;
        Unequip();
        Transform mainCamera = Camera.main != null ? Camera.main.transform : null;
        if (mainCamera == null)
        {
            return;
        }

        transform.SetParent(mainCamera);
        transform.DOLocalMove(new Vector3(0f, 0f, _inspectDistance), _transitionDuration)
            .SetEase(_easeType);
        transform.DOLocalRotate(_inspectRotation, _transitionDuration)
            .SetEase(_easeType)
            .OnComplete(FinishInspectionAndOpenDoor);
    }

    void FinishInspectionAndOpenDoor()
    {
        if (_targetDoor != null) _targetDoor.OpenDoor();
        Destroy(gameObject);
    }

    void OnDestroy() => Unequip();
}