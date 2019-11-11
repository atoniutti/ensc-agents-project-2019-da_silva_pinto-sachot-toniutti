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
        energyRate = energyBoxEnter.currentPercent;
        toxicRate = wasteBoxEnter.currentPercent;
    }

    // Update is called once per frame
    private void Update()
    {
        energyRate = energyBoxEnter.currentPercent;
        toxicRate = wasteBoxEnter.currentPercent;
    }
}
