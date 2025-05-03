using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _player;
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private float _actualSpeed;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _mouseSensitivity = 2f;

    private bool isRunning;

    [SerializeField] private float _crouchHeight = 1f;
    [SerializeField] private float _standHeight = 2f;
    [SerializeField] private float _crouchSpeed = 2f;
    [SerializeField] private float _crouchCameraOffset = -0.5f;
    private bool _isCrouching;
    private Vector3 _originalCameraLocalPos;

    private float _verticalLookRotation = 0f;
    private Vector3 _moveDirection;
    private float _fallVelocity;

    [SerializeField] private float bobSpeed = 5f;
    [SerializeField] private float bobAmount = 0.05f;
    private float defaultCamY;
    private float bobTimer = 0f;

    private void Awake()
    {
        _player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Oculta y bloquea el cursor
        _originalCameraLocalPos = _cameraHolder.localPosition;
        defaultCamY = _originalCameraLocalPos.y + (_isCrouching ? _crouchCameraOffset : 0f);
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleCrouch();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        _actualSpeed = isRunning ? _runSpeed : _moveSpeed;


        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        _moveDirection.x = move.x * _actualSpeed;
        _moveDirection.z = move.z * _actualSpeed;

        ApplyGravity();

        _player.Move(_moveDirection * Time.deltaTime);

        HandleHeadBobbing();
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

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _isCrouching = false;
        }

        // Suaviza el cambio de altura
        float targetHeight = _isCrouching ? _crouchHeight : _standHeight;
        _player.height = Mathf.Lerp(_player.height, targetHeight, Time.deltaTime * 10f);

        // Suaviza el cambio de velocidad
        if (_isCrouching)
            _actualSpeed = _crouchSpeed;

        // Suaviza la cámara hacia abajo
        Vector3 targetCameraPos = _originalCameraLocalPos + (_isCrouching ? Vector3.up * _crouchCameraOffset : Vector3.zero);
        _cameraHolder.localPosition = Vector3.Lerp(_cameraHolder.localPosition, targetCameraPos, Time.deltaTime * 10f);
    }

    private void HandleHeadBobbing()
    {
        if (_player.isGrounded && (_moveDirection.x != 0 || _moveDirection.z != 0))
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float newY = defaultCamY + Mathf.Sin(bobTimer) * bobAmount;
            Vector3 localPos = _cameraHolder.localPosition;
            _cameraHolder.localPosition = new Vector3(localPos.x, newY, localPos.z);
        }
        else
        {
            // Vuelve a la posición base suavemente
            Vector3 localPos = _cameraHolder.localPosition;
            _cameraHolder.localPosition = new Vector3(localPos.x, Mathf.Lerp(localPos.y, defaultCamY, Time.deltaTime * bobSpeed), localPos.z);
            bobTimer = 0f;
        }
    }
}

