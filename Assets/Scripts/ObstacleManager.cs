using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType { Lasers, Saw, Oil, Lava }
public enum RotationAxis { X, Y, Z } // Enum do wyboru osi obrotu

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private ObstacleType obstacleType;
    [SerializeField] private ScrapManager scrapManager;

    [Header("Saw")]
    [SerializeField] private Transform saw; // Obiekt piły do obracania
    [SerializeField] private RotationAxis rotationAxis; // Wybór osi obrotu
    float speedMultiplier = 20f; // Mnożnik prędkości
    [SerializeField, Range(-10, 10)] private float rotateSawSpeed = 10;
    [SerializeField] private int sawPower = 10;

    [Header("Laser")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform startLaserPos;
    [SerializeField] private float initialTime;
    [SerializeField] private int timeToLoad;
    [SerializeField] private int laserActiveTime = 2;
    [SerializeField] private int laserPower;
    [SerializeField] private new GameObject light;

    [Header("Push options")]
    [SerializeField] private float cogsPower = 5;
    [SerializeField] private float cogsSpeed = 5;

    private Vector3 targetPos;
    private bool hit;
    private Transform player;
    private int laserTimer;
    private bool laserHit;
    private bool laserOn;

    private void Awake()
    {
        if (obstacleType == ObstacleType.Lasers)
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
            laserHit = true;
            laserTimer = 0;
            StartCoroutine(InitialTime());
        }
    }

    private void Update()
    {
        if (obstacleType == ObstacleType.Saw && saw != null)
        {
            switch (rotationAxis)
            {
                case RotationAxis.X:
                    saw.Rotate(Vector3.up * rotateSawSpeed * Time.deltaTime * speedMultiplier);
                    break;
                case RotationAxis.Y:
                    saw.Rotate(Vector3.right * rotateSawSpeed * Time.deltaTime * speedMultiplier);
                    break;
                case RotationAxis.Z:
                    saw.Rotate(Vector3.forward * rotateSawSpeed * Time.deltaTime * speedMultiplier);
                    break;
            }

            if (hit)
            {
                float step = cogsSpeed * Time.deltaTime;
                player.position = Vector3.MoveTowards(player.position, targetPos, step);

                if (Vector3.Distance(player.position, targetPos) < 0.1f)
                {
                    player.GetComponent<PlayerController>().enabled = true;
                    hit = false;
                }
            }
        }
        else if (obstacleType == ObstacleType.Lasers && laserOn)
        {
            lineRenderer.SetPosition(0, startLaserPos.position);
            RaycastHit rHit;

            if (Physics.Raycast(startLaserPos.position, startLaserPos.forward, out rHit))
            {
                if (rHit.collider)
                {
                    lineRenderer.SetPosition(1, rHit.point);
                }

                if (laserHit)
                {
                    if (rHit.collider.tag == "Player")
                    {
                        StartCoroutine(LaserHitCourutine());
                        rHit.collider.gameObject.GetComponent<ScrapExplosion>().DropScrap();
                        scrapManager.SubtractScraps(laserPower);
                    }
                    else if (rHit.collider.tag == "Enemy")
                    {
                        rHit.collider.GetComponent<EnemyManager>().DestroyEnemy("Obstacle");
                    }
                }
            }
            else
            {
                lineRenderer.SetPosition(1, startLaserPos.forward * 50000);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && obstacleType == ObstacleType.Saw)
        {
            if (player == null)
            {
                player = collision.gameObject.GetComponent<Transform>();
            }
            player.GetComponent<ScrapExplosion>().DropScrap();
            scrapManager.SubtractScraps(sawPower);
            Saw(player.GetComponent<Rigidbody>(), player);
        }
        else if (collision.gameObject.tag == "Scrap" && obstacleType == ObstacleType.Oil)
        {
            Destroy(collision.gameObject);
        }
        else if (obstacleType == ObstacleType.Lava)
        {
            if (collision.gameObject.tag != "Ground" && collision.gameObject.tag != "Wall")
            {
                if (collision.gameObject.tag == "Enemy")
                {
                    collision.gameObject.GetComponent<EnemyManager>().DestroyEnemy("Obstacle");
                }
                else if (collision.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<ScrapExplosion>().DropScrap();
                    scrapManager.EndGame(false);
                }
                else
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    private void Saw(Rigidbody targetRB, Transform targetTransorm)
    {
        player.GetComponent<PlayerController>().enabled = false;

        Vector3 pos = saw.forward * cogsPower;
        pos.y = 0;

        targetPos = targetTransorm.position + pos;
        hit = true;
    }

    private IEnumerator LaserCourutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            laserTimer++;

            if (laserTimer == timeToLoad - 1f)
            {
                light.SetActive(true);
            }

            if (laserTimer == timeToLoad)
            {
                laserTimer = 0;
                StopAllCoroutines();
                light.SetActive(false);
                StartCoroutine(OnOfLaserCourutine());
            }
        }
    }

    private IEnumerator OnOfLaserCourutine()
    {
        laserOn = true;
        yield return new WaitForSeconds(laserActiveTime);
        laserOn = false;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
        StartCoroutine(LaserCourutine());
    }

    private IEnumerator LaserHitCourutine()
    {
        laserHit = false;
        yield return new WaitForSeconds(0.5f);
        laserHit = true;
    }

    private IEnumerator InitialTime()
    {
        yield return new WaitForSeconds(initialTime);
        StartCoroutine(LaserCourutine());
    }
}
