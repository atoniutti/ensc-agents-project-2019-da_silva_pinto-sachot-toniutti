using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentStates {Idle, FindingEnergy, FindingToxic, HavingEnergy, HavingToxic, GoToPileEnergy, GoToPileToxic, Start}
public enum AgentType { Liar, honest}
public enum Discussion
{
     NeedFindEnergy, NeedFindToxic, HaveManyAtNorth, HaveManyAtSouth, HaveManyAtEast, HaveManyAtWest,
    DonthaveManyAtNorth, DonthaveManyAtSouth, DonthaveManyAtEast, DonthaveManyAtWest,
}

public enum Direction
{
    NorthPoint, SouthPoint, EastPoint, WestPoint,ToxicPoint, BatteryWastePoint, BatteryEnergyPoint
}