using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentButton : MonoBehaviour
{
    public Agent agent;
    public Text name;

    // Start is called before the first frame update
    private void Start()
    {
        name.text = agent._name.text;
    }
}
