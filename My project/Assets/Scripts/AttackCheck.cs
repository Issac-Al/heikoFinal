using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    private Ayako playerAttack;
    // Start is called before the first frame update
    void Start()
    {
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<Ayako>();
    }

    public void animEvent()
    {
        UnityEngine.Debug.Log("animEvent");
        playerAttack.CheckAttackPhase();
    }

    public void SpellEvent()
    {
        playerAttack.SpellType();
    }

    public void StaffReturn()
    {
        UnityEngine.Debug.Log("regresaStaff");
        playerAttack.Ocultara_Staff_Mano();
    }

    public void worldChange()
    {
        playerAttack.PutMask();
    }
}
