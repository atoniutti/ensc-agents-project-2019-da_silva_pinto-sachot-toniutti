using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIAgent : MonoBehaviour
{
    // UI
    public Agent owner;
    public GameObject pointPosition;
    public Text _name; // Name of the agent
    public ProgressBar _energyProgressBar;
    public ProgressBar _wasteProgressBar;
    CameraManager cameraManager;

    public Canvas canvasAgent;

    // Start is called before the first frame update
    void Start()
    {
        _name.text = GetNameCode();
        cameraManager = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraManager.currentAgent == owner)
        {
            // Point Position
            pointPosition.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            pointPosition.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);

            // Canvas Agent
            canvasAgent.enabled = false;
        }
        else
        {
            // Point Position
            pointPosition.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            pointPosition.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);

            // Canvas Agent
            canvasAgent.enabled = true;
        }

        // Update Percent Of Energy & Toxic Piles
        _energyProgressBar.UpdateBar(owner._percentOfEnergyPile);
        _wasteProgressBar.UpdateBar(owner._percentOfWastePile);
    }

    private string GetNameCode()
    {
        string name = "Agent ";

        if (owner._code < 10)
        {
            name += "00" + owner._code;
        }
        else
        {
            name += "0" + owner._code;
        }

        return name;
    }
}
