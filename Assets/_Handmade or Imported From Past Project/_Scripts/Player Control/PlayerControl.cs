﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Components")]
    public GameObject PlayerBody;
    public GameObject MainCamera;
    public GameObject Indicator;
    public GameObject AimingCameraAnchor;
    public PlayerManager playerManager;

    [Header("Movement Physic")]
    public float PlayerMovementSpeed;
    Vector2 movementDirection;
    Vector2 velocityCache;
    Vector2 curveVelocity = Vector3.zero;
    public float friction = 3;
    public float rotationSpeed = 5;

    [Header("Combat Control")]
    public int ammo;
    public enum CombatState
    {
        normal,
        aiming
    };
    public CombatState combatState;

    public bool AllowedToAttack = true;
    public bool ReadyToAttack = false;
    bool attacking = false;
    public float AttackCooldown = 0;
    float attackCcountdowm;
    public InventorySlotController EquippedMelle;
    public InventorySlotController EquippedRange;
    public bool IsIdle;
    public bool IsRight;

    public bool stunned = false;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.Find("Main Camera");
        Indicator = PlayerBody.transform.GetChild(1).gameObject;
        AimingCameraAnchor = PlayerBody.transform.GetChild(0).gameObject;
        playerManager = this.GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (playerManager.aliveState == PlayerManager.AliveState.alive)
        {
            if (stunned == false)
            {
                ProcessInput();
                Combat();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerManager.aliveState == PlayerManager.AliveState.alive)
        {
            if (stunned == false)
            {
                Movement();
            }
        }
    }
    
    private void ProcessInput()
    {
        //Movement input
        float HorizontalMove = Input.GetAxis("Horizontal");
        float VerticalMove = Input.GetAxis("Vertical");

        if (HorizontalMove == 0 && VerticalMove == 0)
        {
            IsIdle = true;
        }
        else
        {
            IsIdle = false;
        }

        if (HorizontalMove >= 0)
        {
            IsRight = true;
        }
        else
        {
            IsRight = false;
        }

        movementDirection = new Vector2(HorizontalMove, VerticalMove);
        movementDirection.Normalize();
        velocityCache = movementDirection * PlayerMovementSpeed * 0.01f;
        curveVelocity = Vector2.Lerp(curveVelocity, velocityCache, 5 * friction * Time.deltaTime);

        //Combat input
        if (Input.GetMouseButton(1))
        {
            combatState = CombatState.aiming;
            Debug.Log("Pressed secondary button.");
        }
        else
        {
            combatState = CombatState.normal;
        }

        //Attack
        if (attackCcountdowm > 0)
        {
            attackCcountdowm -= 1 * Time.deltaTime;
        }
        if (attackCcountdowm <= 0)
        {
            ReadyToAttack = true;
        }
        else
        {
            ReadyToAttack = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (AllowedToAttack)
            {
                if (combatState == CombatState.normal)
                {
                    if (ReadyToAttack)
                    {
                        attacking = true;
                    }
                }
                else
                {
                    if (ReadyToAttack && playerManager.AmmoPoint > 0)
                    {
                        attacking = true;
                    }
                }
            }
        }
    }

    //MOVEMENT====================================================================================================
    private void Movement()
    {
        transform.Translate(curveVelocity);

        if (movementDirection != Vector2.zero && combatState == CombatState.normal)
        {
            Quaternion toRotate = Quaternion.LookRotation(Vector3.forward, movementDirection);
            PlayerBody.transform.rotation = Quaternion.RotateTowards(PlayerBody.transform.rotation, toRotate, (rotationSpeed * 100) * Time.deltaTime);
        }
    }

    //COMBAT=============================================================================================================
    void Combat()
    {
        //Look At Camera
        if (combatState == CombatState.normal)
        {
            MainCamera.GetComponent<ChaseController>().objectToFollow = PlayerBody;
            MainCamera.GetComponent<ChaseController>().followSpeed = 5f;

            var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(PlayerBody.transform.position);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            PlayerBody.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            if (attacking)
            {
                if (EquippedMelle.ItemStored != null)
                {
                    BasicMelleAttack();
                }
                attacking = false;
            }
        }
        else if (combatState == CombatState.aiming)
        {
            MainCamera.GetComponent<ChaseController>().objectToFollow = AimingCameraAnchor;
            MainCamera.GetComponent<ChaseController>().followSpeed = 5f;

            var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(PlayerBody.transform.position);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            PlayerBody.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            if (attacking)
            {
                if (EquippedRange.ItemStored != null)
                {
                    BasicRangeAttack();
                }
                attacking = false;
            }
        }
    }

    void BasicMelleAttack()
    {
        ItemSO equippedSO = EquippedMelle.ItemStored;
        GameObject hitboxPrefab = equippedSO.ProjectileObject;
        GameObject BasicAttack = Instantiate(hitboxPrefab, Indicator.transform);
        BasicAttack.GetComponent<HitBoxBehaviour>().owner = this.tag;
        BasicAttack.GetComponent<HitBoxBehaviour>().ownerObject = this.gameObject;
        BasicAttack.GetComponent<HitBoxBehaviour>().despawn = AttackCooldown - 0.1f;
        attackCcountdowm = AttackCooldown;
    }

    void BasicRangeAttack()
    {
        ItemSO equippedSO = EquippedRange.ItemStored;
        GameObject projectilePrefab = equippedSO.ProjectileObject;
        GameObject BasicAttack = Instantiate(projectilePrefab, Indicator.transform.position, Indicator.transform.rotation);
        BasicAttack.GetComponent<HitBoxBehaviour>().owner = this.tag;
        BasicAttack.GetComponent<HitBoxBehaviour>().ownerObject = this.gameObject;
        attackCcountdowm = AttackCooldown;
        
        //EquippedRange.Amount -= 1;
    }

    public void LockUnlockAttack()
    {
        AllowedToAttack = !AllowedToAttack;
    }

    //Collider========================================================================================================
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Text Trigger" || collision.tag == "Checkpoint")
        {
            //collision.GetComponent<ActivateTextBubble>().playerIsReading = false;
        }
        if (collision.tag == "Enemy Vision")
        {
            collision.GetComponent<NPCVision>().target = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint")
        {
            playerManager.HealthPoint = playerManager.MaxHealth;
            playerManager.levelManager.LastCheckpoint = collision.gameObject;
        }
        if (collision.tag == "Finish")
        {
            playerManager.levelManager.gameState = LevelManager.GameState.notPlaying;
        }
        if (collision.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<NPCManager>().aliveState == NPCManager.AliveState.alive)
            {
                this.GetComponent<PlayerManager>().CallTakeDamage(1);
                GotKnocked(collision.gameObject, 4);
            }
        }
        if (collision.tag == "Key")
        {
            Destroy(collision.gameObject);
        }
        if (collision.tag == "CollectibleItem")
        {
            InventoryController.Instance.AddItem(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<NPCManager>().aliveState == NPCManager.AliveState.alive)
            {
                this.GetComponent<PlayerManager>().CallTakeDamage(1);
                GotKnocked(collision.gameObject, 4);
            }
        }
    }

    //Physic==============================================================================================
    public void GotKnocked(GameObject attacker, float knockForce)
    {
        StartCoroutine(callKnock(attacker, knockForce));
    }

    IEnumerator callKnock(GameObject attacker, float knockForce)
    {
        stunned = true;
        Vector2 dir = attacker.transform.position - transform.position;
        dir.Normalize();
        GetComponent<Rigidbody2D>().AddForce((-dir) * knockForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        stunned = false;
    }

}
