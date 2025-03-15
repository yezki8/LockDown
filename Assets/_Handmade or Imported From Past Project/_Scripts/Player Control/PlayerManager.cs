using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] Health;
    public int healthType;
    public float MaxHealth = 6;
    public float HealthPoint = 16;
    public float[] HealthTreshold;

    public GameObject[] Mana;
    public int manaType;
    public float MaxMana = 20;
    public float ManaPoint = 20;
    public float[] ManaTreshold;

    public enum AliveState
    {
        alive,
        dead
    }
    public AliveState aliveState;
    public bool invincible = false;

    //SceneManager
    public GameObject LevelManagerObject;
    public LevelManager levelManager;
 
    // Start is called before the first frame update
    void Start()
    {
        LevelManagerObject = GameObject.Find("Level Manager");
        levelManager = LevelManagerObject.GetComponent<LevelManager>();
        CheckHealthManaTreshold();
        StartCoroutine(EnumHealthMana());
    }

    // Update is called once per frame
    void Update()
    {        
        CheckAliveState();
        if (aliveState == AliveState.dead)
        {
            
        }
    }

    public void CheckHealthManaTreshold()
    {
        int Hpercentage = HealthTreshold.Length;
        int Mpercentage = ManaTreshold.Length;

        for (int i = 0; i < HealthTreshold.Length; i++)
        {
            HealthTreshold[i] = (MaxHealth * (HealthTreshold.Length - i)) / (HealthTreshold.Length);
        }
        for (int i = 0; i < ManaTreshold.Length; i++)
        {
            ManaTreshold[i] = (MaxMana * (ManaTreshold.Length - i)) / (ManaTreshold.Length);
        }
    }

    IEnumerator EnumHealthMana()
    {
        CheckHealthMana();
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(EnumHealthMana());
    }

    public void CheckHealthMana()
    {
        //Health Aribute ==================================================================
        //Error Handling Treshold
        if (HealthPoint < 0)
        {
            HealthPoint = 0;
        }
        else if (HealthPoint > MaxHealth)
        {
            HealthPoint = MaxHealth;
        }
        //Bar
        if (aliveState == AliveState.alive)
        {
            for (int i = 0; i < HealthTreshold.Length; i++)
            {
                if (i == 0)
                {
                    if (HealthPoint >= HealthTreshold[i])
                    {
                        healthType = i;
                    }
                }
                else if (i < HealthTreshold.Length - 1)
                {
                    if (HealthPoint >= HealthTreshold[i] && HealthPoint < HealthTreshold[i - 1])
                    {
                        healthType = i;
                    }
                }
            }
        }
        else
        {
            healthType = HealthTreshold.Length - 1;
        }
        for (int i = 0; i < Health.Length; i++)
        {
            if (i == healthType)
            {
                Health[i].SetActive(true);
            }
            else
            {
                Health[i].SetActive(false);
            }
        }

        //Mana Attribute ==========================================================================
        //Error handling treshold
        if (ManaPoint < 0)
        {
            ManaPoint = 0;
        }
        else if (ManaPoint > MaxMana)
        {
            ManaPoint = MaxMana;
        }
        //Bar
        if (ManaPoint > 0)
        {
            for (int i = 0; i < ManaTreshold.Length; i++)
            {
                if (i == 0)
                {
                    if (ManaPoint >= ManaTreshold[i])
                    {
                        manaType = i;
                    }
                }
                else if (i < ManaTreshold.Length - 1)
                {
                    if (ManaPoint >= ManaTreshold[i] && ManaPoint < ManaTreshold[i - 1])
                    {
                        manaType = i;
                    }
                }
            }
        }
        else
        {
            manaType = ManaTreshold.Length - 1;
        }

        for (int i = 0; i < Mana.Length; i++)
        {
            if (i == manaType)
            {
                Mana[i].SetActive(true);
            }
            else
            {
                Mana[i].SetActive(false);
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
        yield return new WaitForSeconds(0.2f);
        invincible = false;
    }

    public void PlayerRespawn(Transform spawnLocation)
    {
        HealthPoint = MaxHealth;
        aliveState = AliveState.alive;
        this.transform.position = spawnLocation.position;
    }
}
