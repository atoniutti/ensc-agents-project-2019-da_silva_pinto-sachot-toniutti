using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAgent : MonoBehaviour
{
    public Agent _owner;

    // For pickable Object(Toxic and energy)
    public bool _energyFront; // Boolean if gamObject energy enter in the fieldOfView
    public bool _toxicFront; // Boolean if gamObject toxic enter in the fieldOfView
    public bool _pileFront; // Boolean if gamObjects pile enter in the fieldOfView
    public bool _agentFront; // Agent enter in the fieldOfView
    public bool _energyPickable = false; // Boolean if the energy is pickable

    // For battery(Toxic and energy)
    private PickableEnergy _battery;
    public int _identifiant = 0; // Name of the combustible
    public int _ownerCombustible; // Owner of combustible
    public GameObject currentObjet;
    public Transform _position; // Position of combustible

    // For agent
    public Agent _agentMember;
    public Direction _agentMemberTarget;
    public AgentStates _agentMemberState;
    public Discussion _agentMemberDialogue;
    public int[] numberOfbattery;

    // Look pile
    public float percentOfEnergy;
    public float percentOfWaste;

    public void Start()
    {
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        _energyFront = false;
        _toxicFront = false;
        _pileFront = false;
        numberOfbattery = new int[4];

    }

    private void Update()
    {
        View(_battery);

        // Allow to reload boolean energyPickable when the agent take the object because sometime he stay false while he have to be true 
        if (currentObjet != null)
        {
            if (currentObjet.transform.parent == _owner.transform)
            {
                _energyPickable = true;
                _ownerCombustible = currentObjet.GetComponent<PickableEnergy>().matriculAgent;
            }
        }

    }

    void OnTriggerEnter(Collider col)
    {
        // Allow to check the number of battery in the zone
        if (col.gameObject.name == "EastInformationBox")
        {
            numberOfbattery[(int)Direction.EastPoint] = col.GetComponent<SpawnListener>().numberOfPile;
        }
        if (col.gameObject.name == "NorthInformationBox")
        {
            numberOfbattery[(int)Direction.NorthPoint] = col.GetComponent<SpawnListener>().numberOfPile;
        }
        if (col.gameObject.name == "SouthInformationBox")
        {
            numberOfbattery[(int)Direction.SouthPoint] = col.GetComponent<SpawnListener>().numberOfPile;
        }
        if (col.gameObject.name == "WestInformationBox")
        {
            numberOfbattery[(int)Direction.WestPoint] = col.GetComponent<SpawnListener>().numberOfPile;
        }

        if (col.gameObject.name == "EnergyCoil(Clone)" && _owner._currentState == AgentStates.FindingEnergy && _owner._canTakeEnergy == 0)
        {
            // Look roughly the number of pile there are in the area if there are many or not( A VOIR PLUS TARD!!!)
            _battery = col.GetComponent<PickableEnergy>();

            // If the energy enter in the field of view
            if (_battery.hasPlayer == false)
            {
                _energyFront = true;
                currentObjet = col.gameObject;
                _position = col.transform;
                _identifiant = _battery.idEnergy;
            }

        }

        // If the toxic enter in the field of view
        if (col.gameObject.name == "Toxic(Clone)" && _owner._currentState == AgentStates.FindingToxic && _owner._canTakeEnergy == 0)
        {
            _battery = col.GetComponent<PickableEnergy>();
            if (_battery.hasPlayer == false)
            {
                _toxicFront = true;
                currentObjet = col.gameObject;
                _position = col.transform;
                _identifiant = _battery.idEnergy;
            }
        }

        // If the box Energy enter in the field of view 
        if (col.gameObject.name == "EnergyBoxEnterAgent")
        {
            if (currentObjet != null && currentObjet.name == "EnergyCoil(Clone)")
            {
                PutEnergy();
            }
        }

        // If the box Waste enter in the field of view 
        if (col.gameObject.name == "WasteBoxEnterAgent")
        {
            if (currentObjet != null && currentObjet.name == "Toxic(Clone)")
            {
                PutEnergy();
            }
        }

        // If the box informationBox enter in the field of view 
        if (col.gameObject.name == "PileInformationBox")
        {
            percentOfEnergy = col.GetComponent<InformationPiles>().energyRate;
            percentOfWaste = col.GetComponent<InformationPiles>().toxicRate;
            _pileFront = true;
        }

        // If an agent enter in the field of view 
        if (col.gameObject.tag == "agent")
        {
            _agentFront = true;
            _agentMember = col.GetComponent<Agent>();
            if (_agentMember._code != _owner._code)
            {
                _agentMemberDialogue = _agentMember._currentDialogue;
                _agentMemberState = _agentMember._currentState;
                _agentMemberTarget = _agentMember._currentTarget;
            }
        }

        // Check if the Agent Friend is near enough to consider if he is front of him or not
        if (_agentMember != null)
        {
            if (Vector2.Distance(new Vector2(_agentMember.transform.position.x, _agentMember.transform.position.z), new Vector2(transform.position.x, transform.position.z)) > 2)
            {
                _agentFront = false;
            }
            if (Vector2.Distance(new Vector2(_agentMember.transform.position.x, _agentMember.transform.position.z), new Vector2(transform.position.x, transform.position.z)) <= 2)
            {
                _agentFront = true;
            }
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "PileInformationBox" && _owner._checkPile)
        {
            _pileFront = false;
        }
        if (col.gameObject.name == "EastInformationBox" || col.gameObject.name == "NorthInformationBox" ||
           col.gameObject.name == "SouthInformationBox" || col.gameObject.name == "WestInformationBox")
        {
            _pileFront = false;
        }

    }

    // Check if the battery is taked or not 
    private void View(PickableEnergy battery)
    {
        if (_identifiant != 0 && battery.matriculAgent == _owner._code)
        {
            _energyPickable = battery.hasPlayer;
            _ownerCombustible = battery.matriculAgent;
        }
    }

    // When the agent put energy in the pile
    public void PutEnergy()
    {
        Destroy(currentObjet);
        _identifiant = 0;
        currentObjet = null;
        _ownerCombustible = 0;
        _position = null;
        _battery = null;
        _toxicFront = false;
        _energyFront = false;
        _energyPickable = false;
        _owner._currentState = AgentStates.PutObject;
    }
}
