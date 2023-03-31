using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Ayako : MonoBehaviour
{
    private Animator playerAnimator;
    private AnimatorStateInfo animatorState;
    private AnimatorStateInfo animatorStateWave;
    public CapsuleCollider playerCollider;
    private Vector3 velocity, direction;
    private CharacterController playerController;
    public float speed, jumpHeight;
    private float rotationPerFrame = 15f;
    public Camera MainCamera;
    bool seMueve, grounded;
    private Transform playerTransform;
    private float gravity = -9.81f, ySpeed;
    public int HP = 1250, MP = 500, currentHP, currentMP, regenerationSpeedHP, regenerationSpeedMP;
    public GameObject StaffBack, StaffHand, mask, blast, forceField, speedParticles, lightBeam, sparkles, detectionZone, UI, GameOver;
    public PlayerHp HPSet;
    public PlayerMp MPSet;
    public float nextAttackTime = 0f, blastForce = 10f;
    private float spell1CD = 0f, spell2CD = 0f, spell3CD = 0f, spell4CD = 0f, maskCD = 0f;
    public GameEvent spell1, spell2, spell3, spell4, world1Event, world2Event;
    private bool maskOn, forceFieldOn, speedBoost, attacking = false;
    public Transform rightHandPos;
    private float shieldDuration = 10f, boostTime, canBeHurt = 0f;
    public List<GameObject> enemiesNear;
    bool isTalking = false;
    private int CountAttackClick = 0, spellType = 0;
    private Transform closerEnemy;
    private Vector3 playerJump;
    public Animator maskAnimator, worldAnimator;

    void Start()
    {        
        playerAnimator = GetComponent<Animator>();
        playerController = GetComponent<CharacterController>();
        playerTransform = GetComponent<Transform>();
        enemiesNear = new List<GameObject>();
        currentHP = HP;
        currentMP = MP;
        HPSet.SetMaxHP(HP);
        MPSet.SetMaxMp(MP);
        nextAttackTime = 0f;
        maskOn = false;
    }
    void Update()
    {
        enemiesNear = FindObjectOfType<DetectEnemies>().ListSize();
        HPSet.SetHealth(currentHP);
        MPSet.SetMp(currentMP);
        grounded = playerController.isGrounded;
        if (currentHP <= 0 && playerAnimator.GetBool("Alive") == true)
        {
            playerAnimator.SetBool("Alive", false);
            UI.SetActive(false);
            GameOver.SetActive(true);
        }
        else
        {
            if (!isTalking)
            {
                if (Time.time > shieldDuration)
                {
                    forceField.SetActive(false);
                    forceFieldOn = false;
                }
                if (Time.time > boostTime)
                {
                    speedParticles.SetActive(false);
                    playerAnimator.SetFloat("runMultiplier", 1);
                    speedBoost = false;
                }

                if (speedBoost)
                {
                    speed = 20f;
                }
                else speed = 12f;
                Regeneration();
                animatorState =
                    playerAnimator.GetCurrentAnimatorStateInfo(0);
                HandleRotation();

                float vertical = Input.GetAxis("Vertical");
                float horizontal = Input.GetAxis("Horizontal");

                Vector3 forward = MainCamera.transform.forward;
                Vector3 right = MainCamera.transform.right;
                forward.y = 0;
                right.y = 0;
                forward = forward.normalized;
                right = right.normalized;

                //UnityEngine.Debug.Log(forward);

                if (grounded && playerJump.y < 0)
                {
                    playerJump.y = 0f;
                }

                if (playerJump.y < -6)
                {
                    playerAnimator.SetBool("grounded", true);
                }


                Vector3 forwardRelativeVerticalInput = vertical * forward;
                Vector3 rightRelativeHorizontalInput = horizontal * right;

                Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeHorizontalInput;

                this.transform.Translate(cameraRelativeMovement, Space.World);

                direction = cameraRelativeMovement;

                velocity = new Vector3(direction.x * speed, 0, direction.z * speed);

                float inputMagnitude = Mathf.Clamp01(direction.magnitude);


                if (Input.GetKeyDown(KeyCode.Space) && grounded)
                {
                    playerAnimator.SetTrigger("Jump");
                    playerAnimator.SetBool("grounded", false);
                    playerJump.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
                }

                playerJump.y += gravity * Time.deltaTime;
                //UnityEngine.Debug.Log(grounded);
                playerController.Move(playerJump * Time.deltaTime);

                if (animatorState.IsName("attack1") || animatorState.IsName("second") || animatorState.IsName("third") || animatorState.IsName("Spell1") || animatorState.IsName("Spell2") || animatorState.IsName("Spell3") || animatorState.IsName("Spell4") || animatorState.IsName("land") || animatorState.IsName("mask") || animatorState.IsName("Death"))
                {
                    //UnityEngine.Debug.Log("Se llama ataque1?" + animatorState.IsName("attack1"));
                    velocity = new Vector3(0, 0, 0);
                    playerController.Move(velocity * Time.deltaTime);
                    playerAnimator.SetFloat("Speed", inputMagnitude);

                }
                else
                {
                    if (seMueve)
                    {
                        playerController.Move(velocity * Time.deltaTime);
                        playerAnimator.SetFloat("Speed", inputMagnitude);
                    }
                    else
                    {
                        playerController.Move(Vector3.zero);
                        playerAnimator.SetFloat("Speed", 0);
                    }
                }
                //UnityEngine.Debug.Log(animatorState);
                //UnityEngine.Debug.Log(direction);
                //UnityEngine.Debug.Log(direction);

                //UnityEngine.Debug.Log(ControlPlayerInput);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    CountAttackClick++;
                    //UnityEngine.Debug.Log(CountAttackClick);
                    StaffHand.GetComponent<Collider>().enabled = true;
                    StaffBack.SetActive(false);
                    StaffHand.SetActive(true);
                    attacking = true;
                    //playerController.Move(Vector3.zero);
                    if (enemiesNear.Count > 0)
                    {
                        GetCloserEnemy();
                        Vector3 targetPosition = new Vector3(closerEnemy.position.x,
                                                             playerTransform.position.y,
                                                             closerEnemy.position.z);
                        playerTransform.LookAt(targetPosition);
                        //playerTransform.position.y = 0;
                    }
                    if (CountAttackClick == 1)
                    {
                        playerAnimator.SetInteger("AttackPhase", 1);
                    }
                    if (CountAttackClick >= 24)
                    {
                        ResetAttackPhase();
                    }

                }

                if (inputMagnitude > 0 && !attacking)
                {
                    seMueve = true;
                }
                else seMueve = false;


                //if (Input.GetKeyDown(KeyCode.Mouse1))
                //{
                //    playerAnimator.SetTrigger("Magic");
                //}

                if (Input.GetKeyDown(KeyCode.Z) && Time.time >= spell1CD)
                {
                    if (currentMP >= 90 && enemiesNear.Count > 0)
                    {
                        StaffHand.GetComponent<Collider>().enabled = false;
                        StaffBack.SetActive(false);
                        StaffHand.SetActive(true);
                        sparkles.SetActive(true);
                        GetCloserEnemy();
                        Vector3 targetPosition = new Vector3(closerEnemy.position.x,
                                                             playerTransform.position.y,
                                                             closerEnemy.position.z);
                        playerTransform.LookAt(targetPosition);
                        playerAnimator.SetTrigger("spell3");
                        spellType = 1;
                    }
                }

                if (Input.GetKeyDown(KeyCode.X) && Time.time >= spell2CD)
                {
                    if (currentMP >= 65)
                    {
                        StaffHand.GetComponent<Collider>().enabled = false;
                        StaffBack.SetActive(false);
                        StaffHand.SetActive(true);
                        sparkles.SetActive(true);
                        playerAnimator.SetTrigger("spell4");
                        spellType = 2;
                    }
                }

                if (Input.GetKeyDown(KeyCode.C) && Time.time >= spell3CD)
                {
                    if (currentMP >= 200 && enemiesNear.Count > 0)
                    {
                        StaffHand.GetComponent<Collider>().enabled = false;
                        StaffBack.SetActive(false);
                        StaffHand.SetActive(true);
                        sparkles.SetActive(true);
                        GetCloserEnemy();
                        Vector3 targetPosition = new Vector3(closerEnemy.position.x,
                                                             playerTransform.position.y,
                                                             closerEnemy.position.z);
                        playerTransform.LookAt(targetPosition);
                        playerAnimator.SetTrigger("spell1");
                        spellType = 3;
                    }
                }

                if (Input.GetKeyDown(KeyCode.V) && Time.time >= spell4CD)
                {
                    if (currentMP >= 120)
                    {
                        StaffHand.GetComponent<Collider>().enabled = false;
                        StaffBack.SetActive(false);
                        StaffHand.SetActive(true);
                        sparkles.SetActive(true);
                        playerAnimator.SetTrigger("spell2");
                        spellType = 4;
                    }
                }

                if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= maskCD)
                {
                    PutMask();
                }
            }
        }
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = direction.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = direction.z;

        Quaternion currentRotation = transform.rotation;
        //UnityEngine.Debug.Log(seMueve);
        if (seMueve == true && animatorState.IsName("Idle") || seMueve == true && animatorState.IsName("run"))
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationPerFrame * Time.deltaTime);
        }
    }

    public void Ocultar_Staff_Espalda()
    {
        StaffBack.SetActive(false);
        StaffHand.SetActive(true);
    }

    public void Ocultara_Staff_Mano()
    {
        StaffHand.SetActive(false);
        StaffBack.SetActive(true);
        sparkles.SetActive(false);
    }

    public void Regeneration()
    {
        if (currentHP < HP)
            currentHP += (int)Mathf.Round(regenerationSpeedHP * Time.deltaTime);
        if (currentMP < MP)
            currentMP += (int)Mathf.Round(regenerationSpeedMP * Time.deltaTime);
        //UnityEngine.Debug.Log(currentMP);
    }

    public void GetHurt(int damage)
    {
        if (!forceFieldOn)
            currentHP -= damage;
    }

    public void SetTalk()
    {
        velocity = new Vector3(0, gravity, 0);
        playerController.Move(velocity * Time.deltaTime);
        UnityEngine.Debug.Log("Esta hablando");
        isTalking = true;
    }

    public void RemoveTalk()
    {
        isTalking = false;
    }

    public void CheckAttackPhase()
    {
        UnityEngine.Debug.Log("revisando fase de ataque" + animatorState.IsName("attack1"));
        if (animatorState.IsName("attack1"))
        {
            if(CountAttackClick > 1)
            {
                playerAnimator.SetInteger("AttackPhase", 2);
            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (animatorState.IsName("second"))
        {
            if(CountAttackClick > 2)
            {
                playerAnimator.SetInteger("AttackPhase", 3);
            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if(animatorState.IsName("third"))
        {
            if(CountAttackClick >= 3)
            {
                ResetAttackPhase();
            }
        }
    }

    public void ResetAttackPhase()
    {
        CountAttackClick = 0;
        playerAnimator.SetInteger("AttackPhase", 0);
        StaffHand.SetActive(false);
        StaffBack.SetActive(true);
        attacking = false;
    }

    public void SpellType()
    {
        if(spellType == 1)
        {
            currentMP -= 90;
            spell1.Raise();
            spell1CD = Time.time + 6.26f;
            GameObject currentBlast = Instantiate(blast, rightHandPos.position, Quaternion.identity);
            Vector3 blastDirection = closerEnemy.position - rightHandPos.position;
            currentBlast.GetComponent<Rigidbody>().AddForce(blastDirection * blastForce, ForceMode.Impulse);
            UnityEngine.Debug.Log("blast direction" + blastDirection);
            AudioManager.Instance.PlaySFX(1);
        }

        if(spellType == 2)
        {
            currentMP -= 65;
            spell2.Raise();
            spell2CD = Time.time + 20f;
            speedParticles.SetActive(true);
            speedBoost = true;
            playerAnimator.SetFloat("runMultiplier", 2);
            boostTime = Time.time + 15f;
        }

        if(spellType == 3)
        {
            currentMP -= 200;
            spell3.Raise();
            spell3CD = Time.time + 33.62f;
            foreach (GameObject enemyTransform in enemiesNear)
            {
                Destroy(Instantiate(lightBeam, enemyTransform.transform.position, Quaternion.identity), 2f);

            }
        }

        if(spellType == 4)
        {
            currentMP -= 120;
            spell4.Raise();
            spell4CD = Time.time + 14.5f;
            forceField.SetActive(true);
            forceFieldOn = true;
            shieldDuration = Time.time + 10f;
            AudioManager.Instance.PlaySFX(3);
        }
        spellType = 0;
    }

    public void GetCloserEnemy()
    {
        float distance = 999;
        foreach(GameObject enemy in enemiesNear)
        {
            if (Mathf.Abs(Vector3.Distance(playerTransform.position, enemy.transform.position)) < distance)
            {
                distance = Mathf.Abs(Vector3.Distance(playerTransform.position, enemy.transform.position));
                closerEnemy = enemy.transform;
            }
        }
    }

    public void PutMask()
    {
        if (maskOn && enemiesNear.Count == 0 && Time.time >= maskCD)
        {
            maskAnimator.SetTrigger("mask");
            worldAnimator.SetTrigger("change");
            maskOn = false;
            world1Event.Raise();
            maskCD = Time.time + 15f;
        }
        else
        {
            if (!maskOn && enemiesNear.Count == 0 && Time.time >= maskCD)
            {
                maskAnimator.SetTrigger("mask");
                worldAnimator.SetTrigger("change");
                maskOn = true;
                world2Event.Raise();
                maskCD = Time.time + 15f;
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    { 
        if(collision.transform.CompareTag("EnemyDamager") && canBeHurt < Time.time)
        {
            UnityEngine.Debug.Log("collision con damager");
            int damageReceived = (int)collision.gameObject.GetComponent<EnemyBullet>().PassDamage();
            GetHurt(damageReceived);
            canBeHurt = Time.time + 0.5f;
        }

        if (collision.transform.CompareTag("EnemyDamagerSpecial") && canBeHurt < Time.time)
        {
            UnityEngine.Debug.Log("collision con damager");
            int damageReceived = (int)collision.gameObject.GetComponent<BossKatana>().PassDamage();
            GetHurt(damageReceived);
            canBeHurt = Time.time + 0.5f;
        }
    }
}