using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LosePanel : MonoBehaviour
{
    [SerializeField] private GameObject reasumeButton;
    [SerializeField] private TextMeshProUGUI text; 

    private Animator animator;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("BGMgame");
        reasumeButton.SetActive(false);
        animator = GetComponent<Animator>();
    }

    public void OpenPanel(bool win)
    {
        if (!win)
        {
            reasumeButton.SetActive(true);
            text.SetText("Lose");
            audioManager.Play("PlayersDeath");
        }
        else
        {  
            text.SetText("Win");
        }
        animator.SetTrigger("Start");
    }

    public void LoadScene(int sceneIndex)
    {
        audioManager.Play("Click");

        if (sceneIndex == 0)
        {
            audioManager.Stop("BGMgame");
        }

        SceneManager.LoadScene(sceneIndex);
    }
}
