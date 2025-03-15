using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBehaviour : MonoBehaviour
{
    [Header("Main parameter")]
    public float speed;
    public float damage;
    public float despawn;
    public string owner;
    public GameObject ownerObject;
    public enum HitType
    {
        melle,
        range
    }
    public HitType hitType;

    [Header("Additional parameter")]
    public bool isHostileToPlayer = false;
    public float knockForce = 2f;
    public GameObject childHidBox;

    // Start is called before the first frame update
    void Start()
    {
        if (despawn <= 0)
        {
            despawn = 1; 
        }
        StartCoroutine(HitBoxDespawn(despawn));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ProjectileMovement();
    }

    void ProjectileMovement()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.up * speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitType == HitType.range)
        {
            if (collision.tag != "Enemy Vision")
            {
                if (collision.tag != "Enemy" && isHostileToPlayer)
                {
                    StartCoroutine(HitBoxDespawn(0));
                }
            }
        }
        if (collision.gameObject.tag == "Untagged")
        {
            
        }
        if (isHostileToPlayer)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (collision.gameObject.GetComponent<PlayerManager>().aliveState == PlayerManager.AliveState.alive)
                {
                    collision.gameObject.GetComponent<PlayerManager>().CallTakeDamage(damage);
                    if (hitType == HitType.range)
                    {
                        collision.gameObject.GetComponent<PlayerControl>().GotKnocked(this.gameObject, knockForce);
                        StartCoroutine(HitBoxDespawn(0));
                    }
                    else
                    {
                        collision.gameObject.GetComponent<PlayerControl>().GotKnocked(ownerObject, knockForce);
                    }
                }
            }
        }
        else
        {
            if (collision.gameObject.tag == "Enemy")
            {                
                if (collision.gameObject.GetComponent<NPCManager>().aliveState == NPCManager.AliveState.alive)
                {
                    collision.gameObject.GetComponent<NPCManager>().CallTakeDamage(damage);
                    if (hitType == HitType.range)
                    {
                        collision.gameObject.GetComponent<NPCController>().GotKnocked(this.gameObject, knockForce);
                        StartCoroutine(HitBoxDespawn(0));
                    }
                    else
                    {
                        collision.gameObject.GetComponent<NPCController>().GotKnocked(ownerObject, knockForce);
                        ownerObject.GetComponent<PlayerManager>().ManaPoint += 1;
                    }
                }
            }
        }
    }

    public void ForceDespawn(float time)
    {
        StartCoroutine(HitBoxDespawn(time));
    }

    IEnumerator HitBoxDespawn(float time)
    {         
        yield return new WaitForSeconds(time);
        if (childHidBox != null)
        {
            GameObject child = Instantiate(childHidBox, this.transform.position, this.transform.rotation);
            child.GetComponent<HitBoxBehaviour>().damage = damage;
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
