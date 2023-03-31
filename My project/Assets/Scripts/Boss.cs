using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
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
    private float attackCD1 = 0f, timeTillDmg = 0f;
    private Animator enemyAnimator;
    private bool alive = true, playerSeen = false;
    private float dmg = 100;
    private AnimatorStateInfo animatorState;
    public Collider sword;
    private Vector3 targetPosition;
    public GameEvent win;
    private float timeForExit = 0;

    // Start is called before the first frame update

    public void Start()
    {
        currentHp = HP;
        setHp.SetMaxHP(HP);
        playerTransform = GameObject.FindGameObjectWithTag(
                          "Player").transform;
        currentState = EnemyState.PATROL;
        UpdateState();
        enemyAnimator = GetComponent<Animator>();
        sword.enabled = false;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Damager") && canBeHurt < Time.time && currentHp > 0)
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
        if (collision.transform.CompareTag("DamagerBeam") && canBeHurt < Time.time && currentHp > 0)
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
        if (currentHp <= 0 && alive)
        {
            alive = false;
            FindObjectOfType<DetectEnemies>().DestroyEnemyInList(gameObject);
            FindObjectOfType<WorldManager>().DestroyEnemy(gameObject);
            enemyAgent.velocity = Vector3.zero;
            enemyAnimator.SetBool("alive", false);
            enemyAgent.Stop();
            timeForExit = Time.time + 3f;
        }
    }

    public void Update()
    {
        Death();
        if (timeForExit < Time.time && !alive)
        {
            win.Raise();
        }
        setHp.SetHealth(currentHp);
        animatorState = enemyAnimator.GetCurrentAnimatorStateInfo(0);
        if (alive)
        {
            if (animatorState.IsName("PrepareForCombat") || animatorState.IsName("attack2") || animatorState.IsName("death"))
                enemyAgent.velocity = Vector3.zero;
            ChasePlayer();
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
                    targetPosition = new Vector3(playerTransform.position.x,
                                                                 gameObject.transform.position.y,
                                                                 playerTransform.position.z);
                    gameObject.transform.LookAt(targetPosition);
                    Attack();
                    break;
            }
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
        if (distance > 40)
        {
            currentState = EnemyState.PATROL;
        }
        else
        {
            if (distance <= 2.5)
                currentState = EnemyState.ATTACK;
            else
            {
                if (!playerSeen)
                {
                    enemyAnimator.SetTrigger("lookAtPlayer");
                    playerSeen = true;
                }
                currentState = EnemyState.CHASE;
            }
        }
        //UnityEngine.Debug.Log(currentState);
        if (animatorState.IsName("attack1") && timeTillDmg < Time.time || animatorState.IsName("attack2") && timeTillDmg < Time.time)
        {
            sword.enabled = true;
        }
        else
            sword.enabled = false;
    }


    public enum EnemyState
    {
        PATROL,
        CHASE,
        ATTACK
    };

    public void Attack()
    {
        if (attackCD1 < Time.time)
        {
            enemyAnimator.SetTrigger("attack1");
            attackCD1 = Time.time + 10f;
            dmg = 350;
        }
        else
        {
            enemyAnimator.SetTrigger("attack2");
            dmg = 200;
        }
      
    }

    public float AttackDmg()
    {
        return dmg;
    }

    private void OnAnimatorMove()
    {
        targetPosition = new Vector3(playerTransform.position.x,
                                     gameObject.transform.position.y,
                                     playerTransform.position.z);
        gameObject.transform.LookAt(targetPosition);
        enemyAgent.SetDestination(targetPosition);
    }
}