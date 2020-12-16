using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public WaypointManager waypointManager;
    public SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnManager != null)
        {
            spawnManager.Initialize(waypointManager);
            spawnManager.StartSpawners();
        }
        else
        {
            Debug.Log("SpawnManager is NULL!");
        }
    }

}
