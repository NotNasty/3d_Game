using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyViewZone : MonoBehaviour
{
    public event Action<GameObject> EnemySeesPlayer;
    public event Action EnemyStopsSeePlayer;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            EnemySeesPlayer?.Invoke(collider.gameObject);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            EnemyStopsSeePlayer?.Invoke();
        }
    }
}
