using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class GenerateNewObject : MonoBehaviour {
    public GameObject[] indicatorShapes;
    public GameObject[] generatedShapes;
    public GameObject[] queueShapes;

    public delegate void NewShapeSpawned(GameObject newObject);
    public NewShapeSpawned newShapeSpawned;

    private QueueObjectIndicator queueObject;
    private Queue<int> indexShapeQueue = new Queue<int>();
    private CanSpawnLinkedObject canSpawn;
    private GameObject currentIndicator;

	void Start () {
        queueObject = QueueObjectIndicator.instance;
        newShapeSpawned = queueObject.UpdateQueue;
        canSpawn = GetComponent<CanSpawnLinkedObject>();
	}

    public int[] GetQueueAsList() 
    {
        return indexShapeQueue.ToArray<int>();    
    }

    public void InitializeQueueServer() {
        for (int i = 0; i < 3; ++i) 
        {
            int nextIndex = Random.Range(0, indicatorShapes.Length);
            indexShapeQueue.Enqueue(nextIndex);
            if (newShapeSpawned != null)
            {
                newShapeSpawned(queueShapes[nextIndex]);
            }
            if (i == 0)
            {
                currentIndicator = Instantiate(indicatorShapes[nextIndex], transform.position, Quaternion.identity, transform);
                canSpawn.detector = currentIndicator.GetComponent<DetectInvalidPosition>();
            }
        }
    }

    public void InitializeQueueClient(int[] addedObjects) 
    {
        for (int i = 0; i < addedObjects.Length; ++i)
        {
            int nextIndex = addedObjects[i];
            indexShapeQueue.Enqueue(nextIndex);
            if (newShapeSpawned != null)
            {
                newShapeSpawned(queueShapes[nextIndex]);
            }
            if (i == 0)
            {
                currentIndicator = Instantiate(indicatorShapes[nextIndex], transform.position, Quaternion.identity, transform);
                canSpawn.detector = currentIndicator.GetComponent<DetectInvalidPosition>();
            }
        }    
    }

    public GameObject GetNextShape() 
    {
        int nextIndex = indexShapeQueue.Peek();
        return generatedShapes[nextIndex];
    }

    public int UpdateQueueServer() {
        indexShapeQueue.Dequeue();
        int nextIndex = indexShapeQueue.Peek();

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

        return addedIndex;
    }

    public void UpdateQueueClient(int addedIndex) {
        indexShapeQueue.Dequeue();
        int nextIndex = indexShapeQueue.Peek();

        GameObject nextIndicator = Instantiate(indicatorShapes[nextIndex], transform.position, Quaternion.identity, transform);
        canSpawn.detector = nextIndicator.GetComponent<DetectInvalidPosition>();

        Destroy(currentIndicator);
        indexShapeQueue.Enqueue(addedIndex);
        currentIndicator = nextIndicator;

        if (newShapeSpawned != null)
        {
            newShapeSpawned(queueShapes[addedIndex]);
        }
    }
}
