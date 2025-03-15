using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCVision : MonoBehaviour
{
    [Header("Parameter")]
    public GameObject Owner;
    public Vector3 startingPlace;
    public NPCController npController;
    public float stoppingDistance;

    [Header("Targer and movement")]
    public GameObject target;
    public bool isHunting;

    // Start is called before the first frame update
    void Start()
    {
        npController = Owner.GetComponent<NPCController>();
        startingPlace = new Vector3(Owner.transform.position.x, Owner.transform.position.y,0);
        npController.Target = startingPlace;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            npController.Target = startingPlace;
            npController.movementStopDistance = 0.1f;
            isHunting = false;
        }
        else
        {
            npController.Target = target.transform.position;
            npController.movementStopDistance = stoppingDistance;
            isHunting = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //error handling kalo player mati di dalem collidernya
            if (collision.GetComponent<PlayerManager>().aliveState == PlayerManager.AliveState.alive)
            {
                target = collision.gameObject;
            }
            else
            {
                target = null;
            }
        }
    }
}
