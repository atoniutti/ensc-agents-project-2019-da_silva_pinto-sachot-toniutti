using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickableEnergy : MonoBehaviour
{
    private List<GameObject> listAgent = new List<GameObject>();
    private Transform player;
    private Transform playerCam;
    private Agent agent;
    private float[] distance = new float[2];
    private bool hasPlayer = false;
    private bool beingCarried = false;
    private bool touched = false;
    public static int idPrec = 0;
    public int id;

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
                distance[i]= Vector3.Distance(gameObject.transform.position, listAgent[i].transform.position);
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
            float dist = Vector3.Distance(gameObject.transform.position, player.position);


            // if - or = 0.3 distance = you can carry 
            if (dist <= 0.5f)
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
                beingCarried = true;
            }

            //  If you carry  the object
            if (beingCarried)
            {
                // si l'objet touche un mur / objet avec collider
                if (touched)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    transform.parent = null;
                    beingCarried = false;
                    touched = false;
                }
            }
        }
    }
        
}
