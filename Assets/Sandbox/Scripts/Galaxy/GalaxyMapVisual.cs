using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyMapVisual : MonoBehaviour {

    GalaxyMapNode[] nodes;
    GalaxyMapEdge[] edges;

    public GalaxyMapNode[] Nodes { get { return nodes; } }
    public GalaxyMapEdge[] Edges { get { return edges; } }

	// Use this for initialization
	void Awake () {
        nodes = GetComponentsInChildren<GalaxyMapNode>();
        edges = GetComponentsInChildren<GalaxyMapEdge>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public GalaxyMapNode GetDefaultNode()
    {   
        foreach(var node in Nodes)
        {
            if (node.IsDefaultLocation)
                return node;
        }

        //By default, return the first node (though this is just to avoid errors, there really should be a default set)
        return nodes[0];
    }

    //Copy Unity-side hierarchy information over to "student safe" data containers
    public GalaxyMapData GenerateGalaxyMapData()
    {
        //Data copies for GalaxyMapNodes
        GalaxyMapNodeData[] nodeDataToReturn = new GalaxyMapNodeData[nodes.Length];
        for(int i = 0; i < nodes.Length; i++)
        {
            nodeDataToReturn[i] = new GalaxyMapNodeData();
            nodeDataToReturn[i].systemName = nodes[i].name;
            nodeDataToReturn[i].galacticPosition = nodes[i].transform.position;            
        }

        //Data copies for GalaxyMapEdges
        GalaxyMapEdgeData[] edgeDataToReturn = new GalaxyMapEdgeData[edges.Length];
        for(int i = 0; i < edges.Length; i++)
        {
            edgeDataToReturn[i] = new GalaxyMapEdgeData();
            edgeDataToReturn[i].edgeCost = edges[i].EdgeCost;
            
            for(int n = 0; n < nodeDataToReturn.Length; n++)
            {
                if(nodeDataToReturn[n].systemName == edges[i].nodeA.name)
                {
                    edgeDataToReturn[i].nodeA = nodeDataToReturn[n];
                }

                if (nodeDataToReturn[n].systemName == edges[i].nodeB.name)
                {
                    edgeDataToReturn[i].nodeB = nodeDataToReturn[n];
                }
            }
        }

        foreach(var node in nodeDataToReturn)
        {
            List<GalaxyMapEdgeData> edges = new List<GalaxyMapEdgeData>();
            foreach(var edge in edgeDataToReturn)
            {
                if (edge.nodeA == node || edge.nodeB == node)
                    edges.Add(edge);
            }
            node.edges = edges.ToArray();
        }

        //Package the data up and return it to the caller
        GalaxyMapData dataToReturn = new GalaxyMapData();
        dataToReturn.nodeData = nodeDataToReturn;
        dataToReturn.edgeData = edgeDataToReturn;
        return dataToReturn;
    }

    public GalaxyMapNode FindNodeNamed(string nodeName)
    {
        foreach(var node in Nodes)
        {
            if (node.name == nodeName)
                return node;
        }

        return null;
    }

    public GalaxyMapNode FindDestinationNodeForWarpGate(WarpGate departureGate)
    {
        GameCore gameCore = FindObjectOfType<GameCore>();

        foreach (var edge in Edges)
        {
            if (edge.nodeA.name == gameCore.CurrentSolarSystemName)
            {
                if (edge.gateIndexA == departureGate.gateIndex)
                {
                    return edge.nodeB;
                }
            }
            else if (edge.nodeB.name == gameCore.CurrentSolarSystemName)
            {
                if (edge.gateIndexB == departureGate.gateIndex)
                {
                    return edge.nodeA;
                }
            }
        }

        return null;
    }
}
