using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapExplosion : MonoBehaviour
{
    public GameObject[] scrapMetal;
    public int maxPieces = 100;
    public int minPieces = 75;
    public float yOffset = 8f;

    public bool shoudDropScrap;
    public GameObject scrapToDrop;
    
    public void DropScrap()
    {
        int piecesToDrop = Random.Range(minPieces, maxPieces);

        for (int i = 0; i < piecesToDrop; i++)
        {
            int randomScrap = Random.Range(0, scrapMetal.Length);
            Instantiate(scrapMetal[randomScrap], new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), transform.rotation);
        }

        /*
        if (shoudDropScrap)
        {
            float dropChance = Random.Range(0f, 100f);

            if (dropChance < itemDropPercent)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);

                Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }*/
    }
}
