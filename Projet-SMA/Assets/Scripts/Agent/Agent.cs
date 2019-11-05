using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{
    public NavMeshAgent _agent;
    public static int _precCode = 0;
    public int _code; // code name of the agent
    public Text _name; // name of the agent
    public FieldOfViewAgent _fieldOfView; //field of view of the agent
    public Camera _camera; //camera of the agent in order to modified display
    public Transform[] target; // An array where he have to go
    private Vector3 targetPileEnergy = new Vector3(-2f, 0f, -3f);
    private Animator animator;
    public int canTakeEnergy; //identifiant of the energy that the agent can take
    public AgentStates currentState;
    public GameObject pointPosition;
    public Canvas canvasAgent;
    public List<Agent> agentsList = new List<Agent>();

    private void Start()
    {
        _agent = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        _code = _precCode + 1;
        _name.text = GetNameCode();
        currentState = AgentStates.Idle;
        _precCode = _code;
        animator = GetComponent<Animator>();
        _camera.targetDisplay = _code;
        _camera.enabled = true;

        // Canvas Agent
        ScrollRect sc = canvasAgent.GetComponent<ScrollRect>();
        foreach (Agent a in agentsList)
        {
            //AgentButton agentButton = new AgentButton();
            //agentButton.transform.SetParent(sc.content);
        }
    }

    private void Update()
    {
        // Point on Minimap
        if (Camera.current == _camera)
        {
            // Point Position
            pointPosition.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            pointPosition.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);

            // Canvas Agent
            canvasAgent.enabled = false;
        }
        else
        {
            // Point Position
            pointPosition.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            pointPosition.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);

            // Canvas Agent
            canvasAgent.enabled = true;
        }

        //detection of energy in the field of view of the agent 
        if (_fieldOfView._energyFront == true && currentState == AgentStates.FindingEnergy)
        {
            if (_fieldOfView._ownerEnergy == _code)
            {
                canTakeEnergy = _fieldOfView._identifiant;
                animator.SetBool("walk", true);
                _agent.SetDestination(new Vector3(_fieldOfView._position.position.x, transform.position.y, _fieldOfView._position.position.z));

                // if the agent enough near of the energy 
                if (_fieldOfView._energyPickable == true && _agent.SetDestination(new Vector3(_fieldOfView._position.position.x, transform.position.y, _fieldOfView._position.position.z)))
                {
                    currentState = AgentStates.HavingEnergy;
                    animator.SetTrigger("takeRessource");
                    animator.SetBool("walk", true);
                    _agent.SetDestination(target[(int)Direction.BatteryEnergyPoint].position); //agent go to the battery
                }
            }
            else
            {
                canTakeEnergy = 0;
            }
        }

        //detection of toxic in the field of view of the agent 
        if (_fieldOfView._toxicFront == true && currentState == AgentStates.FindingToxic)
        {
            if (_fieldOfView._ownerEnergy == _code)
            {
                canTakeEnergy = _fieldOfView._identifiant;
                animator.SetBool("walk", true);
                _agent.SetDestination(new Vector3(_fieldOfView._position.position.x, transform.position.y, _fieldOfView._position.position.z));

                // if the agent enough near of the energy 
                if (_fieldOfView._energyPickable == true && _agent.SetDestination(new Vector3(_fieldOfView._position.position.x, transform.position.y, _fieldOfView._position.position.z)))
                {
                    currentState = AgentStates.HavingToxic;
                    animator.SetTrigger("takeRessource");
                    animator.SetBool("walk", true);
                    _agent.SetDestination(target[(int)Direction.BatteryWastePoint].position); //agent go to the battery
                }
            }
            else
            {
                canTakeEnergy = 0;
            }
        }

        if ((_fieldOfView._energyFront == false && currentState == AgentStates.FindingEnergy)
            || (_fieldOfView._toxicFront == false && currentState == AgentStates.FindingToxic))
        {
            canTakeEnergy = 0;
        }

        //if the object is posed or destroyed
        if (_fieldOfView.currentObjet == null)
        {

            _agent.SetDestination(target[_fieldOfView.Destination].position);
            //  Pour tester (a supprimer)
            if (_fieldOfView.Destination == 4)
            {
                currentState = AgentStates.FindingToxic;
            }

            if (_fieldOfView.Destination != 4)
            {
                currentState = AgentStates.FindingEnergy;
            }

            animator.SetBool("walk", true);
        }
    }

    private string GetNameCode()
    {
        string name = "Agent ";

        if (_code < 10)
        {
            name += "00" + _code;
        }
        else
        {
            name += "0" + _code;
        }

        return name;
    }
}
