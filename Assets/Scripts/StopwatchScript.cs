using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CameraSwitcher;
using static FinishGame;

public class StopwatchScript : MonoBehaviour
{
    [SerializeField] Minigame minigameScript;
    
    [SerializeField] Sprite[] stopwatchSprites;
    int curSprite = 0;

    GameObject player;

    [SerializeField] float time = 20f;
    float timeBetweenSpawns = 0f;
    [HideInInspector] public bool isTimeOut = false;

    float oneEight;
    float twoEights;
    float threeEights;
    float fourEights;
    float fiveEights;
    float sixEights;
    float sevenEights;

    private void Start()
    {
        transform.GetComponent<SpriteRenderer>().sprite = stopwatchSprites[0];

        player = GameObject.Find("Player");
        time = Vector3.Distance(transform.position, player.transform.position) * 0.1f;

        oneEight = time / 8;
        twoEights = oneEight * 2;
        threeEights = oneEight * 3;
        fourEights = oneEight * 4;
        fiveEights = oneEight * 5;
        sixEights = oneEight * 6;
        sevenEights = oneEight * 7;
    }
    void Update()
    {
        if (isTimeOut)
        {
            return;
        }

        if (isInMinigame)
        {
            if (minigameScript.isMinigameWon)
            {
                transform.GetComponent<SpriteRenderer>().sprite = stopwatchSprites[9];
                isTimeOut = true;
            }
            return;
        }

        timeBetweenSpawns += Time.deltaTime;
        if (minigameScript.isMinigameOver)
        {
            curSprite = 8;
            isTimeOut = true;
            minigamesOverCount++;
        }
        else if (timeBetweenSpawns >= time)
        {
            curSprite = 8;
            isTimeOut = true;
        }
        else if (timeBetweenSpawns >= sevenEights)
        {
            curSprite = 7;
        }
        else if (timeBetweenSpawns >= sixEights)
        {
            curSprite = 6;
        }
        else if (timeBetweenSpawns >= fiveEights)
        {
            curSprite = 5;
        }
        else if (timeBetweenSpawns >= fourEights)
        {
            curSprite = 4;
        }
        else if (timeBetweenSpawns >= threeEights)
        {
            curSprite = 3;
        }
        else if (timeBetweenSpawns >= twoEights)
        {
            curSprite = 2;
        }
        else if (timeBetweenSpawns >= oneEight)
        {
            curSprite = 1;
        }

        transform.GetComponent<SpriteRenderer>().sprite = stopwatchSprites[curSprite];
    }
}
