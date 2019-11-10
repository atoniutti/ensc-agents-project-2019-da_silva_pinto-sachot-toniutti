using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentButton : MonoBehaviour
{
    public Agent agent;
    public Text id;

    // Start is called before the first frame update
    private void Start()
    {
        id.text = agent._name.text;
    }
    
}
