using System;
using System.Collections.Generic;
using UnityEngine;
using Sandbox;

public class PropulsionSubsystemController
{
    float THRUST_STRENGTH = 0f;
    float TURN_STRENGTH = 100;
    public Vector2 targetVector = Vector2.down;
    public Vector2 originalVector = Vector2.right;
    public bool engineOn = true;

    private SubsystemReferences subsystemRefs;

    private void rotateLeft(ThrusterControls thrusterControls, float value)
    {
        Debug.Log("Rotateleft called");
        thrusterControls.starboardBowThrust = value;
        thrusterControls.portAftThrust = value;
        thrusterControls.portBowThrust = 0;
        thrusterControls.starboardAftThrust = 0;
    }

    private void rotateRight(ThrusterControls thrusterControls, float value)
    {
        Debug.Log("Rotateright called");
        thrusterControls.portBowThrust = value;
        thrusterControls.starboardAftThrust = value;
        thrusterControls.starboardBowThrust = 0;
        thrusterControls.portAftThrust = 0;
    }

    private void stopRotate(ThrusterControls thrusterControls) 
    {
        thrusterControls.starboardBowThrust = 0;
        thrusterControls.portAftThrust = 0;
        thrusterControls.portBowThrust = 0;
        thrusterControls.starboardAftThrust = 0;
    }

    public void rotateToVector2(Vector2 target, Vector2 cur, ThrusterControls thrusterControls)
    {
        if (targetVector == Vector2.zero) return;
        double otheta = Math.Atan((double)originalVector.y / (double)originalVector.x);
        double curtheta = Math.Atan((double)cur.y / (double)cur.x);
        double targettheta = Math.Atan((double)target.y / (double)target.x);
        double deltatheta = Math.Abs(curtheta - targettheta);
        // Theta you are supposed to rotate through
        double deltathetao = Math.Abs(otheta - targettheta);

        // Correct for negative angles
        // Dirtheta in default position fluctuates
        if (curtheta < 0) { curtheta = (2 * Math.PI + curtheta); }
        if (targettheta < 0) { targettheta = (2 * Math.PI + targettheta); }

        Debug.Log("Curtheta: " + curtheta);
        Debug.Log("Targettheta: " + targettheta);
        //Debug.Log("Deltatheta: " + deltatheta);
        //Debug.Log("Deltathetao: " + deltathetao);

        // If dirtheta is less than pos, rotate right. lr -1 is right
        // If dirtheta is greater than pos, rotate left. lr 1 is left
        int lr = 0;
        if (curtheta > targettheta)
        {
            Debug.Log("Target turning right!");
            lr = -1;
        }
        else if (curtheta < targettheta)
        {
            Debug.Log("Target turning left!");
            lr = 1;
        }

        // While the spaceship has not yet rotated through half the target distance
        // continue to accelerate
        if (deltatheta > deltathetao/2)
        {
            Debug.Log("Speeding up rotation");
            Debug.Log("LR: "+lr);
            if (lr == -1)
            {
                //Rotate right
                Debug.Log("Rotating right!");
                rotateRight(thrusterControls, TURN_STRENGTH);
            }
            else if (lr == 1)
            {
                //Rotate left
                Debug.Log("Rotating left!");
                rotateLeft(thrusterControls, TURN_STRENGTH);
            }
        }
        // Decelerate once the halfway point is passed
        if (deltatheta < deltathetao/2 )
        {
            Debug.Log("Slowing down rotation");
            if (lr == -1)
            {
                //Rotate left
                rotateLeft(thrusterControls, TURN_STRENGTH);
            }
            else if (lr == 1)
            {
                //Rotate right
                rotateRight(thrusterControls, TURN_STRENGTH);
            }
        }
        if(Math.Abs(curtheta - targettheta) < 0.05) 
        {
            Debug.Log("STOPPING ROTATE");
            targetVector = Vector2.zero;
            stopRotate(thrusterControls);
        }
    }

    // Set the target spaceship vector
    public void setTargetVector(Vector2 target) {
        targetVector = target;
        originalVector = subsystemRefs.forward;
    }

    // Turn the engine on and off
    public void setEngineState(bool state) {
        engineOn = state;
    }

    public void setThrustStrength(float strength) {
        THRUST_STRENGTH = strength;
    }

    public void PropulsionUpdate(SubsystemReferences subsystemReferences, ThrusterControls thrusterControls)
    {
        subsystemRefs = subsystemReferences;
        if(engineOn){
            rotateToVector2(targetVector, subsystemReferences.forward, thrusterControls);
            thrusterControls.mainThrust = THRUST_STRENGTH;
        }
    }
}

/*
if(subsystemReferences.forward > startPos){
    UnityEngine.Debug.Log(Time.frameCount);
}

string currentSystem = subsystemReferences.currentGalaxyMapNodeName;
UnityEngine.Debug.Log(currentSystem);
if (currentSystem == "Sol")
{
    thrusterControls.mainThrust = 100;
    thrusterControls.portBowThrust = 50;
    thrusterControls.starboardBowThrust = 17;
    thrusterControls.portAftThrust = 50;
    thrusterControls.starboardAftThrust = 17;
}
else if (currentSystem == "Sirius")
{
    thrusterControls.mainThrust = 100;
    thrusterControls.portBowThrust = 5;
    thrusterControls.starboardBowThrust = 12;
    thrusterControls.portAftThrust = 5;
    thrusterControls.starboardAftThrust = 12;
}
else if (currentSystem == "Barnard's Star")
{
    thrusterControls.mainThrust = 0;
    thrusterControls.portBowThrust = 0;
    thrusterControls.starboardBowThrust = 0;
    thrusterControls.portAftThrust = 0;
   thrusterControls.starboardAftThrust = 0;
}*/
