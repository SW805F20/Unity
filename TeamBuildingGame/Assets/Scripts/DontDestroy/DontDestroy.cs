using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        //When we make something dont destroy on load, but reload the scene later, it will create duplicates.
        // To avoid this, the duplicates need to be destroyed.
        // We need two objects that are ddol, so we check if we exceed that number, and destroy if we do to ensure we keep the originals.
        if (FindObjectsOfType(GetType()).Length > 2)
        {
            Destroy(gameObject);
        }
    }
}