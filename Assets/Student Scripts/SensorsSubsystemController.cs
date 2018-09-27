using System;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Sandbox;

public class SensorSubsystemController
{

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

    public List<WarpStruct> GWIWarpData = new List<WarpStruct>();

    public struct EMSDetection
    {
        Vector2 pos;
        Vector2 vel;
        int sig;
        bool water;
        bool common;
        bool metal;

        //position, velocity, signatureStrength, water, common, metal
        public EMSDetection(Vector2 pos, Vector2 vel, int sig, bool water, bool common, bool metal)
        {
            this.pos = pos;
            this.vel = vel;
            this.sig = sig;
            this.water = water;
            this.common = common;
            this.metal = metal;
        }

    }   

    public List<EMSDetection> EMSData = new List<EMSDetection>();

    public void SensorsUpdate(SubsystemReferences subsysRef, ShipSensors Data)
    {
        double EMSangle;
        float signalStrength;
        int EMSsignature;

        float EMSdistance;
        double EMSposX, EMSposY;

        Vector2 pos;
        Vector2 vel;

        bool water = false;
        bool common = false;
        bool metal = false;

        for (int i = 0; i < Data.EMSensor.Count; i++){
            EMSangle = (double)Data.EMSensor[i].angle;
            signalStrength = Data.EMSensor[i].signalStrength;
            EMSsignature = Data.EMSensor[i].materialSignature;

            EMSdistance = ShipSensors.EMConstant / signalStrength;
            
            EMSposX = EMSdistance * Math.Cos(EMSangle);
            EMSposY = EMSdistance * Math.Sin(EMSangle);

            vel = Data.EMSensor[i].velocity;

            pos = new Vector2((float)EMSposX, (float)EMSposY);

            if (Data.CheckSignatureForSpaceMaterial(EMSsignature, SpaceMaterial.Water)){
                water = true;
            }
            if (Data.CheckSignatureForSpaceMaterial(EMSsignature, SpaceMaterial.Common)){
                common = true;
            }
            if (Data.CheckSignatureForSpaceMaterial(EMSsignature, SpaceMaterial.Metal)){
                metal = true;
            }

            EMSData.Add(new EMSDetection(pos, vel, EMSsignature, water, common, metal));
        }


        String warpgateDest;
        double angle;
        float waveAmplitude;
        GravitySignature signature;
        SpaceMaterial[] material = { };

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

            if(signature == GravitySignature.WarpGate){
                GWIWarpData.Add(new WarpStruct(vector, signature, warpgateDest));
            }

        }
    }
}