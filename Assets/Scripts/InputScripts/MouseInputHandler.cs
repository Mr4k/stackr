using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputHandler : MonoBehaviour {	
    public Vector2 GetMouseScreenPosition () {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane projectionPlane = new Plane(Vector3.forward, Vector3.zero);
        float distance;
        projectionPlane.Raycast(mouseRay, out distance);
        return mouseRay.GetPoint(distance);
    }
}
