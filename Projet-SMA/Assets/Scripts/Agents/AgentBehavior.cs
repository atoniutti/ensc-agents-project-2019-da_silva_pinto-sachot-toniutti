using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentStates {Idle, FindingEnergy, FindingToxic, HavingEnergy, HavingToxic, GoToPileEnergy, GoToPileToxic, Start}
public enum AgentType { Liar, honest}
public enum Discussion
{
    HaveManyAtNorth, HaveManyAtSouth, HaveManyAtEast, HaveManyAtWest,
    DontHaveManyAtNorth, DontHaveManyAtSouth, DontHaveManyAtEast, DontHaveManyAtWest, NeedFindEnergy, NeedFindToxic, IDontKnow
}

public enum Direction
{
    NorthPoint, SouthPoint, EastPoint, WestPoint,ToxicPoint, PileWastePoint, PileEnergyPoint
}