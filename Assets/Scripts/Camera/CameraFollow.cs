using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public string followTag;
    public float interpAmount = 0.01f;

    private Transform topBlock = null;

	// Use this for initialization
	void Start () {
        
	}

    // Update is called once per frame
    void Update()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag(followTag);

        float topPosition = -10000;
        if (topBlock)
        {
            topPosition = topBlock.transform.position.y;
        }

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].transform.position.y > topPosition)
            {
                topPosition = blocks[i].transform.position.y;
                topBlock = blocks[i].transform;
            }
        }

        Vector3 targetPosition = new Vector3(
            topBlock.position.x,
            topBlock.position.y,
            transform.position.z);

        if (topBlock) {
            target.transform.position = Vector3.Lerp(target.transform.position,
                                             targetPosition,
                                             interpAmount);
        }
	}
}
