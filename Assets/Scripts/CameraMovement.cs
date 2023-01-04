using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float distanceFromTarget = 3f;

    [HideInInspector] public float camrotY = 0f;
    float camrotX = 0f;

    [SerializeField] float sensitivity = 2f;

    [SerializeField] int camVMax = 40;
    [SerializeField] int camVMin = -5;

    private Vector3 currentRotation = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private float smoothTime = 0.01f;

    void Update()
    {
        camrotY += Input.GetAxis("Mouse X") * sensitivity;
        camrotX -= Input.GetAxis("Mouse Y") * sensitivity;
        camrotX = Mathf.Clamp(camrotX, camVMin, camVMax);

        Vector3 nextRotation = new Vector3(camrotX, camrotY, 0f);
        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref velocity, smoothTime);

        transform.localEulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}
