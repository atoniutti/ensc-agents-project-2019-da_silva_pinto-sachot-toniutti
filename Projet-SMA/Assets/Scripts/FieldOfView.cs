using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public bool _energyFront= false;
    public bool _energyPickable = false;
    public Transform _position;
    public PickableEnergy _energy;
    public int _identifiant = 0;

    private void Update()
    {
        if(_identifiant!=0)
        {
            _energyPickable = _energy.hasPlayer;
        }
    }
    // If the energy enter in the field of view
    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.name == "EnergyCoil(Clone)" && _identifiant==0)
        {
            Debug.Log("energy");
            _energyFront = true;
            _energy = col.GetComponent<PickableEnergy>();
            _position = col.transform;
            _identifiant = _energy.id;
        }
      


    }

    // If the energy is destroy by the pile
    private void OnTriggerExit(Collider col)
    {
        
    }

}
