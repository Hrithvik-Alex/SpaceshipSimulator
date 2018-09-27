﻿using System;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Sandbox;

public class PropulsionSubsystemController
{
    public void PropulsionUpdate(SubsystemReferences subsystemReferences, ThrusterControls thrusterControls)
    {
        string currentSystem = subsystemReferences.currentGalaxyMapNodeName;
        UnityEngine.Debug.Log(currentSystem);
        if (currentSystem == "Sol")
        {
            thrusterControls.mainThrust = 100;
            thrusterControls.portBowThrust = 50;
            thrusterControls.starboardBowThrust = 17;
            thrusterControls.portAftThrust = 50;
            thrusterControls.starboardAftThrust = 17;
        }
        else if (currentSystem == "Sirius")
        {
            thrusterControls.mainThrust = 100;
            thrusterControls.portBowThrust = 5;
            thrusterControls.starboardBowThrust = 12;
            thrusterControls.portAftThrust = 5;
            thrusterControls.starboardAftThrust = 12;
        }
        else if (currentSystem == "Barnard's Star")
        {
            thrusterControls.mainThrust = 0;
            thrusterControls.portBowThrust = 0;
            thrusterControls.starboardBowThrust = 0;
            thrusterControls.portAftThrust = 0;
            thrusterControls.starboardAftThrust = 0;
        }
    }
}