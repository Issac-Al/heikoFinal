using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateTimer : MonoBehaviour
{
    public GameObject image;

    public void ActivateImage()
    {
        image.SetActive(true);
    }
}
