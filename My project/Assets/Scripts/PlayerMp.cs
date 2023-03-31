using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMp : MonoBehaviour
{
    public Slider slider;

    public void SetMaxMp(int mp)
    {
        slider.maxValue = mp;
        slider.value = mp;
    }

    public void SetMp(int mp)
    {
        slider.value = mp;
    }
}
