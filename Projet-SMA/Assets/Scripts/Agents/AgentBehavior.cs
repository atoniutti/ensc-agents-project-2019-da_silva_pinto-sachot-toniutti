using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentStates { PutObject, FindingEnergy, FindingAcid, TakingEnergy, TakingAcid,CarryingEnergyToPile,
    CarryingAcidToPile, Start}

public enum AgentMovement { Move, Standby }

public enum AgentType { NotVeryClever, Clever }

public enum Discussion
{
    HaveManyAtSouth, HaveManyAtNorth, HaveManyAtEast, HaveManyAtWest,
    DontHaveManyAtSouth, DontHaveManyAtNorth, DontHaveManyAtEast, DontHaveManyAtWest, 
    NeedFindEnergy, NeedFindAcid, NothingToSay
}

public enum Direction
{
    SouthPoint, NorthPoint, EastPoint, WestPoint, AcidPoint, PileWastePoint, PileEnergyPoint
}