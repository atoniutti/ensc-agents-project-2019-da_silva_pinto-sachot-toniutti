using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderLevel : MonoBehaviour
{
    public GameObject _cylinder;
    public float _rateStart;//level of the battery at the beggining
    public float _rate;
    public float addNumber;
    public string nameOfTheObject;
    public CylinderLevel energy;
    private void Start()
    {
        _rate =_rateStart;
        _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x,_rateStart, _cylinder.transform.localPosition.z);
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == nameOfTheObject)
        {
            _rate += addNumber;
            _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, _rate, _cylinder.transform.localPosition.z);
        }

    }
    //probleme a la premiere pile mise
    private void Update()
    {
        _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, _rate, _cylinder.transform.localPosition.z);
    }
    
}

 
