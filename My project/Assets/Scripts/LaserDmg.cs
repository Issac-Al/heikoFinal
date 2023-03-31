using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDmg : MonoBehaviour
{
    //public GameObject StaffBack, StaffHand;
    public GameEvent collisionEvent;
    public float dmg = 120;
    public Collider staffCollider;
    private float timer = 0.92f;
    public bool throwable;

    void Start()
    {
        staffCollider = GetComponent<Collider>();
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            if (timer > 0.6f)
            {
                collisionEvent.Raise();
                timer = 0;
                if (throwable)
                    Destroy(gameObject);
                UnityEngine.Debug.Log("Entro al trigger");
            }
        }
    }

    public float PassDamage()
    {
        return dmg;
    }


    // Start is called before the first frame update
}
