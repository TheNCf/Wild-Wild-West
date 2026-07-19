using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Camera _camera;

    [SerializeField] private float _mouseSensivity = 0.1f;
    [SerializeField] private float _zoomSensivity = 0.1f;
    [SerializeField] private float _aimingZoomValue = 0;
    [SerializeField] private float _smoothTime = 3.0f;
    [SerializeField] private float _verticalAngleClamp = 85.0f;
    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private Vector3 _aimingCameraOffset;
    [SerializeField] private AnimationCurve _zoomAnimationCurve;
    [SerializeField] private LayerMask _collisionLayerMask;

    private PlayerInput _playerInput;

    private bool _isAiming = false;

    private float _currentVerticalAngle = 0;
    private float _zoomValue = 0.5f;
    private float _currentZoom = 0.5f;
    private Vector3 _currentOffset = Vector3.zero;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();

        _playerInput.Character.MouseMove.performed += Rotate;
        _playerInput.Character.Zoom.performed += OnZoom;
        _playerInput.Character.Aim.started += OnAim;
        _playerInput.Character.Aim.canceled += OnAim;
    }

    private void OnDisable()
    {
        _playerInput.Disable();

        _playerInput.Character.MouseMove.performed -= Rotate;
        _playerInput.Character.Zoom.performed -= OnZoom;
        _playerInput.Character.Aim.started -= OnAim;
        _playerInput.Character.Aim.canceled -= OnAim;
    }

    private void Update()
    {
        Vector3 desiredOffset = _isAiming ? _aimingCameraOffset : _cameraOffset;
        _currentOffset = Vector3.Slerp(_currentOffset, desiredOffset, Time.deltaTime * _smoothTime);
        
        transform.position = _target.position + _target.TransformDirection(_currentOffset);
        float desiredZoom = _isAiming ? _aimingZoomValue : _zoomValue;
        _currentZoom = Mathf.Lerp(_currentZoom, desiredZoom, Time.deltaTime * _smoothTime);
        SetZoom(_currentZoom);
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        float zoomDelta = context.ReadValue<float>() * _zoomSensivity;
        _zoomValue += zoomDelta * Time.deltaTime;
        _zoomValue = Mathf.Clamp01(_zoomValue);
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>() * _mouseSensivity * Time.deltaTime;
        transform.Rotate(0, mouseDelta.x, 0);
        _currentVerticalAngle -= mouseDelta.y;
        _currentVerticalAngle = Mathf.Clamp(_currentVerticalAngle, -_verticalAngleClamp, _verticalAngleClamp);
        Vector3 newRotation = transform.localEulerAngles;
        newRotation.z = 0;
        newRotation.x = _currentVerticalAngle;
        transform.localEulerAngles = newRotation;
    }

    private void SetZoom(float zoomValue)
    {
        Vector3 cameraTargetPosition = new Vector3();
        Ray ray = new Ray(transform.position, -_camera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, _zoomAnimationCurve.Evaluate(_currentZoom), _collisionLayerMask))
            cameraTargetPosition = hitInfo.point;
        else
            cameraTargetPosition = transform.position - transform.forward * _zoomAnimationCurve.Evaluate(zoomValue);

        _camera.transform.position = cameraTargetPosition;
    }

    private void OnAim(InputAction.CallbackContext context)
    {
        _isAiming = context.ReadValueAsButton();
    }
}
