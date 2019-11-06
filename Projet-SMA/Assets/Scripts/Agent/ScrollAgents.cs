using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollAgents : MonoBehaviour
{
    public Agent currentAgent;
    public ScrollRect scrollView;
    public GameObject scrollContent;
    public AgentButton agentButton;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Agent a in currentAgent.agentsList)
        {
            GenerateItem(a);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateItem(Agent a)
    {
        AgentButton scrollItem = Instantiate(agentButton);
        scrollItem.transform.SetParent(scrollContent.transform, false);
        scrollItem.agent = a;
    }
}
