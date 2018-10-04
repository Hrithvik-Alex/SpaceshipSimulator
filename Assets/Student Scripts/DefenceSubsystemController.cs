using UnityEngine;
using System;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Sandbox;

public class DefenceSubsystemController
{
    float shipVelocityNorm;
    Vector2 shipVelocity;
    Vector2 shipPosition;
    float asteroidVelocityNorm;
    float time;
    public bool WillCrashIntoShip(SubsystemReferences subsystemReferences) {
        shipVelocityNorm = (subsystemReferences.forward).magnitude;
        shipVelocity = subsystemReferences.velocity;
        shipPosition = subsystemReferences.currentShipPositionWithinGalaxyMapNode;
        asteroidVelocityNorm = (subsystemReferences.Sensors.EMSData[0].vel).magnitude;
        time = ((shipPosition - subsystemReferences.Sensors.EMSData[0].pos)/(subsystemReferences.Sensors.EMSData[0].vel-shipVelocity)).magnitude;
        if (time < 0) {
            return false;
        } else {
            return true;
        }
    }
    public Vector2 Asteroid(SubsystemReferences subsystemReferences) {
        int asteroidCount = subsystemReferences.Sensors.EMSData.Count;
        Vector2 closestAsteroid = subsystemReferences.Sensors.EMSData[0].pos;
        for (int i = 1; i < asteroidCount; i++) {
            float distance = ((subsystemReferences.Sensors.EMSData[i].pos) - (shipPosition)).magnitude;
            if (distance < ((closestAsteroid) - (shipPosition)).magnitude) {
                closestAsteroid = subsystemReferences.Sensors.EMSData[i].pos;
            }
        }
        return closestAsteroid;
    }

    public void DefenceUpdate(SubsystemReferences subsystemReferences, TurretControls turretControls) {
        //turretControls.aimTo = new Vector3 (0,1,1);
        turretControls.aimTo = Asteroid(subsystemReferences);
        if (subsystemReferences.Sensors.EMSData.Count == 0) {
            turretControls.isTriggerPulled = false;
            Debug.Log("false");
        } else {
            turretControls.isTriggerPulled = true;
        }
    }
}