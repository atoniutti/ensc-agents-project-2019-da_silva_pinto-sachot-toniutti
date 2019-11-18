using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnListener : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int actualBattery;
    public int actualAgentOut;
    public int numberOfPile;
    public List<int> listBatteryInArea = new List<int>();

    void Start()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        listBatteryInArea.Add(0);
    }

    // If the agent enter in the area spawn toxic 
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "EnergyCoil(Clone)" )
        {
            bool batteryHere = false;
            actualBattery = col.GetComponent<PickableEnergy>().idEnergy;
            foreach (int batteryPresence in listBatteryInArea)
            {
                if (batteryPresence == actualBattery)
                {
                    batteryHere = true;
                }
            }
            if (batteryHere == false)
            {

                listBatteryInArea.Add(actualBattery);
                batteryHere = false;
            }
        }
        if (col.gameObject.tag == "agent" && col.GetComponent<Agent>().currentState == AgentStates.GoToPileEnergy)
        {
            actualAgentOut = col.GetComponent<Agent>()._code;
            listBatteryInArea.Remove(actualAgentOut);
        }
    }

}

