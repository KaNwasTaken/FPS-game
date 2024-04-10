using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    CharacterController characterController;
    public int speed = 5;
    Vector3 playerVelocity;
    public float gravity = -9.8f;
    public float jumpHeight = 5f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerVelocity = Vector3.zero;
    }

    // Called Every Frame From InputManager
    public void ProcessMove(Vector2 vectorInput)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = vectorInput.x;
        moveDirection.z = vectorInput.y;
        playerVelocity.y += gravity * Time.deltaTime;
        if (characterController.isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

        characterController.Move(speed * Time.deltaTime * transform.TransformDirection(moveDirection));
        characterController.Move(playerVelocity * Time.deltaTime);
    }
    private void Update()
    {

    }
    public void Jump()
    {
        if (characterController.isGrounded)
            playerVelocity.y = jumpHeight;
    }
}
