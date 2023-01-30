using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PauseMenu;
using static CameraSwitcher;

public class ResumeButton : MonoBehaviour
{
    [SerializeField] GameObject parent;

    public void ResumeGame()
    {
        if (!isInMinigame)
        {
            Cursor.visible = false;
        }
        parent.GetComponent<Canvas>().enabled = false;
        isPaused = false;
        Time.timeScale = 1;
    }
}
