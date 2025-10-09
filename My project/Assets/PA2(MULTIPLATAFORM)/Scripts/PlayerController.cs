using System;
using UnityEngine;
using UnityEngine.InputSystem;

//Reqeriments
[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    [Header(" Player Parameters")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f, gravity = -9.8f;
    
    [Header("Camera Parameters")]
    [SerializeField] private Transform playerCamera;

    [SerializeField] private bool shouldFaceMovePlayer = false;
    
    private CharacterController _characterController;
    private Vector3 _moveDirection;
    private Vector3 _velocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void onMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>().normalized;
        //Debug.Log("Move direction: "  + _moveDirection);
    }

    public void onJump(InputAction.CallbackContext context)
    {
        //Debug.Log($"Move Input: {context.performed} - isGround: {_characterController.isGrounded}");
        if (context.performed && _characterController.isGrounded)
        {
            //Debug.Log("Jump");
            _velocity.y = (float)Math.Sqrt(jumpForce * -2f * gravity);
        }
    }
    private void Update()
    {
        //Movement Logic
        Vector3 foward = playerCamera.forward;
        Vector3 right = playerCamera.right;
        
        foward.y = 0f;
        right.y = 0f;
        foward.Normalize();
        right.Normalize();
        
        Vector3 movedirection = foward * _moveDirection.y + right * _moveDirection.x;
        _characterController.Move(movedirection * (speed * Time.deltaTime));

        if (shouldFaceMovePlayer && movedirection.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movedirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
    
        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
        
        
        
    }
}
