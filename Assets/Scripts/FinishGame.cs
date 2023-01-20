using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    public static int minigamesOverCount = 0;
    public static int minigamesCount = 0;
    public static int minigamesWon = 0;

    [SerializeField] bool isGameOver = false;

    void Update()
    {
        if(minigamesCount > 0 && minigamesOverCount >= minigamesCount)
        {
            isGameOver = true;
        }
    }
}
