using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoving : MonoBehaviour
{
    private Vector3 target;
    public float repathTime;
    public Animator animator;

    private NavMeshAgent navMeshAgent;

    [SerializeField] private Transform leftTopCorner;
    [SerializeField] private Transform rightDownCorner;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GetComponent<Transform>().position;
        StartCoroutine(Repath());
    }

    IEnumerator Repath()
    {
        yield return new WaitForSeconds(repathTime);
        navMeshAgent.SetDestination(target);

        StartCoroutine(Repath());
    }

    private void Update()
    {
        animator?.SetFloat("Velocity", navMeshAgent.velocity.magnitude);
    }

    public bool IsCloseToTarget()
    {
        return Vector3.Distance(transform.position, target) < 1f;
    }

    public void NewTargetPoint()
    {
        float xRandom = Random.Range(leftTopCorner.position.x, rightDownCorner.position.x);
        float zRandom = Random.Range(rightDownCorner.position.z, leftTopCorner.position.z);
        target = new Vector3(xRandom, leftTopCorner.position.y, zRandom);
    }
}
