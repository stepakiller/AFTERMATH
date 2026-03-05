using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Настройки мыши")]
    [SerializeField] float mouseSensitivity = 1f;
    
    [Header("Настройки геймпада")]
    [SerializeField] float gamepadSensitivity = 150f;

    [Header("Общие настройки")]
    [Range(0.01f, 0.5f)] [SerializeField] float smoothTime = 0.05f; 
    [SerializeField] float minVerticalAngle = -80f;
    [SerializeField] float maxVerticalAngle = 80f;

    float xRotation = 0f;
    float yRotation = 0f;
    float currentXRotation;
    float currentYRotation;
    float xRotationVelocity;
    float yRotationVelocity;
    Transform playerBody;

    void Start()
    {
        playerBody = transform.parent;
        
        yRotation = playerBody.eulerAngles.y;
        currentYRotation = yRotation;
    }

    void Update()
    {
        if (InputManager.Instance == null) return;

        Vector2 lookInput = InputManager.Instance.LookInput;
        bool isMouse = InputManager.Instance.IsMouseInput;

        float lookX = 0f;
        float lookY = 0f;

        if (isMouse)
        {
            lookX = lookInput.x * mouseSensitivity;
            lookY = lookInput.y * mouseSensitivity;
        }
        else
        {
            lookX = lookInput.x * gamepadSensitivity * Time.deltaTime;
            lookY = lookInput.y * gamepadSensitivity * Time.deltaTime;
        }

        yRotation += lookX;
        xRotation -= lookY;

        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationVelocity, smoothTime);
        currentYRotation = Mathf.SmoothDampAngle(currentYRotation, yRotation, ref yRotationVelocity, smoothTime);

        transform.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);
        playerBody.rotation = Quaternion.Euler(0f, currentYRotation, 0f);
    }
}