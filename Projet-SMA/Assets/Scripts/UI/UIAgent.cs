using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAgent : MonoBehaviour
{
    //UI
    public Agent owner;
    public GameObject pointPosition;
    public Canvas canvasAgent;
    public List<AgentTrust> agentsList = new List<AgentTrust>();
    AgentButton agentButton;

    // Start is called before the first frame update
    void Start()
    {
        owner._camera.targetDisplay = owner._code;
        owner._camera.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.current == owner._camera)
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
    }
}
