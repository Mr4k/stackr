using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnObjectsOnServerStart : NetworkBehaviour {
    public GameObject[] prefabsToSpawn;
    public override void OnStartServer()
    {
        foreach (GameObject obj in prefabsToSpawn) 
        {
            GameObject spawnedObj = Instantiate(obj);
            NetworkServer.Spawn(spawnedObj);
        }
    }
}
