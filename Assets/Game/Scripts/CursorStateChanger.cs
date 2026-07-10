using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStateChanger : MonoBehaviour
{
    [SerializeField] private bool _isLockedOnStart = true;
    [SerializeField] private Canvas _crosshairCanvas;
    [SerializeField] private RectTransform _crosshair;

    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _aimLayerMask;
    [SerializeField] private Transform _aimOrigin;
    [SerializeField] private AnimationCurve _crosshairSizeByDistanceMultiplier;

    private PlayerInput _playerInput;

    private bool _isAiming = false;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        if (_isLockedOnStart)
            Cursor.lockState = CursorLockMode.Locked;

        _crosshairCanvas.enabled = _isAiming;
    }

    private void Update()
    {
        Ray ray = new Ray(_aimOrigin.position, _aimOrigin.up);
        RaycastHit raycastHit;

        bool castSuccessful = Physics.Raycast(ray, out raycastHit, float.MaxValue, _aimLayerMask);

        Vector3 crosshairPosition = castSuccessful ? _camera.WorldToScreenPoint(raycastHit.point) : new Vector3(Screen.width / 2, Screen.height / 2);

        _crosshair.position = crosshairPosition;

        float crosshairScaleMult = castSuccessful ? _crosshairSizeByDistanceMultiplier.Evaluate(raycastHit.distance) : 0.40f;

        _crosshair.localScale = Vector3.one * crosshairScaleMult;
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
