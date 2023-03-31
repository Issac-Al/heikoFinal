using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHP : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHP(float sethealth)
    {
        slider.maxValue = sethealth;
        slider.value = sethealth;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

}