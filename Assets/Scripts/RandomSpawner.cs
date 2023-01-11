using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    Camera curCamera;
    [SerializeField] GameObject spherePrefab;

    [SerializeField] float distanceFromCam = 2f;
    [SerializeField] float x = 1;
    [SerializeField] float y = 1;

    private void Start()
    {
        curCamera = gameObject.GetComponentInChildren<Camera>();
    }
    void Update()
    {
        if (!curCamera.enabled)
        {
            return;
        }

        if (Input.GetKeyDown("space"))
        {
            Vector3 pos = curCamera.ViewportToWorldPoint(new Vector3(x, y, curCamera.nearClipPlane + distanceFromCam));
            Instantiate(spherePrefab, pos, Quaternion.identity);
        }

    }
}
