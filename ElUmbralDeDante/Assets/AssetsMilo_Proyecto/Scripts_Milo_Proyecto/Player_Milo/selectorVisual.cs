using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectorVisual : MonoBehaviour
{
    private MeshRenderer _renderer;
    private Color _originalColor;
    public Color colorResaltado = Color.red;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        if (_renderer != null)
        {
            _originalColor = _renderer.material.color;
        }
    }

    public void ActivarResaltado()
    {
        if (_renderer != null)
        {
            _renderer.material.color = colorResaltado;
        }
    }

    public void DesactivarResaltado()
    {
        if (_renderer != null)
        {
            _renderer.material.color = _originalColor;
        }
    }
}
