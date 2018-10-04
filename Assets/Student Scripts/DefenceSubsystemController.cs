using System;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Sandbox;


public class DefenceSubsystemController
{
    public bool WillCrashIntoShip(SubsystemReferences subsystemReferences) {
        Vector2 shipVelocityNorm = subsystemReferences.SensorsUpdate.forward;
        Vector2 shipVelocity = subsystemReferences.SensorsUpdate.velocity;
        Vector2 asteroidVelocityNorm = subsystemReferences.SensorsUpdate.vel.Normalize;
        if (shipVelocityNorm == asteroidVelocityNorm || shipVelocityNorm == -asteroidVelocityNorm) {
            return false;
        }
        float time = (shipPosition - subsystemReferences.SensorsUpdate.pos)/(subsystemReferences.sensorsUpdate.vel-shipVelocity);
        if (time < 0) {
            return false;
        } else {
            return true;
        }
    }
    public float DistanceFromShip(SubsystemReferences subsystemReferences) {
        float distance = (SubsystemRefences.SensorsUpdate.pos-shipPosition).magnitude;
        return distance;
    }

    public Vector2 missileAim() {
        turretControls.aimTo = DefenceSubsystemController,.pos;
    }

    public void DefenceUpdate(SubsystemReferences subsystemReferences, TurretControls turretControls) {
        turretControls.aimTo = new Vector3 (0,1,1);
        turretControls.isTriggerPulled = true;
        Ship.missileSpeed = 200;
    }
}