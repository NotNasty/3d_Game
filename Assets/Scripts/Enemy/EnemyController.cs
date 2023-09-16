using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyMoving enemyMoving;
    private EnemyShooting enemyShooting;
    [SerializeField] private EnemyViewZone enemyViewZone;

    public EnemyState State;

    private void Start()
    {
        enemyMoving = GetComponent<EnemyMoving>();
        enemyShooting = GetComponent<EnemyShooting>();
        State = TryStartPatrolling();
    }

    public void Update()
    {
        if (State == EnemyState.Patrolling && enemyMoving.IsCloseToTarget())
        {
            enemyMoving.NewTargetPoint();
        }
    }

    public void OnEnable()
    {
        enemyViewZone.EnemySeesPlayer += PlayerDetected;
        enemyViewZone.EnemyStopsSeePlayer += StopShooting;

    }

    public void OnDisable()
    {
        enemyViewZone.EnemySeesPlayer -= PlayerDetected;
        enemyViewZone.EnemyStopsSeePlayer -= StopShooting;
    }

    private void PlayerDetected(GameObject player)
    {
        if (State != EnemyState.Shooting)
        {
            enemyShooting.StartShooting(player.transform);
            State = EnemyState.Shooting;
        }
    }

    private void StopShooting()
    {
        if (State == EnemyState.Shooting)
        {
            enemyShooting.StopShooting();
            State = TryStartPatrolling();
        }
    }

    private EnemyState TryStartPatrolling()
    {
        if (State != EnemyState.Death)
        {
            enemyMoving.NewTargetPoint();
            return EnemyState.Patrolling;
        }

        return EnemyState.Idle;
    }
}

public enum EnemyState
{
    Idle,
    Patrolling,
    Reloading,
    Shooting,
    Death
}
