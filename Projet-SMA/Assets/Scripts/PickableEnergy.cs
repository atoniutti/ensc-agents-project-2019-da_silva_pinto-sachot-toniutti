using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickableEnergy : MonoBehaviour
{
    private List<GameObject> listAgent = new List<GameObject>(); //list of the agnet in the scene
    private Transform player;
    private Transform playerCam;
    private Agent agent;
    private float[] distance = new float[2];
    public bool hasPlayer = false;
    public static int idPrec = 0;
    public int id; //identifiant of the enrgy


    private void Start()
    {
        id = idPrec + 1;
        foreach (GameObject agent in GameObject.FindGameObjectsWithTag("agent"))
        {
            listAgent.Add(agent);
        }
        idPrec = id;
    }
    void Update()
    {
        
        if (player==null)
        {
            for(int i=0; i<listAgent.Count; i++)
            {
                distance[i]= Vector3.Distance(transform.position, listAgent[i].transform.position);
                if (distance[i]<=0.8)
                {
                    player = listAgent[i].transform;
                    playerCam = listAgent[i].transform;
                    agent = listAgent[i].GetComponent<Agent>();
                }
            }
        }
        
        if (player!=null && agent.canTake==id)
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

            // If you can carry  the object
            if (hasPlayer)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                transform.parent = playerCam;
            }

           


        }
    }
        
}
