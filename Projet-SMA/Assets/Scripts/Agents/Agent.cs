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

    private static int _precCode;
    public int _code; // Code name of the agent
    public FieldOfViewAgent _fieldOfView; // Field of view of the agent

    // Agent
    public Transform[] target; // An array where he have to go
    public Direction currentTarget; // Diection of the agent
    public Direction previousTarget; // Previous direction of the agent
    public AgentStates currentState;// State of the agent
    public Discussion dialogue;

    // Variable corresponding to the other agents in the scene
    private Agent actualInteractionAgent;
    private AgentTrust actualAgentTrust;
    public bool[] listenAnOtherAgent;
    public List<AgentTrust> agentsList = new List<AgentTrust>();
    public float PercentTrustStart;
    private bool BoolStartTrust;
    public int fluctuationTrust;
    private int numberOfAgent;
    // Corresponding to the battery that the agent can take
    public int canTakeEnergy; //identifiant of the energy that the agent can take
    private bool doneHistoric;

    // Corresponding to the parametre of the two pile
    public float PercentOfEnergyPile;
    public float PercentOfWastePile;
    private float probability ;
    public bool checkPile;
    private int randomDirection;
    public int[] numberOfBatteryByPlace;

    private void Start()
    {
        foreach (GameObject agentOther in GameObject.FindGameObjectsWithTag("agent"))
        {
            numberOfAgent += 1;
        }
        listenAnOtherAgent = new bool[numberOfAgent];
        SilenceDialogue();
        _agent = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        _code = _precCode + 1;
        currentState = AgentStates.Start;
        _precCode = _code;
        animator = GetComponent<Animator>();
        _camera.enabled = false;
        BoolStartTrust = true;
        checkPile = false;
        dialogue = Discussion.NothingToSay;
        randomDirection = Random.Range(0, 4);
        numberOfBatteryByPlace = new int[4];
        actualInteractionAgent = null;
        doneHistoric = false;
        if(fluctuationTrust>10 &&fluctuationTrust<0)
        {
            fluctuationTrust = 5;
        }
    }

    private void Update()
    {
        // At the beginning the agent trust is the same for everyone
        if (BoolStartTrust)
        {
            InstanciateTrust(PercentTrustStart);
            BoolStartTrust = false;
        }

        //Look if the percent of trust is >0 and <100
        TrustManager();

        //list of number of battery seen by the agent by place
        numberOfBatteryByPlace = _fieldOfView.numberOfbattery;
        actualInteractionAgent = _fieldOfView._agentMember;
        
        //actual agent in the fieldOfview
        if(actualInteractionAgent!=null )
        {
            dialogue = DialogueUpdtate(currentState, numberOfBatteryByPlace);
        }
        
        // Pickable Objet
        // Detection of energy in the field of view of the agent 
        if ((_fieldOfView._energyFront == true && currentState == AgentStates.FindingEnergy))
        {
            if (_fieldOfView._ownerCombustible == _code)
            {
                SilenceDialogue();
                SeeObject();
                // if the agent enough near of the energy 
                if (_fieldOfView._energyPickable == true )
                {
                    doneHistoric = false;
                    currentState = AgentStates.HavingEnergy;
                    TakeObject(currentState);
                    currentState = AgentStates.GoToPileEnergy;

                    if (currentTarget!=Direction.PileEnergyPoint && currentTarget!=Direction.PileWastePoint)
                    {
                        previousTarget = currentTarget;
                    }
                    currentTarget = Direction.PileEnergyPoint;
                    DestinationAgent(currentTarget);
                }
            }
            else canTakeEnergy = 0;
        }
        
        // Detection of toxic in the field of view of the agent 
        if (_fieldOfView._toxicFront == true && currentState == AgentStates.FindingToxic)
        {
            SilenceDialogue();
            if (_fieldOfView._ownerCombustible == _code)
            {
                SeeObject();
                // If the agent enough near of the energy 
                if (_fieldOfView._energyPickable == true)
                {
                    doneHistoric = false;
                    currentState = AgentStates.HavingToxic;
                    TakeObject(currentState);
                    currentState = AgentStates.GoToPileToxic;
                    if (currentTarget != Direction.PileEnergyPoint && currentTarget != Direction.PileWastePoint)
                    {
                        previousTarget = currentTarget;
                    }
                    currentTarget = Direction.PileWastePoint;
                    DestinationAgent(currentTarget);
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
        //Destination of the agent if he carry nothing
        if (_fieldOfView.currentObjet == null)
        {
            //At the start of the simulation
            if (currentState == AgentStates.Start)
            {
                previousTarget = (Direction)randomDirection;
                currentTarget = Direction.PileEnergyPoint;
                DestinationAgent(currentTarget);
                AnimationMove(currentState);

            }

            //Choice in front of the pile : Go check the need of the different pile
            if (_fieldOfView._pileFront == true && checkPile == false && (currentState==AgentStates.Pose|| currentState == AgentStates.Start))
            {
                ListenDialogue();
                PercentOfEnergyPile = _fieldOfView.percentOfEnergy;
                PercentOfWastePile = _fieldOfView.percentOfWaste;
                currentState = MakeAChoiceState(PercentOfEnergyPile, PercentOfWastePile);
                AnimationMove(currentState);
                randomDirection = Random.Range(0, 4);
                checkPile = true;
            }
            if (_fieldOfView._pileFront == false)
            {
                checkPile = false;
            }

            //Target if currentState=findingToxic
            if (currentState == AgentStates.FindingToxic && doneHistoric==false
                && _fieldOfView._energyPickable == false && _fieldOfView.currentObjet==null)
            {
                SilenceDialogue();
                if (currentTarget!=Direction.PileEnergyPoint && currentTarget!= Direction.PileWastePoint)
                {
                    previousTarget = currentTarget;
                }
                currentTarget = Direction.ToxicPoint;
                doneHistoric = true;
            }
            
            if(actualInteractionAgent!=null)
            {
                //Target if currentState=findingEnergy
                if (currentState == AgentStates.FindingEnergy && listenAnOtherAgent[actualInteractionAgent._code - 1] == true && doneHistoric == false
                    && _fieldOfView._energyPickable == false)
                {

                    Direction newTarget = MakeAChoiceDirection(numberOfBatteryByPlace, previousTarget, currentTarget);
                    if (currentTarget != Direction.PileEnergyPoint && currentTarget != Direction.PileWastePoint)
                    {
                        previousTarget = currentTarget;
                    }
                    currentTarget = newTarget;
                    doneHistoric = true;
                }

                // If he meet an other agent 
                if (listenAnOtherAgent[actualInteractionAgent._code - 1] == true && _fieldOfView._agentFront == true && _fieldOfView._agentMember == actualInteractionAgent
               && currentState != AgentStates.FindingToxic && currentState != AgentStates.GoToPileToxic)
                {
                    //Correspond to the Discussion (enumartion) about presence of battery : HaveManyAtNorth, HaveManyAtSouth, HaveManyAtEast, HaveManyAtWest
                    if ((int)_fieldOfView._agentMemberDialogue >= 0 && (int)_fieldOfView._agentMemberDialogue < 4 && (currentState == AgentStates.FindingEnergy || dialogue == Discussion.NeedFindEnergy))
                    {
                        Direction precedentchoiceTarget = currentTarget;
                        actualInteractionAgent = _fieldOfView._agentMember;
                        IndexOfAgentTrust(actualInteractionAgent); //attribute the value to actualAgentTrust 

                        if ((int)_fieldOfView._agentMemberDialogue != (int)currentTarget)
                        {
                            Direction newTarget = MakeAChoice(precedentchoiceTarget, _fieldOfView._agentMemberDialogue, agentsList[System.Array.IndexOf(agentsList.ToArray(), actualAgentTrust)].trust);

                            if (newTarget != precedentchoiceTarget)
                            {
                                if (precedentchoiceTarget != Direction.PileEnergyPoint && precedentchoiceTarget != Direction.PileWastePoint)
                                {
                                    previousTarget = currentTarget;
                                }
                                currentTarget = newTarget;
                                agentsList[System.Array.IndexOf(agentsList.ToArray(), actualAgentTrust)].trust += fluctuationTrust;
                            }
                            else
                            {
                                agentsList[System.Array.IndexOf(agentsList.ToArray(), actualAgentTrust)].trust -= fluctuationTrust;
                            }
                        }
                        listenAnOtherAgent[actualInteractionAgent._code - 1] = false;
                        actualInteractionAgent = null;
                    }
                    //Correspond to the Discussion (enumartion) about no presence of battery :  DonthaveManyAtEast, DonthaveManyAtNorth, DonthaveManyAtWest, DonthaveManyAtSouth
                    //Caution -4 it is in order to have the same correspondance according the AgentBhavior (North, south,...)
                    if ((int)_fieldOfView._agentMemberDialogue >= 4 && (int)_fieldOfView._agentMemberDialogue <= 7 && currentState == AgentStates.FindingEnergy)
                    {
                        Direction precedentchoiceTarget = currentTarget;
                        actualInteractionAgent = _fieldOfView._agentMember;
                        IndexOfAgentTrust(actualInteractionAgent);//attribute the value to actualAgentTrust 

                        if ((int)_fieldOfView._agentMemberDialogue - 4 == (int)currentTarget
                            && MakeAChoiceTrust(agentsList[System.Array.IndexOf(agentsList.ToArray(), actualAgentTrust)].trust)==true)
                        {
                            numberOfBatteryByPlace[(int)currentTarget] = 0;
                            Direction newTarget = MakeAChoiceDirection(numberOfBatteryByPlace, previousTarget, currentTarget);
                            if (precedentchoiceTarget != Direction.PileEnergyPoint && precedentchoiceTarget != Direction.PileWastePoint)
                            {
                                previousTarget = currentTarget;
                            }
                            currentTarget = newTarget;
                            agentsList[System.Array.IndexOf(agentsList.ToArray(), actualAgentTrust)].trust += fluctuationTrust;
                        }
                        if (MakeAChoiceTrust(agentsList[System.Array.IndexOf(agentsList.ToArray(), actualAgentTrust)].trust)==false)
                        {
                            currentTarget = precedentchoiceTarget;
                            agentsList[System.Array.IndexOf(agentsList.ToArray(), actualAgentTrust)].trust -= fluctuationTrust;

                        }
                        listenAnOtherAgent[actualInteractionAgent._code - 1] = false;
                        actualInteractionAgent = null;

                    }
                }
            }
           
            //animation and  destination if currentState !=GoToPileEnergy and GoToPileToxic
            if (currentState!=AgentStates.GoToPileEnergy && currentState!=AgentStates.GoToPileToxic)
            {
                DestinationAgent(currentTarget);
                AnimationMove(currentState);
            }
        }

        //Destination of the agent if he carry something (Toxic/Energy)
        if (_fieldOfView.currentObjet!=null && currentState==AgentStates.FindingEnergy 
            && _fieldOfView._energyPickable==true && _fieldOfView._ownerCombustible == _code)
        {
            SilenceDialogue();
            currentState = AgentStates.GoToPileEnergy;
            if ((currentTarget != Direction.PileEnergyPoint && currentTarget!= Direction.PileWastePoint ) && doneHistoric == false)
            {
                previousTarget = currentTarget;
                doneHistoric = true;
            }
            currentTarget = Direction.PileEnergyPoint;
            DestinationAgent(currentTarget);
           
        }
        if (_fieldOfView.currentObjet != null && currentState == AgentStates.FindingToxic
            && _fieldOfView._energyPickable==true && _fieldOfView._ownerCombustible == _code && currentTarget==Direction.ToxicPoint )
        {
            SilenceDialogue();
            currentState = AgentStates.GoToPileToxic;
            if ((currentTarget != Direction.PileEnergyPoint && currentTarget != Direction.PileWastePoint) && doneHistoric == false)
            {
                previousTarget = currentTarget;
                doneHistoric = true;
            }
            currentTarget = Direction.PileWastePoint;
            DestinationAgent(currentTarget);
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
    public void DestinationAgent(Direction TargetAgent)
    {
        _agent.SetDestination(target[(int)TargetAgent].position);
    }

    // Agent animation manager
    public void AnimationMove(AgentStates agentStates)
    {
        if (agentStates==AgentStates.Pose ||(_agent.SetDestination(target[(int)currentTarget].position)==false && agentStates==AgentStates.FindingEnergy))
        {
            animator.SetBool("walk", false);
        }
        if((agentStates==AgentStates.FindingEnergy||agentStates==AgentStates.FindingToxic 
            || agentStates==AgentStates.Start || agentStates==AgentStates.GoToPileEnergy||agentStates==AgentStates.GoToPileToxic) && _agent.SetDestination(target[(int)currentTarget].position)==true)
        {
            animator.SetBool("walk", true);
        }
        if (agentStates==AgentStates.HavingEnergy|| agentStates==AgentStates.HavingToxic)
        {
            animator.SetTrigger("takeRessource");
        }
    }
  
    //boolean if the agent is agree or not with dialogue of the other agent
    public bool MakeAChoiceTrust(float trustOfHim)
    {
        float proba = Random.Range(0f, 1f);
        
        if (proba < (trustOfHim / 100))
        {
            return (true);  //if agent agree with dialogue of the other agent
        }
        else
        {
            return false;
        }
    }

    // Make a choice of Direction/Objectif(States) if an other agent tell him something in the discussion
    public Direction MakeAChoice(Direction yourTarget, Discussion dialogue,float trustOfHim)
    {
        float proba = Random.Range(0f, 1f);
       
        if (proba <=(trustOfHim / 100))
        {
            return ((Direction)(dialogue)); // Caution -2 it is in order to have the same correspondance according the AgentBhavior ( enuration)
        }
        else
        {
            return yourTarget ;
        }
       
    }
    
    // Make a choice of Objectif (State) when he see the two pile 
    public AgentStates MakeAChoiceState(float percentOfEnergy, float percentOfToxic)
    {
        float proba = Random.Range(0f, 1f);
        if (percentOfEnergy >= 0 && percentOfEnergy <= 99 && percentOfToxic<=20 )
        {
            AgentStates state = AgentStates.FindingEnergy;
            return state;
        }
        if (percentOfEnergy >= 20 && percentOfEnergy <= 99 && percentOfToxic > 20 && percentOfToxic < 70)
        {
            if (proba>=0.6f)
            {
                AgentStates state = AgentStates.FindingToxic;
                return state;
            }
            if (proba < 0.6f)
            {
                AgentStates state = AgentStates.FindingEnergy;
                return state;
            }
        }
        if (percentOfToxic >= 70)
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
    
    // Make a choice of Direction according the historic
    public Direction MakeAChoiceDirection(int[] list, Direction precTarget, Direction curTarget)
    {
        int maxValue = Mathf.Max(list);
        int direction = System.Array.IndexOf(list, maxValue);
        if (maxValue >= 2 )
        {
            return (Direction)direction;
        }
        else
        {
            while(direction ==(int)precTarget || direction==(int)curTarget)
            {
                int random = Random.Range(0, 4);
                direction = random;
            }
            return (Direction)direction;
        }
    }
   
    //Generate dialogue according of the need of the other agent
    public Discussion DialogueUpdtate(AgentStates stateAgent, int[] list)
    {
        int maxValue = Mathf.Max(list);
        
        Direction friendTarget = _fieldOfView._agentMemberTarget;
        Discussion friendDialogue = _fieldOfView._agentMemberDialogue;
        AgentStates friendState = _fieldOfView._agentMemberState;
        if (stateAgent == AgentStates.Start || stateAgent==AgentStates.Pose)
        {
            return Discussion.NothingToSay;
        }
        if ((friendState == AgentStates.FindingEnergy || friendDialogue == Discussion.NeedFindEnergy)
            || (currentState == AgentStates.GoToPileToxic || currentState == AgentStates.GoToPileEnergy)
            && maxValue >= 1)
        {
            
            if ((previousTarget == friendTarget || currentTarget == friendTarget) && (int)friendTarget <4)
            {
                if (list[(int)friendTarget] >= 1)
                {
                    return (Discussion)friendTarget;
                }
                else return (Discussion)(friendTarget + 4); //correwpond of the no presence of battery in this point
            }
            else
            {
                if (maxValue>=1)
                {
                    return (Discussion)System.Array.IndexOf(list, maxValue);
                }
                else
                {
                    return (Discussion)previousTarget+4; //correwpond of the no presence of battery in this point
                }
            }
        }
        if (_fieldOfView.currentObjet == null && maxValue==0 && stateAgent == AgentStates.FindingEnergy)
        {
            return Discussion.NeedFindEnergy;
        }
        else return Discussion.NothingToSay;//If the agent have no historic about the place


    }
   
    //return the index agentTrust of the the actual interaction agent
    public void IndexOfAgentTrust(Agent actualInteraction)
    {
        foreach (AgentTrust element in agentsList)
        {
            if (actualInteraction == element.agent)
            {
                actualAgentTrust=element;
            }
        }
    }

    public void SilenceDialogue()
    {
        for (int i = 0; i < numberOfAgent; i++)
        {
            listenAnOtherAgent[i] = false;
        }
    }

    public void SilenceDialogue(int codeAgent)
    {
        
            listenAnOtherAgent[codeAgent-1] = false;
        
    }
    public void ListenDialogue()
    {
        for (int i = 0; i <numberOfAgent; i++)
        {
            listenAnOtherAgent[i] = true;
        }
    }

    public void TrustManager()
    {
        foreach(AgentTrust element in agentsList)
        {
            if(element.trust>100)
            {
                element.trust = 100;
            }
            if(element.trust<0)
            {
                element.trust = 0;
            }
        }
    }
}
  

