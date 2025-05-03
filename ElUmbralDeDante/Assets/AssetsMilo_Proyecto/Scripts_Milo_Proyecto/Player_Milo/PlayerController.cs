using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _player;
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _mouseSensitivity = 2f;

    private float _verticalLookRotation = 0f;
    private Vector3 _moveDirection;
    private float _fallVelocity;

    private void Awake()
    {
        _player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Oculta y bloquea el cursor
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        _moveDirection.x = move.x * _moveSpeed;
        _moveDirection.z = move.z * _moveSpeed;

        ApplyGravity();

        _player.Move(_moveDirection * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (_player.isGrounded)
        {
            _fallVelocity = -1f;
            if (Input.GetButtonDown("Jump"))
                _fallVelocity = Mathf.Sqrt(2f * _jumpHeight * _gravity);
        }
        else
        {
            _fallVelocity -= _gravity * Time.deltaTime;
        }

        _moveDirection.y = _fallVelocity;
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        // Rotar el jugador (izquierda/derecha)
        transform.Rotate(0, mouseX, 0);

        // Rotar la cámara (arriba/abajo) con límites
        _verticalLookRotation -= mouseY;
        _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -40f, 40f);

        _cameraHolder.localEulerAngles = new Vector3(_verticalLookRotation, 0, 0);
    }
}

