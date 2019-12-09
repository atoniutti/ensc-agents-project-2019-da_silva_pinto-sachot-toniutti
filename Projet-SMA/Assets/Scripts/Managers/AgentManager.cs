using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public Agent _agent;
    public List<Agent> _agents;
    public int _numberOfAgents = 5;
    

    List<Transform> _spawnPoints = new List<Transform>();
    float _maxX = 0f;
    float _maxZ = 0f;
    float _minX = -8f;
    float _minZ = -5f;
    float _labX = -3.5f;
    float _labZ = -3.3f;

    // Start is called before the first frame update
    void Start()
    {
        _numberOfAgents = PlayerPrefs.GetInt("NumberOfAgents");
        _agents = new List<Agent>();

        GenerateRandomSpawnPoints();

        foreach (Transform sp in _spawnPoints)
        {
            Spawn(sp);
        }
    }

    void GenerateRandomSpawnPoints()
    {
        for (int i = 0; i < _numberOfAgents; i++)
        {
            GameObject point = new GameObject();

            // Find a random position and rotation
            float positionX = Random.Range(_minX, _maxX);
            float positionZ = Random.Range(_minZ, _maxZ);
            if (positionX > _labX)
            {
                while (positionZ > _labZ)
                {
                    positionZ = Random.Range(_minZ, _maxZ);
                }
            }

            float rotationY = Random.Range(0, 360);

            Vector3 position = new Vector3(positionX, 0, positionZ);
            Quaternion rotation = new Quaternion(0, rotationY, 0, 0);

            point.transform.SetPositionAndRotation(position, rotation);

            _spawnPoints.Add(point.transform);

            Destroy(point);
        }
    }

    void Spawn(Transform spawnPoint)
    {
        // Create an instance of the agent prefab
        _agent = Instantiate(_agent, spawnPoint.position, spawnPoint.rotation);
        _agents.Add(_agent);
    }
}
