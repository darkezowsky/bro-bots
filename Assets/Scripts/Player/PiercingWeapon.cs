using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingWeapon : MonoBehaviour
{
    public float zOffset = 2.5f;

    public BoxCollider boxCollider;
    public Transform rotatingWeaponTransform;
    
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        rotatingWeaponTransform = GetComponent<Transform>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("lmaoooooo");
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyManager>().DestroyEnemy("Player");
            Debug.Log("ayyyyy");
        }
    }

    public void HitEnemy()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + zOffset);
    }
}

