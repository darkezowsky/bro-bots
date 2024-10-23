using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapAttractor : MonoBehaviour
{
    public float attractorSpeed = 10f;
    private SphereCollider sphereCollider;
    private bool isAttracting = false;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = true;
        Debug.Log("SphereCollider enabled");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected");
            isAttracting = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isAttracting && other.CompareTag("Player") && Vector3.Distance(transform.position, other.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, attractorSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Scrap")
        {
            // Możesz tu dodać logikę, która zadecyduje co zrobić w przypadku kolizji
        }
    }
}
