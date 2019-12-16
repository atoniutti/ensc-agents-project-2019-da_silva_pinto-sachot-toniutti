using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIAgent : MonoBehaviour
{
    // UI
    public Agent _owner;
    public GameObject _pointPosition;
    public Text _name; // Name of the agent
    public ProgressBar _energyProgressBar;
    public ProgressBar _wasteProgressBar;
    public Canvas canvasAgent;

    CameraManager _cameraManager;

    // Start is called before the first frame update
    void Start()
    {
        _name.text = GetNameCode();
        _cameraManager = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cameraManager._currentAgent == _owner)
        {
            // Point Position
            _pointPosition.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            _pointPosition.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);

            // Canvas Agent
            canvasAgent.enabled = false;
        }
        else
        {
            // Point Position
            _pointPosition.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            _pointPosition.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);

            // Canvas Agent
            canvasAgent.enabled = true;
        }

        // Update Percent Of Energy & Toxic Piles
        _energyProgressBar.UpdateBar(_owner._percentOfEnergyPile);
        _wasteProgressBar.UpdateBar(_owner._percentOfWastePile);
    }

    private string GetNameCode()
    {
        string name = "Agent ";

        if (_owner._code < 10)
        {
            name += "00" + _owner._code;
        }
        else
        {
            name += "0" + _owner._code;
        }

        return name;
    }
}
