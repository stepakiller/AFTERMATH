using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    [Header("Позиция")]
    [SerializeField] float walkingBobbingSpeed = 14f;
    [SerializeField] float bobbingAmount = 0.025f;
    [SerializeField] float runningBobbingFactor = 1.5f;
    [SerializeField] float transitionSpeed = 10f; 

    [Header("Вращение")]
    [SerializeField] float bobbingRotX = 0.3f;
    [SerializeField] float bobbingRotY = 0.25f;
    [SerializeField] float runningRotFactor = 1.5f; 

    [Header("Настройки приземления")]
    [SerializeField] float landDipAmount = 0.4f;   
    [SerializeField] float landEntrySpeed = 15f;   
    [SerializeField] float landReturnSpeed = 5f;   

    [SerializeField] CharacterController controller;

    float defaultPosY = 0;
    float timer = 0;
    Vector3 targetPos;
    Quaternion targetRot; 
    float currentLandOffset = 0f;
    float targetLandOffset = 0f; 
    bool wasGrounded;
    Vector3 lastPosition; 

    void Start()
    {
        defaultPosY = transform.localPosition.y;
        targetPos = transform.localPosition;
        targetRot = transform.localRotation;
        wasGrounded = true;
        lastPosition = controller.transform.position;
    }

    void Update()
    {
        HandleLanding();
        HandleBobbing();
    }

    void HandleLanding()
    {
        if (!wasGrounded && controller.isGrounded) targetLandOffset = landDipAmount;
        wasGrounded = controller.isGrounded;

        if (targetLandOffset > 0)
        {
            currentLandOffset = Mathf.Lerp(currentLandOffset, targetLandOffset, Time.deltaTime * landEntrySpeed);
            if (Mathf.Abs(currentLandOffset - targetLandOffset) < 0.05f) targetLandOffset = 0;
        }
        else currentLandOffset = Mathf.Lerp(currentLandOffset, 0, Time.deltaTime * landReturnSpeed);
    }

    void HandleBobbing()
    {
        float targetPosX = 0;
        float targetPosY = defaultPosY;
        float targetRotX = 0;
        float targetRotY = 0;

        Vector3 currentPos = controller.transform.position;
        Vector3 horizontalPos = new Vector3(currentPos.x, 0, currentPos.z);
        Vector3 horizontalLastPos = new Vector3(lastPosition.x, 0, lastPosition.z);
        
        float speed = Vector3.Distance(horizontalPos, horizontalLastPos) / Time.deltaTime;
        lastPosition = currentPos;

        if (speed > 0.1f && controller.isGrounded)
        {
            float waveSpeed = walkingBobbingSpeed;
            float waveAmount = bobbingAmount;
            
            float rotMultX = bobbingRotX;
            float rotMultY = bobbingRotY;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                waveSpeed *= 1.3f;
                waveAmount *= runningBobbingFactor;
                rotMultX *= runningRotFactor;
                rotMultY *= runningRotFactor;
            }
            timer += Time.deltaTime * waveSpeed;
            targetPosX = Mathf.Cos(timer / 2) * waveAmount;
            targetPosY = defaultPosY + Mathf.Sin(timer) * waveAmount;
            targetRotX = Mathf.Sin(timer) * rotMultX;
            targetRotY = Mathf.Cos(timer / 2) * rotMultY;
        }
        else 
        {
            timer = 0;
            targetPosX = 0;
            targetPosY = defaultPosY;
            targetRotX = 0;
            targetRotY = 0;
        }
        Vector3 bobbingPos = new Vector3(targetPosX, targetPosY, 0);
        targetPos = Vector3.Lerp(targetPos, bobbingPos, Time.deltaTime * transitionSpeed);
        transform.localPosition = new Vector3(targetPos.x, targetPos.y - currentLandOffset, targetPos.z);
        Quaternion bobbingRot = Quaternion.Euler(targetRotX, targetRotY, 0);
        targetRot = Quaternion.Slerp(targetRot, bobbingRot, Time.deltaTime * transitionSpeed);
        transform.localRotation = targetRot;
    }
}