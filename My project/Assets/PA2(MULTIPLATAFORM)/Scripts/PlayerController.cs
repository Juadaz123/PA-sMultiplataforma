using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Parameters")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.5f; // duración total del salto
    [SerializeField] private float gravity = -9.81f;

    [Header("Camera Parameters")]
    [SerializeField] private Transform playerCamera;

    private CharacterController _characterController;
    private Vector3 _moveDirection;
    private Vector3 _velocity;
    private bool _isJumping;
    private bool _isGrounded;
    private float _groundCheckDistance = 0.2f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _moveDirection = new Vector3(input.x, 0f, input.y);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && _isGrounded && !_isJumping)
        {
            StartCoroutine(JumpRoutine());
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {        if (context.performed)
        {
            // Llama a SpawnBullet desde el Singleton del pooler
            if (GameManagerBasket.Instance != null)
            {
                GameManagerBasket.Instance.SpawnBullet();
            }
            else
            {
                Debug.LogError("GameManagerBasket Instance is missing.");
            }
        }
    }

    private void Update()
    {
        CheckGrounded();
        HandleMovement();
        ApplyGravity();
    }

    private void CheckGrounded()
    {
        // Usamos Raycast para una detección de suelo más precisa
        Vector3 origin = transform.position + Vector3.down * (_characterController.height / 2f - _characterController.skinWidth);
        _isGrounded = Physics.Raycast(origin, Vector3.down, _groundCheckDistance);

        if (_isGrounded && !_isJumping && _velocity.y < 0)
        {
            _velocity.y = -2f; // mantiene al jugador pegado al suelo
        }
    }

    private void HandleMovement()
    {
        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;
        forward.y = 0f;
        right.y = 0f;

        Vector3 move = (forward * _moveDirection.z + right * _moveDirection.x).normalized;
        _characterController.Move(move * (speed * Time.deltaTime));

        // Rotación del jugador hacia la dirección de movimiento
        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        if (!_isGrounded && !_isJumping)
        {
            _velocity.y += gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }

    private IEnumerator JumpRoutine()
    {
        _isJumping = true;
        float elapsed = 0f;
        Vector3 initialPosition = transform.position;

        while (elapsed < jumpDuration)
        {
            float normalizedTime = elapsed / jumpDuration;
            float height = Mathf.Sin(normalizedTime * Mathf.PI) * jumpHeight;

            float currentY = initialPosition.y + height;
            float deltaY = currentY - transform.position.y;

            // Movemos el CharacterController de forma controlada (segura)
            _characterController.Move(Vector3.up * deltaY);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _isJumping = false;
    }

    private void OnDrawGizmos()
    {
        if (!_characterController) return;

        // Gizmo para depurar si está tocando el suelo
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Vector3 origin = transform.position + Vector3.down * (_characterController.height / 2f - _characterController.skinWidth);
        Gizmos.DrawWireSphere(origin, _characterController.radius);
        Gizmos.DrawLine(origin, origin + Vector3.down * _groundCheckDistance);
    }
}
