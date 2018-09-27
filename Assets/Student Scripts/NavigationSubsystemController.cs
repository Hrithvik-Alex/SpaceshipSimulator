using System;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;
using Sandbox;

public class NavigationSubsystemController
{
    // This is needed to read data from the sensor subsystems
    public SensorSubsystemController sensorSubsystemController;

    List<string> visitedGalaxies = new List<string>();
    List<SensorSubsystemController.WarpStruct> warpGatesInCurrentGalaxy = new List<SensorSubsystemController.WarpStruct>();

    public SensorSubsystemController.WarpStruct destinationWarpGate;

    public void NavigationUpdate(SubsystemReferences SystemReferences, GalaxyMapData galaxyMapData)
    {
        if (!visitedGalaxies.Contains(SystemReferences.currentGalaxyMapNodeName))
        {
            visitedGalaxies.Add(SystemReferences.currentGalaxyMapNodeName);
        }

        foreach (SensorSubsystemController.WarpStruct warpStruct in SystemReferences.Sensors.GWIWarpData)
        {
            warpGatesInCurrentGalaxy.Add(warpStruct);
        }

        destinationWarpGate = GetDestinationWarpGate();
    }
    
    public SensorSubsystemController.WarpStruct GetDestinationWarpGate() 
    {
        //warpGatesInCurrentGalaxy = SystemReferences.Sensors.warpGates;

        foreach (SensorSubsystemController.WarpStruct warpGate in warpGatesInCurrentGalaxy)
        {
            if (!visitedGalaxies.Contains(warpGate.warpDest))
            {
                return warpGate;
            }
        }
        // Return null if warpGatesInCurrentGalaxy is empty, or if visitedGalaxies
        // contains all warp gates in current galaxy.

        return new SensorSubsystemController.WarpStruct();
    }
}