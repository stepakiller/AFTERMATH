using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Events;
public class ZoomObject : MonoBehaviour, Interactable
{
    [SerializeField] PauseController pauseController;
    [SerializeField] GameObject crosshair;
    [SerializeField] CinemachineCamera zoomCamera;
    [SerializeField] int activeCameraPriority = 20;
    [SerializeField] VisionController visionController;
    [SerializeField] UnityEvent _event;
    [SerializeField] DictafonController dictafonController;
    public void Interact()
    {
        EnterZoomMode();
    }
    
    void EnterZoomMode()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crosshair.SetActive(false);
        pauseController.IsPauseBlocked = true;
        zoomCamera.Priority = activeCameraPriority;
        InputManager.Instance.OnPausePressed += ExitZoomMode;
    }

    public void ExitZoomMode()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair.SetActive(true);
        pauseController.IsPauseBlocked = false;
        InputManager.Instance.OnPausePressed -= ExitZoomMode;
        zoomCamera.Priority = 0;
        _event?.Invoke();
    }

    public void OnVisionScript()
    {
        visionController.enabled = true;
    }

    public void ZapiskaPodDoor()
    {
        dictafonController.canOpenDoor = true;
    }
}
