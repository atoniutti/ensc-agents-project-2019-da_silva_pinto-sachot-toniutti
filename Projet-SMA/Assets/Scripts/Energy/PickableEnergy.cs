using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickableEnergy : MonoBehaviour
{
    private List<GameObject> listAgent = new List<GameObject>(); //list of the agent in the scene
    private Transform player;
    public Agent agent;
    private float[] distance;
    public bool hasPlayer = false;
    public static int idPrec = 0;
    public int idEnergy; //identifiant of the energy
    public int matriculAgent;


    private void Start()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
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

            //check the distance between the energy/Toxic pile and the different agent
            for(int i=0; i<listAgent.Count; i++)
            {
                distance[i]= Vector3.Distance(transform.position, listAgent[i].transform.position);

                //If agent has near of the pile and he want this type of pile
                if (distance[i]<=2  )
                {
                    if(name == "EnergyCoil(Clone)" && listAgent[i].GetComponent<Agent>().currentState == AgentStates.FindingEnergy)
                    {
                        agent = listAgent[i].GetComponent<Agent>();
                        player = agent.transform;
                        matriculAgent = agent._code;
                    }
                    if (name == "Toxic(Clone)" && listAgent[i].GetComponent<Agent>().currentState == AgentStates.FindingToxic)
                    {
                        agent = listAgent[i].GetComponent<Agent>();
                        player = agent.transform;
                        matriculAgent = agent._code;
                    }
                }
            }
        }
        
      //if there is an agnet near and he want this energy pile
        if (player != null  )
        {
            if  (agent.canTakeEnergy == idEnergy )
            {
                // check distance between objet and player
                float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.position.x, player.position.z));

                // if - or = 0.5 distance = you can carry 
                if (dist <= 0.5f)
                {
                    hasPlayer = true;
                }
                else
                {
                    hasPlayer = false;
                }

                // If you can carry the object
                if (hasPlayer==true)
                {
                    GetComponent<Rigidbody>().isKinematic = true;
                    transform.parent = player;
                    transform.localPosition = new Vector3(0.6f,6, 4);
                    transform.localRotation = new Quaternion(0f,0f,180f,0f );
                }
            }

            if(agent.canTakeEnergy != idEnergy && (agent.currentState == AgentStates.GoToPileEnergy || agent.currentState == AgentStates.GoToPileToxic))
            {
                agent = null;
                matriculAgent = 0;
                player = null;
                hasPlayer = false;
                transform.parent = null;
               

            }

        }
        
        

    }
        
}
