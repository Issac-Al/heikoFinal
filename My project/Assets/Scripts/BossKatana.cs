using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKatana : MonoBehaviour
{
    //public GameObject StaffBack, StaffHand;
    public float dmg = 120;
    private float timer = 0.92f;
    public GameObject boss;

    void Start()
    {
        dmg = boss.GetComponent<Boss>().AttackDmg();
    }

    void Update()
    {
        timer += Time.deltaTime;
    }
        //UnityEngine.Debug.Log("Entro al trigger");

    public float PassDamage()
    {
        return dmg;
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            dmg = boss.GetComponent<Boss>().AttackDmg();
            UnityEngine.Debug.Log(dmg);
        }
    }
    // Start is called before the first frame update
}
