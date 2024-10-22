using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private bool open;
    [SerializeField] private Transform target1;
    [SerializeField] private Transform target2;
    private float smoothing = 3f;

    void Awake()
    {
        open = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (open == false)
        {
            StartCoroutine(Open(target1));
        }
        else
        {
            StartCoroutine(Close(target2));
        }
        IEnumerator Open(Transform target1)
        {
            while (Vector3.Distance(transform.position, target1.position) > 0.05f)
            {
                door.transform.position =  Vector3.Lerp(door.transform.position, target1.position, smoothing * Time.deltaTime);
                yield return open = true;
            }
        }
        IEnumerator Close(Transform target2)
        {
            while (Vector3.Distance(transform.position, target2.position) > 0.05f)
            {
                door.transform.position = Vector3.Lerp(door.transform.position, target2.position, smoothing * Time.deltaTime);
                yield return open = false;
            }
        }
    }
}
