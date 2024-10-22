using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private bool tutorial;
    public GameObject[] GFX;

    [HideInInspector] public EnemySpawner enemySpawner;
    [HideInInspector] public ScrapExplosion scrapExplosion;
    [HideInInspector] public TutorialCore tutorialCore;

    private AudioManager audioManager;

    private void Awake()
    {
        scrapExplosion = GetComponent<ScrapExplosion>();
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnDestroy()
    {
        if (!tutorial)
        {
            enemySpawner.numberOfSpownedEnemies--;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            DestroyEnemy("Obstacle");
        }
    }

    public void DestroyEnemy(string destroyIndicator)
    {
        scrapExplosion.DropScrap();
        Destroy(gameObject);
        audioManager.Play("EnemiesDeath");

        if (tutorial)
        {
            if (destroyIndicator == "Obstacle" && tutorialCore.tutorialState == TutorialState.Kamikaze)
            {
                tutorialCore.SetTutorialSate(TutorialState.Play);
            }
            else if (destroyIndicator != "Obstacle" && tutorialCore.tutorialState == TutorialState.Kamikaze)
            {
                tutorialCore.SetTutorialSate(TutorialState.Fail);
            }
        }
    }
}
