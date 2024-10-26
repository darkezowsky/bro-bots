using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FirstEnemyLogic : MonoBehaviour
{
    [SerializeField] private float timeToLocatePlayer = 2f; // Czas opóźnienia w sekundach
    [SerializeField] private Transform obstacle;

    private NavMeshAgent meshAgent;
    private float locatePlayerTimer = 0f; // Zmienna do liczenia czasu

    private void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Zwiększaj licznik czasu
        locatePlayerTimer += Time.deltaTime;

        // Sprawdź, czy minęło wystarczająco dużo czasu, aby zacząć śledzenie
        if (locatePlayerTimer >= timeToLocatePlayer)
        {
            meshAgent.SetDestination(obstacle.position);
        }

        // Sprawdź odległość do przeszkody
        if (Vector3.Distance(obstacle.position, transform.position) <= 2.5f)
        {
            Destroy(this.gameObject); // Zniszcz obiekt, jeśli jest blisko przeszkody
        }
    }
}