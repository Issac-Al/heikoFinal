using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWorlds : MonoBehaviour
{
    public Light DirectionalLight;
    public GameObject spiritualWorld, normalWorld;

    public void NormalWorld()
    {
        normalWorld.SetActive(true);
        spiritualWorld.SetActive(false);
        DirectionalLight.colorTemperature = 4348;
    }

    public void SpiritualWorld()
    {
        normalWorld.SetActive(false);
        spiritualWorld.SetActive(true);
        DirectionalLight.colorTemperature = 16511;
    }
}
