using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnListener : MonoBehaviour
{
    private PickableEnergy actualBattery;
    private Agent actualAgent;
    private int actualBatteryOut;
    public int numberOfPile = 0;
    private List<int> listBatteryInArea;

    void Start()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        listBatteryInArea = new List<int>();
    }

    public void Update()
    {
        numberOfPile = listBatteryInArea.Count;
    }

    // If the agent enter in the area spawn toxic 
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "EnergyCoil(Clone)")
        {

            bool batteryHere = false;
            actualBattery = col.GetComponent<PickableEnergy>();
            if (actualBattery.hasPlayer == false)
            {
                foreach (int batteryPresence in listBatteryInArea)
                {
                    if (batteryPresence == actualBattery.idEnergy)
                    {
                        batteryHere = true;
                    }
                }
                if (batteryHere == false)
                {
                    listBatteryInArea.Add(actualBattery.idEnergy);
                    batteryHere = true;
                }
            }
        }
        if (col.gameObject.tag == "agent" && listBatteryInArea.Count > 0)
        {
            actualAgent = col.GetComponent<Agent>();
            if ((actualAgent._currentState == AgentStates.GoToPileEnergy || actualAgent._currentState == AgentStates.FindingEnergy)
                && actualAgent._canTakeEnergy != 0)
            {
                actualBatteryOut = col.GetComponent<Agent>()._canTakeEnergy;
                listBatteryInArea.Remove(actualBatteryOut);
            }
        }
    }
}
