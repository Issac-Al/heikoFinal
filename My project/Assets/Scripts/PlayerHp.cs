using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHP : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHP(int sethealth)
    {
        slider.maxValue = sethealth;
        slider.value = sethealth;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

}
