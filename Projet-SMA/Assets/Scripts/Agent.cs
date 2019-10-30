using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public NavMeshAgent _agent;
    public static int _precName = 0;
    public int _name; // name of the agent
    public FieldOfViewAgent _fieldOfView; //field of view of the agent
    public Camera _camera; //camera of the agent in order to modified display
    public Transform[] target; // An array where he have to go
    private Vector3 targetPileEnergy = new Vector3(-2f, 0f, -3f);
    private Animator animator;
    public int canTakeEnergy; //identifiant of the energy that the agent can take
    public AgentStates currentState ;
    private void Start()
    {
        _agent = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        _name = _precName + 1;
        currentState = AgentStates.Idle;
        _precName = _name;
        animator = GetComponent<Animator>();
        _camera.targetDisplay = _name;
    }
    private void Update()
    {  
        //detection of energy in the field of view of the agent 
        if (_fieldOfView._energyFront == true  && currentState == AgentStates.FindingEnergy)
        {
            if (_fieldOfView._ownerEnergy == _name)
            {
                
                canTakeEnergy = _fieldOfView._identifiant;
                animator.SetBool("walk", true);
                _agent.SetDestination(new Vector3(_fieldOfView._position.position.x, _fieldOfView.transform.position.y, _fieldOfView._position.position.z));

                // if the agent enough near of the energy 
                if (_fieldOfView._energyPickable == true)
                {

                    currentState = AgentStates.HavingEnergy;
                    animator.SetTrigger("takeRessource");
                    animator.SetBool("walk", true);
                    _agent.SetDestination(target[(int)Direction.BatteryEnergyPoint].position); //agent go to the battery
                }
            }
        }
        if (_fieldOfView._energyFront == false)
        {
            canTakeEnergy = 0;
        }
        //if the object is posed or destroyed
        if (_fieldOfView.currentObjet==null)
        {
            animator.SetBool("walk", false);
        }

    }
    

}
