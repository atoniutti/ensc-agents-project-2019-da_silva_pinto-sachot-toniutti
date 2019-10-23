using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public bool _energyFront= false;
    public Transform _position;
    public PickableEnergy _pile;
    public int _identifiant = 0;
    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.name == "EnergyCoil(Clone)" && _identifiant==0)
        {
            Debug.Log("energy");
            _energyFront = true;
            _pile = col.GetComponent<PickableEnergy>();
            _position = col.transform;
            _identifiant = _pile.id;
           
        }
    }
}
