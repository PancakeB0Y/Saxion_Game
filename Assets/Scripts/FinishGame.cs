using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    public static int minigamesCount = 0;
    public static int minigamesOverCount = 0;
    public static int minigamesWon = 0;

    void Update()
    {
        Debug.Log("Minigames Count: " + minigamesCount + " MinigamesOverCount: " + minigamesOverCount);
        if(minigamesCount > 0 && minigamesOverCount >= minigamesCount)
        {
            SceneManager.LoadScene(4);
        }
    }
}
