using UnityEngine;
using Sandbox;

/// <summary>
/// This turret fires an instance of the missilePrefab every second from a pool. 
/// Adjust the fire rate in InvokeRepeating() method.
/// </summary>

public class Turret : MonoBehaviour
{
    public TurretControls fromDefence = new TurretControls();
    public GameObject TurretObj;
    public GameObject missilePrefab;
    public float turretAngle;

    float cooldown = 0f;
    float cooldownDuration = 0.5f;

    void Start()
    {
        SimplePool.Preload(missilePrefab, 4);
        //StartFiring();
    }

    public void StartFiring()
    {
        InvokeRepeating("Aim", 1, 1f);
    }

    void FixedUpdate()
    {
        if (fromDefence.aimTo != null)
        {
            turretAngle = Mathf.Atan2(fromDefence.aimTo.y - TurretObj.transform.position.y, fromDefence.aimTo.x - TurretObj.transform.position.x) * Mathf.Rad2Deg;
            turretAngle -= 90;
            TurretObj.transform.localRotation = Quaternion.Euler(0, 0, turretAngle + 90);
            //Debug.Log(fromDefence.aimTo);
            
        }
        else
        {
            return;
        }

        cooldown = Mathf.Max(0f, cooldown - Time.fixedDeltaTime);
        fromDefence.cooldownRemaining = cooldown; //Report the cooldown back to the subsystems
        if (fromDefence.isTriggerPulled)
        {
            if(cooldown <= 0f)
            {
                FireMissile(fromDefence.aimTo, turretAngle);
                cooldown = cooldownDuration;
            }
        }
    }

    void FireMissile(Vector3 deployPos, float turretAngle)
    {
        float deployDist = Vector3.Distance(deployPos, TurretObj.transform.position);
        GameObject firedMissile = SimplePool.Spawn(missilePrefab, TurretObj.transform.position, Quaternion.Euler(0, 0, turretAngle));
        //firedMissile.transform.SetParent(GameCore.DynamicObjectsRoot); //Keeps the hierarchy tidy
        Rigidbody2D missileRb = firedMissile.GetComponent<Rigidbody2D>();
        Missile missileScript = firedMissile.GetComponent<Missile>();
        missileScript.LockOn();
        missileRb.velocity = Ship.missileSpeed * firedMissile.transform.up;
    }
}