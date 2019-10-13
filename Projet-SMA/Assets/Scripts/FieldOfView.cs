using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public bool energyFront= false;
    public Transform position;
    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.name == "EnergyCoil")
        {
            Debug.Log("energy");
            energyFront = true;
            position = col.transform;
           
        }
    }
}
