using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapExplosion : MonoBehaviour
{
    public GameObject[] scrapMetal;
    public int maxPieces = 100;
    public int minPieces = 75;
    public float yOffset = 8f;
    public bool shouldDropScrap;
    public GameObject scrapToDrop;
    public float itemDropPercent = 50f; // Dodane pole procentowe dla dropu przedmiotu
    public GameObject[] itemsToDrop; // Dodana tablica z przedmiotami do dropu

    private bool scrapDropped = false; // Flaga sprawdzająca, czy scrap został zdropowany

    public void DropScrap()
    {
        if (scrapDropped) return; // Jeżeli scrap został już zdropowany, przerwij metodę

        int piecesToDrop = Random.Range(minPieces, maxPieces);
        for (int i = 0; i < piecesToDrop; i++)
        {
            int randomScrap = Random.Range(0, scrapMetal.Length);
            Instantiate(scrapMetal[randomScrap], new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), transform.rotation);
        }

        if (shouldDropScrap)
        {
            float dropChance = Random.Range(0f, 100f);
            if (dropChance < itemDropPercent)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);
                Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }

        scrapDropped = true; // Ustaw flagę na true po zdropowaniu scrapu
    }
}
