using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public static bool isInMinigame = false;
    Camera mainCamera;
    Camera[] allCameras;
    [SerializeField] List<Camera> cameras;

    [SerializeField] LayerMask npcLayer;
    [SerializeField] float range = 5f;

    private void Start()
    {
        mainCamera = Camera.main;
        allCameras = FindObjectsOfType<Camera>(true);
        foreach(Camera curCam in allCameras)
        {
            if(curCam.tag == "NPC Camera")
            {
                cameras.Add(curCam);
            }
        }
    }

    void Update()
    {
        isInMinigame = mainCamera.enabled ? false : true;
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
        
        for(int i = 0; i < cameras.Count; i++)
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
        Cursor.visible = !Cursor.visible;
    }
}
