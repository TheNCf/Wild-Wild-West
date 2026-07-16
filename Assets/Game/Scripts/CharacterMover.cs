using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(CharacterController))]
public class CharacterMover : MonoBehaviour
{
    [SerializeField] private Camera _characterCamera;
    [SerializeField] private Animator _animator;

    [Space(10)]
    [SerializeField] private float _runSpeed = 3.0f;
    [SerializeField] private float _sprintSpeed = 6.0f;
    [SerializeField] private float _aimWalkingSpeed = 1.0f;
    [SerializeField] private float _speedSmoothCoef = 5.0f;
    [SerializeField] private float _rotationSpeed = 15.0f;

    [Space(10)]
    [SerializeField] private Transform _aimOrigin;
    [SerializeField] private float _aimEndPointForwardDistance = 100.0f;
    [SerializeField] private float _aimEndPointVerticalDistance = -30.0f;

    [Header("Smoothing settings")]
    [Range(0, 1)] private float _aimWeight = 1f;
    [SerializeField] private float _bodyWeight = 0.8f;
    [SerializeField] private float _headWeight = 1f;
    [SerializeField] private float _eyesWeight = 1f;
    [SerializeField] private float _clampWeight = 0.5f;

    private CharacterController _characterController;

    private PlayerInput _playerInput;

    private Vector2 _inputDirection;

    private bool _canMove;
    private bool _isSprinting = false;
    private bool _isWalking = false;
    private bool _isAiming = false;

    private float _currentSpeed = 0f;
    private float _smoothedInputX = 0f;
    private float _smoothedInputY = 0f;

    public bool IsAiming => _isAiming;

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
        _playerInput.Character.WalkToggle.started += OnWalkToggle;
        _playerInput.Character.Aim.started += OnAim;
        _playerInput.Character.Aim.canceled += OnAim;
    }

    private void OnDisable()
    {
        _playerInput.Disable();

        _playerInput.Character.Move.performed -= OnMoved;
        _playerInput.Character.Move.canceled -= OnMoved;
        _playerInput.Character.SprintToggle.started -= OnSprintToggle;
        _playerInput.Character.SprintToggle.canceled -= OnSprintToggle;
        _playerInput.Character.WalkToggle.started -= OnWalkToggle;
        _playerInput.Character.Aim.started -= OnAim;
        _playerInput.Character.Aim.canceled -= OnAim;
    }

    private void Update()
    {
        Move();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_isAiming == false)
            return;

        if (_aimWeight > 0)
        {
            _animator.SetLookAtWeight(_aimWeight, _bodyWeight, _headWeight, _eyesWeight, _clampWeight);
            _animator.SetLookAtPosition(_characterCamera.transform.forward * _aimEndPointForwardDistance + _aimOrigin.position + Vector3.up * _aimEndPointVerticalDistance);
        }
        else
        {
            _animator.SetLookAtWeight(0);
        }

        _canMove = true;
    }

    public void SetCanMove()
    {
        _canMove = true;
    }

    public void SetCantMove()
    {
        _canMove = false;
    }

    private void Move()
    {
        float desiredSpeed = 0;

        Vector3 forward = _characterCamera.transform.forward;
        Vector3 right = _characterCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * _inputDirection.y + right * _inputDirection.x;
        bool _noInput = moveDirection.sqrMagnitude <= 0.001f;

        if (_noInput == false)
        {
            desiredSpeed = _isSprinting ? _sprintSpeed : _runSpeed;

            if (_isWalking || _isAiming)
                desiredSpeed = _aimWalkingSpeed;

            Quaternion targetRotation = _isAiming ? Quaternion.LookRotation(forward) : Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        if (_canMove)
        {
            Vector3 movement = moveDirection * _currentSpeed * Time.deltaTime;
            _characterController.Move(movement);
        }

        _currentSpeed = Mathf.Lerp(_currentSpeed, desiredSpeed, Time.deltaTime * _speedSmoothCoef);
        _smoothedInputX = Mathf.Lerp(_smoothedInputX, _inputDirection.x, Time.deltaTime * _speedSmoothCoef);
        _smoothedInputY = Mathf.Lerp(_smoothedInputY, _inputDirection.y, Time.deltaTime * _speedSmoothCoef);

        _animator.SetFloat(CharacterAnimatorData.Params.Speed, _currentSpeed);
        _animator.SetBool(CharacterAnimatorData.Params.NoInput, _noInput);
        _animator.SetFloat(CharacterAnimatorData.Params.InputX, _smoothedInputX);
        _animator.SetFloat(CharacterAnimatorData.Params.InputY, _smoothedInputY);
        _animator.SetBool(CharacterAnimatorData.Params.IsAiming, _isAiming);
        _animator.SetBool(CharacterAnimatorData.Params.IsWalking, _isWalking);
    }

    private void OnMoved(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }

    private void OnSprintToggle(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isSprinting = context.ReadValueAsButton();
    }

    private void OnWalkToggle(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isWalking = !_isWalking;
    }

    private void OnAim(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isAiming = context.ReadValueAsButton();
    }
}
