using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public NavMeshAgent _agent;
    private Transform _moveTarget;
    private void Start()
    {
        _agent = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                var clickPosition = hit.transform.position;
                _agent.SetDestination(clickPosition);
            }
        }
    }

}
