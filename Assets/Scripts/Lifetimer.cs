using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetimer : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float lifeTime = 2;

    private bool start;

    private void Awake()
    {
        StartCoroutine(ScaleAndDestroyCorutine()); 
    }

    private void Update()
    {
        if (start)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * speed);

            if (transform.localScale.x < 0.1)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator ScaleAndDestroyCorutine()
    {
        yield return new WaitForSeconds(lifeTime);
        start = true;
    }
}
