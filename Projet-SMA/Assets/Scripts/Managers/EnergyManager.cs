using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public GameObject _energy; // The energy prefab to be spawned.
    public float _spawnTime = 0.5f; // How long between each spawn.
    public Transform[] _spawnPoints; // An array of the spawn points this object can spawn from.

    // Start is called before the first frame update
    void Start()
    {
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("Spawn", _spawnTime, _spawnTime);
    }

    void Spawn()
    {
        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range(0, _spawnPoints.Length);

        // Create an instance of the energy prefab at the randomly selected spawn point's position and rotation.
        Instantiate(_energy, _spawnPoints[spawnPointIndex].position, _spawnPoints[spawnPointIndex].rotation);
    }
}
