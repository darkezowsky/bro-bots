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
        audioManager = FindObjectOfType<AudioManager>();
        enemyManager = GetComponent<EnemyManager>();

        SelectPath();
        meshAgent = GetComponent<NavMeshAgent>();
        playerScrap = player.GetComponent<ScrapExplosion>();

        StartCoroutine(FindPlayerCoroutine());
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > 2.1f && meshAgent.enabled)
        {
            meshAgent.SetDestination(currentPath);
            atack = false;
            atackIsActive = false;
        }
        else if (distance <= 2.1f && meshAgent.enabled)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            hit = false;
            meshAgent.enabled = true;
        }
    }

    private void SelectPath()
    {
        currentPath = player.position;
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
        meshAgent.enabled = false;
        transform.LookAt(player);
        targetPos = transform.position - transform.forward * pushPower;
        hit = true;
        audioManager.Play("EnemiesPush");
    }

    private IEnumerator AtackPlayerCourutine()
    {
        while (atack)
        {
            playerScrap.DropScrap();
            scrapManager.SubtractScraps(atackValue);
            audioManager.Play("EnemyAttack");
            yield return new WaitForSeconds(atackDelay);
        }
        yield return null;
    }
}
