using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoving : MonoBehaviour
{
    public Transform target;
    public float repathTime;
    public Animator animator;

    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(Repath());
    }

    IEnumerator Repath()
    {
        yield return new WaitForSeconds(repathTime);
        navMeshAgent.SetDestination(target.position);

        StartCoroutine(Repath());
    }

    private void Update()
    {
        animator?.SetFloat("Velocity", navMeshAgent.velocity.magnitude);
    }
}
