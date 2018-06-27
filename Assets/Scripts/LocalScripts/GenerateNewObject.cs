using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GenerateNewObject : NetworkBehaviour {
    public GameObject[] indicatorShapes;
    public GameObject[] generatedShapes;

    private CanSpawnLinkedObject canSpawn;

	void Start () {
        if (isLocalPlayer)
        {
            GameObject spawnedIndicator = Instantiate(indicatorShapes[0], transform.position, Quaternion.identity, transform);
            canSpawn = GetComponent<CanSpawnLinkedObject>();
            canSpawn.detector = spawnedIndicator.GetComponent<DetectInvalidPosition>();
        }
	}
}
