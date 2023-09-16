using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    private Transform target;

    public int bullets;
    public int bulletsMax;

    [SerializeField] private Animator animator;

    private void Start()
    {
        //animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (target is not null)
        {
            transform.LookAt(target);
        }
    }

    public void StartShooting(Transform newTarget)
    {
        target = newTarget;
        animator.SetBool("isFiring", true);
    }

    public void StopShooting()
    {
        target = null;
        animator.SetBool("isFiring", false);
    }
}
