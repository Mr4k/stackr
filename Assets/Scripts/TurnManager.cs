using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnManager : NetworkBehaviour {

    private const int COLLISION_REDUCTION_DIGITS = 100;
    private static int numPlayers = 0;
    private static List<int> playerTurns = new List<int>();
    private static int turnIndex = 0;

    private static int currentPlayer = -2;
    private int myId = -1;

    public bool isMyTurn {
        get {
            return currentPlayer == myId;
        }
    }

    public override void OnStartClient() {
        if (isServer){
            numPlayers += 1;
            // generate new player id
            int newPlayerId = Random.Range(0, COLLISION_REDUCTION_DIGITS)
                                + numPlayers * COLLISION_REDUCTION_DIGITS * 10;
            playerTurns.Add(newPlayerId);
            StartCoroutine(DelayedAssignId(newPlayerId));
            print(playerTurns);
            if (numPlayers == 1) {
                currentPlayer = newPlayerId;
                // TODO wrap this in a coroutine so it will work
                // This line is just in case we are seperating server and client for host
                //RpcSetCurrentPlayer(newPlayerId);
            }
            myId = newPlayerId;
        }
    }

    // We cannot directly call an RPC method from OnStartClient so we have to delay a little
    IEnumerator DelayedAssignId(int newPlayerId) {
        yield return new WaitForSeconds(0.1f);
        RpcAssignId(newPlayerId);
    }

    // TODO try making these synced vars instead
    [ClientRpc]
    void RpcAssignId(int id) {
        print(id);
        myId = id;
    }

    [Command]
    public void CmdEndTurn() {
        if (myId == currentPlayer) {
            turnIndex++;
            if (turnIndex == playerTurns.Count) {
                turnIndex = 0;
            }
            int nextPlayer = playerTurns[turnIndex];
            RpcSetCurrentPlayer(nextPlayer);
            currentPlayer = nextPlayer;
            print(currentPlayer);
        }
    }

    // TODO try making these synced vars instead
    [ClientRpc]
    void RpcSetCurrentPlayer(int newCurrentPlayer) {
        currentPlayer = newCurrentPlayer;
        print(currentPlayer);
    } 
}
