using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffSpawners : MonoBehaviour
{
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private GameObject[] laserWalls;
    [SerializeField] private bool spawnersAreActive = true;

    private void Awake()
    {
        Setup();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleSpawners();
        }
    }

    private void Setup()
    {
        // Ustawienie początkowego stanu dla laserowych ścian i spawnerów
        SetActiveState(laserWalls, !spawnersAreActive);
        SetActiveState(spawners, spawnersAreActive);
    }

    private void ToggleSpawners()
    {
        spawnersAreActive = !spawnersAreActive;  // Przełącz stan

        // Ustawienie stanu dla laserowych ścian i spawnerów
        SetActiveState(laserWalls, !spawnersAreActive);
        SetActiveState(spawners, spawnersAreActive);
    }

    private void SetActiveState(GameObject[] objects, bool state)
    {
        foreach (var item in objects)
        {
            item.SetActive(state);
        }
    }
}
