using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBillboard : MonoBehaviour
{
    [SerializeField] Camera closestCam;
    Camera[] allCameras;
    [SerializeField] List<Camera> npcCameras;
    Vector3 cameraDir;
    Transform parent;
    void Start()
    {
        allCameras = FindObjectsOfType<Camera>(true);
        foreach (Camera curCam in allCameras)
        {
            if (curCam.tag == "NPC Camera")
            {
                npcCameras.Add(curCam);
            }
        }

        float minDistance = Mathf.Infinity;

        for (int i = 0; i < npcCameras.Count; i++)
        {
            Camera curCam = npcCameras[i];
            float curDistance = Vector3.Distance(transform.position, curCam.transform.position);
            if (curDistance < minDistance)
            {
                minDistance = curDistance;
                closestCam = curCam;
            }
        }

        parent = closestCam.transform.parent.parent.parent;
        Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, parent.eulerAngles.y + transform.eulerAngles.y + 90, 0);
        transform.rotation = Quaternion.Euler(eulerRotation);
    }
}
