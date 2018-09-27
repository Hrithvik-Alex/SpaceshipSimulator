using Sandbox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WarpGate : MonoBehaviour
{
    [Header("Tunable")]
    public int gateIndex = -1;

    public event Action<WarpGate, Ship, string, int> OnWarpGateTriggered; //Triggering gate, triggering ship, destination GalaxyMapNode, destination WarpGate index

    //Internal
    GameCore gameCore;
    GalaxyMapVisual galaxyMap;

    private void Start()
    {
        gameCore = FindObjectOfType<GameCore>();
        galaxyMap = FindObjectOfType<GalaxyMapVisual>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ship triggeringShip = collision.gameObject.GetComponent<Ship>();
        if (triggeringShip != null)
        {   
            //Find relevant GalaxyMapEdge, if any
            foreach(var edge in galaxyMap.Edges)
            {
                if(edge.nodeA.name == gameCore.CurrentSolarSystemName)
                {
                    if(edge.gateIndexA == gateIndex)
                    {
                        //We've found the edge we're attached to, trigger a warp to the opposite node+gate
                        OnWarpGateTriggered?.Invoke(this, triggeringShip, edge.nodeB.name, edge.gateIndexB);
                    }
                } else if(edge.nodeB.name == gameCore.CurrentSolarSystemName)
                {
                    if(edge.gateIndexB == gateIndex)
                    {
                        //We've found the edge we're attached to, trigger a warp to the opposite node+gate
                        OnWarpGateTriggered?.Invoke(this, triggeringShip, edge.nodeA.name, edge.gateIndexA);
                    }
                }
            }
        }
    }

    /*
    [ContextMenu("Manually trigger gate (find Ship)")] //You can right-click on a WarpGate component to manually trigger the jump
    private void DebugTrigger()
    {      
        Ship ship = FindObjectOfType<Ship>();
        GalaxyMapVisual galaxyMap = FindObjectOfType<GalaxyMapVisual>();
        GalaxyMapNode node = galaxyMap.FindNodeNamed(destination);

        if(node != null)
        {
            OnWarpGateTriggered?.Invoke(this, ship, destination);
        }        
    }
    */
}