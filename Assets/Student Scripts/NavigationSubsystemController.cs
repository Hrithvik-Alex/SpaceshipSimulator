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
    List<WarpGate> warpGatesInCurrentGalaxy = new List<WarpGate>();
    WarpGate destinationWarpGate;

    public void NavigationUpdate(SubsystemReferences SystemReferences, GalaxyMapData galaxyMapData)
    {
        Debug.Log(galaxyMapData.nodeData[1].galacticPosition);
        destinationWarpGate = GetDestinationWarpGate();
    }

    public WarpGate GetDestinationWarpGate() 
    {
        //warpGatesInCurrentGalaxy = SystemReferences.Sensors.warpGates;

        foreach (WarpGate warpGate in warpGatesInCurrentGalaxy)
        {
            if (!visitedGalaxies.Contains(warpGate.name))
            {
                return warpGate;
            }
        }
        // Return null if warpGatesInCurrentGalaxy is empty, or if visitedGalaxies
        // contains all warp gates in current galaxy.
        return null;
    }
}