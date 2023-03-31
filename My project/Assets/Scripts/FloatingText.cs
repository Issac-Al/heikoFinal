using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Vector3 Offset = new Vector3(0, 2, 0);
    public Vector3 RandomPos = new Vector3(0.5f, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition += Offset;
        transform.localPosition += new Vector3(UnityEngine.Random.Range(-RandomPos.x, RandomPos.x),
                                                UnityEngine.Random.Range(-RandomPos.y, RandomPos.y),
                                                UnityEngine.Random.Range(-RandomPos.z, RandomPos.z));
    }

    // Update is called once per frame

}
