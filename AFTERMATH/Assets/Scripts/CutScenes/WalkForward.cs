using UnityEngine;

public class WalkForward : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] CharacterController controller;
    float gravity = -9.81f; 

    void Update()
    {
        float input = Input.GetAxis("Vertical");
        if (input < 0) input = 0;
        Vector3 move = transform.forward * (walkSpeed * input);
        move.y = gravity; 
        controller.Move(move * Time.deltaTime);
    }
}