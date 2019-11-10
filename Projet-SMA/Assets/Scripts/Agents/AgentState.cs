using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentStates {Idle, FindingEnergy, FindingToxic, HavingEnergy, HavingToxic, Start}
public enum AgentType { Liar, honest}
public enum Discussion
{
     NeedFindEnergy, NeedFindToxic, HaveManyAtEast, HaveManyAtWest, HaveManyAtNorth, HaveManyAtSouth,
    DonthaveManyAtEast, DonthaveManyAtNoth, DonthaveManyAtWest, DonthaveManyAtSouth
}

public enum Answer
{
    FindingEnergy, FindingToxic, EastPoint, WestPoint, NorthPoint, SouthPoint
}
public enum Direction
{
    NorthPoint, SouthPoint, EastPoint, WestPoint,ToxicPoint, BatteryWastePoint, BatteryEnergyPoint
}