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

        int n = galaxyMapData.nodeData.Length;
        Dictionary<GalaxyMapNodeData, int> nodeToInt = new Dictionary<GalaxyMapNodeData, int>();
        for(int i = 0; i < n; i ++)
        {
            nodeToInt[galaxyMapData.nodeData[i]] = i;
        }

        GalaxyMapEdgeData[] edges = galaxyMapData.edgeData;
        int[,] adj = new int[1005,1005];

        galaxyMapNodeData root = galaxyMapData.nodeData[0];

        foreach (GalaxyMapEdgeData data in edges)
        {
            int A = data.nodeA, B = data.nodeB;
            data.edgeCost = Mathf.Pow((A.galacticPosition.x - B.galacticPosition.x), 2) - Mathf.Pow((A.galacticPosition.y - B.galacticPosition.y), 2);
            adj[nodeToInt[A]][nodeToInt[B]] = data.edgeCost;
        }

        float dist = new float[105];
        bool vis = new bool[105];
        for(int i = 0; i < 105; i ++)
        {
            dist[i] = 999999;
        }
        List<Tuple<float,int>> edgeList = new List<Tuple<float, int>>(); // cost, id of next node

        edgeList.Add(new Tuple(edges[0].edgeCost, edges[0].nodeB));

        /*while(edgeList.size() > 0)
        {
            float minEdge = edgeList[0];



            
        }*/


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