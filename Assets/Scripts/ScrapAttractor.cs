using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapAttractor : MonoBehaviour
{
    public float attractorSpeed = 10f;
   private SphereCollider sphereCollider;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, attractorSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Scrap")
        {
            sphereCollider.enabled = true;
        }
    }
}
