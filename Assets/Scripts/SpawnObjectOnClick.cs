using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnObjectOnClick : NetworkBehaviour {
    private CanSpawnLinkedObject canSpawn;
    private GenerateNewObject newObjectGenerator;
    private TurnManager turnManager;

    public override void OnStartServer()
	{
        base.OnStartServer();
	}

    public override void OnStartClient()
    {
        canSpawn = GameManager.instance.GetComponent<CanSpawnLinkedObject>();
        newObjectGenerator = GameManager.instance.GetComponent<GenerateNewObject>();
        turnManager = gameObject.GetComponent<TurnManager>();
    }

    public override void OnStartLocalPlayer()
    {
        CmdStartQueue();
    }

    [Command]
    void CmdStartQueue() 
    {
        print("Started queue on server with: " + isLocalPlayer.ToString());
        if (isLocalPlayer)
        {
            newObjectGenerator.InitializeQueueServer(); 
        }
        else  
        {
            RpcStartQueue(newObjectGenerator.GetQueueAsList());
        }
    }

    [ClientRpc]
    void RpcStartQueue(int[] addedObjects)
    {
        if (isServer)
        {
            return;
        }
        newObjectGenerator.InitializeQueueClient(addedObjects);
    }

    [Command]
    void CmdSpawn(float x, float y) 
    {
        GameObject obj = Instantiate(newObjectGenerator.GetNextShape(), new Vector3(x, y, 0), Quaternion.identity);
        NetworkServer.Spawn(obj);
        int addedIndex = newObjectGenerator.UpdateQueueServer();
        RpcPropagateQueueUpdates(addedIndex);
    }

    [ClientRpc]
    void RpcPropagateQueueUpdates(int nextAddedObject) 
    {
        if (isServer)
        {
            return;
        }
        newObjectGenerator.UpdateQueueClient(nextAddedObject);
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane projectionPlane = new Plane(Vector3.forward, Vector3.zero);
        float distance;
        projectionPlane.Raycast(mouseRay, out distance);
        Vector2 mousePos = mouseRay.GetPoint(distance);

        if (Input.GetMouseButtonDown(0)) {
            if (canSpawn.canSpawnObject && turnManager.isMyTurn)
            {
                CmdSpawn(mousePos.x, mousePos.y);
                turnManager.CmdEndTurn();
            }
        }	
	}
}
