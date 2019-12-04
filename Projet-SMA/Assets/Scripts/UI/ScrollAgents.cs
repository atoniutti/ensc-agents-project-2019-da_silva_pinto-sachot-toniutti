using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollAgents : MonoBehaviour
{
    public Agent _currentAgent;
    public ScrollRect _scrollView;
    public GameObject _scrollContent;
    public AgentButton _agentButton;

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
        foreach (AgentTrust a in _currentAgent._agentsList)
        {
            GenerateItem(a);
        }
    }

    void GenerateItem(AgentTrust a)
    {
        AgentButton scrollItem = Instantiate(_agentButton);
        scrollItem.transform.SetParent(_scrollContent.transform, false);
        scrollItem._agentTrust = a;
    }
}
