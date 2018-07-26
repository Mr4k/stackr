using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.Events;

public class GenerateNewObject : MonoBehaviour {
    public GameObject[] indicatorShapes;
    public GameObject[] generatedShapes;
    public GameObject[] queueShapes;

    public delegate void NewShapeSpawned(GameObject newObject);
    public NewShapeSpawned newShapeSpawned;

    public delegate void NewIndicatorSpawned(GameObject newIndicator);
    public NewIndicatorSpawned newIndicatorSpawned;

    private Queue<int> indexShapeQueue = new Queue<int>();
    private CanSpawnLinkedObject canSpawn;
    private GameObject currentIndicator;

	void Start () {
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
                Vector3 indicatorPosition = GetNextIndicatorPosition();
                currentIndicator = Instantiate(indicatorShapes[nextIndex], indicatorPosition, Quaternion.identity, transform);
                if (newIndicatorSpawned != null)
                {
                    newIndicatorSpawned(currentIndicator);
                }
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
                Vector3 indicatorPosition = GetNextIndicatorPosition();
                currentIndicator = Instantiate(indicatorShapes[nextIndex], indicatorPosition, Quaternion.identity, transform);
                if (newIndicatorSpawned != null)
                {
                    newIndicatorSpawned(currentIndicator);
                }
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
        int addedIndex = Random.Range(0, indicatorShapes.Length);
        indexShapeQueue.Enqueue(addedIndex);
        CreateNextIndicator();

        if (newShapeSpawned != null)
        {
            newShapeSpawned(queueShapes[addedIndex]);
        }

        return addedIndex;
    }

    public void UpdateQueueClient(int addedIndex) {
        indexShapeQueue.Dequeue();
        indexShapeQueue.Enqueue(addedIndex);
        CreateNextIndicator();

        if (newShapeSpawned != null)
        {
            newShapeSpawned(queueShapes[addedIndex]);
        }
    }

    public Vector3 GetNextIndicatorPosition()
    {
        string followTag = "Blocks";
        GameObject topBlock = null;
        GameObject[] blocks = GameObject.FindGameObjectsWithTag(followTag);

        float topPosition = -Mathf.Infinity;

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].transform.position.y > topPosition)
            {
                topPosition = blocks[i].transform.position.y;
                topBlock = blocks[i];
            }
        }

        if (topBlock != null)
        {
            if (topBlock.GetComponent<Collider2D>() != null)
            {
                Vector3 blockSize = topBlock.GetComponent<Collider2D>().bounds.size;
                float offset = 2.0f;
                return new Vector3(topBlock.transform.position.x, 
                                   topBlock.transform.position.y + blockSize.y * 0.5f + offset, 
                                   -0.3f);
            }
        }

        return new Vector3(0.0f, 0.0f, -0.3f);
    }

    void CreateNextIndicator()
    {
        int nextIndex = indexShapeQueue.Peek();

        Vector3 indicatorPosition = GetNextIndicatorPosition();
        GameObject nextIndicator = Instantiate(indicatorShapes[nextIndex], indicatorPosition, Quaternion.identity, transform);
        canSpawn.detector = nextIndicator.GetComponent<DetectInvalidPosition>();

        Destroy(currentIndicator);
        currentIndicator = nextIndicator;
        if (newIndicatorSpawned != null)
        {
            newIndicatorSpawned(currentIndicator);
        }
    }
}
