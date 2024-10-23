using System.Collections;
using UnityEngine;

public class Lifetimer : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Prędkość zmniejszania
    [SerializeField] private float lifeTime = 2f; // Czas życia obiektu

    private bool startScaling;

    private void Awake()
    {
        StartCoroutine(ScaleAndDestroyCoroutine());
    }

    private void Update()
    {
        if (startScaling)
        {
            // Lerp do skali zero
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * speed);

            // Sprawdzenie, czy skala jest wystarczająco mała, aby zniszczyć obiekt
            if (transform.localScale.magnitude < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator ScaleAndDestroyCoroutine()
    {
        yield return new WaitForSeconds(lifeTime); // Czekaj przez określony czas
        startScaling = true; // Rozpocznij zmniejszanie
    }
}
