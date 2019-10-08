using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public NavMeshAgent _agent;
    public FieldOfView _fieldOfView;
    private Vector3 targetPileEnergy ;
    private Transform _moveTarget;
    private Animator animator;
    private void Start()
    {
        _agent = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        animator = GetComponent<Animator>();
        targetPileEnergy = new Vector3(-0.98f, 0f, -3.65f);
    }
    private void Update()
    {
        //inutile 
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
       
            
        //detection dans le champs de vision
        if (_fieldOfView.energyFront == true)
        {
            animator.SetBool("walk", true);
            _agent.SetDestination(targetPileEnergy);
            if (transform.position.x == targetPileEnergy.x && transform.position.z == targetPileEnergy.z)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    animator.SetBool("walk", false);
            }
        }
       
    }
    

}
