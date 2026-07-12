using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;

    private float _health;

    public event Action<float> DamageTaken;
    public event Action HealthRanOut;

    private void Awake()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        DamageTaken?.Invoke(damage);

        if (_health <= 0)
        {
            _health = 0;

            HealthRanOut?.Invoke();
        }
    }
}