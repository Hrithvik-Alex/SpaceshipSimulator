using System;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Sandbox;

public class DefenceSubsystemController
{
    public bool WillCrashIntoShip(SubsystemReferences subsystemReferences) {
        int shipVelocityNorm = subsystemReferences.forward;
        Vector2 shipVelocity = subsystemReferences.velocity;
        Vector2 shipPosition = subsystemReferences.currentShipPositionWithinGalaxyMapNode;
        int asteroidVelocityNorm = (subsystemReferences.Sensors.EMSData[0].vel).magnitude;
        float time = (shipPosition - subsystemReferences.Sensors.EMSData[0].pos)/(subsystemReferences.Sensors.EMSData[0].vel-shipVelocity).magnitude;
        if (time < 0) {
            return false;
        } else {
            return true;
        }
    }
    public float DistanceFromShip(SubsystemReferences subsystemReferences) {
        float distance = ((subsystemReferences.Sensors.EMSData[0].pos) - (shipPosition)).magnitude;
        return distance;
    }

    public Vector2 missileAim(SubsystemReferences subsystemReferences, TurretControls turretControls) {
        turretControls.aimTo = subsystemReferences.Sensors.EMSData[0].pos;
    }
    public void DefenceUpdate(SubsystemReferences subsystemReferences, TurretControls turretControls) {
        //turretControls.aimTo = new Vector3 (0,1,1);
        Vector2 aim = subsystemReferences.Sensors.EMSData[0].pos;
        turretControls.aimTo = aim;
        turretControls.isTriggerPulled = true;
        Ship.missileSpeed = 20;
    }
}