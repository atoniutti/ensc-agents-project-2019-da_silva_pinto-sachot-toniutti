using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderLevel : MonoBehaviour
{
    public GameObject _cylinder;
    public CylinderLevel _energyBoxEnter; // Only for WasteBoxEnter
    private string _nameOfTheObject;

    public float _rateStart; // Level of the battery at the beggining
    private float _ratePercent;
    public float _currentPercent;
    public float _percentQuantiteIn;
    public float _percentQuantiteOff;
    public float _countDownUtilisateur;
    private float _countDown;

    private int _actualBatteryPose;
    private int _batteryPose;

    private void Start()
    {
        // Initiate percent
        if(_cylinder.name== "energy-cylinder")
        {
            _rateStart = PlayerPrefs.GetFloat("EnergyPercent");
            _nameOfTheObject = "EnergyCoil(Clone)";
        }
        if (_cylinder.name == "waste-cylinder")
        {
            _rateStart = PlayerPrefs.GetFloat("WastePercent");
            _nameOfTheObject = "Toxic(Clone)";
        }

        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        if (_rateStart > 90 || _rateStart < 10)
        {
            _ratePercent = 50;
        }
        else
        {
            _ratePercent = _rateStart;
        }

        _countDown = _countDownUtilisateur;
        _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, FloatConverter(_ratePercent), _cylinder.transform.localPosition.z);
        _actualBatteryPose = 0;
        _batteryPose = 1000;
        _currentPercent = _ratePercent;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == _nameOfTheObject)
        {
            _batteryPose = col.GetComponent<PickableEnergy>()._idEnergy;
            // Allow to verify to not put more energy that the QuantiteOfEnergy allow
            if (_actualBatteryPose != _batteryPose)
            {
                if (_ratePercent <= 100 - _percentQuantiteIn && _ratePercent >= 0)
                {
                    _ratePercent += _percentQuantiteIn;
                }
                _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, FloatConverter(_ratePercent), _cylinder.transform.localPosition.z);
                _actualBatteryPose = _batteryPose;
            }
        }
    }

    // Probleme a la premiere pile mise
    private void Update()
    {
        _currentPercent = _ratePercent;
        if (_cylinder.name == "energy-cylinder")
        {
            // Decrease/Increase lecel in fonction of time
            _countDownUtilisateur -= Time.deltaTime;
            if (_countDownUtilisateur <= 0.0f && _ratePercent >= _percentQuantiteOff)
            {
                _ratePercent -= _percentQuantiteOff;
                _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, FloatConverter(_ratePercent), _cylinder.transform.localPosition.z);
                _countDownUtilisateur = _countDown;
            }
        }
        if (_cylinder.name == "waste-cylinder")
        {
            if (_energyBoxEnter != null)
            {
                if (_energyBoxEnter._ratePercent >= _energyBoxEnter._percentQuantiteOff)
                {
                    _countDownUtilisateur -= Time.deltaTime;
                    if (_countDownUtilisateur <= 0.0f && _ratePercent <= 100 + _percentQuantiteIn)
                    {
                        _ratePercent -= _percentQuantiteOff;
                        _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, FloatConverter(_ratePercent), _cylinder.transform.localPosition.z);
                        _countDownUtilisateur = _countDown;
                    }
                }
            }
            if (_cylinder.transform.position.y > 1.6f)
            {
                _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, 1.6f, _cylinder.transform.localPosition.z);
            }
            if (_cylinder.transform.position.y < -5.7f)
            {
                _cylinder.transform.localPosition = new Vector3(_cylinder.transform.localPosition.x, -5.7f, _cylinder.transform.localPosition.z);
            }
        }
    }

    private float PercentConverter(float valReal)
    {
        return ((valReal + 5.7f) / 0.071f);
    }

    private float FloatConverter(float valPercent)
    {
        return (valPercent * 0.071f - 5.7f);
    }
}
