using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAnimationController : MonoBehaviour
{
    public GameObject NPCBody;
    Transform NPCTransform;
    public NPCManager nPCManager;
    public NPCController nPCController;
    public NPCVision nPCVision;
    public Animator anim;

    [Header("Parameters")]
    public float directionValue;
    public enum Direction
    {
        north,
        south,
        east,
        west
    }
    public Direction direction;
    // Start is called before the first frame update
    void Start()
    {
        NPCTransform = NPCBody.transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDirection();
        CheckAnimation();
    }

    void CheckDirection()
    {
        directionValue = NPCTransform.localRotation.eulerAngles.z;
        //45 ,135, 225, 315, 45
        if ((directionValue > 0 && directionValue <= 180))
        {
            direction = Direction.west;
        }
        else if (directionValue > 180 && directionValue <= 360)
        {
            direction = Direction.east;
        }
    }

    void CheckAnimation()
    {
        switch (direction)
        {
            case Direction.east:
                if (nPCManager.aliveState == NPCManager.AliveState.alive)
                {
                    if (nPCController.stunned)
                    {
                        anim.Play("attacked e");
                    }
                    else
                    {
                        if (nPCController.canAttack == false)
                        {
                            anim.Play("attack e");
                        }
                        else
                        {
                            if (nPCController.isMoving == true)
                            {
                                anim.Play("move e");
                            }
                            else
                            {
                                anim.Play("idle w");
                            }
                        }
                    }
                }
                else
                {
                    anim.Play("dead");
                }
                break;

            case Direction.west:
                if (nPCManager.aliveState == NPCManager.AliveState.alive)
                {
                    if (nPCController.stunned)
                    {
                        anim.Play("attacked w");
                    }
                    else
                    {
                        if (nPCController.canAttack == false)
                        {
                            anim.Play("attack w");
                        }
                        else
                        {
                            if (nPCController.isMoving == true)
                            {
                                anim.Play("move w");
                            }
                            else
                            {
                                anim.Play("idle w");
                            }
                        }
                    }
                }
                else
                {
                    anim.Play("dead");
                }
                break;
        }
    }

    public void ToIdle()
    {
        nPCController.stunned = false;
        nPCController.canAttack = true;
    }

    public void CallAttack()
    {
        nPCController.AttackingVoid();
    }
}
