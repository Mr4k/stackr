using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameStateManager : NetworkBehaviour {

    private const float epsilon = 0.01f;
    private const float waitTimeBeforeStable = 2;

    public enum GameState {
        Placing,
        Testing,
        GameOver,
    }

    private static GameObject trackingBlock = null;
    private static GameState currentState;
    private static float lastStableTime = 0;

    private TurnManager turnManager;

	// Use this for initialization
	void Start () {
        turnManager = gameObject.GetComponent<TurnManager>();
	}

    void GameOver()
    {
        SceneManager.LoadScene("SampleScenePeter");
    }

    public void TestBlockAndEndTurn(GameObject block) {
        if (turnManager.isMyTurn && isServer) {
            ServerSetGameState(GameState.Testing);
            trackingBlock = block;
            turnManager.PauseTurn(true);
            lastStableTime = Time.time;
        }
    }

    [ClientRpc]
    public void RpcSetGameState(GameState state) {
        currentState = state;
        if (state == GameState.GameOver)
        {
            GameOver();
        }
    }

    public void ServerSetGameState(GameState state) {
        if (isServer) {
            currentState = state;
            RpcSetGameState(state);
        }
    }

    // Update is called once per frame
    void Update() {
        // authoratative server
        if (!isServer || !isLocalPlayer) return;

        switch(currentState) {
            case GameState.Testing:
                if (Time.time - lastStableTime >= waitTimeBeforeStable) {
                    turnManager.EndTurn();
                    ServerSetGameState(GameState.Placing);
                    // unpause the turn sorry about the name
                    turnManager.PauseTurn(false);
                } else {
                    // check if the object seems stable if so wait a little bit to verify
                    Rigidbody2D trackingBlockBody = trackingBlock.GetComponent<Rigidbody2D>();
                    if (trackingBlockBody.velocity.magnitude > epsilon && trackingBlockBody.angularVelocity > epsilon) {
                        lastStableTime = Time.time;
                    }
                    if (Camera.main.WorldToScreenPoint(trackingBlock.transform.position).y < 0) {
                        ServerSetGameState(GameState.GameOver);
                        turnManager.PauseTurn(true);
                    }
                }
                break;
        }
    }
}
