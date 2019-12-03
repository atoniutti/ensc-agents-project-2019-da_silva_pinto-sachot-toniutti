using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Update Percent Of Energy & Toxic Piles
        _energyProgressBar.UpdateBar(_energyLevel._currentPercent);
        _wasteProgressBar.UpdateBar(_wasteLevel._currentPercent);
    }
}
