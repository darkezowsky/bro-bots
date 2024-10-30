using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float deceleration = 5f;
    [SerializeField] private int scrapValue = 1;

    private Vector3 moveDirection;
    private ScrapManager scrapManager;

    void Start()
    {
        SetRandomDirection();
        scrapManager = Object.FindFirstObjectByType<ScrapManager>();
    }

    void Update()
    {
        transform.position += moveDirection * Time.deltaTime;

        // Zmiana kierunku w losowych odstępach czasu
        if (moveDirection.magnitude > 0.1f)
        {
            moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);
        }
        else
        {
            SetRandomDirection();
        }
    }

    private void SetRandomDirection()
    {
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.z = Random.Range(-moveSpeed, moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            scrapManager.AddScraps(scrapValue);
            Destroy(this.gameObject);
        }
    }
}
