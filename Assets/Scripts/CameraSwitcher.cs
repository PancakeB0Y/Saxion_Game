using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticValues;
public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera[] cameras;
    int cameraCount;

    [SerializeField] float range = 5f;
    [SerializeField] LayerMask npcLayer;

    private void Start()
    {
        cameraCount = cameras.Length;

    }

    void Update()
    {
        if(Physics.CheckSphere(transform.position, range, npcLayer))
        {
            if (Input.GetKeyDown("e"))
            {
                SwitchToClosestCam(GetClosestCam());
            }
        }
    }

    Camera GetClosestCam()
    {
        Camera closestCam = new Camera();
        float minDistance = Mathf.Infinity;
        
        for(int i = 0; i < cameraCount; i++)
        {
            Camera curCam = cameras[i];
            float curDistance = Vector3.Distance(transform.position, curCam.transform.position);
            if (curDistance < minDistance)
            {
                minDistance = curDistance;
                closestCam = curCam;
            }
        }

        return closestCam;
    }

    void SwitchToClosestCam(Camera closestCam)
    {
        mainCamera.enabled = !mainCamera.enabled;
        closestCam.enabled = !closestCam.enabled;
        isInGame = !isInGame;
        /*
        for (int i = 0; i < cameraCount; i++)
        {
            if (cameras[cameraCount - 1].enabled == true && cameras[cameraCount - 1] != closestCam)
            {
                cameras[cameraCount - 1].enabled = false;
            }
        }
        */
    }
}
