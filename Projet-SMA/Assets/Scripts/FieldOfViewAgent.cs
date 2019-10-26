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
    private void Update()
    {
        View();

    }
    // If the energy enter in the field of view
    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.name == "EnergyCoil(Clone)" )
        {
            currentObjet = col.gameObject;
            _energyFront = true;
            _energy = col.GetComponent<PickableEnergy>();
            _position = col.transform;
            _identifiant = _energy.id;
            
        }
        if(col.gameObject.name == "EnergyBoxEnter")
        {
            if(currentObjet!=null)
            {
                PoseEnergy();
            }
        }
    }

    // If the energy is destroy by the pile
     void OnTriggerExit(Collider col)
    {
        
    }
    private void View()
    {
        if (_identifiant != 0 && _owner._name == _energy.matriculAgent)
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
    }

}
