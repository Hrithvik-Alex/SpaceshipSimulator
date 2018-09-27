using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sandbox
{
    public class SubsystemReferences
    {
        // Subsystems
        public SensorSubsystemController Sensors { get; private set; }
        public DefenceSubsystemController Defence { get; private set; }
        public NavigationSubsystemController Navigation { get; private set; }
        public PropulsionSubsystemController Propulsion { get; private set; }

        public SubsystemReferences()
        {
            Sensors = new SensorSubsystemController();
            Defence = new DefenceSubsystemController();
            Navigation = new NavigationSubsystemController();
            Propulsion = new PropulsionSubsystemController();
        }

        // Ship Console
        public Vector3 currentShipPositionWithinGalaxyMapNode = new Vector3();
        public Vector3 velocity;
        public Vector3 forward;
        public Vector3 back;
        public Vector3 right;
        public Vector3 left;
        public Vector3 inwards;
        public Vector3 outwards;
        public float deltaTime;
        public string currentGalaxyMapNodeName;
    }
}