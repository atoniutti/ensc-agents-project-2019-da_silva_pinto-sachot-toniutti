using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickableEnergy : MonoBehaviour
{
    private List<GameObject> listAgent = new List<GameObject>(); //list of the agnet in the scene
    private Transform player;
    public Agent agent;
    private float[] distance;
    public bool hasPlayer = false;
    public static int idPrec = 0;
    public int idEnergy; //identifiant of the energy
    public int matriculAgent;


    private void Start()
    {
        idEnergy = idPrec + 1;
        idPrec = idEnergy;
        //make a list of the agent
        foreach (GameObject agent in GameObject.FindGameObjectsWithTag("agent"))
        {
            listAgent.Add(agent);
        }
        distance = new float[listAgent.Count];
    }
    void Update()
    {
        
        if (player==null)
        {
            //check the distance vetxeen the energy pile and the different agent
            for(int i=0; i<listAgent.Count; i++)
            {
                distance[i]= Vector3.Distance(transform.position, listAgent[i].transform.position);

                //If agent has near of the pile and he want this type of pile
                if (distance[i]<=1 && listAgent[i].GetComponent<Agent>().currentState!=AgentStates.HavingEnergy)
                {
                    agent = listAgent[i].GetComponent<Agent>();
                    player = agent.transform;
                    matriculAgent = agent._name;
                }
            }
        }
        
      //if there is an agnet near and he want this energy pile
        if (player != null && agent.canTakeEnergy == idEnergy )
        {
            // check distance between objet and player
            float dist = Vector3.Distance(transform.position, player.position);

            // if - or = 0.3 distance = you can carry 
            if (dist <= 0.3f)
            {
                hasPlayer = true;
            }
            else
            {
                hasPlayer = false;
            }

            // If you can carry the object
            if (hasPlayer)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                transform.parent = player;
            }
            
        }
        


    }
        
}
