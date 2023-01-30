using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CameraSwitcher;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (!isPaused)
            {
                Cursor.visible = true;
                gameObject.GetComponent<Canvas>().enabled = true;
                isPaused = true;
                Time.timeScale = 0;
            }
            else
            {
                if (!isInMinigame)
                {
                    Cursor.visible = false;
                }
                gameObject.GetComponent<Canvas>().enabled = false;
                isPaused = false;
                Time.timeScale = 1;
            }
        }
    }
}
