using UnityEngine;
using UnityEngine.UI;
using Sandbox;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ShipSensors))]
[RequireComponent(typeof(Turret))]
[RequireComponent(typeof(Thrusters))]

/// <summary>\
/// This script goes on the ship gameObject tagged "Player".
/// It calls all of the student code and handles the ship's properties.
/// </summary>

public class Ship : MonoBehaviour
{
    //Internal
    GameCore parentGameCore;
    Rigidbody2D rigidBody2D;
    ShipSensors shipSensors;
    Turret turret;
    Thrusters thrusters;
    public GalaxyMapData galaxyMapData;
    string solarSystem = "Sol";

    ThrusterControls propToThrust;
    TurretControls defToTurret;
        
    public static float missileSpeed = 14f;
    float shipHealth;
    float maxHealth = 60;
    float damage = 2;
    Slider healthbar;

    public void Setup(GameCore parentGameCore)
    {
        this.parentGameCore = parentGameCore;
    }

    void Start()
    {
        // Get Ship Components
        rigidBody2D = GetComponent<Rigidbody2D>();
        shipSensors = GetComponent<ShipSensors>();
        turret = GetComponent<Turret>();
        thrusters = GetComponent<Thrusters>();
            
        UpdateSystemReference();

        // Health Bar
        healthbar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
        shipHealth = 0f;
        healthbar.value = Health();

    }

    private void UpdateSystemReference()
    {
        subsystems.currentShipPositionWithinGalaxyMapNode = transform.position;
        subsystems.velocity = rigidBody2D.velocity;
        subsystems.forward = transform.up;
        subsystems.back = -subsystems.forward;
        subsystems.right = transform.right;
        subsystems.left = -subsystems.right;
        subsystems.inwards = transform.forward;
        subsystems.outwards = -subsystems.inwards;
        subsystems.deltaTime = Time.deltaTime;
        subsystems.currentGalaxyMapNodeName = solarSystem;
}


    // Create references to student code
    private SubsystemReferences subsystems = new SubsystemReferences();

    private void FixedUpdate()
    {
        // Pre-SubsystemUpdate
        UpdateSystemReference();
        shipSensors.NewSensorDataForThisFrame();

        // SubsystemUpdate
        // The student code runs. One method call per subsystem
        subsystems.Sensors.SensorsUpdate(subsystems, shipSensors);
        subsystems.Defence.DefenceUpdate(subsystems, turret.fromDefence);
        subsystems.Navigation.NavigationUpdate(subsystems, galaxyMapData);
        subsystems.Propulsion.PropulsionUpdate(subsystems, thrusters.thrusterControlInputs);

        // Post-SubsystemUpdate
        // After the student code has run, the ship must execute
        propToThrust = thrusters.thrusterControlInputs;
        defToTurret = turret.fromDefence;

        UpdateSystemReference();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SmallBodies"))
        {
            shipHealth += damage;
            healthbar.value = Health();
            Debug.Log("Ship health: " + shipHealth);
        }
    }

    float Health()
    {
        return (shipHealth / maxHealth);
    }

    public void SetCurrentSolarSystem(string solarSystemName)
    {
        solarSystem = solarSystemName;
    }

    //for student's debug purposes
    public static void Log(object message)
    {
        Debug.Log(message); 
    }
}