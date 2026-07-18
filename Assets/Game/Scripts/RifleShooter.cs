using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RifleShooter : MonoBehaviour
{
    [SerializeField] private Transform _firePoint;

    [SerializeField] private LayerMask _enemyLayerMask;

    [SerializeField] private CharacterMover _characterMover;
    [SerializeField] private AmmoStorage _ammoStorage;
    [SerializeField] private Animator _characterAnimator;

    [SerializeField] private float _damage = 1f;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Character.Shoot.performed += OnShoot;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Character.Shoot.performed -= OnShoot;
    }

    public void Shoot()
    {
        if (_ammoStorage.TryTakeBullet() == false)
            return;

        _characterAnimator.SetTrigger(CharacterAnimatorData.Params.Shoot);

        Ray ray = new Ray(_firePoint.position, _firePoint.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, _enemyLayerMask) == false)
            return;

        if (hitInfo.collider.TryGetComponent(out IDamageable damageable) == false)
            return;

        damageable.TakeDamage(_damage);
    }

    private void OnShoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (_characterMover.IsAiming)
            Shoot();
    }
}
