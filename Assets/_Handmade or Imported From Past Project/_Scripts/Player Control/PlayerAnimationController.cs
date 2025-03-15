using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Script Declaration")]
    public GameObject PlayerBody;
    Transform PlayerTransform;
    public PlayerManager playerManager;
    public PlayerControl playerControl;
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
        PlayerTransform = PlayerBody.transform;
        playerManager = this.transform.parent.GetComponent<PlayerManager>();
        playerControl = this.transform.parent.GetComponent<PlayerControl>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDirection();
        CheckAnimation();
    }

    void CheckDirection()
    {
        directionValue = PlayerTransform.localRotation.eulerAngles.z;
        //45 ,135, 225, 315, 45
        if ((directionValue > 315 && directionValue <= 360) || (directionValue > 0 && directionValue <= 45))
        {
            direction = Direction.north;
        }
        else if (directionValue > 225 && directionValue <= 315)
        {
            direction = Direction.east;
        }
        else if (directionValue > 135 && directionValue <= 225)
        {
            direction = Direction.south;
        }
        else if (directionValue > 45 && directionValue <= 135)
        {
            direction = Direction.west;
        }
    }

    void CheckAnimation()
    {
        switch (direction)
        {
            case Direction.north:
                if (playerManager.aliveState == PlayerManager.AliveState.alive)
                {
                    if (playerControl.stunned)
                    {
                        anim.Play("hit n");
                    }
                    else
                    {
                        if (playerControl.combatState == PlayerControl.CombatState.normal)
                        {
                            if (playerControl.ReadyToAttack == false)   //ini dangerous bts, harus diubah ke depan
                            {
                                anim.Play("melle f");
                            }
                            else
                            {
                                if (playerControl.IsIdle)
                                {
                                    anim.Play("idle f");
                                }
                                else
                                {
                                    anim.Play("run 1");
                                }
                            }
                        }
                        else
                        {
                            if (playerControl.ReadyToAttack == false)
                            {
                                anim.Play("range f");
                            }
                            else
                            {
                                if (playerControl.IsIdle)
                                {
                                    anim.Play("idle f");
                                }
                                else
                                {
                                    anim.Play("run 2");
                                }
                            }
                        }
                    }                    
                }
                else
                {
                    anim.Play("dead e");
                }
                break;

            case Direction.west:
                if (playerManager.aliveState == PlayerManager.AliveState.alive)
                {
                    if (playerControl.stunned)
                    {
                        anim.Play("hit w");
                    }
                    else
                    {
                        if (playerControl.combatState == PlayerControl.CombatState.normal)
                        {
                            if (playerControl.ReadyToAttack == false)
                            {
                                anim.Play("melle w");
                            }
                            else
                            {

                                if (playerControl.IsIdle)
                                {
                                    anim.Play("idle w");
                                }
                                else
                                {
                                    anim.Play("run 1 fw");
                                }
                            }
                        }
                        else
                        {
                            if (playerControl.ReadyToAttack == false)
                            {
                                anim.Play("range w");
                            }
                            else
                            {
                                if (playerControl.IsIdle)
                                {
                                    anim.Play("idle w");
                                }
                                else
                                {
                                    if (playerControl.IsRight)
                                    {
                                        anim.Play("run 2 bw");
                                    }
                                    else
                                    {
                                        anim.Play("run 2 fw");
                                    }
                                }
                            }
                        }
                    }                    
                }else
                {
                    anim.Play("dead e");
                }
                break;

            case Direction.east:
                if (playerManager.aliveState == PlayerManager.AliveState.alive)
                {
                    if (playerControl.stunned)
                    {
                        anim.Play("hit e");
                    }
                    else
                    {
                        if (playerControl.combatState == PlayerControl.CombatState.normal)
                        {
                            if (playerControl.ReadyToAttack == false)
                            {
                                anim.Play("melle e");
                            }
                            else
                            {
                                if (playerControl.IsIdle)
                                {
                                    anim.Play("idle e");
                                }
                                else
                                {
                                    anim.Play("run 1 fe");
                                }
                            }
                        }
                        else
                        {
                            if (playerControl.ReadyToAttack == false)
                            {
                                anim.Play("range e");
                            }
                            else
                            {
                                if (playerControl.IsIdle)
                                {
                                    anim.Play("idle e");
                                }
                                else
                                {
                                    //anim.Play("run 2 fe");
                                    if (playerControl.IsRight)
                                    {
                                        anim.Play("run 2 fe");
                                    }
                                    else
                                    {
                                        anim.Play("run 2 be");
                                    }
                                }
                            }
                        }
                    }                    
                }
                else
                {
                    anim.Play("dead w");
                }
                break;

            case Direction.south:
                if (playerManager.aliveState == PlayerManager.AliveState.alive)
                {
                    if (playerControl.stunned)
                    {
                        anim.Play("hit s");
                    }
                    else
                    {
                        if (playerControl.combatState == PlayerControl.CombatState.normal)
                        {
                            if (playerControl.ReadyToAttack == false)
                            {
                                anim.Play("melle s");
                            }
                            else
                            {
                                if (playerControl.IsIdle)
                                {
                                    anim.Play("idle s");
                                }
                                else
                                {
                                    anim.Play("run 1 s");
                                }
                            }
                        }
                        else
                        {
                            if (playerControl.ReadyToAttack == false)
                            {
                                anim.Play("range s");
                            }
                            else
                            {
                                if (playerControl.IsIdle)
                                {
                                    anim.Play("idle s");
                                }
                                else
                                {
                                    anim.Play("run 2 s");
                                }
                            }
                        }
                    }                    
                }
                else
                {
                    anim.Play("dead w");
                }
                break;
        }
    }
}
