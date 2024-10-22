using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private int secondsToSpownEnemy = 2;
    [SerializeField] private int maxSpawnedEnemies = 3;
    [Header("Objects")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private ScrapManager scrapManager;

    private int timer;

    [HideInInspector] public int numberOfSpownedEnemies;

    private void Start()
    {
        SpawnEnemy();
        timer = 0;

        StartCoroutine(SpawnLogicCourutine());
    }

    private void SpawnEnemy()
    {
        var newEnemy = Instantiate(enemyPrefab);

        Vector3 newPos = newEnemy.transform.position;
        newPos.x = transform.position.x;
        newPos.z = transform.position.z;
        newEnemy.transform.position = newPos;

        newEnemy.GetComponent<EnemyMovement>().scrapManager = scrapManager;
        newEnemy.GetComponent<EnemyMovement>().player = player;
        newEnemy.GetComponent<EnemyManager>().enemySpawner = this;
        newEnemy.GetComponent<EnemyManager>().GFX[Random.Range(0, 3)].SetActive(true);

        numberOfSpownedEnemies++;
    }

    private IEnumerator SpawnLogicCourutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timer++;

            if (timer >= secondsToSpownEnemy && numberOfSpownedEnemies < maxSpawnedEnemies)
            {
                SpawnEnemy();
                timer = 0;
            }
        }
    }
}

