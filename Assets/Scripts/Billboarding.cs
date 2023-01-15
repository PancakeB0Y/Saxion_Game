using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    Camera mainCamera;
    Vector3 cameraDir;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        cameraDir = mainCamera.transform.forward;
        cameraDir.y = 0;
        
        transform.rotation = Quaternion.LookRotation(cameraDir);
    }
}
