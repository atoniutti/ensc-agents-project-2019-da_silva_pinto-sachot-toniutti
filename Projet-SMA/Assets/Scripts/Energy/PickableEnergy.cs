using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickableEnergy : MonoBehaviour
{
    private List<GameObject> _listAgent = new List<GameObject>(); // List of the agent in the scene
    private Transform _player;
    private Agent _agent;
    private float[] _distance;
    public bool _hasPlayer = false;
    private static int _idPrec = 0;
    public int _idEnergy; // Identifiant of the energy
    public int _matriculAgent;

    private void Start()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
        _idEnergy = _idPrec + 1;
        _idPrec = _idEnergy;
        // Make a list of the agent
        foreach (GameObject agent in GameObject.FindGameObjectsWithTag("agent"))
        {
            _listAgent.Add(agent);
        }
        _distance = new float[_listAgent.Count];
    }

    void Update()
    {
        if (_player == null)
        {
            // Check the distance between the energy/Toxic pile and the different agent
            for (int i = 0; i < _listAgent.Count; i++)
            {
                _distance[i] = Vector3.Distance(transform.position, _listAgent[i].transform.position);

                // If agent has near of the pile and he want this type of pile
                if (_distance[i] <= 1)
                {
                    if (name == "EnergyCoil(Clone)" && _listAgent[i].GetComponent<Agent>()._currentState == AgentStates.FindingEnergy)
                    {
                        _agent = _listAgent[i].GetComponent<Agent>();
                        _player = _agent.transform;
                        _matriculAgent = _agent._code;
                    }
                    if (name == "Toxic(Clone)" && _listAgent[i].GetComponent<Agent>()._currentState == AgentStates.FindingAcid)
                    {
                        _agent = _listAgent[i].GetComponent<Agent>();
                        _player = _agent.transform;
                        _matriculAgent = _agent._code;
                    }
                }
            }
        }

        // If there is an agnet near and he want this energy pile
        if (_player != null)
        {
            if (_agent._canTakeEnergy == _idEnergy)
            {
                // Check distance between objet and player
                float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(_player.position.x, _player.position.z));

                // If - or = 0.6 distance = you can carry 
                if (dist <= 0.6f)
                {
                    _hasPlayer = true;
                }
                else
                {
                    _hasPlayer = false;
                }

                // If you can carry the object
                if (_hasPlayer == true)
                {
                    GetComponent<Rigidbody>().isKinematic = true;
                    _matriculAgent = _agent._code;
                    transform.parent = _player;
                    transform.localPosition = new Vector3(0.6f, 6, 4);
                    transform.localRotation = new Quaternion(0f, 0f, 180f, 0f);
                }
            }

            if (_agent._canTakeEnergy != _idEnergy && (_agent._currentState == AgentStates.CarryingEnergyToPile || _agent._currentState == AgentStates.CarryingAcidToPile))
            {
                _agent = null;
                _matriculAgent = 0;
                _player = null;
                _hasPlayer = false;
                transform.parent = null;
            }
        }
    }
}
