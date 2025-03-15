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
    public float[] HealthThreshold;

    public GameObject[] Ammo;
    public int ammoType;
    public float MaxAmmo = 20;
    public float AmmoPoint = 20;
    public float[] AmmoTreshold;

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
        int Hpercentage = HealthThreshold.Length;
        int Mpercentage = AmmoTreshold.Length;

        for (int i = 0; i < HealthThreshold.Length; i++)
        {
            HealthThreshold[i] = (MaxHealth * (HealthThreshold.Length - i)) / (HealthThreshold.Length);
        }
        for (int i = 0; i < AmmoTreshold.Length; i++)
        {
            AmmoTreshold[i] = (MaxAmmo * (AmmoTreshold.Length - i)) / (AmmoTreshold.Length);
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
            for (int i = 0; i < HealthThreshold.Length; i++)
            {
                if (i == 0)
                {
                    if (HealthPoint >= HealthThreshold[i])
                    {
                        healthType = i;
                    }
                }
                else if (i < HealthThreshold.Length - 1)
                {
                    if (HealthPoint >= HealthThreshold[i] && HealthPoint < HealthThreshold[i - 1])
                    {
                        healthType = i;
                    }
                }
            }
        }
        else
        {
            healthType = HealthThreshold.Length - 1;
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
        if (AmmoPoint < 0)
        {
            AmmoPoint = 0;
        }
        else if (AmmoPoint > MaxAmmo)
        {
            AmmoPoint = MaxAmmo;
        }
        //Bar
        if (AmmoPoint > 0)
        {
            for (int i = 0; i < AmmoTreshold.Length; i++)
            {
                if (i == 0)
                {
                    if (AmmoPoint >= AmmoTreshold[i])
                    {
                        ammoType = i;
                    }
                }
                else if (i < AmmoTreshold.Length - 1)
                {
                    if (AmmoPoint >= AmmoTreshold[i] && AmmoPoint < AmmoTreshold[i - 1])
                    {
                        ammoType = i;
                    }
                }
            }
        }
        else
        {
            ammoType = AmmoTreshold.Length - 1;
        }

        for (int i = 0; i < Ammo.Length; i++)
        {
            if (i == ammoType)
            {
                Ammo[i].SetActive(true);
            }
            else
            {
                Ammo[i].SetActive(false);
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
