using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float lockOnRange = 10f;

    [SerializeField] float turnSmoothTime = 0.2f;
    float smoothTurnVelocity;

    Transform target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if(Physics.CheckSphere(transform.position, lockOnRange, targetLayer))
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
}
