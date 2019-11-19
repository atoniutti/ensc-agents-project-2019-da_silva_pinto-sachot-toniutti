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
    private int randomDirection;
    public int[] numberOfBatteryByPlace;

    private void Start()
    {
        _agent = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        _code = _precCode + 1;
        currentState = AgentStates.Start;
        _precCode = _code;
        animator = GetComponent<Animator>();
        _camera.enabled = false;
        currentState = AgentStates.Start;
        BoolStartTrust = true;
        checkPile = false;
        listenAnOtherAgent = false;
        dialogue = Discussion.I_Don_t_Know;
        randomDirection = Random.Range(0, 3);
        numberOfBatteryByPlace = new int[4];
    }

    private void Update()
    { 

        numberOfBatteryByPlace = _fieldOfView.numberOfbattery;
        dialogue=DialogueUpdtate(currentState, numberOfBatteryByPlace);
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
                    currentTarget = Direction.PileEnergyPoint;
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
                    currentTarget = Direction.PileWastePoint;
                    DestinationAgent((int)currentTarget);
                }
            }
            else canTakeEnergy = 0;
        }
        
        // If nothing in front look for energy or toxic battery
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
            listenAnOtherAgent = true;
            if ((currentState == AgentStates.Idle || currentState == AgentStates.Start))
            {
                currentState = MakeAChoice(PercentOfEnergyPile, PercentOfWastePile);
            }
            AnimationMove(currentState);
            randomDirection = Random.Range(0, 3);
            checkPile = true;
        }
        if (currentState==AgentStates.Idle)
        {
            currentState = MakeAChoice(PercentOfEnergyPile, PercentOfWastePile);
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
                currentTarget = Direction.PileEnergyPoint;
                DestinationAgent((int)currentTarget);
                AnimationMove(currentState);
               
            }
            if (currentState == AgentStates.FindingToxic)
            {
                currentTarget = Direction.ToxicPoint;
            }
            if ((dialogue==Discussion.NeedFindEnergy ||dialogue==Discussion.I_Don_t_Know )&&currentState==AgentStates.FindingEnergy)
            {
                currentTarget = (Direction)randomDirection;
            }
            // If he meet an other agent 
            if (listenAnOtherAgent &&_fieldOfView._agentFront == true )
            {

                //Correspond to the Discussion (enumartion) about presence of battery : HaveManyAtNorth, HaveManyAtSouth, HaveManyAtEast, HaveManyAtWest
                if ((int)_fieldOfView._agentMemberDialogue >= 0 && (int)_fieldOfView._agentMemberDialogue <= 3 &&  currentState==AgentStates.FindingEnergy)
                {
                    Direction precedentchoiceTarget = currentTarget;
                    actualDialogueWithAgent = _fieldOfView._agentMember._code;
                    
                    if ((int)_fieldOfView._agentMemberDialogue!= (int)currentTarget)
                    {
                        currentTarget = MakeAChoice(precedentchoiceTarget, _fieldOfView._agentMemberDialogue, agentsList[Mathf.Abs(actualDialogueWithAgent-_code)-1].trust);
                        
                        if (currentTarget != precedentchoiceTarget)
                        {
                            listenAnOtherAgent = false;
                        }
                        else listenAnOtherAgent = true;
                    }
                }
                //Correspond to the Discussion (enumartion) about no presence of battery :  DonthaveManyAtEast, DonthaveManyAtNoth, DonthaveManyAtWest, DonthaveManyAtSouth
                if ((int)_fieldOfView._agentMemberDialogue >= 4 && (int)_fieldOfView._agentMemberDialogue <= 7 && currentState == AgentStates.FindingEnergy)
                {
                    Direction precedentchoiceTarget = currentTarget;
                    actualDialogueWithAgent = _fieldOfView._agentMember._code;
                    //Caution -4 it is in order to have the same correspondance according the AgentBhavior ( enumeration)
                    if ((int)_fieldOfView._agentMemberDialogue - 4 == (int)currentTarget && MakeAChoice(agentsList[Mathf.Abs(actualDialogueWithAgent - _code) - 1].trust))
                    {
                        numberOfBatteryByPlace[(int)currentTarget] = 0;
                        int maxValue = Mathf.Max(numberOfBatteryByPlace);
                        currentTarget = (Direction)System.Array.IndexOf(numberOfBatteryByPlace, maxValue);
                    }
                }
                //Correspond to the Discussion (enumartion) when going to find Toxic 
                if ((int)_fieldOfView._agentMemberDialogue ==(int)Discussion.NeedFindToxic && currentState == AgentStates.FindingToxic && listenAnOtherAgent)
                {
                    currentTarget = Direction.ToxicPoint;
                    listenAnOtherAgent = false;
                }
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
    public Direction MakeAChoice(Direction yourTarget, Discussion dialogue,float trustOfHim)
    {
        float proba = Random.Range(0f, 1f);
        if (trustOfHim>=80)
        {
            return ((Direction)(dialogue)); // Caution -2 it is in order to have the same correspondance according the AgentBhavior ( enuration)
        }
        if (trustOfHim < 80)
        {
            if (proba <=(trustOfHim / 100))
            {
                return ((Direction)(dialogue)); // Caution -2 it is in order to have the same correspondance according the AgentBhavior ( enuration)
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
        float proba = Random.Range(0f, 1f);
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
    public Direction TargetMostBattery(int[]list)
    {
        int maxValue = Mathf.Max(list);
        int minValue = Mathf.Min(list);
        if(maxValue>=1)
        {
            return (Direction)System.Array.IndexOf(list, maxValue);
        }
        else
        {
            return (Direction)randomDirection;
        }
    }
   
    public Discussion DialogueUpdtate(AgentStates stateAgent, int[] list)
    {
        int maxValue = Mathf.Max(list);
        int minValue = Mathf.Min(list);
        if (maxValue > 1)
        {
            return (Discussion)System.Array.IndexOf(list, maxValue);
        }
        if (stateAgent==AgentStates.FindingEnergy && maxValue <= 1)
        {
            return Discussion.NeedFindEnergy;
        }
        if (stateAgent == AgentStates.FindingToxic)
        {
            return Discussion.NeedFindToxic;
        }
        if (stateAgent == AgentStates.Start)
        {
            return Discussion.I_Don_t_Know;
        }
        else return (Discussion.I_Don_t_Know);
        


    }
   }

