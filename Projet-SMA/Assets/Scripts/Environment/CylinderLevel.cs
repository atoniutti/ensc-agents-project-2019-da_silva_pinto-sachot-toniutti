using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderLevel : MonoBehaviour
{
    public GameObject _cylinder;
    public CylinderLevel _energyBoxEnter;//only for WasteBoxEnter
    public string nameOfTheObject;

    public float _rateStart;//level of the battery at the beggining
    private float _ratePercent;
    public float currentPercent;
    public float PercentQuantiteIn;
    public float PercentQuantiteOff;
    public float countDownUtilisateur;
    private float  _countDown;
   
    private int actualBatteryPose;
    private int batteryPose;

    private void Start()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        if (_rateStart > 50 || _rateStart < 0)
        {
            _ratePercent = 50;
        }
        else
        {
            _ratePercent = _rateStart;
        }
        _countDown = countDownUtilisateur;
        _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, FloatConverter(_ratePercent), _cylinder.transform.localPosition.z);
        actualBatteryPose = 0;
        batteryPose = 1000;
        currentPercent = _ratePercent;
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == nameOfTheObject)
        {
            batteryPose =col.GetComponent<PickableEnergy>().idEnergy;
            //Allow to verify to not put more energy that the QuantiteOfEnergy allow
            if(actualBatteryPose != batteryPose)
            {
                if (_ratePercent <= 100-PercentQuantiteIn && _ratePercent >= 0 )
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
        currentPercent = _ratePercent;
        if (_cylinder.name== "energy-cylinder")
        {
            //Decrease/ Increase lecel in fonction of time
            countDownUtilisateur -= Time.deltaTime;
            if (countDownUtilisateur <= 0.0f && _ratePercent >= PercentQuantiteOff)
            {
                _ratePercent -= PercentQuantiteOff;
                _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, FloatConverter(_ratePercent), _cylinder.transform.localPosition.z);
                countDownUtilisateur = _countDown;
            }
        }
        if (_cylinder.name == "waste-cylinder")
        {
            if (_energyBoxEnter!=null)
            {
                if(_energyBoxEnter._ratePercent >= _energyBoxEnter.PercentQuantiteOff )
                {
                    countDownUtilisateur -= Time.deltaTime;
                    if (countDownUtilisateur <= 0.0f && _ratePercent <= 100 + PercentQuantiteIn)
                    {
                        _ratePercent -= PercentQuantiteOff;
                        _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, FloatConverter(_ratePercent), _cylinder.transform.localPosition.z);
                        countDownUtilisateur = _countDown;
                    }
                }
            }
            if(_cylinder.transform.position.y > 1.6f)
            {
                _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x,1.6f, _cylinder.transform.localPosition.z);
            }
            if (_cylinder.transform.position.y < -5.7f)
            {
                _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, -5.7f, _cylinder.transform.localPosition.z);
            }
        }
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

 
