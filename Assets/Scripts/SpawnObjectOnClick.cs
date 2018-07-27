using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnObjectOnClick : NetworkBehaviour {
    private CanSpawnLinkedObject canSpawn;
    private GenerateNewObject newObjectGenerator;
    private TurnManager turnManager;
    private GameStateManager gameStateManager;

    public override void OnStartServer()
	{
        base.OnStartServer();
	}

    public override void OnStartClient()
    {
        canSpawn = GameManager.instance.GetComponent<CanSpawnLinkedObject>();
        newObjectGenerator = GameManager.instance.GetComponent<GenerateNewObject>();
        turnManager = gameObject.GetComponent<TurnManager>();
        gameStateManager = gameObject.GetComponent<GameStateManager>();
    }

    public override void OnStartLocalPlayer()
    {
        newObjectGenerator.newIndicatorSpawned = ReceiveNewIndicator;
        CmdStartQueue();
    }

    [Command]
    void CmdStartQueue() 
    {
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
        gameStateManager.TestBlockAndEndTurn(obj);
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

    void RequestSpawn()
    {
        if (!isLocalPlayer)
            return;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane projectionPlane = new Plane(Vector3.forward, Vector3.zero);
        float distance;
        projectionPlane.Raycast(mouseRay, out distance);
        Vector2 mousePos = mouseRay.GetPoint(distance);

        if (turnManager.isMyTurn)
        {
            Vector3 positionVector;
            if (canSpawn.canSpawnObject)
            {
                CmdSpawn(mousePos.x, mousePos.y);
            }
            else if (canSpawn.GetLastValidPosition(out positionVector))
            {
                CmdSpawn(positionVector.x, positionVector.y);
            }
        }
    }

    void ReceiveNewIndicator(GameObject newIndicator)
    {
        if (!isLocalPlayer)
            return;
        
        if (newIndicator != null)
        {
            if (newIndicator.GetComponent<FollowCursor>() != null)
            {
                newIndicator.GetComponent<FollowCursor>().MouseDisengaged = RequestSpawn;
            }
        }
    }
}
