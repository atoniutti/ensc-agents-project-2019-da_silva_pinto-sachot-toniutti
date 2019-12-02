using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentButton : MonoBehaviour
{
    public AgentTrust _agentTrust;
    public Text _name;
    public CircularProgressBar _trustProgressBar;

    // Start is called before the first frame update
    public void Start()
    {
        _name.text = _agentTrust._agent.GetComponent<UIAgent>()._name.text;
        _trustProgressBar.UpdateBar(_agentTrust._trust);
    }

    // Update is called once per frame
    void Update()
    {
        _trustProgressBar.UpdateBar(_agentTrust._trust);
    }

    public void SetCurrentAgent()
    {
        CameraManager cameraManager = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraManager>();
        cameraManager.SwitchAgent(_agentTrust._agent);
    }
}