using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicManager : MonoBehaviour
{
    public GameObject toxic; // The energy prefab to be spawned.
    public Transform spawnPoint;  // An array of the spawn points this object can spawn from.
    private int actualAgent;
    private int actualAgentOut;
    private List<int> listAgentInArea = new List<int>();

    void Start()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        listAgentInArea.Add(0);
    }

    // If the agent enter in the area spawn toxic 
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "agent" && col.GetComponent<Agent>()._currentState == AgentStates.FindingToxic)
        {
            bool here = false;
            actualAgent = col.GetComponent<Agent>()._code;
            foreach (int agentPresence in listAgentInArea)
            {
                if (agentPresence == actualAgent)
                {
                    here = true;
                }
            }
            if (here == false)
            {

                Instantiate(toxic, spawnPoint.position, spawnPoint.rotation);
                listAgentInArea.Add(actualAgent);
                here = false;
            }
        }
        if (col.gameObject.tag == "agent" && col.GetComponent<Agent>()._currentState == AgentStates.GoToPileToxic)
        {
            actualAgentOut = col.GetComponent<Agent>()._code;
            listAgentInArea.Remove(actualAgentOut);
        }
    }
}
