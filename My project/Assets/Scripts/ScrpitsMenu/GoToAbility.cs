using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToAbility : MonoBehaviour
{
   public GameObject scrollView, d1, d2, d3, d4;
    public void ToD1()
    {
        scrollView.SetActive(false);
        d1.SetActive(true);
    }

    public void ToD2()
    {
        scrollView.SetActive(false);
        d2.SetActive(true);
    }

    public void ToD3()
    {
        scrollView.SetActive(false);
        d3.SetActive(true);
    }

    public void ToD4()
    {
        scrollView.SetActive(false);
        d4.SetActive(true);
    }


}
