using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour {
    void FixedUpdate()
    {
        Vector2 mousePosition = GameManager.instance.GetComponent<MouseInputHandler>().GetMouseScreenPosition();
        transform.position = mousePosition;
    }
}
