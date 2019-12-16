using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    private static int PRE_CODE = 0;

    public NavMeshAgent _agent;
    private Animator _animator;
    public Camera _camera; // Camera of the agent in order to modified display
    public int _code; // Code name of the agent
    public FieldOfViewAgent _fieldOfView; // Field of view of the agent
    public Canvas _canvas;
    public float _countDownUtilisateur;
    private float _countDown;

    // Agent communication and mouvement 
    public Transform[] _target; // An array where he have to go
    public Direction _currentTarget; // Diection of the agent
    public Direction _previousTarget; // Previous direction of the agent
    public AgentStates _currentState; // State of the agent
    public AgentMovement _mouvement;  // current mouvement 
    public Discussion _currentDialogue;

    // Variable corresponding to the other agents in the scene
    public Agent _actualInteractionAgent;
    private AgentTrust _actualAgentTrust;
    public bool[] _listenAnOtherAgent;
    public List<AgentTrust> _agentsList = new List<AgentTrust>();
    public float _percentTrustStart;
    private bool _boolStartTrust;
    public int _fluctuationTrust;
    private int _numberOfAgent;

    // Corresponding to the battery that the agent can take
    public int _canTakeEnergy; // Identifiant of the energy that the agent can take
    private bool _doneHistoric;
    public int[] _numberOfBatteryByPlace;

    // Corresponding to the parametre of the two pile
    public float _percentOfEnergyPile;
    public float _percentOfWastePile;
    private float _probability;
    public bool _checkPile;
    private int _randomDirection;

    private void Start()
    {
        foreach (GameObject agentOther in GameObject.FindGameObjectsWithTag("agent"))
        {
            _numberOfAgent += 1;
        }
        _listenAnOtherAgent = new bool[_numberOfAgent];
        SilenceDialogue();
        _agent = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        _code = PRE_CODE + 1;
        _currentState = AgentStates.Start;
        PRE_CODE = _code;
        _animator = GetComponent<Animator>();
        _camera.enabled = false;
        _canvas.enabled = false;
        _boolStartTrust = true;
        _checkPile = false;
        _currentDialogue = Discussion.NothingToSay;
        _randomDirection = Random.Range(0, 4);
        _numberOfBatteryByPlace = new int[4];
        _actualInteractionAgent = null;
        _doneHistoric = false;
        if (_fluctuationTrust > 10 && _fluctuationTrust < 0)
        {
            _fluctuationTrust = 5;
        }
        _countDown = _countDownUtilisateur;
    }

    private void Update()
    {
        // At the beginning the agent trust is the same for everyone
        if (_boolStartTrust)
        {
            InstanciateTrust(_percentTrustStart);
            _boolStartTrust = false;
        }

        //go out or no after x seconde of the currentTarget if there is no energy
        if (_mouvement == AgentMovement.Standby && _currentState == AgentStates.FindingEnergy && PlayerPrefs.GetInt("High") == 1)
        {
            _countDown -= Time.deltaTime;
            bool stayOrNo = MakeAChoiceTrust(50);
            if (_countDown <= 0.0f && stayOrNo == true)
            {
                _currentTarget = MakeAChoiceDirection(_numberOfBatteryByPlace, _previousTarget, _currentTarget);
                _countDown = _countDownUtilisateur;
                ListenDialogue();
            }
            if (_countDown <= 0.0f && stayOrNo == false)
            {
                _countDown = _countDownUtilisateur;
            }
        }

        // Look if the percent of trust is >0 and <100
        TrustManager();

        // set the mouvement (walk or idle) of the agent
        SetAgentMovement(_currentTarget);

        // List of number of battery seen by the agent by place
        _numberOfBatteryByPlace = _fieldOfView._numberOfbattery;

        _actualInteractionAgent = _fieldOfView._agentMember;

        // Actual agent in the fieldOfview
        if (_actualInteractionAgent != null)
        {
            _currentDialogue = DialogueUpdtate(_currentState, _numberOfBatteryByPlace);
        }

        #region PICKABLE OBJECT
        // Detection of energy in the field of view of the agent 
        if ((_fieldOfView._energyFront == true && _currentState == AgentStates.FindingEnergy))
        {
            if (_fieldOfView._ownerCombustible == _code)
            {
                SilenceDialogue();
                SeeObject();
                // if the agent enough near of the energy 
                if (_fieldOfView._energyPickable == true)
                {
                    _doneHistoric = false;
                    _currentState = AgentStates.TakingEnergy;
                    TakeObject(_currentState);
                    _currentState = AgentStates.CarryingEnergyToPile;

                    if (_currentTarget != Direction.PileEnergyPoint && _currentTarget != Direction.PileWastePoint)
                    {
                        _previousTarget = _currentTarget;
                    }
                    _currentTarget = Direction.PileEnergyPoint;
                    DestinationAgent(_currentTarget);
                }
            }
            else _canTakeEnergy = 0;
        }

        // Detection of toxic in the field of view of the agent 
        if (_fieldOfView._toxicFront == true && _currentState == AgentStates.FindingAcid)
        {
            SilenceDialogue();
            if (_fieldOfView._ownerCombustible == _code)
            {
                SeeObject();
                // If the agent enough near of the energy 
                if (_fieldOfView._energyPickable == true)
                {
                    _doneHistoric = false;
                    _currentState = AgentStates.TakingAcid;
                    TakeObject(_currentState);
                    _currentState = AgentStates.CarryingAcidToPile;
                    if (_currentTarget != Direction.PileEnergyPoint && _currentTarget != Direction.PileWastePoint)
                    {
                        _previousTarget = _currentTarget;
                    }
                    _currentTarget = Direction.PileWastePoint;
                    DestinationAgent(_currentTarget);
                }
            }
            else _canTakeEnergy = 0;
        }

        // If nothing in front look for energy or toxic battery
        if ((_fieldOfView._energyFront == false && _currentState == AgentStates.FindingEnergy)
            || (_fieldOfView._toxicFront == false && _currentState == AgentStates.FindingAcid))
        {
            _canTakeEnergy = 0;
        }
        #endregion

        #region AGENT CHOICE
        // Destination of the agent if he carry nothing
        if (_fieldOfView._currentObjet == null)
        {
            // At the start of the simulation
            if (_currentState == AgentStates.Start)
            {
                _previousTarget = (Direction)_randomDirection;
                _currentTarget = Direction.PileEnergyPoint;
                DestinationAgent(_currentTarget);
                AnimationMove(_currentState);
            }

            // Choice in front of the pile : Go check the need of the different pile
            if (_fieldOfView._pileFront == true && _checkPile == false && (_currentState == AgentStates.PutObject || _currentState == AgentStates.Start))
            {
                ListenDialogue();
                _percentOfEnergyPile = _fieldOfView._percentOfEnergy;
                _percentOfWastePile = _fieldOfView._percentOfWaste;
                _currentState = MakeAChoiceState(_percentOfEnergyPile, _percentOfWastePile);
                _currentTarget = MakeAChoiceDirection(_numberOfBatteryByPlace, _previousTarget, _currentTarget);
                AnimationMove(_currentState);
                _randomDirection = Random.Range(0, 4);
                _checkPile = true;
            }

            if (_fieldOfView._pileFront == false)
            {
                _checkPile = false;
            }
            //if none agent front
            if (_fieldOfView._agentFront == false)
            {
                _actualInteractionAgent = null;
            }

            // Target if currentState = findingToxic
            if (_currentState == AgentStates.FindingAcid && _doneHistoric == false
                && _fieldOfView._energyPickable == false && _fieldOfView._currentObjet == null)
            {
                SilenceDialogue();
                if (_currentTarget != Direction.PileEnergyPoint && _currentTarget != Direction.PileWastePoint)
                {
                    _previousTarget = _currentTarget;
                }
                _currentTarget = Direction.AcidPoint;
                _doneHistoric = true;
            }

            // If the agent have an interaction with an other agent
            if (_actualInteractionAgent != null)
            {
                // Target if currentState = findingEnergy
                if (_currentState == AgentStates.FindingEnergy && _listenAnOtherAgent[_actualInteractionAgent._code - 1] == true && _doneHistoric == false
                    && _fieldOfView._energyPickable == false)
                {

                    Direction newTarget = MakeAChoiceDirection(_numberOfBatteryByPlace, _previousTarget, _currentTarget);
                    if (_currentTarget != Direction.PileEnergyPoint && _currentTarget != Direction.PileWastePoint)
                    {
                        _previousTarget = _currentTarget;
                    }
                    _currentTarget = newTarget;
                    _doneHistoric = true;
                }

                // If he meet an other agent 
                if (_listenAnOtherAgent[_actualInteractionAgent._code - 1] == true && _fieldOfView._agentFront == true && _fieldOfView._agentMember == _actualInteractionAgent
               && _currentState != AgentStates.FindingAcid && _currentState != AgentStates.CarryingAcidToPile)
                {
                    // Correspond to the Discussion (enumartion) about presence of battery : HaveManyAtNorth, HaveManyAtSouth, HaveManyAtEast, HaveManyAtWest
                    if ((int)_fieldOfView._agentMemberDialogue >= 0 && (int)_fieldOfView._agentMemberDialogue < 4 && (_currentState == AgentStates.FindingEnergy || _currentDialogue == Discussion.NeedFindEnergy))
                    {
                        Direction precedentchoiceTarget = _currentTarget;
                        _actualInteractionAgent = _fieldOfView._agentMember;
                        IndexOfAgentTrust(_actualInteractionAgent); // Attribute the value to actualAgentTrust 

                        if ((int)_fieldOfView._agentMemberDialogue != (int)_currentTarget)
                        {
                            Direction newTarget = MakeAChoice(precedentchoiceTarget, _fieldOfView._agentMemberDialogue, _agentsList[System.Array.IndexOf(_agentsList.ToArray(), _actualAgentTrust)]._trust);

                            if (newTarget != precedentchoiceTarget)
                            {
                                if (precedentchoiceTarget != Direction.PileEnergyPoint && precedentchoiceTarget != Direction.PileWastePoint)
                                {
                                    _previousTarget = _currentTarget;
                                }
                                _currentTarget = newTarget;
                                _agentsList[System.Array.IndexOf(_agentsList.ToArray(), _actualAgentTrust)]._trust += _fluctuationTrust;
                            }
                            else
                            {
                                _agentsList[System.Array.IndexOf(_agentsList.ToArray(), _actualAgentTrust)]._trust -= _fluctuationTrust;
                            }
                        }
                        if ((int)_fieldOfView._agentMemberDialogue == (int)_currentTarget)
                        {
                            _agentsList[System.Array.IndexOf(_agentsList.ToArray(), _actualAgentTrust)]._trust += _fluctuationTrust;
                        }
                        SilenceDialogue(_actualInteractionAgent._code);
                        _actualInteractionAgent = null;
                    }
                    // Correspond to the Discussion (enumartion) about no presence of battery :  DonthaveManyAtEast, DonthaveManyAtNorth, DonthaveManyAtWest, DonthaveManyAtSouth
                    // Caution -4 it is in order to have the same correspondance according the AgentBhavior (North, south,...)
                    if ((int)_fieldOfView._agentMemberDialogue >= 4 && (int)_fieldOfView._agentMemberDialogue <= 7 && _currentState == AgentStates.FindingEnergy)
                    {
                        Direction precedentchoiceTarget = _currentTarget;
                        _actualInteractionAgent = _fieldOfView._agentMember;
                        IndexOfAgentTrust(_actualInteractionAgent); // Attribute the value to actualAgentTrust 

                        if ((int)_fieldOfView._agentMemberDialogue - 4 == (int)_currentTarget
                            && MakeAChoiceTrust(_agentsList[System.Array.IndexOf(_agentsList.ToArray(), _actualAgentTrust)]._trust) == true)
                        {
                            _numberOfBatteryByPlace[(int)_currentTarget] = 0;
                            Direction newTarget = MakeAChoiceDirection(_numberOfBatteryByPlace, _previousTarget, _currentTarget);
                            if (precedentchoiceTarget != Direction.PileEnergyPoint && precedentchoiceTarget != Direction.PileWastePoint)
                            {
                                _previousTarget = _currentTarget;
                            }
                            _currentTarget = newTarget;
                            _agentsList[System.Array.IndexOf(_agentsList.ToArray(), _actualAgentTrust)]._trust += _fluctuationTrust;
                        }
                        if (MakeAChoiceTrust(_agentsList[System.Array.IndexOf(_agentsList.ToArray(), _actualAgentTrust)]._trust) == false)
                        {
                            _currentTarget = precedentchoiceTarget;
                            _agentsList[System.Array.IndexOf(_agentsList.ToArray(), _actualAgentTrust)]._trust -= _fluctuationTrust;

                        }
                        SilenceDialogue(_actualInteractionAgent._code);
                        _actualInteractionAgent = null;
                    }
                }
            }

            // Animation and  destination if currentState != GoToPileEnergy and GoToPileToxic
            if (_currentState != AgentStates.CarryingEnergyToPile && _currentState != AgentStates.CarryingAcidToPile)
            {
                DestinationAgent(_currentTarget);
                AnimationMove(_currentState);
            }
        }

        // Destination of the agent if he carry something (Toxic/Energy)
        if (_fieldOfView._currentObjet != null && _currentState == AgentStates.FindingEnergy
            && _fieldOfView._energyPickable == true && _fieldOfView._ownerCombustible == _code)
        {
            SilenceDialogue();
            _currentState = AgentStates.CarryingEnergyToPile;
            if ((_currentTarget != Direction.PileEnergyPoint && _currentTarget != Direction.PileWastePoint) && _doneHistoric == false)
            {
                _previousTarget = _currentTarget;
                _doneHistoric = true;
            }
            _currentTarget = Direction.PileEnergyPoint;
            DestinationAgent(_currentTarget);

        }

        if (_fieldOfView._currentObjet != null && _currentState == AgentStates.FindingAcid
            && _fieldOfView._energyPickable == true && _fieldOfView._ownerCombustible == _code && _currentTarget == Direction.AcidPoint)
        {
            SilenceDialogue();
            _currentState = AgentStates.CarryingAcidToPile;
            if ((_currentTarget != Direction.PileEnergyPoint && _currentTarget != Direction.PileWastePoint) && _doneHistoric == false)
            {
                _previousTarget = _currentTarget;
                _doneHistoric = true;
            }
            _currentTarget = Direction.PileWastePoint;
            DestinationAgent(_currentTarget);
        }
        #endregion 
    }

    public void RestartCode()
    {
        PRE_CODE = 0;
    }

    // Agent animation manager
    public void AnimationMove(AgentStates agentStates)
    {
        if (_mouvement == AgentMovement.Standby && _currentState == AgentStates.FindingEnergy )
        {
            _animator.SetBool("walk", false);
        }
        else
        {
            if (agentStates == AgentStates.FindingEnergy || agentStates == AgentStates.FindingAcid
            || agentStates == AgentStates.Start || agentStates == AgentStates.CarryingEnergyToPile || agentStates == AgentStates.CarryingAcidToPile)
            {
                _animator.SetBool("walk", true);
            }
            if (agentStates == AgentStates.TakingEnergy || agentStates == AgentStates.TakingAcid)
            {
                _animator.SetTrigger("takeRessource");
            }
        }

    }

    private void SetAgentMovement(Direction currenTarget)
    {
        if (currenTarget<=(Direction)4 && Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(_target[(int)currenTarget].position.x, _target[(int)currenTarget].position.z)) < 0.5)
        {
            _mouvement = AgentMovement.Standby;
        }
        else
        {
            _mouvement = AgentMovement.Move;
        }
    }
    // Instanciate trust for each agent friend
    private void InstanciateTrust(float trust)
    {
        foreach (GameObject agentOther in GameObject.FindGameObjectsWithTag("agent"))
        {
            if (_code != agentOther.GetComponent<Agent>()._code)
            {
                AgentTrust trustInAgent = new AgentTrust(agentOther.GetComponent<Agent>(), trust);
                _agentsList.Add(trustInAgent);
                // Debug.Log("L'agent " + _code + " a une confiance en " + trustInAgent.agent._code + " de : " + trustInAgent.trust);
            }
        }
    }

    // Manage to have a trust >0% and <100%
    public void TrustManager()
    {
        foreach (AgentTrust element in _agentsList)
        {
            if (element._trust > 100)
            {
                element._trust = 100;
            }
            if (element._trust < 0)
            {
                element._trust = 0;
            }
        }
    }

    // Boolean if the agent is agree or not with dialogue of the other agent
    public bool MakeAChoiceTrust(float trustOfHim)
    {
        float proba = Random.Range(0f, 1f);

        if (proba < (trustOfHim / 100))
        {
            return (true);  // If agent agree with dialogue of the other agent
        }
        else
        {
            return false;
        }
    }

    // Return the index agentTrust of the the actual interaction agent
    public void IndexOfAgentTrust(Agent actualInteraction)
    {
        foreach (AgentTrust element in _agentsList)
        {
            if (actualInteraction == element._agent)
            {
                _actualAgentTrust = element;
            }
        }
    }

    // When the see the object he can take this one and he walk to him
    public void SeeObject()
    {
        _canTakeEnergy = _fieldOfView._identifiant;
        _animator.SetBool("walk", true);
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
        _agent.SetDestination(_target[(int)TargetAgent].position);
    }

    // Make a choice of Direction/Objectif(States) if an other agent tell him something in the discussion
    public Direction MakeAChoice(Direction yourTarget, Discussion dialogue, float trustOfHim)
    {
        float proba = Random.Range(0f, 1f);
        float distanceToYourTarget = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(_target[(int)_currentTarget].position.x, _target[(int)_currentTarget].position.z));
        float distanceyToHisTarget = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(_target[(int)dialogue].position.x, _target[(int)dialogue].position.z));
        float percentAdd = 0;
        if (distanceToYourTarget - distanceyToHisTarget > 0)
        {
            percentAdd = (distanceyToHisTarget / 17) * (trustOfHim / 10); // Equivalant of 10% max of the trust in this agent
        }
        if (distanceToYourTarget - distanceyToHisTarget < 0)
        {
            percentAdd = -(distanceToYourTarget / 17) * (trustOfHim / 10); // Equivalant of -10% max of the trust in this agent and 17 represent the longer distance between to spawnPoint
        }

        if (proba <= ((trustOfHim + percentAdd) / 100))
        {
            return ((Direction)(dialogue)); // Caution -2 it is in order to have the same correspondance according the AgentBhavior ( enuration)
        }
        if (proba > ((trustOfHim + percentAdd) / 100))
        {
            return yourTarget;
        }
        else
            return ((Direction)(dialogue));
    }

    // Make a choice of Objectif (State) when he see the two pile 
    public AgentStates MakeAChoiceState(float percentOfEnergy, float percentOfToxic)
    {
        float proba = Random.Range(0f, 1f);
        if (percentOfEnergy >= 0 && percentOfEnergy <= 99 && percentOfToxic <= 20)
        {
            AgentStates state = AgentStates.FindingEnergy;
            return state;
        }
        if (percentOfEnergy <= 10)
        {
            AgentStates state = AgentStates.FindingEnergy;
            return state;
        }
        if (percentOfEnergy >= 0 && percentOfEnergy <= 99 && percentOfToxic > 20 && percentOfToxic < 70)
        {
            if (proba >= 0.6f)
            {
                AgentStates state = AgentStates.FindingAcid;
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
            AgentStates state = AgentStates.FindingAcid;
            return state;
        }
        else
        {
            if (proba >= 0.5f)
            {
                AgentStates state = AgentStates.FindingAcid;
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
        if (maxValue >= 2)
        {
            return (Direction)direction;
        }
        else
        {
            while (direction == (int)precTarget && direction == (int)curTarget)
            {
                int random = Random.Range(0, 4);
                direction = random;
            }
            return (Direction)direction;
        }
    }

    // Generate dialogue according of the need of the other agent
    public Discussion DialogueUpdtate(AgentStates stateAgent, int[] list)
    {
        int maxValue = Mathf.Max(list);

        Direction friendTarget = _fieldOfView._agentMemberTarget;
        Discussion friendDialogue = _fieldOfView._agentMemberDialogue;
        AgentStates friendState = _fieldOfView._agentMemberState;
        if (stateAgent == AgentStates.Start || stateAgent == AgentStates.PutObject)
        {
            return Discussion.NothingToSay;
        }
        if ((friendState == AgentStates.FindingEnergy || friendDialogue == Discussion.NeedFindEnergy)
            || (_currentState == AgentStates.CarryingAcidToPile || _currentState == AgentStates.CarryingEnergyToPile)
            && maxValue >= 1)
        {

            if ((_previousTarget == friendTarget || _currentTarget == friendTarget) && (int)friendTarget < 4)
            {
                if (list[(int)friendTarget] >= 1)
                {
                    return (Discussion)friendTarget;
                }
                else return (Discussion)(friendTarget + 4); // Correspond of the no presence of battery in this point
            }
            else
            {
                if (maxValue >= 1)
                {
                    return (Discussion)System.Array.IndexOf(list, maxValue);
                }
                else
                {
                    return (Discussion)_previousTarget + 4; // Correwpond of the no presence of battery in this point
                }
            }
        }
        if (_fieldOfView._currentObjet == null && maxValue == 0 && stateAgent == AgentStates.FindingEnergy)
        {
            return Discussion.NeedFindEnergy;
        }
        else
        {
            return Discussion.NothingToSay; // If the agent have no historic about the place
        }
    }

    // The agent don't listen the other agent 
    public void SilenceDialogue()
    {
        for (int i = 0; i < _numberOfAgent; i++)
        {
            _listenAnOtherAgent[i] = false;
        }
    }

    // The agent don't listen an agent
    public void SilenceDialogue(int codeAgent)
    {
        _listenAnOtherAgent[codeAgent - 1] = false;
    }

    // The agent can listen all the agent
    public void ListenDialogue()
    {
        for (int i = 0; i < _numberOfAgent; i++)
        {
            _listenAnOtherAgent[i] = true;
        }
    }
}
