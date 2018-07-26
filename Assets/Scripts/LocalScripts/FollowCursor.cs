using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FollowCursor : MonoBehaviour {
    private bool isFollowing = false;
    public UnityAction MouseEngaged;
    public UnityAction MouseDisengaged;

    void SetFollow(bool newValue)
    {
        isFollowing = newValue;
    }

    private void OnMouseDown()
    {
        if (MouseEngaged != null)
        {
            MouseEngaged();
        }
        SetFollow(true);
    }

    private void OnMouseUp()
    {
        if (MouseDisengaged != null)
        {
            MouseDisengaged();
        }
        SetFollow(false);
    }

    void FixedUpdate()
    {
        if (isFollowing)
        {
            Vector2 mousePosition = GameManager.instance.GetComponent<MouseInputHandler>().GetMouseScreenPosition();
            transform.position = new Vector3(mousePosition.x, mousePosition.y, -0.3f);
        }
    }
}
