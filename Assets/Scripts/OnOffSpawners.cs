using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffSpawners : MonoBehaviour
{
    [SerializeField] private GameObject[] spowners;
    [SerializeField] private GameObject[] laserWall;
    [SerializeField] private bool spawnersAreOff;

    private void Awake()
    {
        Setup();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Setup();
        }
    }

    private void Setup()
    {
        if (spawnersAreOff)
        {
            spawnersAreOff = true;
           
            foreach (var item in laserWall)
            {
                item.SetActive(true);
            }

            foreach (var item in spowners)
            {
                item.SetActive(true);
            }
        }
        else
        {
            spawnersAreOff = false;

            foreach (var item in laserWall)
            {
                item.SetActive(false);
            }

            foreach (var item in spowners)
            {
                item.SetActive(false);
            }
        }
    }
}
