using UnityEngine;

public class Targeter : MonoBehaviour, ITargetable
{
    public Vector3 Position => transform.position;
    public Vector3 Velocity { get; private set; }

    private Vector3 _lastPosition;

    private void Start()
    {
        _lastPosition = transform.position;
    }

    private void Update()
    {
        Velocity = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;
    }
}