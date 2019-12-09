using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicManager : MonoBehaviour
{
    public GameObject _toxic; // The energy prefab to be spawned.
    public Transform _spawnPoint;  // An array of the spawn points this object can spawn from.
    private int _actualAgent;
    private int _actualAgentOut;
    private List<int> _listAgentInArea = new List<int>();

    void Start()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        _listAgentInArea.Add(0);
    }

    // If the agent enter in the area spawn toxic 
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "agent" && col.GetComponent<Agent>()._currentState == AgentStates.FindingToxic)
        {
            bool here = false;
            _actualAgent = col.GetComponent<Agent>()._code;
            foreach (int agentPresence in _listAgentInArea)
            {
                if (agentPresence == _actualAgent)
                {
                    here = true;
                }
            }
            if (here == false)
            {

                Instantiate(_toxic, _spawnPoint.position, _spawnPoint.rotation);
                _listAgentInArea.Add(_actualAgent);
                here = false;
            }
        }
        if (col.gameObject.tag == "agent" && col.GetComponent<Agent>()._currentState == AgentStates.CarryingToxicToPile)
        {
            _actualAgentOut = col.GetComponent<Agent>()._code;
            _listAgentInArea.Remove(_actualAgentOut);
        }
    }
}
