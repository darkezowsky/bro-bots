using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TutorialState { Kamikaze, Play, Fail }

public class TutorialCore : MonoBehaviour
{
    public TutorialState tutorialState;

    [SerializeField] private TextMeshProUGUI popUp;
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private string[] texts;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private ScrapManager scrapManager;
    [SerializeField] private Transform player;

    private TutorialState lastState;

    void Awake()
    {
        foreach (var item in spawners)
        {
            item.SetActive(false);
        }

        popupPanel.SetActive(false);
        SetTutorialSate(TutorialState.Kamikaze);
    }

    public void SetTutorialSate(TutorialState state)
    {
        lastState = tutorialState;
        tutorialState = state;

        switch (state)
        {
            case TutorialState.Kamikaze:
                SpawnEnemy();
                ShowPopUp(0);
                break;
            case TutorialState.Play:
                foreach (var item in spawners)
                {
                    item.SetActive(true);
                }
                ShowPopUp(2);
                break;
            case TutorialState.Fail:
                SpawnEnemy();
                ShowPopUp(3);
                tutorialState = lastState;
                break;
            default:
                break;
        }
    }

    private void SpawnEnemy()
    {
        var newEnemy = Instantiate(enemy);

        Vector3 newPos = newEnemy.transform.position;
        newPos.x = transform.position.x;
        newPos.z = transform.position.z;
        newEnemy.transform.position = newPos;

        newEnemy.GetComponent<EnemyMovement>().scrapManager = scrapManager;
        newEnemy.GetComponent<EnemyMovement>().player = player;
        newEnemy.GetComponent<EnemyManager>().tutorialCore = this;
        newEnemy.GetComponent<EnemyManager>().GFX[Random.Range(0, 3)].SetActive(true);
    }

    private void ShowPopUp(int indexText)
    {
        Time.timeScale = 0;
        popupPanel.SetActive(true);
        popUp.SetText(texts[indexText]);
    }

    public void OK()
    {
        Debug.Log("okkk");
        Time.timeScale = 1;
        popupPanel.SetActive(false);
    }
}
