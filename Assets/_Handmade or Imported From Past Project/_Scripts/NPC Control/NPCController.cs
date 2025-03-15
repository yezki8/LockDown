using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("Scripts")]
    public NPCManager nPCManager;
    public NPCVision nPCVision;
    public GameObject NpcBody;

    [Header("Movement Physic")]
    public float MovementSpeed;
    Vector2 movementDirection;
    Vector2 velocityCache;
    Vector2 curveVelocity = Vector3.zero;
    public float friction = 1;
    public float rotationSpeed = 10;
    public float movementStopDistance;
    public Vector3 Target;
    public bool stunned = false;
    public Collider2D collider2D;

    [Header("Combat Control")]
    public int ammo;
    public enum CombatState
    {
        still,
        melle,
        range,
        both
    };
    public CombatState combatState;
    public float attackRate;
    public bool canAttack = true;
    public GameObject MelleObject;
    public GameObject RangeObject;
    public float cooldown = 0.5f;
    public GameObject Indicator;
    public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        nPCManager = this.GetComponent<NPCManager>();
        NpcBody = this.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (nPCManager.aliveState == NPCManager.AliveState.alive)
        {
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            collider2D.enabled = true;
        }

        else
        {
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            collider2D.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (nPCManager.aliveState == NPCManager.AliveState.alive)
        {
            if (stunned == false)
            {
                BasicMovement();
            }
        }
    }

    //MOVEMENT==========================================================================================
    public void BasicMovement()
    {
        float dist = Vector2.Distance(Target, transform.position);
        if (dist > movementStopDistance)
        {
            float movementDirection = MovementSpeed * 0.1f;
            transform.position = Vector2.MoveTowards(transform.position, Target, movementDirection * Time.deltaTime);
            isMoving = true;
        }
        else
        {
            if (nPCVision.isHunting)
            {
                canAttack = false;
            }
            isMoving = false;
        }

        Vector2 dir = Target - this.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        NpcBody.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    //COLLIDER==========================================================================================
    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    //Attack=================================================================================================
    public void AttackingVoid()
    {
        GameObject BasicAttack = Instantiate(MelleObject, Indicator.transform);
        BasicAttack.GetComponent<HitBoxBehaviour>().isHostileToPlayer = true;
        BasicAttack.GetComponent<HitBoxBehaviour>().owner = this.tag;
        BasicAttack.GetComponent<HitBoxBehaviour>().ownerObject = this.gameObject;
        BasicAttack.GetComponent<HitBoxBehaviour>().despawn = cooldown - 0.1f;
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

    //SNEED==================================================================================================
    public void goIdle()
    {
        stunned = false;
        canAttack = true;
    }
}
