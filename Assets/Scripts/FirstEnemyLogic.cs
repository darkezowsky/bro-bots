using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FirstEnemyLogic : MonoBehaviour
{
    [SerializeField] private float timeToLocatePlayer = 2;
    [SerializeField] private Transform obstacle;

    private NavMeshAgent meshAgent;

    private void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        meshAgent.SetDestination(obstacle.position);

        if (Vector3.Distance(obstacle.position, transform.position) <= 2.5f)
        {
            Destroy(this.gameObject);
        }
    }
}
