using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
    public int p1x, p1y, p2x, p2y, p3x, p3y, p4x, p4y;
    // Start is called before the first frame update
    void Start()
    {
        GameObject PlayingField = GameObject.Find("PlayingField");
        PlayingField.transform.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
