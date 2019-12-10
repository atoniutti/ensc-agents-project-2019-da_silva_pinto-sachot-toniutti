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

    // Slider
    public Slider _energySlider;
    public Slider _wasteSlider;

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

        // Update Slider Of Energy & Waste Piles
        _energySlider.value = _energyLevel._currentPercent;
        _wasteSlider.value = _wasteLevel._currentPercent;
    }
}
