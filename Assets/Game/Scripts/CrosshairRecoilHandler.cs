using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairRecoilHandler : MonoBehaviour
{
    [SerializeField] private CrosshairCustomizer _customizer;

    [SerializeField] private float _recoilRegenerationSpeed = 1.0f;
    [SerializeField] private float _maxRecoil = 30.0f;

    private float _startCrosshairSize;
    private float _currentSize;

    void Awake()
    {
        _startCrosshairSize = _customizer.DistanceFromCenter;
        _currentSize = _customizer.DistanceFromCenter;
    }

    void Update()
    {
        _customizer.DistanceFromCenter = _currentSize;

        _currentSize = Mathf.Lerp(_currentSize, _startCrosshairSize, Time.deltaTime * _recoilRegenerationSpeed);
    }

    public void AddRecoil(float amount)
    {
        _currentSize = Mathf.Clamp(_currentSize + amount, _startCrosshairSize, _maxRecoil);
    }
}