using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //public GameEvent collisionEvent;
    public float HP = 1200;
    public GameObject floatingPoints;
    public bool dmgTarget = false;
    private float canBeHurt = 0f, currentHp;
    private float damageReceived;
    public EnemyHP setHp;
    private Transform playerTransform;
    public UnityEngine.AI.NavMeshAgent enemyAgent;
    public Vector2 patrolRange;
    private Vector3 randomPosition;
    public float patrolTime;
    private EnemyState currentState;
    // Start is called before the first frame update

    public void Start()
    {
        currentHp = HP;
        setHp.SetMaxHP(HP);
        playerTransform = GameObject.FindGameObjectWithTag(
                          "Player").transform;
        currentState = EnemyState.PATROL;
        UpdateState();
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Damager") && canBeHurt < Time.time)
        {
            damageReceived = collision.gameObject.GetComponent<Weapon>().PassDamage();
            currentHp = currentHp - damageReceived;
            GameObject points = Instantiate(floatingPoints, transform.position, Quaternion.identity) as GameObject;
            points.transform.GetChild(0).GetComponent<TextMesh>().text = damageReceived.ToString();
            Destroy(points, 0.5f);
            canBeHurt = Time.time + 0.5f;
        }

        if (collision.transform.CompareTag("DamagerBeam") && canBeHurt < Time.time)
        {
            damageReceived = collision.gameObject.GetComponent<Weapon>().PassDamage();
            currentHp = currentHp - damageReceived;
            GameObject points = Instantiate(floatingPoints, transform.position, Quaternion.identity) as GameObject;
            points.transform.GetChild(0).GetComponent<TextMesh>().text = damageReceived.ToString();
            Destroy(points, 0.5f);
            canBeHurt = Time.time + 0.5f;
        }
    }

    public void OnTriggerStay(Collider collision)
    {
        if (collision.transform.CompareTag("DamagerBeam") && canBeHurt < Time.time)
        {
            damageReceived = collision.gameObject.GetComponent<LaserDmg>().PassDamage();
            currentHp = currentHp - damageReceived;
            GameObject points = Instantiate(floatingPoints, transform.position, Quaternion.identity) as GameObject;
            points.transform.GetChild(0).GetComponent<TextMesh>().text = damageReceived.ToString();
            Destroy(points, 0.5f);
            canBeHurt = Time.time + 0.5f;
        }
    }

    public void Death()
    {
        if (currentHp <= 0)
        {
            FindObjectOfType<DetectEnemies>().DestroyEnemyInList(gameObject);
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        setHp.SetHealth(currentHp);
        ChasePlayer();
        Death();
        switch (currentState)
        {
            case EnemyState.CHASE:
                enemyAgent.SetDestination(playerTransform.position);

                break;
            case EnemyState.PATROL:
                InvokeRepeating("GenerateRandomDestination", 0f,
                    patrolTime);
                break;
            case EnemyState.ATTACK:
                enemyAgent.velocity = Vector3.zero;
                UnityEngine.Debug.Log("Toma consejo!");
                break;
        }
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.PATROL:
                InvokeRepeating("GenerateRandomDestination", 0f,
                    patrolTime);
                break;
        }
    }

    private void GenerateRandomDestination()
    {
        randomPosition = transform.position +
                            new Vector3(
                                UnityEngine.Random.Range(-patrolRange.x, patrolRange.x),
                                0f,
                                UnityEngine.Random.Range(-patrolRange.y, patrolRange.y));
        enemyAgent.SetDestination(randomPosition);
    }

    public void ChasePlayer()
    {
        float distance = Mathf.Abs(Vector3.Distance(gameObject.transform.position, playerTransform.position));
        //UnityEngine.Debug.Log(distance);
        //UnityEngine.Debug.Log(currentState);
        if (distance > 25)
        {
            currentState = EnemyState.PATROL;
        }else
            {
                if (distance <= 2.5)
                currentState = EnemyState.ATTACK;
                else
                {
                    currentState = EnemyState.CHASE;
                }
            }
        UnityEngine.Debug.Log(currentState);
    }


    public enum EnemyState
    {
        PATROL,
        CHASE,
        ATTACK
    };
}
