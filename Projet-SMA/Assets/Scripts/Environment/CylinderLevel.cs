using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderLevel : MonoBehaviour
{
    public GameObject _cylinder;
    public CylinderLevel _energyBoxEnter;//only for WasteBoxEnter
    public string nameOfTheObject;
    private float _rateStart;//level of the battery at the beggining
    public float _ratePercent;
    public float PercentQuantiteIn;
    public float countdown;
    public float PercentQuantiteOff;
    public int actualBatteryPose;
    private int batteryPose;

    private void Start()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        countdown = 5;
        _rateStart = -5.7f;
        _ratePercent =0;
        _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, _rateStart, _cylinder.transform.localPosition.z);
        actualBatteryPose = 0;
        batteryPose = 1000;
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == nameOfTheObject)
        {
            batteryPose =col.GetComponent<PickableEnergy>().idEnergy;
            //Allow to verify to not put more energy that the QuantiteOfEnergy allow
            if(actualBatteryPose != batteryPose)
            {
                if (_ratePercent <= 100-PercentQuantiteIn)
                {
                    _ratePercent += PercentQuantiteIn;
                }
                _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, FloatConverter(_ratePercent), _cylinder.transform.localPosition.z);
                actualBatteryPose = batteryPose;
            }
        }
    }
    //probleme a la premiere pile mise
    private void Update()
    {
        if (_cylinder.name== "energy-cylinder")
        {
            //Decrease/ Increase lecel in fonction of time
            countdown -= Time.deltaTime;
            if (countdown <= 0.0f && _ratePercent >= PercentQuantiteOff)
            {
                _ratePercent -= PercentQuantiteOff;
                countdown = 5;
            }
        }
        if (_cylinder.name == "waste-cylinder")
        {
            if (_energyBoxEnter!=null)
            {
                if(_energyBoxEnter._ratePercent >= _energyBoxEnter.PercentQuantiteOff )
                {
                    countdown -= Time.deltaTime;
                    if (countdown <= 0.0f && _ratePercent <= 100 + PercentQuantiteIn)
                    {
                        _ratePercent -= PercentQuantiteOff;
                        countdown = 5;
                    }
                }
            }
        }
        _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, FloatConverter(_ratePercent), _cylinder.transform.localPosition.z);
    }
    private float PercentConverter(float valReal)
    {
        return((valReal+5.7f)/0.071f);
    }
    private float FloatConverter(float valPercent)
    {
        return (valPercent*0.071f-5.7f);
    }

}

 
