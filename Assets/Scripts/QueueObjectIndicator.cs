﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueObjectIndicator : MonoBehaviour {
    public Vector3[] localPositions;
    private GameObject[] objects = new GameObject[3];
    private int numberOfCurrentObjects = 0;

    void Start () {
        GameManager.instance.GetComponent<GenerateNewObject>().newShapeSpawned = UpdateQueue;
	}

    public void UpdateQueue (GameObject addedObject) {
        if (numberOfCurrentObjects >= 3) {
            Destroy(objects[numberOfCurrentObjects - 1]);
        } else {
            numberOfCurrentObjects += 1;
        }
        for (int i = numberOfCurrentObjects - 1; i > 0; --i) {
            objects[i] = objects[i - 1];
            objects[i].transform.localPosition = localPositions[i];
        }
        objects[0] = Instantiate(addedObject, Vector3.zero, Quaternion.identity, transform);
        objects[0].transform.localPosition = localPositions[0];
	}
}
