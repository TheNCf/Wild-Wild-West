using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Camera _camera;

    [SerializeField] private float _mouseSensivity = 0.1f;
    [SerializeField] private float _zoomSensivity = 0.1f;
    [SerializeField] private float _smoothTime = 3.0f;
    [SerializeField] private float _verticalAngleClamp = 85.0f;
    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private AnimationCurve _zoomAnimationCurve;

    private PlayerInput _input;

    private float _currentVerticalAngle = 0;
    private float _zoomValue = 0.5f;

    private Vector3 _cameraTargetPosition;

    private void Awake()
    {
        _input = new PlayerInput();

        SetZoom();
    }

    private void OnEnable()
    {
        _input.Enable();

        _input.Character.MouseMove.performed += Rotate;
        _input.Character.Zoom.performed += OnZoom;
    }

    private void OnDisable()
    {
        _input.Disable();

        _input.Character.MouseMove.performed -= Rotate;
        _input.Character.Zoom.performed -= OnZoom;
    }

    private void Update()
    {
        transform.position = _target.position + _target.TransformDirection(_cameraOffset);
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        float zoomDelta = context.ReadValue<float>() * _zoomSensivity;
        _zoomValue += zoomDelta * Time.deltaTime;
        _zoomValue = Mathf.Clamp01(_zoomValue);

        SetZoom();
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

    private void SetZoom()
    {
        _cameraTargetPosition = transform.position - transform.forward * _zoomAnimationCurve.Evaluate(_zoomValue);
        _camera.transform.position = _cameraTargetPosition;
    }
}
