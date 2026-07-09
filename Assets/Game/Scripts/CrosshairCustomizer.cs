using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairCustomizer : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _crosshairParts = new List<RectTransform>();

    [SerializeField] private float _scale = 1f;
    [SerializeField] private float _distanceFromCenter = 8.0f;
    [SerializeField] private Color _color = Color.white;

    private void OnValidate()
    {
        transform.localScale = Vector3.one * _scale;

        if (_crosshairParts.Count > 0)
            foreach (RectTransform crosshairPart in _crosshairParts)
                if (crosshairPart != null)
                {
                    crosshairPart.anchoredPosition = crosshairPart.anchoredPosition.normalized * _distanceFromCenter;

                    if (crosshairPart.TryGetComponent(out Image image))
                        image.color = _color;
                }

        
    }
}
