﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNewObject : MonoBehaviour {
    public GameObject[] indicatorShapes;
    public GameObject[] generatedShapes;

    private CanSpawnLinkedObject canSpawn;

	void Start () {
        print("HELLO");
        GameObject spawnedIndicator = Instantiate (indicatorShapes[0], transform.position, Quaternion.identity, transform);
        canSpawn = GetComponent<CanSpawnLinkedObject>();
        canSpawn.detector = spawnedIndicator.GetComponent<DetectInvalidPosition>();
	}
}