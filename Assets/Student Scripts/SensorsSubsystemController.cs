using System;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Sandbox;

public class SensorSubsystemController
{
    //Structure
    public struct PlanetStruct
    {
        public Vector2 vector;

        public GravitySignature finalSignature; 

        public PlanetStruct(Vector2 vector, GravitySignature signature)
        {
            this.vector = vector;
            finalSignature = signature;
        }
    }

    public struct WarpStruct
    {
        public Vector2 vector;
        public String warpDest;
        public GravitySignature finalSignature;

        public WarpStruct(Vector2 vector, GravitySignature signature, String dest)
        {
            this.warpDest = dest;
            this.vector = vector;
            finalSignature = signature;
        }
    }


    public List<PlanetStruct> GWIPlanetData = new List<PlanetStruct>();
    public List<WarpStruct> GWIWarpData = new List<WarpStruct>();

    public void SensorsUpdate(SubsystemReferences subsysRef, ShipSensors Data)
    {
        String warpgateDest;
        double angle;
        float waveAmplitude;
        GravitySignature signature;

        //Good data
        float distance;
        double distX, distY;
        Vector2 vector;


        for (int i = 0; i < Data.GWInterferometer.Count; i++)

        {
            warpgateDest = Data.GWInterferometer[i].warpGateDestination;
            angle = (double)Data.GWInterferometer[i].angle;
            waveAmplitude = Data.GWInterferometer[i].waveAmplitude;
            signature = Data.GWInterferometer[i].signature;

            distance = ShipSensors.GConstant / waveAmplitude;


            distX = distance * Math.Cos(angle);
            distY = distance * Math.Sin(angle);

            vector = new Vector2((float)distX, (float)distY);

            if (signature == GravitySignature.Planetoid)
            {
                GWIPlanetData.Add(new PlanetStruct(vector, signature));
            }
            else if(signature == GravitySignature.WarpGate){
                GWIWarpData.Add(new WarpStruct(vector, signature, warpgateDest));
            }

        }      
    }
}