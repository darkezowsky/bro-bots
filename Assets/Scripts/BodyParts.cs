using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyParts : MonoBehaviour
{
    [SerializeField] private Image[] parts;
    [SerializeField] private Color desactiveColor;
    [SerializeField] private ScrapManager scrapManager;

    private bool[] takedPart = new bool[3];

    public void ActualizeParts(int indexOfTakenPart)
    {
        takedPart[indexOfTakenPart] = true;

        for (int i = 0; i < parts.Length; i++)
        {
            if (takedPart[i] == true)
            {
                parts[i].color = Color.white;
            }
            else
            {
                parts[i].color = desactiveColor;
            }
        }

        if (CheckWin())
        {
            scrapManager.EndGame(true);
        }
    }

    private bool CheckWin()
    {
        int trueNumber = 0;

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i])
            {
                trueNumber++;
            }
        }

        if (trueNumber == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
