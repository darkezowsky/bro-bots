using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("MenuMusic");
    }

    public void Play()
    {
        audioManager.Play("Click");
        audioManager.Stop("MenuMusic");
        SceneManager.LoadScene(1);
    }
}
