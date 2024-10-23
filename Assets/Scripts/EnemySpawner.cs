using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float secondsToSpawnEnemy = 2f;
    [SerializeField] private int maxSpawnedEnemies = 3;
    [SerializeField] private float spawnRadius = 2f; // Promień wokół spawnera
    [SerializeField] private int enemiesPerSpawn = 1; // Liczba przeciwników do zespawnowania podczas jednej iteracji

    [Header("Objects")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private ScrapManager scrapManager;

    [HideInInspector] public int numberOfSpawnedEnemies;

    private void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("enemyPrefab nie jest przypisany w EnemySpawner.");
            return;
        }
        if (player == null)
        {
            Debug.LogError("Player nie jest przypisany w EnemySpawner.");
            return;
        }
        if (scrapManager == null)
        {
            Debug.LogError("ScrapManager nie jest przypisany w EnemySpawner.");
            return;
        }
        StartCoroutine(SpawnLogicCoroutine());
    }

    private void SpawnEnemy()
    {
        // Sprawdzenie, czy osiągnięto maksymalną liczbę przeciwników
        if (numberOfSpawnedEnemies >= maxSpawnedEnemies)
        {
            return;
        }

        // Losowa pozycja spawnu w promieniu spawnRadius
        Vector3 randomSpawnPosition = transform.position + (Random.insideUnitSphere * spawnRadius);
        randomSpawnPosition.y = transform.position.y; // Upewnij się, że przeciwnik spawnuje się na tej samej wysokości

        var newEnemy = Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity);
        var enemyMovement = newEnemy.GetComponent<EnemyMovement>();
        var enemyManager = newEnemy.GetComponent<EnemyManager>();

        if (enemyMovement != null)
        {
            enemyMovement.scrapManager = scrapManager;
            enemyMovement.player = player;
        }
        else
        {
            Debug.LogWarning("Brak komponentu EnemyMovement na prefabrykacie wroga.");
        }

        if (enemyManager != null)
        {
            enemyManager.SetSpawner(this); // Przypisuje EnemySpawner do spawnowanego przeciwnika
            
            // Dezaktywuj wszystkie GFX na początku
            foreach (var gfx in enemyManager.GFX)
            {
                gfx.SetActive(false);
            }
            // Wybierz losowy GFX i aktywuj go
            if (enemyManager.GFX != null && enemyManager.GFX.Length > 0)
            {
                int randomIndex = Random.Range(0, enemyManager.GFX.Length);
                enemyManager.GFX[randomIndex].SetActive(true);
            }
            else
            {
                Debug.LogWarning("GFX nie są przypisane w EnemyManager lub tablica jest pusta.");
            }
        }
        else
        {
            Debug.LogWarning("Brak komponentu EnemyManager na prefabrykacie wroga.");
        }

        // Zwiększ licznik zespawnowanych przeciwników
        numberOfSpawnedEnemies++;
    }

    private IEnumerator SpawnLogicCoroutine()
    {
        while (true)
        {
            // Czekaj przez określoną liczbę sekund przed kolejnym spawnem
            yield return new WaitForSeconds(secondsToSpawnEnemy);

            // Sprawdź, ilu przeciwników można jeszcze zespawnować
            int enemiesToSpawn = Mathf.Min(enemiesPerSpawn, maxSpawnedEnemies - numberOfSpawnedEnemies);

            // Spawnowanie kilku przeciwników na raz (w zależności od enemiesPerSpawn)
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                if (numberOfSpawnedEnemies < maxSpawnedEnemies)
                {
                    SpawnEnemy();
                }
            }
        }
    }

    // Metoda do zmniejszenia liczby wrogów, gdy zostaną zniszczeni
    public void EnemyDestroyed()
    {
        numberOfSpawnedEnemies = Mathf.Max(0, numberOfSpawnedEnemies - 1); // Zapewnia, że liczba wrogów nie będzie mniejsza niż 0
    }
}
