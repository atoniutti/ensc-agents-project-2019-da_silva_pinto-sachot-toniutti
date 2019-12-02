using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentKnowledges : MonoBehaviour
{
    public Agent _owner;
    public Text _state;
    public Text _target;
    public Text _InteractionAgent;
    public Text _discussion;

    // Update is called once per frame
    void Update()
    {
        _state.text = _owner._currentState.ToString();
        _target.text = _owner._currentTarget.ToString();
        _discussion.text = _owner._currentDialogue.ToString();

        if (_owner._actualInteractionAgent != null)
        {
            _InteractionAgent.text = "to " + GetNameCode(_owner._actualInteractionAgent);
        }
    }

    private string GetNameCode(Agent agent)
    {
        string name = "Agent ";

        if (agent._code < 10)
        {
            name += "00" + agent._code;
        }
        else
        {
            name += "0" + agent._code;
        }

        return name;
    }
}
