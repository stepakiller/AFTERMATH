using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity;

    [Header("Настройки приседания")]
    [SerializeField] float standingHeight;
    [SerializeField] float crouchingHeight;
    [SerializeField] float crouchSpeed;
    [SerializeField] Transform cameraTransform;
    [SerializeField] LayerMask obstacleMask; 
    [SerializeField] float checkRadius;

    CharacterController controller;
    Vector3 velocity;
    bool isGrounded;
    bool isCrouching;
    float currentHeight;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHeight = standingHeight;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        HandleCrouchInput();
        ApplyCrouchLerp();
        HandleMovement();
        HandleJump();

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float currentSpeed = isCrouching ? walkSpeed * 0.5f : (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed);

        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > 1) move.Normalize();
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching && CanStandUp()) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void HandleCrouchInput()
    {
        if (Input.GetKey(KeyCode.LeftControl)) isCrouching = true;
        else if (CanStandUp()) isCrouching = false;
    }

    void ApplyCrouchLerp()
    {
        float targetH = isCrouching ? crouchingHeight : standingHeight;
        currentHeight = Mathf.Lerp(currentHeight, targetH, Time.deltaTime * crouchSpeed);
        float lastHeight = controller.height;
        controller.height = currentHeight;
        
        controller.center = new Vector3(0, controller.height / 2f, 0);
        transform.position += new Vector3(0, (controller.height - lastHeight) / 2f, 0);

        float camY = isCrouching ? crouchingHeight * 0.8f : standingHeight * 0.8f;
        Vector3 newCamPos = cameraTransform.localPosition;
        newCamPos.y = Mathf.Lerp(newCamPos.y, camY, Time.deltaTime * crouchSpeed);
        cameraTransform.localPosition = newCamPos;
    }

    bool CanStandUp()
    {
        Vector3 startPoint = transform.position + Vector3.up * (currentHeight - checkRadius);
        float distance = standingHeight - currentHeight;
        if (distance <= 0.05f) return true;
        return !Physics.SphereCast(startPoint, checkRadius, Vector3.up, out _, distance, obstacleMask);
    }
}