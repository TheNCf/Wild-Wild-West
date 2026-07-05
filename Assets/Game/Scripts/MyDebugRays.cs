using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDebugRays : MonoBehaviour
{
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up * 100, Color.red);
    }
}
