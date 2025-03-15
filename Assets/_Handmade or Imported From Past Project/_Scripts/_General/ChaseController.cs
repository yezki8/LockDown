using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseController : MonoBehaviour
{
    public GameObject objectToFollow;
    public Transform transformToFollow;
    public Vector3 offset;
    public float followSpeed = 10;
    public float lookSpeed = 10;

    public void FixedUpdate()
    {
        if (objectToFollow != null)
        {
            transformToFollow = objectToFollow.transform;
        }
        LookAtTarger();
        MoveToTarget();
    }

    public void LookAtTarger()
    {
        Vector3 _lookDirection = transformToFollow.position - transform.position;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
    }

    public void MoveToTarget()
    {
        Vector3 _targetPos = transformToFollow.position +
            transformToFollow.forward * offset.z +
            transformToFollow.right * offset.x +
            transformToFollow.up * offset.y;

        transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
    }

}
