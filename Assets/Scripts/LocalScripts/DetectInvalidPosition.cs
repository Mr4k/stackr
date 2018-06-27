using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectInvalidPosition : MonoBehaviour {
    public Material validMaterial;
    public Material invalidMateiral;

    private MeshRenderer rend;
    private int numberIntersectingBodies = 0;
    public bool isValid {
        get {
            return numberIntersectingBodies == 0;
        }
    }


    void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        numberIntersectingBodies -= 1;
        if (numberIntersectingBodies == 0)
        {
            rend.material = validMaterial;
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        numberIntersectingBodies += 1;
        rend.material = invalidMateiral;
    }
}
