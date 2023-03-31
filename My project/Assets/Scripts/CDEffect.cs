using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class CDEffect : MonoBehaviour
{
    public Image fill;
    public GameObject timerShow;
    public float coolDown, actualTimer;
    // Start is called before the first frame update
    void Start()
    {
        actualTimer = 100;
    }

    void Update()
    {
        CallCD();
    }

    // Update is called once per frame
    public void CallCD()
    {
        if (actualTimer > 0)
        {
            actualTimer -= coolDown * Time.deltaTime;
        }
        else
        {
            actualTimer = 100;
            timerShow.SetActive(false);
        }
        fill.fillAmount = actualTimer / 100;
    }
}
