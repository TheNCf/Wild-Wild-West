using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMover : MonoBehaviour
{
    [SerializeField] private Camera _characterCamera;
    [Space(10)]

    [SerializeField] private float _runSpeed = 5.0f;
    [SerializeField] private float _sprintSpeed = 9.0f;
    [SerializeField] private float _aimWalkingSpeed = 2.0f;
    [SerializeField] private float _rotationSpeed = 180.0f;

    private CharacterController _characterController;

    private PlayerInput _playerInput;

    private Vector2 _inputDirection;

    private bool _isSprinting = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
         _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();

        _playerInput.Character.Move.performed += OnMoved;
        _playerInput.Character.Move.canceled += OnMoved;
        _playerInput.Character.SprintToggle.started += OnSprintToggle;
        _playerInput.Character.SprintToggle.canceled += OnSprintToggle;
    }

    private void OnDisable()
    {
        _playerInput.Disable();

        _playerInput.Character.Move.performed -= OnMoved;
        _playerInput.Character.Move.canceled -= OnMoved;
        _playerInput.Character.SprintToggle.started -= OnSprintToggle;
        _playerInput.Character.SprintToggle.canceled -= OnSprintToggle;
    }

    private void Update()
    {
        Vector3 forward = _characterCamera.transform.forward;
        Vector3 right = _characterCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * _inputDirection.y + right * _inputDirection.x;

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            float currentSpeed = _isSprinting ? _sprintSpeed : _runSpeed;

            Vector3 movement = moveDirection * currentSpeed * Time.deltaTime;
            _characterController.Move(movement);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void OnMoved(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }

    private void OnSprintToggle(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isSprinting = context.ReadValueAsButton();
    }
}
