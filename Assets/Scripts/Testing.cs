using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;

    private void Start()
    {
        audioManager.Play("Test");
    }
}
