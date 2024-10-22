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
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.z = Random.Range(-moveSpeed, moveSpeed);
        scrapManager = FindObjectOfType<ScrapManager>();
    }

    void Update()
    {
        transform.position += moveDirection * Time.deltaTime;

        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            scrapManager.AddScraps(scrapValue);
            Destroy(this.gameObject);
        }
    }
}
