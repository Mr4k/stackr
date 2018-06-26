using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnObjectOnClick : NetworkBehaviour {

    public GameObject target;

	// Use this for initialization
	public override void OnStartServer()
	{
        base.OnStartServer();

	}

    void Spawn() {
        GameObject obj = Instantiate(target, new Vector3(0, 0, 0), Quaternion.identity);
        NetworkServer.Spawn(obj);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Spawn();
        }	
	}
}
