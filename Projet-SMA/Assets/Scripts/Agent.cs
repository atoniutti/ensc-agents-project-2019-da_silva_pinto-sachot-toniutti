using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public NavMeshAgent _agent;
    public static int _compteur = 0;
    public int _name;
    public FieldOfViewAgent _fieldOfView;
    public Camera _camera; //camera of the agent in order to modified display
    private Vector3 targetPileEnergy = new Vector3(-2f, 0f, -3f);
    private Transform _moveTarget;
    private Animator animator;
    public int canTake;
    private void Start()
    {
        _agent = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        _name = _compteur + 1;
        _compteur = _name;
        animator = GetComponent<Animator>();
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
        if (_fieldOfView._energyFront == true  )
        {
            if (_fieldOfView._ownerEnergy == _name)
            {
                canTake = _fieldOfView._identifiant;
                _agent.SetDestination(new Vector3(_fieldOfView._position.position.x, _fieldOfView.transform.position.y, _fieldOfView._position.position.z));
                animator.SetBool("walk", true);


                if (_fieldOfView._energyPickable == true)
                {
                    animator.SetBool("walk", true);
                    _agent.SetDestination(targetPileEnergy);
                }
            }
        }

        if (_fieldOfView.currentObjet==null)
        {
            animator.SetBool("walk", false);
        }

    }
    

}
