using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoStorage : MonoBehaviour
{
    [SerializeField] private int _magazineSize = 8;
    [SerializeField] private int _ammoMaxCapacity = 40;

    private int _bulletsInMagazine;
    private int _currentAmmo;

    private void Awake()
    {
        _bulletsInMagazine = _magazineSize;
        _currentAmmo = _ammoMaxCapacity;
    }

    public bool TryTakeBullet()
    {
        if (_bulletsInMagazine <= 0)
            return false;

        _bulletsInMagazine--;

        return true;
    }

    public void ReloadBullet()
    {
        if (_currentAmmo <= 0 || _bulletsInMagazine >= _magazineSize)
            return;

        _currentAmmo--;
        _bulletsInMagazine++;
    }
}
