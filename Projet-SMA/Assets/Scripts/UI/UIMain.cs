using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    // Environment
    public CylinderLevel _energyLevel;
    public CylinderLevel _wasteLevel;

    // UI
    public ProgressBar _energyProgressBar;
    public ProgressBar _wasteProgressBar;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Update Percent Of Energy & Waste Piles
        _energyProgressBar.UpdateBar(_energyLevel._currentPercent);
        _wasteProgressBar.UpdateBar(_wasteLevel._currentPercent);
    }
}
