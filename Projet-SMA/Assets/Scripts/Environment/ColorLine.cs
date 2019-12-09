using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLine : MonoBehaviour
{
    private Renderer _rend;
    private InformationPiles _piles;
    private float _intensity;

    private void Start()
    {
        _piles = GameObject.FindGameObjectWithTag("InformationBox").GetComponent<InformationPiles>();
        _rend = GetComponent<Renderer>();
        _intensity = 10;
    }

    void Update()
    {
        if (_piles._toxicRate > _piles._energyRate && _piles._toxicRate > 20)
        {
            _rend.material.SetColor("_EmissionColor", Color.yellow * _intensity);
        }
        if (_piles._toxicRate < _piles._energyRate && _piles._energyRate >= 20)
        {
            _rend.material.SetColor("_EmissionColor", new Vector4(0.2235294f, 10, 10, 1));
        }
        if (_piles._energyRate < 20 || _piles._toxicRate > 80)
        {
            _rend.material.SetColor("_EmissionColor", Color.red * _intensity);
        }
    }
}
