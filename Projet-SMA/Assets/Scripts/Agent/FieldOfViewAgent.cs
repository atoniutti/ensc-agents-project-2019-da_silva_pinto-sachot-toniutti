using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAgent : MonoBehaviour
{
    public Agent _owner;
    public bool _energyFront= false; // boolean if gamObject enter in the fieldOfView
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
        Destination=Random.Range(0, 4);//a supprimer test
    }
    private void Update()
    {
        View();
    }
    void OnTriggerEnter(Collider col)
    {
        //Look roughly the number of pile there are in the area if there are many or not( A VOIR PLUS TARD!!!)
        if (col.gameObject.name == "EnergyCoil(Clone)")
        {
            numberOfPile += 1;
            if (numberOfPile>1)
            {
               
                
            }

        }
        
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
        // If the box Energy enter in the field of view 
        if (col.gameObject.name == "EnergyBoxEnterAgent")
        {
            if (currentObjet != null && currentObjet.name == "EnergyCoil(Clone)")
            {
                PoseEnergy();
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
    public void PoseEnergy()
    {
        Destroy(currentObjet);
        _identifiant = 0;
        currentObjet = null;
        _ownerEnergy = 0;
        _position = null;
        _energy = null;
        _energyFront = false;
        _energyPickable = false;
        Destination = Random.Range(0, 4); //a supprimer test

    }

}
