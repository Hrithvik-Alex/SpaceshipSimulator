using System;
using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Sandbox;
using UnityEngine;


public class DefenceSubsystemController
{
    private List<List<Vector3>> asteroids;
    private float missileSpeed = Ship.missileSpeed;
    public void GetAsteriods(List<List<Vector3>> listOfAsteroids){ // idx 0 = position, idx 1 = velocity
        asteroids = listOfAsteroids;
    }

    public List<Vector3> identifyTarget(List<List<Vector3>> asteroids){

    }

    public Vector2 calcMissileDistanceVect(List<Vector3> oneAsteroid, List<Vector3> spaceshipPosition) {
        Vector2 aposition = oneAsteroid[0];
        Vector2 avelocity = oneAsteroid[1];
        Vector2 mposition = spaceshipPosition[0];
        Vector2 mvelocity = spaceshipPosition[1];


        float apx = aposition.x;
        float apy = aposition.y;
        float avx = avelocity.x;
        float avy = avelocity.y;

        float mpx = mposition.x;
        float mpy = mposition.y;
        float mvx = mposition.x;
        float mvy = mposition.y;

        //solve for time
        double modP = Math.Sqrt((apx-mpx)*(apx-mpx) + (apy - mpy)*(mpy - mpy));
        double modV = Math.Sqrt((avx - mvx) * (avx - mvx) + (avy - mvy) * (mvy - mvy));
        double time = modP/modV;
        float ftime = (float)time;

        //use time to figure out where to shoot
            Vector2 dA = new Vector2();

            dA.x = apx + avx*ftime; //asteroid position vector
            dA.y = apy + avy*ftime;

        Vector2 shotVector = new Vector2();
        shotVector.x = dA.x - mpx;
        shotVector.y = dA.y - mpy;

        return shotVector;
    }

    public void DefenceUpdate(SubsystemReferences subsystemReferences, TurretControls turretControls)
    {
        /*Debug.Log(subsystemReferences.currentShipPositionWithinGalaxyMapNode);
        Debug.Log(subsystemReferences.velocity);
        Debug.Log(subsystemReferences.forward);
        Debug.Log(subsystemReferences.back);
        Debug.Log(subsystemReferences.right);
        Debug.Log(subsystemReferences.left);
        Debug.Log(subsystemReferences.inwards);
        Debug.Log(subsystemReferences.outwards);
        Debug.Log(subsystemReferences.deltaTime);
        Debug.Log(subsystemReferences.currentGalaxyMapNodeName);*/
        turretControls.aimTo= new Vector3(-2,-4,-5);
        turretControls.isTriggerPulled = true;
    }
}