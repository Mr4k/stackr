using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GenerateNewObject : MonoBehaviour {
    public GameObject[] indicatorShapes;
    public GameObject[] generatedShapes;
    public GameObject[] queueShapes;

    public delegate void NewShapeSpawned(GameObject newObject);
    public NewShapeSpawned newShapeSpawned;
    public QueueObjectIndicator queueObject;

    private Queue<int> indexShapeQueue = new Queue<int>();
    private CanSpawnLinkedObject canSpawn;
    private GameObject currentIndicator; 

	void Start () {
        newShapeSpawned = queueObject.UpdateQueue;
        newShapeSpawned(queueShapes[0]);
        currentIndicator = Instantiate(indicatorShapes[0], transform.position, Quaternion.identity, transform);

        for (int i = 0; i < 2; ++i) {
            int nextIndex = Random.Range(0, indicatorShapes.Length);
            indexShapeQueue.Enqueue(nextIndex);
            if (newShapeSpawned != null)
            {
                newShapeSpawned(queueShapes[nextIndex]);
            }
        }

        canSpawn = GetComponent<CanSpawnLinkedObject>();
        canSpawn.detector = currentIndicator.GetComponent<DetectInvalidPosition>();
	}

    public GameObject GetNextGeneratedShape() {
        int nextIndex = indexShapeQueue.Dequeue();
        GameObject nextGameObject = generatedShapes[nextIndex];
        GameObject nextIndicator = Instantiate(indicatorShapes[nextIndex], transform.position, Quaternion.identity, transform);
        canSpawn.detector = nextIndicator.GetComponent<DetectInvalidPosition>();

        Destroy(currentIndicator);
        int addedIndex = Random.Range(0, indicatorShapes.Length);
        indexShapeQueue.Enqueue(addedIndex);
        currentIndicator = nextIndicator;

        if (newShapeSpawned != null)
        {
            newShapeSpawned(queueShapes[addedIndex]);
        }

        return nextGameObject;
    }
}
