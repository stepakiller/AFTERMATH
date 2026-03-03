using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] float sensitivity;
    [Range(0.01f, 0.5f)] [SerializeField] float smoothTime; 
    [SerializeField] float minVerticalAngle;
    [SerializeField] float maxVerticalAngle;
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
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationVelocity, smoothTime);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationVelocity, smoothTime);

        transform.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);
        playerBody.rotation = Quaternion.Euler(0f, currentYRotation, 0f);
    }
}
