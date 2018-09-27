using System;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Sandbox;

public class SensorSubsystemController
{
    //Structure
    public struct WarpDetection
    {
        public Vector2 vector;

        public GravitySignature finalSignature; 

        public WarpDetection(Vector2 vector, GravitySignature signature)
        {
            this.vector = vector;
            finalSignature = signature;
        }
    }


    public List<WarpDetection> GWIData = new List<WarpDetection>();

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


   for(int i = 0; i<Data.GWInterferometer.Count; i++)

        {
            warpgateDest = Data.GWInterferometer[i].warpGateDestination;
            angle = (double)Data.GWInterferometer[i].angle;
            waveAmplitude = Data.GWInterferometer[i].waveAmplitude;
            signature = Data.GWInterferometer[i].signature;

            distance = ShipSensors.GConstant / waveAmplitude;


            distX = distance * Math.Cos(angle);
            distY = distance * Math.Sin(angle);

            vector = new Vector2((float)distX, (float)distY);

            GWIData.Add(new WarpDetection(vector, signature));
        }
    }
}