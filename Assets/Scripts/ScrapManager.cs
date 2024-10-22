using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScrapManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scrapCounter;
    [Space(10)]
    [SerializeField] private Slider lvlUpSlider;
    [SerializeField] private TextMeshProUGUI sliderValuesText;
    [SerializeField] [Tooltip("To tak bydzie wyglądać: textInSlider 5/10")] private string textInSlider;
    [Space(10)]
    [SerializeField] private int[] valuesToLvlUp;
    [SerializeField] private LosePanel losePanel;
    [SerializeField] private ScrapExplosion player;

    public int scrapNumber = 0;
    private int points = 0;
    private int actualAtackState = 0;
    private AudioManager audioManager;

    private void Awake()
    {
        SetScrapValueText();
        SetSlider(actualAtackState, 0);
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        SetScrapValueText();
        lvlUpSlider.value = scrapNumber;
    }

    private void SetScrapValueText()
    {
        scrapCounter.SetText(points.ToString());
        sliderValuesText.SetText($"{textInSlider} {lvlUpSlider.value}/{valuesToLvlUp[actualAtackState]}");
    }

    private void SetSlider(int indexOfLvl, int scraps)
    {
        scrapNumber = scraps;
        lvlUpSlider.maxValue = valuesToLvlUp[indexOfLvl];
    }

    private IEnumerator LoseCutdownCorutine()
    {
        while (points > 0)
        {
            yield return new WaitForSeconds(0.4f);
            points--;
            player.DropScrap();
        }
        yield return new WaitForSeconds(0.2f);
        losePanel.OpenPanel(false);
    }

    public void EndGame(bool win)
    {
        if (win)
        {
            losePanel.OpenPanel(true);
        }
        else
        {
            FindObjectOfType<PlayerController>().enabled = false;
            StartCoroutine(LoseCutdownCorutine());
        }
    }

    public void AddScraps(int value)
    {
        points += value;

        if (scrapNumber + value > valuesToLvlUp[actualAtackState])
        {
            if (actualAtackState + 1 > valuesToLvlUp.Length - 1)
            {
                SetSlider(actualAtackState, valuesToLvlUp[actualAtackState]);
            }
            else
            {
                actualAtackState++;
                SetSlider(actualAtackState, scrapNumber + value - valuesToLvlUp[actualAtackState - 1]);
                audioManager.Play("LevelUp");
            }
        }
        else
        {
            scrapNumber += value;
        }
    }

    public void SubtractScraps(int value)
    {
        if (points - value < 0)
        {
            points = 0;
            EndGame(false);
        }
        else
        {
            points -= value;
        }

        if (scrapNumber - value < 0)
        {
            if (actualAtackState - 1 < 0)
            {
                SetSlider(actualAtackState, 0);
            }
            else
            {
                actualAtackState--;
                SetSlider(actualAtackState, valuesToLvlUp[actualAtackState]);
                audioManager.Play("LevelDown");
            }
        }
        else
        {
            scrapNumber -= value;
        }
    }
}
