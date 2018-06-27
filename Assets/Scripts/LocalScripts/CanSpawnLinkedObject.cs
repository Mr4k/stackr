using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSpawnLinkedObject : MonoBehaviour {
    public DetectInvalidPosition detector;
    public bool canSpawnObject {
        get {
            return detector.isValid;
        }
    }
}
