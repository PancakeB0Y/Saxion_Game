using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static FinishGame;

public class VillagersHelped : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        gameObject.GetComponent<TextMeshProUGUI>().text = "<size=80> Game<size=30> </size>Over!  </size> \n \n Villagers Helped:\n" + minigamesWon + "/" + minigamesCount;
        minigamesWon = 0;
        minigamesCount = 0;
        minigamesOverCount = 0;
    }
}
