using UnityEngine;
using DG.Tweening;
public class Door : MonoBehaviour, Interactable
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;
    [SerializeField] float openAngle = 90f;
    [SerializeField] float duration = 0.8f;
    [SerializeField] Ease moveEase = Ease.OutQuad;
    [SerializeField] bool doorLock;
    [SerializeField] ObjectMover objectMover;
    [SerializeField] bool startOpen = false;
    bool isOpen = false;
    Vector3 defaultRotation;
    Transform doorTransform;
    bool objectIsMove = false;

    void Start()
    {
        doorTransform = transform;
        defaultRotation = doorTransform.localEulerAngles;
        if(startOpen) OpenDoor();
    }

    public void Interact()
    {
        if (DOTween.IsTweening(doorTransform)) return;

        if (!isOpen & !doorLock) OpenDoor();
        else if(isOpen & !doorLock) CloseDoor();
        else
        {
            if(!objectIsMove)
            {
                objectMover.MoveObjectRelativeX();
                objectIsMove = true;
            }
        }
    }

    public void OpenDoor()
    {
        doorLock = false;
        Transform player = Bootstrapper.PlayerTransform;
        if (player == null)
        {
            return; 
        }
        audioSource.clip = openSound;
        audioSource.Play();
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
        audioSource.clip = closeSound;
        audioSource.Play();
        doorTransform.DOLocalRotate(defaultRotation, duration).SetEase(moveEase);
        isOpen = false;
    }

    public void CloseDoor2()
    {
        doorLock = true;
        CloseDoor();
    }
}
