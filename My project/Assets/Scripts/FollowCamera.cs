using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - UnityEngine.Camera.main.transform.position);
        //UnityEngine.Debug.Log(transform.rotation);
    }
}
