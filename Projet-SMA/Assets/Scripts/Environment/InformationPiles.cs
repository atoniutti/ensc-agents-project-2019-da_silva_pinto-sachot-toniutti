using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationPiles : MonoBehaviour
{
    public CylinderLevel _energyBoxEnter;
    public CylinderLevel _wasteBoxEnter;
    public float _energyRate;
    public float _toxicRate;

    // Start is called before the first frame update
    private void Start()
    {
        _energyRate = _energyBoxEnter._currentPercent;
        _toxicRate = _wasteBoxEnter._currentPercent;
    }

    // Update is called once per frame
    private void Update()
    {
        _energyRate = _energyBoxEnter._currentPercent;
        _toxicRate = _wasteBoxEnter._currentPercent;
    }
}
