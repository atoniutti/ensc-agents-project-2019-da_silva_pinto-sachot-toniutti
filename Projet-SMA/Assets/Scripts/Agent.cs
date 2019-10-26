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
    public int canTake;
    public bool bol;
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
       
            
        //detection dans le champs de vision cf : probleme pour aller d'abord sur l energy puis la pile.
        if (_fieldOfView._energyFront == true )
        {
                canTake = _fieldOfView._identifiant;
                _agent.SetDestination(new Vector3(_fieldOfView._position.position.x, _fieldOfView.transform.position.y, _fieldOfView._position.position.z));
                animator.SetBool("walk", true);

            bol = _fieldOfView._energyPickable;

            if (_fieldOfView._energyPickable == true)
            {
                animator.SetBool("walk", true);
                int i = 0;
                Debug.Log("test"+i);
                _agent.SetDestination(targetPileEnergy);
                if (transform.position.x == targetPileEnergy.x && transform.position.z == targetPileEnergy.z)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    animator.SetBool("walk", false);
                }
            }
           
        }
            
        
       
    }
    

}
