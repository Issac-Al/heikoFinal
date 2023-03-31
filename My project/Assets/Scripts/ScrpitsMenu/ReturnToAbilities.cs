using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToAbilities : MonoBehaviour
{
    public GameObject scrollView;

    public void returnToScrollView()
    {
        scrollView.SetActive(true);
        gameObject.SetActive(false);
    }
}
