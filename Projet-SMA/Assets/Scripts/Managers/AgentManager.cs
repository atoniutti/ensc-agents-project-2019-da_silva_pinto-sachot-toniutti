using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public Agent agent;
    public List<Agent> agents;
    public int numberOfAgents = 10;

    List<Transform> spawnPoints = new List<Transform>();
    float maxX = 2f;
    float maxZ = 2.3f;
    float minX = -8f;
    float minZ = -7.8f;
    float labX = -3.5f;
    float labZ = -3.3f;

    // Start is called before the first frame update
    void Start()
    {
        agents = new List<Agent>();
        GenerateRandomSpawnPoints();

        foreach (Transform sp in spawnPoints)
        {
            Spawn(sp);
        }

        foreach (Agent a in agents)
        {
            a.agentsList = agents;
        }
    }

    void GenerateRandomSpawnPoints()
    {
        for (int i = 0; i < numberOfAgents; i++)
        {
            GameObject point = new GameObject();

            // Find a random position and rotation
            float positionX = Random.Range(minX, maxX);
            float positionZ = Random.Range(minZ, maxZ);
            if (positionX > labX)
            {
                while (positionZ > labZ)
                {
                    positionZ = Random.Range(minZ, maxZ);
                }
            }

            float rotationY = Random.Range(0, 360);

            Vector3 position = new Vector3(positionX, 0, positionZ);
            Quaternion rotation = new Quaternion(0, rotationY, 0, 0);

            point.transform.SetPositionAndRotation(position, rotation);

            spawnPoints.Add(point.transform);

            Destroy(point);
        }
    }

    void Spawn(Transform spawnPoint)
    {
        // Create an instance of the agent prefab
        Instantiate(agent, spawnPoint.position, spawnPoint.rotation);
        agents.Add(agent);
    }
}
