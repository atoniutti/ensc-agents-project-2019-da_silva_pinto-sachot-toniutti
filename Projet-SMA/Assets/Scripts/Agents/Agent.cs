using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{
    public NavMeshAgent _agent;
    private Animator animator;
    public Camera _camera; //camera of the agent in order to modified display

    public static int _precCode;
    public int _code; // code name of the agent
    public Text _name; // name of the agent
    public FieldOfViewAgent _fieldOfView; //field of view of the agent

    public Transform[] target; // An array where he have to go
    public int currentTarget; //diection of the agent
    public AgentStates currentState;// State of the agent
    public Discussion dialogue;
    public int canTakeEnergy; //identifiant of the energy that the agent can take
    public float PercentOfEnergyPile;
    public float PercentOfToxicPile;


    private void Start()
    {
        _agent = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        _code = _precCode + 1;
        currentState = AgentStates.Idle;
        _precCode = _code;
        animator = GetComponent<Animator>();
        _camera.targetDisplay = _code;
        _camera.enabled = true;
        currentState = AgentStates.Start;
        
    }

    private void Update()
    {
        
        //Pickable Objet
        //detection of energy in the field of view of the agent 
        if ((_fieldOfView._energyFront == true && currentState == AgentStates.FindingEnergy))
        {
            if (_fieldOfView._ownerCombustible == _code)
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
            else canTakeEnergy = 0;
        }

        //detection of toxic in the field of view of the agent 
        if (_fieldOfView._toxicFront == true && currentState == AgentStates.FindingToxic)
        {
            if (_fieldOfView._ownerCombustible == _code)
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
            else canTakeEnergy = 0;
        }

        //If nothing in front look for energy or toxic
        if ((_fieldOfView._energyFront == false && currentState == AgentStates.FindingEnergy)
            || (_fieldOfView._toxicFront == false && currentState == AgentStates.FindingToxic))
        {
            canTakeEnergy = 0;
        }

        //Agent choice
        //Destination of the agent 
        if (_fieldOfView.currentObjet == null)
        {
            if(currentState==AgentStates.Start)
            {
                currentTarget = (int)Direction.BatteryEnergyPoint;
                DestinationAgent(currentTarget);
            }
            if (_fieldOfView._pileFront == true && (currentState == AgentStates.Idle|| currentState == AgentStates.Start))
            {
                PercentOfEnergyPile = _fieldOfView.percentOfEnergy;
                PercentOfToxicPile = _fieldOfView.percentOfToxic;
                currentState = MakeAChoice(PercentOfEnergyPile, PercentOfToxicPile);

            }
            
            /*if (_fieldOfView._agentFront==true)
            {

            }*/
            //  Pour tester (a supprimer)
            if (currentState == AgentStates.FindingToxic)
            {
                currentTarget = 4;
            }
            if (currentState == AgentStates.FindingEnergy)
            {
                currentTarget =0;
            }
            if (currentState == AgentStates.Start)
            {
                currentTarget = 5;
               
            }
            _agent.SetDestination(target[currentTarget].position);
            animator.SetBool("walk", true);
        }

    }

    //When the see the object he can take this one and he wlak to him
    public void SeeObject()
    {
        canTakeEnergy = _fieldOfView._identifiant;
        animator.SetBool("walk", true);
        _agent.SetDestination(new Vector3(_fieldOfView._position.position.x, transform.position.y, _fieldOfView._position.position.z));
    }
    //When the agent is near enough to take the objet
    public void TakeObject()
    {
        animator.SetTrigger("takeRessource");
        animator.SetBool("walk", true);
    }
    
    //Make a choice of Direction if an other agent tell him an direction 
    public Direction MakeAChoice(float yourDistance1, float hisDistance2, float trustOfHim)
    {
        return 0;
    }

    //Make a choice of Direction/Objectif(States) if an other agent tell him something in the discussion
    public string MakeAChoice(Discussion dialogue,float trustOfHim)
    {
        float proba = Random.Range(0f, 1f);
        if (trustOfHim>=80)
        {
            return (dialogue.ToString());
        }
        if (trustOfHim < 80)
        {
            if (proba < (trustOfHim / 100))
            {
                return (dialogue.ToString());
            }
            else
            {
                return "IDontTrustYou" ;
            }
        }
        else return "IDontTrustYou";
    }
    //Make a choice of Objectif (State) when he see the two pile 
    public AgentStates MakeAChoice(float percentOfEnergy, float percentOfToxic)
    {
        if (percentOfEnergy >= 0 && percentOfEnergy < 99 && percentOfToxic<10 )
        {
            AgentStates state = AgentStates.FindingEnergy;
            return state;
        }
        if (percentOfEnergy >= 0 && percentOfEnergy < 99 && percentOfToxic > 30 && percentOfToxic < 60)
        {
            float proba = Random.Range(0f, 1f);
            if (proba>=0.5f)
            {
                AgentStates state = AgentStates.FindingToxic;
                return state;
            }
            if (proba < 0.5f)
            {
                AgentStates state = AgentStates.FindingEnergy;
                return state;
            }
        }
        if (percentOfToxic >= 80)
        {
            AgentStates state = AgentStates.FindingToxic;
            return state;
        }
        else
        {
            float proba = Random.Range(0.0f, 1.0f);
            if (proba >= 0.5f)
            {
                AgentStates state = AgentStates.FindingToxic;
                return state;
            }
            else
            {
                AgentStates state = AgentStates.FindingEnergy;
                return state;
            }
        }
        
    }

    //The agent go to the destination 
    public void DestinationAgent(int TargetAgent)
    {
        _agent.SetDestination(target[TargetAgent].position);
    }

   
    

    public int ChoiceDestination()
    {
        return 0;
    }
}
