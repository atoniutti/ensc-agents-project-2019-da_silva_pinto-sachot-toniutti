using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAgent : MonoBehaviour
{
    public Agent _owner;

    //For pickable Object(Toxic and energy)
    public bool _energyFront; // boolean if gamObject energy enter in the fieldOfView
    public bool _toxicFront; // boolean if gamObject toxic enter in the fieldOfView
    public bool _pileFront; // boolean if gamObjects pile enter in the fieldOfView
    public bool _agentFront; //agent enter in the fieldOfView
    public bool _energyPickable = false; // boolean if the energy is pickable

    //For battery
    public PickableEnergy _battery;
    public int _identifiant = 0; // name of the energy
    public int _ownerCombustible; // owner of object energy
    public GameObject currentObjet; 
    public Transform _position; //position of the objet enr
    private List<int> _batterySeeList; // list of batterie see
    public bool spawnPoint;
    //For agent
    public Agent _agentMember;
    public Direction _agentMemberTarget;
    public int _agentMemberState;
    public Discussion _agentMemberDialogue;
    public int[] numberOfbattery;



    //Look pile
    public float percentOfEnergy ;
    public float percentOfWaste;
     

    
    public void Start()
    {
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        _energyFront = false;
        _toxicFront = false;
        _pileFront = false;
        numberOfbattery = new int[4];
        _batterySeeList = new List<int>();
        spawnPoint = false;

    }
    private void Update()
    {
        View();
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "EastInformationBox")
        {
            numberOfbattery[2]=col.GetComponent<SpawnListener>().numberOfPile;
            spawnPoint = true;
        }
        if (col.gameObject.name == "NorthInformationBox")
        {
            numberOfbattery[0] = col.GetComponent<SpawnListener>().numberOfPile;
            spawnPoint = true;
        }
        if (col.gameObject.name == "SouthInformationBox")
        {
            numberOfbattery[1] = col.GetComponent<SpawnListener>().numberOfPile;
            spawnPoint = true;
        }
        if (col.gameObject.name == "WestInformationBox")
        {
            numberOfbattery[3] = col.GetComponent<SpawnListener>().numberOfPile;
            spawnPoint = true;
        }

        if (col.gameObject.name == "EnergyCoil(Clone)" && _owner.currentState == AgentStates.FindingEnergy && _owner.canTakeEnergy == 0)
        {
            //Look roughly the number of pile there are in the area if there are many or not( A VOIR PLUS TARD!!!)
            _battery = col.GetComponent<PickableEnergy>();
            
            // If the energy enter in the field of view
            if(_battery.hasPlayer==false)
            {
                currentObjet = col.gameObject;
                _energyFront = true;
                _position = col.transform;
                _identifiant = _battery.idEnergy;
            }
            _pileFront = false;
        }
        // If the toxic enter in the field of view
        if (col.gameObject.name == "Toxic(Clone)" && _owner.currentState == AgentStates.FindingToxic && _owner.canTakeEnergy == 0)
        {
            _battery = col.GetComponent<PickableEnergy>();
            if (_battery.hasPlayer == false)
            {
                currentObjet = col.gameObject;
                _toxicFront = true;
                _position = col.transform;
                _identifiant = _battery.idEnergy;
            }
            _pileFront = false;
        }
        
        // If the box Energy enter in the field of view 
        if (col.gameObject.name == "EnergyBoxEnterAgent")
        {
            if (currentObjet != null && currentObjet.name == "EnergyCoil(Clone)")
            {
                PoseEnergy();
            }
        }

        // If the box Waste enter in the field of view 
        if (col.gameObject.name == "WasteBoxEnterAgent")
        {
            if (currentObjet != null && currentObjet.name == "Toxic(Clone)")
            {
                PoseEnergy();
            }
        }

        // If the box informationBox enter in the field of view 
        if (col.gameObject.name == "PileInformationBox")
        {
            
            percentOfEnergy = col.GetComponent<InformationPiles>().energyRate;
            percentOfWaste = col.GetComponent<InformationPiles>().toxicRate;
            _pileFront = true;
            spawnPoint = false;

        }

        // If an agent enter in the field of view 
        if (col.gameObject.tag== "agent" && _owner.listenAnOtherAgent)
        {
            _agentFront = true;
            _agentMember = col.GetComponent<Agent>();
            if (_agentMember._code != _owner._code)
            {
                _agentMemberDialogue = _agentMember.dialogue;
                _agentMemberState = (int)_agentMember.currentState;
                _agentMemberTarget = _agentMember.currentTarget;
            }
            
        }
        if (_owner.listenAnOtherAgent==false)
        {
            _agentFront = false;
        }
    }

    private void View()
    {
        if (_identifiant != 0 && _owner._code == _battery.matriculAgent )
        {
            _energyPickable = _battery.hasPlayer;
            _ownerCombustible = _battery.matriculAgent;
        }
    }
    public void PoseEnergy()
    {
        Destroy(currentObjet);
        _identifiant = 0;
        currentObjet = null;
        _ownerCombustible = 0;
        _position = null;
        _battery = null;
        _toxicFront = false;
        _energyFront = false;
        _pileFront = true;
        _energyPickable = false;
        _owner.currentState = AgentStates.Idle;
        

    }

    public void CheckBattery()
    {

    }
}
