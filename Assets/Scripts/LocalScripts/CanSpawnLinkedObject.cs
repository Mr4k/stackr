using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSpawnLinkedObject : MonoBehaviour {
    public DetectInvalidPosition detector;
    public bool canSpawnObject {
        get {
            if (detector) 
            {
                return detector.isValid;
            }
            else {
                return false;
            }
        }
    }

    public bool GetLastValidPosition(out Vector3 positionVector)
    {
        if (detector)
        {
            positionVector = detector.GetLastValidPosition();
            return true;
        }
        else 
        {
            positionVector = Vector3.zero;
            return false;
        }
    }
}
