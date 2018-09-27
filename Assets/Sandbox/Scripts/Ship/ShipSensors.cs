using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates sensor data for the Sensors Subsystem to use.
/// </summary>

namespace Sandbox
{
    public class ShipSensors : MonoBehaviour
    {
        GalaxyMapVisual galaxyMap;

        public const float GConstant = 1f;
        public const float EMConstant = 1f;
        public const float EMSRange = 30f;

        public LayerMask EMSMask;

        private GameObject WarpGatesParent;
        private GameObject LBodiesParent;

        private void Start()
        {
            galaxyMap = FindObjectOfType<GalaxyMapVisual>();    
        }

        #region Generating Sensor Data

        public List<GWI_Detection> GWInterferometer = new List<GWI_Detection>();

        public void GenerateGWIData()
        {
            WarpGatesParent = GameObject.FindGameObjectWithTag("WarpGates");
            LBodiesParent = GameObject.FindGameObjectWithTag("LargeBodies");

            if (WarpGatesParent != null)
            {
                foreach (Transform warpGate in WarpGatesParent.transform)
                {
                    Vector3 positionDiff = warpGate.transform.position - transform.position;
                    float angle = Mathf.Atan2(positionDiff.y, positionDiff.x);
                    float waveAmplitude = 1f / positionDiff.magnitude;

                    WarpGate gate = warpGate.GetComponent<WarpGate>();
                    GalaxyMapNode destinationNode = galaxyMap.FindDestinationNodeForWarpGate(gate);
                    string destinationNodeName = "Unknown";
                    if(destinationNode != null)
                    {
                        destinationNodeName = destinationNode.name;
                    }
                    GWInterferometer.Add(new GWI_Detection(angle, waveAmplitude, GravitySignature.WarpGate, destinationNodeName));
                }
            }

            if (LBodiesParent != null)
            {
                foreach (Transform largeBody in LBodiesParent.transform)
                {
                    Vector3 positionDiff = largeBody.transform.position - transform.position;
                    float angle = Mathf.Atan2(positionDiff.y, positionDiff.x);
                    float waveAmplitude = 1f / positionDiff.magnitude;
                    GravitySignature signature = largeBody.GetComponent<SpaceObject>().gravitySignature;
                    GWInterferometer.Add(new GWI_Detection(angle, waveAmplitude, signature));
                }
            }
        }

        public bool CheckSignatureForSpaceMaterial(int objectSignature, SpaceMaterial material)
        {
            int knownMaterialSignature = 0x1 << (int)material;
            return (objectSignature & knownMaterialSignature) != 0x0;
        }

        public List<EMS_Detection> EMSensor = new List<EMS_Detection>();

        public void GenerateEMSData()
        {
            Collider2D[] hazardColliders = Physics2D.OverlapCircleAll(transform.position, EMSRange, EMSMask);
            foreach (Collider2D col in hazardColliders)
            {
                if (!col.gameObject.CompareTag("Player") && !col.gameObject.CompareTag("Missile"))
                {
                    Vector3 positionDiff = col.transform.position - transform.position;
                    float angle = Mathf.Atan2(positionDiff.y, positionDiff.x);
                    float waveAmplitude = 1f / positionDiff.magnitude;
                    int signature = col.GetComponent<SpaceObject>().MaterialSignature;
                    Rigidbody2D rb2D = col.GetComponent<Rigidbody2D>();
                    Vector2 velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y);
                    EMS_Detection detection = new EMS_Detection(angle, waveAmplitude, velocity, 0);
                    EMSensor.Add(detection);
                }
            }
        }
        #endregion

        public void NewSensorDataForThisFrame()
        {
            EMSensor.Clear();
            GWInterferometer.Clear();
            GenerateEMSData();
            GenerateGWIData();
        }
    }

    // The GWI outputs a list of these
    public struct GWI_Detection
    {
        public float angle { get; private set; }
        public float waveAmplitude { get; private set; }
        public GravitySignature signature { get; private set; }
        public string warpGateDestination { get; private set; }

        public GWI_Detection(float angle0, float waveAmplitude0, GravitySignature signature0, string destination0 = "")
        {
            angle = angle0;
            waveAmplitude = waveAmplitude0;
            signature = signature0;
            warpGateDestination = destination0;
        }
    }

    // The EMS outputs a list of these
    public struct EMS_Detection
    {
        public float angle { get; private set; }
        public float signalStrength { get; private set; }
        public Vector2 velocity { get; private set; }
        public int materialSignature { get; private set; }

        public EMS_Detection(float angle0, float signalStrength0, Vector2 velocity0, int signature0)
        {
            angle = angle0;
            signalStrength = signalStrength0;
            velocity = velocity0;
            materialSignature = signature0;
        }
    }
}

