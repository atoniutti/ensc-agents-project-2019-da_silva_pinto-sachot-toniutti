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

    //For agent
    public Agent _agentMember;
    public int _agentMemberTarget;
    public AgentStates _agentMemberState;
    public Discussion _agentMemberDialogue;
    public int[] numberOfPileByPlace;



    //Look pile
    public float percentOfEnergy ;
    public float percentOfToxic;
    //For agent 

    
    public void Start()
    {
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        _energyFront = false;
        _toxicFront = false;
        _pileFront = false;
        numberOfPileByPlace = new int[4];

    }
    private void Update()
    {
        View();
    }
    void OnTriggerEnter(Collider col)
    {
        //Look roughly the number of pile there are in the area if there are many or not( A VOIR PLUS TARD!!!)
       /* if (col.gameObject.name == "EnergyCoil(Clone)")
        {
            numberOfPile += 1;
            if (numberOfPile>1)
            {
               
            }

        }*/
        
        // If the energy enter in the field of view
        if (col.gameObject.name == "EnergyCoil(Clone)" && _owner.currentState == AgentStates.FindingEnergy && _owner.canTakeEnergy == 0)
        {
            _battery = col.GetComponent<PickableEnergy>();
            if(_battery.hasPlayer==false)
            {
                currentObjet = col.gameObject;
                _energyFront = true;
                _position = col.transform;
                _identifiant = _battery.idEnergy;
            }
           
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
        }
        
        // If the box Energy enter in the field of view 
        if (col.gameObject.name == "EnergyBoxEnterAgent")
        {
            if (currentObjet != null && currentObjet.name == "EnergyCoil(Clone)")
            {
                PoseEnergy(_energyFront);
            }
        }

        // If the box Waste enter in the field of view 
        if (col.gameObject.name == "WasteBoxEnterAgent")
        {
            if (currentObjet != null && currentObjet.name == "Toxic(Clone)")
            {
                PoseEnergy(_toxicFront);
            }
        }
        // If the box informationBox enter in the field of view 
        if (col.gameObject.name == "InformationBox")
        {
            _pileFront = true;
            percentOfEnergy = col.GetComponent<InformationPiles>().energyRate;
            percentOfToxic = col.GetComponent<InformationPiles>().toxicRate;
            
        }
        if (col.gameObject.name != "InformationBox")
        {
            _pileFront = false;
          

        }
        // If an agent enter in the field of view 
        if (col.gameObject.name == "Agent(Clone)")
        {
            _agentMember = col.GetComponent<Agent>();
            if(_agentMember._code!=_owner._code)
            {
                _agentMemberDialogue = _agentMember.dialogue;
                _agentMemberState = _agentMember.currentState;
                _agentMemberTarget = _agentMember.currentTarget;
            }
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
    public void PoseEnergy(bool front)
    {
        Destroy(currentObjet);
        _identifiant = 0;
        currentObjet = null;
        _ownerCombustible = 0;
        _position = null;
        _battery = null;
        front = false;
        _energyPickable = false;
        _owner.currentState = AgentStates.Idle;
        

    }

    public void CheckBattery()
    {

    }
}
