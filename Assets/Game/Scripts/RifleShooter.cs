using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooter : MonoBehaviour
{
    [SerializeField] private Transform _firePoint;

    [SerializeField] private LayerMask _enemyLayerMask;

    [SerializeField] private float _damage = 1f;

    public void Shoot()
    {
        Ray ray = new Ray(_firePoint.position, _firePoint.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, _enemyLayerMask) == false)
            return;

        if (hitInfo.collider.TryGetComponent(out IDamageable damageable) == false)
            return;

        damageable.TakeDamage(_damage);
    }
}
