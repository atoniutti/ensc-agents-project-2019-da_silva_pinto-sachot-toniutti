using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentStates {Idle, FindingEnergy, FindingToxic, HavingEnergy, HavingToxic}
public enum AgentType { Liar, slave }
public enum Discussion { GoWestPoint, GoNorthPoint, GoEastPoint, GoSouthPoint, NeedFindToxic, NeedFindEnergy, haveManyAtEast, haveManyAtWest, haveManyAtNoth, haveManyAtSouth,
    DonthaveManyAtEast, DonthaveManyAtNoth, DonthaveManyAtWest, DonthaveManyAtSouth }

public enum Direction { EastPoint,  NorthPoint, SouthPoint, WestPoint,ToxicPoint, BatteryWastePoint, BatteryEnergyPoint, }