using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRot : MonoBehaviour
{
    public Vector2 turn;
    public float sensivityV = 0.5f;
    public float sensivityH = 0.5f;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        turn.y += Input.GetAxis("Mouse Y") * sensivityV;
        turn.x += Input.GetAxis("Mouse X") * sensivityH;
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
