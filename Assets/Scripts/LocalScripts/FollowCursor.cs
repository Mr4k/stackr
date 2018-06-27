using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour {
    void FixedUpdate()
    {
        Vector2 mousePosition = MouseInputHandler.instance.GetMouseScreenPosition();
        transform.position = mousePosition;
    }
}
