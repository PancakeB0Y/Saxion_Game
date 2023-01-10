using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera[] cameras;

    [SerializeField] float range = 5f;
    [SerializeField] LayerMask npcMask;

    int cameraCount = 0;

    private void Start()
    {
        cameraCount = cameras.Length;
        mainCamera.enabled = true;
    }

    void Update()
    {
        if(Physics.CheckSphere(transform.position, range, npcMask))
        {
            if (Input.GetKeyDown("e"))
            {
                Camera closestCam = GetClosest();

                mainCamera.enabled = !mainCamera.enabled;
                closestCam.enabled = !closestCam.enabled;
                for(int i = 0; i < cameraCount; i++)
                {
                    if(cameras[cameraCount - 1].enabled == true && cameras[cameraCount - 1] != closestCam)
                    {
                        cameras[cameraCount - 1].enabled = false;
                    }
                }
            }
        }
    }

    Camera GetClosest()
    {
        Camera closestCamera = new Camera();
        float minDistance = Mathf.Infinity;
        
        for(int i = 0; i < cameraCount; i++)
        {
            Camera curCam = cameras[cameraCount - 1];
            float curDistance = Vector3.Distance(transform.position, curCam.transform.position);
            if (curDistance < minDistance)
            {
                minDistance = curDistance;
                closestCamera = curCam;
            }
        }

        return closestCamera;
    }
}
