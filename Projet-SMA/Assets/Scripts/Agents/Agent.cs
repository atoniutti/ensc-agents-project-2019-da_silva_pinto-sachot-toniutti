using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public NavMeshAgent _agent;
    private Animator animator;
    public Camera _camera; // Camera of the agent in order to modified display

    public static int _precCode;
    public int _code; // Code name of the agent
    public FieldOfViewAgent _fieldOfView; // Field of view of the agent

    // Agent
    public Transform[] target; // An array where he have to go
    public Direction currentTarget; // Diection of the agent
    public AgentStates currentState;// State of the agent
    public Discussion dialogue;

    // Variable corresponding to the other agents in the scene
    public int actualDialogueWithAgent;
    public bool listenAnOtherAgent;
    public List<AgentTrust> agentsList = new List<AgentTrust>();
    public float PercentTrustStart;
    private bool BoolStartTrust;

    // Corresponding to the battery that the agent can take
    public int canTakeEnergy; //identifiant of the energy that the agent can take

    // Corresponding to the parametre of the two pile
    public float PercentOfEnergyPile;
    public float PercentOfWastePile;
    private float proba ;
    public bool checkPile;


    private void Start()
    {
        _agent = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        _code = _precCode + 1;
        currentState = AgentStates.Start;
        _precCode = _code;
        animator = GetComponent<Animator>();
        _camera.targetDisplay = _code;
        _camera.enabled = true;
        currentState = AgentStates.Start;
        BoolStartTrust = true;
        checkPile = false;
        listenAnOtherAgent = false;
    }

    private void Update()
    {
        // At the beginning the agent trust is the same for everyone
        if (BoolStartTrust)
        {
            InstanciateTrust(PercentTrustStart);
            BoolStartTrust = false;
        }

        // Pickable Objet
        // Detection of energy in the field of view of the agent 
        if ((_fieldOfView._energyFront == true && currentState == AgentStates.FindingEnergy))
        {
            if (_fieldOfView._ownerCombustible == _code)
            {
                SeeObject();
                bool go = _agent.SetDestination(new Vector3(_fieldOfView._position.position.x, transform.position.y, _fieldOfView._position.position.z));
                // if the agent enough near of the energy 
                if (_fieldOfView._energyPickable == true && go)
                {
                    currentState = AgentStates.HavingEnergy;
                    TakeObject(currentState);
                    currentState = AgentStates.GoToPileEnergy;
                    currentTarget = Direction.BatteryEnergyPoint;
                    DestinationAgent((int)currentTarget);
                }
            }
            else canTakeEnergy = 0;
        }
        
        // Detection of toxic in the field of view of the agent 
        if (_fieldOfView._toxicFront == true && currentState == AgentStates.FindingToxic)
        {
            if (_fieldOfView._ownerCombustible == _code)
            {
                SeeObject();
                bool go=_agent.SetDestination(new Vector3(_fieldOfView._position.position.x, transform.position.y, _fieldOfView._position.position.z));
                
                // If the agent enough near of the energy 
                if (_fieldOfView._energyPickable == true && go)
                {
                    currentState = AgentStates.HavingToxic;
                    TakeObject(currentState);
                    currentState = AgentStates.GoToPileToxic;
                    currentTarget = Direction.BatteryWastePoint;
                    DestinationAgent((int)currentTarget);
                }
            }
            else canTakeEnergy = 0;
        }
        
        // If nothing in front look for energy or toxic
        if ((_fieldOfView._energyFront == false && currentState == AgentStates.FindingEnergy)
            || (_fieldOfView._toxicFront == false && currentState == AgentStates.FindingToxic))
        {
            canTakeEnergy = 0;
        }

        //Agent choice
            //Choice in front of the pile : Go check the need of the different pile
        if (_fieldOfView._pileFront == true &&  checkPile == false)
        {
            PercentOfEnergyPile = _fieldOfView.percentOfEnergy;
            PercentOfWastePile = _fieldOfView.percentOfWaste;
            if((currentState == AgentStates.Idle || currentState == AgentStates.Start))
            {
                listenAnOtherAgent = true;
                currentState = MakeAChoice(PercentOfEnergyPile, PercentOfWastePile);
                checkPile = true;
            }
            AnimationMove(currentState);
        }
        
        if (_fieldOfView._pileFront == false)
        {
            checkPile = false;

        }

        //Destination of the agent if he carry nothing
        if (_fieldOfView.currentObjet == null)
        {
            //At the start of the simulation
            if(currentState==AgentStates.Start)
            {
                currentTarget = Direction.BatteryEnergyPoint;
                DestinationAgent((int)currentTarget);
                AnimationMove(currentState);
            }
            // If he meet an other agent 
            if (listenAnOtherAgent &&_fieldOfView._agentFront == true )
            {
                actualDialogueWithAgent = _fieldOfView._agentMember._code;

                //Correspond to the Discussion (enumartion) about presence of battery : HaveManyAtNorth, HaveManyAtSouth, HaveManyAtEast, HaveManyAtWest
                if ((int)_fieldOfView._agentMemberDialogue <= 5 && (int)_fieldOfView._agentMemberDialogue>=2 && currentState==AgentStates.FindingEnergy)
                {
                    Direction precedentchoiceTarget = currentTarget;
                    //Caution -2 it is in order to have the same correspondance according the AgentBhavior ( enuration)
                    if ((int)_fieldOfView._agentMemberDialogue-2 != (int)currentTarget)
                    {
                        currentTarget = MakeAChoice(precedentchoiceTarget,2, _fieldOfView._agentMemberDialogue, agentsList[Mathf.Abs(actualDialogueWithAgent -_code)-1].trust);
                        dialogue = (Discussion)_fieldOfView._agentMemberDialogue;

                        if (currentTarget != precedentchoiceTarget)
                        {
                            listenAnOtherAgent = false;
                        }
                        else listenAnOtherAgent = true;
                    }
                }
                //Correspond to the Discussion (enumartion) about no presence of battery :  DonthaveManyAtEast, DonthaveManyAtNoth, DonthaveManyAtWest, DonthaveManyAtSouth
                if ((int)_fieldOfView._agentMemberDialogue <= 9 && (int)_fieldOfView._agentMemberDialogue >= 6 && currentState == AgentStates.FindingEnergy)
                {
                    Direction precedentchoiceTarget = currentTarget;
                    //Caution -2 it is in order to have the same correspondance according the AgentBhavior ( enuration)
                    if ((int)_fieldOfView._agentMemberDialogue - 6 == (int)currentTarget && MakeAChoice(agentsList[Mathf.Abs(actualDialogueWithAgent - _code) - 1].trust))
                    {
                        _fieldOfView.numberOfPileByPlace[(int)currentTarget] = -2;
                        int maxValue = Mathf.Max(_fieldOfView.numberOfPileByPlace.ToArray());
                        currentTarget = (Direction)System.Array.IndexOf(_fieldOfView.numberOfPileByPlace.ToArray(), maxValue);
                        _fieldOfView.numberOfPileByPlace[(int)currentTarget] = 0;
                    }
                    
                }
                if ((int)_fieldOfView._agentMemberDialogue ==(int)Discussion.NeedFindToxic && currentState == AgentStates.FindingToxic && listenAnOtherAgent)
                {
                    currentTarget = Direction.ToxicPoint;
                    listenAnOtherAgent = false;
                }
                
            }
            
            if(currentState==AgentStates.FindingToxic)
            {
                currentTarget = Direction.ToxicPoint;
            }

            DestinationAgent((int)currentTarget);
            AnimationMove(currentState);

        }
        
    }

    private void InstanciateTrust(float trust)
    {
        foreach (GameObject agentOther in GameObject.FindGameObjectsWithTag("agent"))
        {
            if (_code != agentOther.GetComponent<Agent>()._code)
            {
                AgentTrust trustInAgent = new AgentTrust(agentOther.GetComponent<Agent>(), trust);
                agentsList.Add(trustInAgent);
                //Debug.Log("L'agent " + _code + " a une confiance en " + trustInAgent.agent._code + " de : " + trustInAgent.trust);
            }
        }
    }

    // When the see the object he can take this one and he wlak to him
    public void SeeObject()
    {
        canTakeEnergy = _fieldOfView._identifiant;
        animator.SetBool("walk", true);
        _agent.SetDestination(new Vector3(_fieldOfView._position.position.x, transform.position.y, _fieldOfView._position.position.z));
    }

    // When the agent is near enough to take the objet
    public void TakeObject(AgentStates state)
    {
        AnimationMove(state);
    }

    // The agent go to the destination 
    public void DestinationAgent(int TargetAgent)
    {
        _agent.SetDestination(target[TargetAgent].position);
    }



    public void AnimationMove(AgentStates agentStates)
    {
        if (agentStates==AgentStates.Idle)
        {
            animator.SetBool("walk", false);
        }
        if(agentStates==AgentStates.FindingEnergy||agentStates==AgentStates.FindingToxic 
            || agentStates==AgentStates.Start || agentStates==AgentStates.GoToPileEnergy||agentStates==AgentStates.GoToPileToxic)
        {
            animator.SetBool("walk", true);
        }
        if (agentStates==AgentStates.HavingEnergy|| agentStates==AgentStates.HavingToxic)
        {
            animator.SetTrigger("takeRessource");
        }
    }
  
    // Make a choice of Direction if an other agent tell him an direction 
    public Direction MakeAChoice(float yourDistance1, float hisDistance2, float trustOfHim)
    {
        return 0;
    }
    
    //boolean if the agent is agree or not with dialogue of the other agent
    public bool MakeAChoice(float trustOfHim)
    {
        float proba = Random.Range(0f, 1f);
        if (trustOfHim >= 80)
        {
            return (true); //if agent agree with dialogue of the other agent
        }
        if (trustOfHim < 80)
        {
            if (proba <= (trustOfHim / 100))
            {
                return (true);  //if agent agree with dialogue of the other agent
            }
            else
            {
                return false;
            }
        }
        else return false;
    }

    // Make a choice of Direction/Objectif(States) if an other agent tell him something in the discussion
    public Direction MakeAChoice(Direction yourTarget,int differenceEnum, Discussion dialogue,float trustOfHim)
    {
        float proba = Random.Range(0f, 1f);
        if (trustOfHim>=80)
        {
            return ((Direction)(dialogue-differenceEnum)); // Caution -2 it is in order to have the same correspondance according the AgentBhavior ( enuration)
        }
        if (trustOfHim < 80)
        {
            if (proba <=(trustOfHim / 100))
            {
                return ((Direction)(dialogue-differenceEnum)); // Caution -2 it is in order to have the same correspondance according the AgentBhavior ( enuration)
            }
            else
            {
                return yourTarget ;
            }
        }
        else return yourTarget;
    }

    // Make a choice of Objectif (State) when he see the two pile 
    public AgentStates MakeAChoice(float percentOfEnergy, float percentOfToxic)
    {
        proba = Random.Range(0f, 1f);
        if (percentOfEnergy >= 0 && percentOfEnergy <= 99 && percentOfToxic<=10 )
        {
            AgentStates state = AgentStates.FindingEnergy;
            return state;
        }
        if (percentOfEnergy >= 20 && percentOfEnergy <= 99 && percentOfToxic > 20 && percentOfToxic < 50)
        {
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
}
