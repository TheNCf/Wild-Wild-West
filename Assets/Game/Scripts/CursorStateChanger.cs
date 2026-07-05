using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStateChanger : MonoBehaviour
{
    [SerializeField] private bool _isLockedOnStart = true;
    [SerializeField] private Canvas _crosshairCanvas;

    private PlayerInput _playerInput;

    private bool _isAiming = false;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        if (_isLockedOnStart)
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Character.Aim.performed += OnAim;
        _playerInput.Character.Aim.canceled += OnAim;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Character.Aim.performed -= OnAim;
        _playerInput.Character.Aim.canceled -= OnAim;
    }

    private void OnAim(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isAiming = context.ReadValueAsButton();

        if (_crosshairCanvas != null)
            _crosshairCanvas.enabled = _isAiming;
    }
}
