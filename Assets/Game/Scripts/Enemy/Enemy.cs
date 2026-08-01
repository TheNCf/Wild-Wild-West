using UnityEngine;

public class Enemy : MonoBehaviour, IPoolableObject
{
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void ResetObject()
    {
        gameObject.SetActive(false);
    }
}