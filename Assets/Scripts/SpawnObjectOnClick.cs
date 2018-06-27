using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnObjectOnClick : NetworkBehaviour {
    public GameObject target;

    private CanSpawnLinkedObject canSpawn;

    private void Start()
    {
        canSpawn = GameManager.instance.GetComponent<CanSpawnLinkedObject>(); 
    }

    // Use this for initialization
    public override void OnStartServer()
	{
        base.OnStartServer();

	}

    [Command]
    void CmdSpawn(float x, float y) {
        GameObject obj = Instantiate(target, new Vector3(x, y, 0), Quaternion.identity);
        NetworkServer.Spawn(obj);
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
            if (canSpawn.canSpawnObject)
            {
                CmdSpawn(mousePos.x, mousePos.y);
            }
        }	
	}
}
