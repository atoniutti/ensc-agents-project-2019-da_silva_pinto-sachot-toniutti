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

    private bool start;

    // Start is called before the first frame update
    void Start()
    {
        start = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            GenerateList();
            start = false;
        }
    }

    public void GenerateList()
    {
        foreach (AgentTrust a in currentAgent.agentsList)
        {
            GenerateItem(a);
        }
    }

    void GenerateItem(AgentTrust a)
    {
        AgentButton scrollItem = Instantiate(agentButton);
        scrollItem.transform.SetParent(scrollContent.transform, false);
        scrollItem._agentTrust = a;
    }
}
