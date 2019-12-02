using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLine : MonoBehaviour
{
    private Renderer _rend;

    private void Start()
    {
        _rend = GetComponent<Renderer>();
    }

    void Update()
    {
        _rend.material.SetColor("_EmissionColor", Color.yellow);
    }
}
