using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility {

	public static void SetLayerRecursively(GameObject obj,int Layerno)
    {
        if (obj == null)
        {
            return;
        }
        obj.layer = Layerno;
        foreach(Transform child in obj.transform)
        {
            if (child == null)
                continue;
            SetLayerRecursively(child.gameObject, Layerno);
        }
    }
}
