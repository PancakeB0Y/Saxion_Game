using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LegendScript : MonoBehaviour
{
    Canvas canvas;

    private void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
    }
    void Update()
    {
        if (Input.GetKeyDown("return"))
        {
            canvas.enabled = !canvas.enabled;
        }
    }
}
