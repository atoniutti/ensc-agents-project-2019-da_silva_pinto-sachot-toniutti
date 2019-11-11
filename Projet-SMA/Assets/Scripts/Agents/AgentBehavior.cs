using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentStates {Idle, FindingEnergy, FindingToxic, HavingEnergy, HavingToxic, GoToPileEnergy, GoToPileToxic, Start}
public enum AgentType { Liar, honest}
public enum Discussion
{
     NeedFindEnergy, NeedFindToxic, HaveManyAtNorth, HaveManyAtSouth, HaveManyAtEast, HaveManyAtWest, 
    DonthaveManyAtEast, DonthaveManyAtNoth, DonthaveManyAtWest, DonthaveManyAtSouth,IDoNotKnow
}

public enum Answer
{
    FindingEnergy, FindingToxic, EastPoint, WestPoint, NorthPoint, SouthPoint
}
public enum Direction
{
    NorthPoint, SouthPoint, EastPoint, WestPoint,ToxicPoint, BatteryWastePoint, BatteryEnergyPoint
}