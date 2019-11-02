using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicManager : MonoBehaviour
{
    public GameObject toxic; // The energy prefab to be spawned.
    public Transform spawnPoint;  // An array of the spawn points this object can spawn from.
    public int actualAgent;
    public int precedentAgent;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        actualAgent = 0;
        precedentAgent = 1000;

    }

    // If the agent enter in the area
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name== "Agent(Clone)")
        {
            precedentAgent = col.GetComponent<Agent>()._name;
            if(actualAgent!= precedentAgent)
            {
                Instantiate(toxic, spawnPoint.position, spawnPoint.rotation);
                 actualAgent= precedentAgent ;
            }
        }
    }

    
}
