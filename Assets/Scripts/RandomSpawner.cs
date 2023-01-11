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

    [SerializeField] float speed = 1f;

    Vector3 pos;
    Vector3 newPos;
    private void Start()
    {
        curCamera = gameObject.GetComponentInChildren<Camera>();
        pos = curCamera.ViewportToWorldPoint(new Vector3(x, y, curCamera.nearClipPlane + distanceFromCam));
        newPos = curCamera.ViewportToWorldPoint(new Vector3(0, y, curCamera.nearClipPlane + distanceFromCam));
    }
    void Update()
    {
        if (!curCamera.enabled)
        {
            return;
        }

        if (Input.GetKeyDown("space"))
        {
            SpawnSphere();
        }

    }

    void SpawnSphere()
    {
        GameObject sphere = Instantiate(spherePrefab, pos, Quaternion.identity);
        sphere.AddComponent<Move>();
        sphere.GetComponent<Move>().speed = speed;
        sphere.GetComponent<Move>().newPos = newPos;
    }

}

public class Move : MonoBehaviour
{
    public float speed;
    public Vector3 newPos;
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
    }
}
