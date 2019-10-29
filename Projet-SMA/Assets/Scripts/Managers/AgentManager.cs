using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public GameObject agent;                // The agent prefab to be spawned
    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from

    // Start is called before the first frame update
    void Start()
    {
        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        Spawn(spawnPointIndex);
    }

    void GenerateRandomSpawnPoints()
    {

    }

    void Spawn(int spawnPointIndex)
    {
        // Create an instance of the agent prefab
        Instantiate(agent, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
