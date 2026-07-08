using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairCustomizer : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _crosshairParts = new List<RectTransform>();

    [SerializeField] private float _scale = 1f;
    [SerializeField] private float _distanceFromCenter = 8.0f;

    private void OnValidate()
    {
        transform.localScale = Vector3.one * _scale;

        if (_crosshairParts.Count > 0)
            foreach (RectTransform crosshairPart in _crosshairParts)
                if (crosshairPart != null)
                    crosshairPart.anchoredPosition = crosshairPart.anchoredPosition.normalized * _distanceFromCenter;
    }
}
