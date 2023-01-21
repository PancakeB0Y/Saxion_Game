using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    int whichPreset;

    private void Start()
    {
        whichPreset = Random.Range(1, 6);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(whichPreset);
    }
}
