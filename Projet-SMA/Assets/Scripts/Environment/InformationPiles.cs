using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationPiles : MonoBehaviour
{

    public CylinderLevel energyBoxEnter;
    public CylinderLevel wasteBoxEnter;
    public float energyRate;
    public float toxicRate;

    // Start is called before the first frame update
    private void Start()
    {
        energyRate = energyBoxEnter._ratePercent;
        toxicRate = wasteBoxEnter._ratePercent;
    }

    // Update is called once per frame
    private void Update()
    {
        energyRate = energyBoxEnter._ratePercent;
        toxicRate = wasteBoxEnter._ratePercent;
    }
}
