using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStateChanger : MonoBehaviour
{
    [SerializeField] private bool _isLockedOnStart = true;

    private void Awake()
    {
        if (_isLockedOnStart)
            Cursor.lockState = CursorLockMode.Locked;
    }
}
