using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sandbox;

/// <summary>
/// There should be only one game core in the game core scene. Another gameObject tagged "StarContainer" is also required.
/// This script controls level loading, winning and losing, as well as spawning in the Player gameObject.
/// </summary>

public class GameCore : MonoBehaviour
{
    [Header("Tunable")]
    [SerializeField]
    SimulationMode simulationMode = SimulationMode.A;
    
    [Header("References")]
    [SerializeField]
    Canvas uiOverlayCanvas;

    [Header("Assets")]
    [SerializeField]
    GameObject shipPrefab;
    [SerializeField]
    GameObject externalCameraPrefab;
    [SerializeField]
    GameObject missionAccomplishedPanelPrefab;
        
    GalaxyMapVisual galaxyMapVisual;
    GalaxyMapCamera galaxyMapCamera;

    //Properties
    public string CurrentSolarSystemName { get { return currentSolarSystemName; } }
    public static Transform DynamicObjectsRoot;
    public SimulationMode SimMode { get { return simulationMode; } }

    //Internal
    bool isHelpVisible = false;
    bool isGameOver = false;
    Ship ship;
    ExternalCamera externalCamera;
    string currentSolarSystemName;

    public enum SimulationMode
    {
        A,
        B,
        C
    }

    // Use this for initialization
    IEnumerator Start()
    {
        //GalaxyMapGenerator generator = new GalaxyMapGenerator();
        //generator.GenerateStarMap();

        switch (simulationMode)
        {
            case SimulationMode.A:
                yield return LoadGalaxyMapRoutine("GalaxyMapA");
                break;
            case SimulationMode.B:
                yield return LoadGalaxyMapRoutine("GalaxyMapB");
                break;
            case SimulationMode.C:
                yield return LoadGalaxyMapRoutine("GalaxyMapC");
                break;
        }
        
        string startingNodeName = galaxyMapVisual.GetDefaultNode().name;
        yield return LoadSolarSystemSceneRoutine(startingNodeName);

        ship = Instantiate(shipPrefab, transform).GetComponent<Ship>();
        ship.galaxyMapData = galaxyMapVisual.GenerateGalaxyMapData();
        ship.SetCurrentSolarSystem(startingNodeName);
        StartingLocation startingLocation = FindObjectOfType<StartingLocation>();

        externalCamera = Instantiate(externalCameraPrefab, transform).GetComponent<ExternalCamera>();
        externalCamera.Setup(ship.transform);

        DynamicObjectsRoot = new GameObject("DynamicObjectsRoots").transform;
        DynamicObjectsRoot.SetParent(transform);
    }

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isHelpVisible = !isHelpVisible;
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Time.timeScale += 1f;
            Debug.Log("New timescale = " + Time.timeScale);
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Time.timeScale -= 1f;
            Debug.Log("New timescale = " + Time.timeScale);
        }

        //Turn the galaxy map camera on/off
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            galaxyMapCamera.gameObject.SetActive(!galaxyMapCamera.gameObject.activeSelf);
        }
    }

    IEnumerator LoadGalaxyMapRoutine(string galaxyMapName)
    {
        yield return SceneManager.LoadSceneAsync("Sandbox/Scenes/GalaxyMaps/" + galaxyMapName, LoadSceneMode.Additive);

        galaxyMapVisual = FindObjectOfType<GalaxyMapVisual>();
        galaxyMapCamera = FindObjectOfType<GalaxyMapCamera>();
        galaxyMapCamera.gameObject.SetActive(false);
    }

    IEnumerator UnloadCurrentSolarSystemSceneRoutine()
    {
        yield return SceneManager.UnloadSceneAsync(currentSolarSystemName);
    }

    IEnumerator LoadSolarSystemSceneRoutine(string solarSystemName)
    {
        currentSolarSystemName = solarSystemName;
        yield return SceneManager.LoadSceneAsync(solarSystemName, LoadSceneMode.Additive);
        yield return SceneManager.SetActiveScene(SceneManager.GetSceneByName(solarSystemName));

        WarpGate[] warpGates = FindObjectsOfType<WarpGate>();
        foreach (var gate in warpGates)
        {
            gate.OnWarpGateTriggered += HandleWarpGateTriggered;
        }
    }

    private void HandleWarpGateTriggered(WarpGate triggeredGate, Ship triggeringShip, string destinationGalaxyMapNodeName, int destinationWarpGateIndex)
    {
        Debug.Log("HandleWarpGateTriggered");

        if (String.IsNullOrEmpty(destinationGalaxyMapNodeName))
        {
            Debug.LogError("triggeredGate.destinationSolarSystem is null or empty");
            return;
        } 

        if(destinationWarpGateIndex < 0)
        {
            Debug.LogError("triggeredGate.destinationWarpGateIndex < 0");
            return;
        }

        StartCoroutine(SolarSystemSwapRoutine(destinationGalaxyMapNodeName, destinationWarpGateIndex));
    }

    IEnumerator SolarSystemSwapRoutine(string destinationGalaxyMapNodeName, int destinationWarpGateIndex)
    {
        string startingSolarSystemName = currentSolarSystemName;
            
        yield return UnloadCurrentSolarSystemSceneRoutine();
        yield return LoadSolarSystemSceneRoutine(destinationGalaxyMapNodeName);

        ship.SetCurrentSolarSystem(destinationGalaxyMapNodeName);

        //Move ship on top of arrival gate
        foreach (var gate in FindObjectsOfType<WarpGate>())
        {
            if (gate.gateIndex == destinationWarpGateIndex)
            {
                ship.transform.position = gate.transform.position + Vector3.right * 10f;
                ship.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                ship.GetComponent<Rigidbody2D>().angularVelocity = 0f;
            }
        }
    }
    /*
    public static GalaxyMap TestGenerateGalaxyMap(int numberOfStars)
    {
        GalaxyMap galaxyMap = new GalaxyMap();

        //int numberOfStars = 10;
        //Vector2 galaxySize = new Vector3(100f, 50f); //Dimensions: Width X Height

        //float halfWidth = galaxySize.x * 0.5f;
        //float halfHeight = galaxySize.y * 0.5f;

        //Hand-crafted galaxy map for the time being
        galaxyMap.starList.Add(new GalaxyStar("Sol", new Vector2(-50f, 50f)));
        galaxyMap.starList.Add(new GalaxyStar("Alpha Centauri", new Vector2(0f, 5f)));
        galaxyMap.starList.Add(new GalaxyStar("Kepler 438", new Vector2(50f, -25f)));

        return galaxyMap;
    }
    */
    public void TriggerVictory()
    {
        if (isGameOver)
            return;

        Debug.Log("You Win!");
        isGameOver = true;
        Time.timeScale = 0f;

        MissionAccomplishedPanel victoryPanel = Instantiate(missionAccomplishedPanelPrefab, uiOverlayCanvas.transform).GetComponent<MissionAccomplishedPanel>();
        victoryPanel.SetTitleText("Mission Accomplished!");
        victoryPanel.SetBodyText("The ship has successfully reached the new interstellar colony.");
    }
    public void TriggerLoss()
    {
        if (isGameOver)
            return;

        Debug.Log("You Lose!");
        isGameOver = true;
        Time.timeScale = 0f;

        MissionAccomplishedPanel victoryPanel = Instantiate(missionAccomplishedPanelPrefab, uiOverlayCanvas.transform).GetComponent<MissionAccomplishedPanel>();
        victoryPanel.SetTitleText("Mission Failed!");
        victoryPanel.SetBodyText("The ship failed to reach the new interstellar colony.");
    }

    private void OnGUI()
    {
        if (!isHelpVisible)
        {
            GUILayout.Label("F1 - Help");
        } else
        {
            GUILayout.Label("Press F1 to hide help");
            GUILayout.Label("Scroll wheel to zoom camera in/out");
            GUILayout.Label("Numpad +/- to speed up or slow down simulation timescale.");
        }
        
    }
}
