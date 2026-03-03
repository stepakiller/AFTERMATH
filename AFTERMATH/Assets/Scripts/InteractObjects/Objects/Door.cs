using UnityEngine;
using DG.Tweening;
public class Door : MonoBehaviour, Interactable
{
    [SerializeField] float openAngle = 90f;
    [SerializeField] float duration = 0.8f;
    [SerializeField] Ease moveEase = Ease.OutQuad;

    bool isOpen = false;
    Vector3 defaultRotation;
    Transform doorTransform;

    void Start()
    {
        doorTransform = transform;
        defaultRotation = doorTransform.localEulerAngles;
    }

    public void Interact()
    {
        if (DOTween.IsTweening(doorTransform)) return;

        if (!isOpen)OpenDoor();
        else CloseDoor();
    }

    void OpenDoor()
    {
        Transform player = Bootstrapper.PlayerTransform;
        Vector3 directionToPlayer = player.position - doorTransform.position;
        directionToPlayer.y = 0; 
        float dot = Vector3.Dot(doorTransform.forward, directionToPlayer.normalized);
        float targetAngleY = (dot < 0) ? -openAngle : openAngle;
        Vector3 targetRotation = new Vector3(defaultRotation.x, defaultRotation.y + targetAngleY, defaultRotation.z);

        doorTransform.DOLocalRotate(targetRotation, duration).SetEase(moveEase);
        isOpen = true;
    }

    void CloseDoor()
    {
        doorTransform.DOLocalRotate(defaultRotation, duration).SetEase(moveEase);
        isOpen = false;
    }
}
