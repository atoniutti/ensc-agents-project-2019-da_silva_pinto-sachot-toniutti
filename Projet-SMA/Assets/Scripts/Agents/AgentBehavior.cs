using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentStates { PutObject, FindingEnergy, FindingToxic, HavingEnergy, HavingToxic, GoToPileEnergy, GoToPileToxic, Start }

public enum AgentMovement { Move, Standby }

public enum AgentType { NotVeryTalkative, Talkative }

public enum Discussion
{
    HaveManyAtSouth, HaveManyAtNorth, HaveManyAtEast, HaveManyAtWest,
    DontHaveManyAtSouth, DontHaveManyAtNorth, DontHaveManyAtEast, DontHaveManyAtWest, 
    NeedFindEnergy, NeedFindToxic, 
    NothingToSay
}

public enum Direction
{
    SouthPoint, NorthPoint, EastPoint, WestPoint, ToxicPoint, PileWastePoint, PileEnergyPoint
}