using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyManager))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float timeToLocatePlayer = 2;
    [SerializeField] private float pushPower = 5;
    [SerializeField] private float pushSpeed = 10;
    [SerializeField] private float atackDistance = 2;
    [SerializeField] private float atackDelay = 1;
    [SerializeField] private int atackValue = 20;

    [HideInInspector] public Transform player;
    [HideInInspector] public ScrapManager scrapManager;

    private ScrapExplosion playerScrap;
    private NavMeshAgent meshAgent;
    private Vector3 currentPath;
    private bool hit;
    private Vector3 targetPos;
    private EnemyManager enemyManager;
    private bool atack;
    private bool atackIsActive;
    private AudioManager audioManager;

    private void Start()
    {
        // Znajdź AudioManager i EnemyManager, upewnij się, że nie są null
        audioManager = Object.FindAnyObjectByType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("Nie znaleziono AudioManager w scenie.");
        }

        enemyManager = GetComponent<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogError("Brak komponentu EnemyManager na obiekcie: " + gameObject.name);
        }

        // Znajdź komponent NavMeshAgent
        meshAgent = GetComponent<NavMeshAgent>();
        if (meshAgent == null)
        {
            Debug.LogError("Brak komponentu NavMeshAgent na obiekcie: " + gameObject.name);
        }

        // Sprawdź, czy player został przypisany
        if (player == null)
        {
            Debug.LogError("Player nie został przypisany do EnemyMovement.");
        }
        else
        {
            playerScrap = player.GetComponent<ScrapExplosion>();
            if (playerScrap == null)
            {
                Debug.LogWarning("Nie znaleziono ScrapExplosion na graczu: " + player.name);
            }
        }

        // Sprawdź ScrapManager
        if (scrapManager == null)
        {
            Debug.LogWarning("ScrapManager nie został przypisany do EnemyMovement.");
        }

        // Wybierz pierwszą ścieżkę
        SelectPath();

        // Rozpocznij coroutine do lokalizowania gracza
        StartCoroutine(FindPlayerCoroutine());
    }

    void Update()
    {
        // Sprawdź odległość do gracza
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > atackDistance && meshAgent.enabled)
            {
                meshAgent.SetDestination(currentPath);
                atack = false;
                atackIsActive = false;
            }
            else if (distance <= atackDistance && meshAgent.enabled)
            {
                atack = true;
                if (!atackIsActive)
                {
                    atackIsActive = true;
                    StartCoroutine(AtackPlayerCourutine());
                }
            }

            if (hit)
            {
                float step = pushSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

                if (Vector3.Distance(transform.position, targetPos) < 0.1)
                {
                    hit = false;
                    meshAgent.enabled = true;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Użyj CompareTag, aby porównać tagi
        if (collision.gameObject.CompareTag("Wall"))
        {
            hit = false;
            meshAgent.enabled = true;
        }
    }

    private void SelectPath()
    {
        if (player != null)
        {
            currentPath = player.position;
        }
        else
        {
            Debug.LogWarning("Nie można ustawić ścieżki, ponieważ player jest null.");
        }
    }

    private IEnumerator FindPlayerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToLocatePlayer);
            SelectPath();
        }
    }

    public void Push()
    {
        if (player != null)
        {
            meshAgent.enabled = false;
            transform.LookAt(player);
            targetPos = transform.position - transform.forward * pushPower;
            hit = true;

            if (audioManager != null)
            {
                audioManager.Play("EnemiesPush");
            }
        }
    }

    private IEnumerator AtackPlayerCourutine()
    {
        while (atack && playerScrap != null && scrapManager != null)
        {
            // Wyrzuć scrap i odejmij wartość ataku
            playerScrap.DropScrap();
            scrapManager.SubtractScraps(atackValue);

            if (audioManager != null)
            {
                audioManager.Play("EnemyAttack");
            }

            yield return new WaitForSeconds(atackDelay);
        }
    }
}
