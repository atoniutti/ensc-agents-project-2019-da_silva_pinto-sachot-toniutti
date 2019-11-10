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
    private Animator animator;
    public Camera _camera; //camera of the agent in order to modified display

    public FieldOfViewAgent _fieldOfView; //field of view of the agent
    public Transform[] target; // An array where he have to go
    public int currentTarget;
    public int canTakeEnergy; //identifiant of the energy that the agent can take
    public AgentStates currentState;// State of the agent
    public GameObject pointPosition;
    public Canvas canvasAgent;
    public List<AgentTrust> agentsList = new List<AgentTrust>();

    AgentButton agentButton;

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
        currentTarget = (int)Direction.BatteryEnergyPoint;
        DestinationAgent(currentTarget);
    }

    private void Update()
    {
        //UI Agent
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

        if (_fieldOfView._pileFront == true)
        {
            //decider de trouver de l'energy ou du toxic
            
        }

            //Pickable Objet
            //detection of energy in the field of view of the agent 
            if (_fieldOfView._energyFront == true && currentState == AgentStates.FindingEnergy)
        {
            if (_fieldOfView._ownerEnergy == _code)
            {
                SeeObject();

                // if the agent enough near of the energy 
                if (_fieldOfView._energyPickable == true && _agent.SetDestination(new Vector3(_fieldOfView._position.position.x, transform.position.y, _fieldOfView._position.position.z)))
                {
                    currentState = AgentStates.HavingEnergy;
                    TakeObject();
                    currentTarget = (int)Direction.BatteryEnergyPoint;
                    DestinationAgent(currentTarget);
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
                SeeObject();

                // if the agent enough near of the energy 
                if (_fieldOfView._energyPickable == true && _agent.SetDestination(new Vector3(_fieldOfView._position.position.x, transform.position.y, _fieldOfView._position.position.z)))
                {
                    currentState = AgentStates.HavingToxic;
                    TakeObject();
                    currentTarget = (int)Direction.BatteryWastePoint;
                    DestinationAgent(currentTarget);
                }
            }
            else
            {
                canTakeEnergy = 0;
            }
        }
        //If nothing in front look for energy or toxic
        if ((_fieldOfView._energyFront == false && currentState == AgentStates.FindingEnergy)
            || (_fieldOfView._toxicFront == false && currentState == AgentStates.FindingToxic))
        {
            canTakeEnergy = 0;
        }
        //Communication Between Agent




        //Destination of the agent 
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
    public void SeeObject()
    {
        canTakeEnergy = _fieldOfView._identifiant;
        animator.SetBool("walk", true);
        _agent.SetDestination(new Vector3(_fieldOfView._position.position.x, transform.position.y, _fieldOfView._position.position.z));
    }
    public void TakeObject()
    {
        animator.SetTrigger("takeRessource");
        animator.SetBool("walk", true);
        
    }
    public void DestinationAgent(int TargetAgent)
    {
        _agent.SetDestination(target[TargetAgent].position);
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

    public int ChoiceDestination()
    {
        return 0;
    }
}
