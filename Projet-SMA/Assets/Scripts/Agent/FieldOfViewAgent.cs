using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAgent : MonoBehaviour
{
    public Agent _owner;
    public bool _energyFront; // boolean if gamObject energy enter in the fieldOfView
    public bool _toxicFront; // boolean if gamObject toxic enter in the fieldOfView
    public bool _energyPickable = false; // boolean if the energy is pickable
    public Transform _position;
    public PickableEnergy _energy; // gameObject Energy
    public int _identifiant = 0; // name of the energy
    public int _ownerEnergy; // owner of the gameObject energy
    public GameObject currentObjet;
    private int numberOfPile = 0;
    public int Destination;//a supprimer test
    public void Start()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        
        _energyFront = false;
        _toxicFront = false;
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
        if (col.gameObject.name == "EnergyCoil(Clone)" && _owner.currentState == AgentStates.FindingEnergy)
        {

            _energy = col.GetComponent<PickableEnergy>();
            if(_energy.hasPlayer==false)
            {
                currentObjet = col.gameObject;
                _energyFront = true;
                _position = col.transform;
                _identifiant = _energy.idEnergy;
            }
           
        }
        if (col.gameObject.name == "Toxic(Clone)" && _owner.currentState == AgentStates.FindingToxic)
        {
            _energy = col.GetComponent<PickableEnergy>();
            if (_energy.hasPlayer == false)
            {
                currentObjet = col.gameObject;
                _toxicFront = true;
                _position = col.transform;
                _identifiant = _energy.idEnergy;
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
    }
   
    private void View()
    {
        if (_identifiant != 0 && _owner._name == _energy.matriculAgent )
        {
            _energyPickable = _energy.hasPlayer;
            _ownerEnergy = _energy.matriculAgent;
        }
    }
    public void PoseEnergy(bool front)
    {
        Destroy(currentObjet);
        _identifiant = 0;
        currentObjet = null;
        _ownerEnergy = 0;
        _position = null;
        _energy = null;
        front = false;
        _energyPickable = false;
        Destination = Random.Range(0, 5); //a supprimer test

    }

    public void CheckBattery()
    {

    }
}
