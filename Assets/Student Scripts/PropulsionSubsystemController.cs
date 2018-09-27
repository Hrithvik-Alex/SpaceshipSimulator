using System;
using System.Collections.Generic;
using UnityEngine;
using Sandbox;

public class PropulsionSubsystemController
{




    private void rotateToVector2(Vector2 direction, Vector2 pos) 
    {
        double dirtheta = Math.Atan((double)direction.y/(double)direction.x);
        double postheta = Math.Atan((double)direction.y/(double)direction.x);
        double deltatheta = Math.Abs(dirtheta - postheta);

        // If dirtheta is less than pos, rotate right. lr 0 is right
        // If dirtheta is greater than pos, rotate left. lr 1 is left
        int lr = -1;
        if (dirtheta < postheta)
        {
            lr = 0;
        }
        else if (dirtheta > postheta)
        {
            lr = 1;
        }

        // While the spaceship has not yet rotated through half the target distance
        // continue to accelerate
        while(Math.Abs(dirtheta-postheta)>deltatheta) 
        {
            if (lr == 0)
            {
                //Rotate right
            }
            else if (lr == 1)
            {
                //Rotate left
            }
        }

        // Decelerate once the halfway point is passed
        while (Math.Abs(dirtheta - postheta) < deltatheta  && dirtheta != postheta)
        {
            if (lr == 0)
            {
                //Rotate left
            }
            else if (lr == 1)
            {
                //Rotate right
            }
        }
    }







    private void rotateLeft(ThrusterControls thrusterControls, float value)
    {
        thrusterControls.starboardBowThrust = value;
        thrusterControls.portAftThrust = value;
    }

    private void rotateRight(ThrusterControls thrusterControls, float value)
    {
        thrusterControls.portBowThrust = value;
        thrusterControls.starboardAftThrust = value;
    }

    private bool startRotate = true;
    Vector3 startPos;

    public void PropulsionUpdate(SubsystemReferences subsystemReferences, ThrusterControls thrusterControls)
    {
        if(startRotate){
            startRotate = false;
            startPos = subsystemReferences.forward;
        }
        rotateLeft(thrusterControls, 50);

        if(subsystemReferences.forward > startPos){
            UnityEngine.Debug.Log(Time.frameCount);
        }

        /*string currentSystem = subsystemReferences.currentGalaxyMapNodeName;
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
    }
}