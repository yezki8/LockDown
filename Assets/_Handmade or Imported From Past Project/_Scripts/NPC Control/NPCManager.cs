using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    [Header("Stats")]
    public float HealthPoint;
    public enum AliveState
    {
        alive,
        dead
    }
    public AliveState aliveState;
    public bool invincible = false;

    [Header("AI")]
    public NavMeshAgent navMeshAgent;
    public enum NpcType
    {
        enemy,
        ally,
        statue
    }
    public NpcType npcType;
    public GameObject ObjectTarget;
    Transform target;
    bool despawning = false;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null){
            npcType = NpcType.statue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckAliveState();
        if (aliveState == AliveState.dead)
        {
            if (despawning == false)
            {
                //StartCoroutine(Despawning());
            }
        }
    }

    public void CheckAliveState()
    {
        if (HealthPoint > 0)
        {
            aliveState = AliveState.alive;
        }
        else
        {
            aliveState = AliveState.dead;
        }
    }

    public void CallTakeDamage(float damage)
    {
        if (invincible == false)
        {
            StartCoroutine(TakeDamage(damage));
        }
    }

    IEnumerator TakeDamage(float damage)
    {
        HealthPoint -= damage;
        invincible = true;
        yield return new WaitForSeconds(0.5f);
        invincible = false;
    }

    IEnumerator Despawning()
    {
        despawning = true;
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);
    }

}
