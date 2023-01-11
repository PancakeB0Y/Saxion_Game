using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    Camera curCamera;
    [SerializeField] GameObject spherePrefab;

    [SerializeField] float distanceFromCam = 2f;

    [SerializeField] float speed = 1f;

    GameObject[] spheres;
    [SerializeField] int sphereCount = 20;
    int spheresSpawned = 0;

    [SerializeField] float spawnRate = 0.5f;
    float timeBetweenSpawns;

    void Start()
    {
        curCamera = gameObject.GetComponentInChildren<Camera>();
        spheres = new GameObject[sphereCount];
        timeBetweenSpawns = spawnRate;
    }

    void Update()
    {
        if (!curCamera.enabled)
        {
            if(spheresSpawned != 0)
            {
                killSpheres();
            }
            return;
        }

        if (spheresSpawned < sphereCount)
        {
            timeBetweenSpawns += Time.deltaTime;
            if (timeBetweenSpawns >= spawnRate)
            {
                timeBetweenSpawns = 0;
                SpawnSphere();
            }
        }
        else
        {
            killSpheres();
        }
        
    }

    void SpawnSphere()
    {
        float x = Random.Range(0, 2);
        float y = Random.Range(1, 10) / 10f;
        Vector3 startPos = curCamera.ViewportToWorldPoint(new Vector3(x, y, curCamera.nearClipPlane + distanceFromCam));
        Vector3 endPos = curCamera.ViewportToWorldPoint(new Vector3(1-x, y, curCamera.nearClipPlane + distanceFromCam));
        GameObject sphere = Instantiate(spherePrefab, startPos, Quaternion.identity);
        sphere.AddComponent<Move>();
        sphere.GetComponent<Move>().speed = speed;
        sphere.GetComponent<Move>().endPos = endPos;
        spheres[spheresSpawned] = sphere;
        spheresSpawned++;
    }

    void killSpheres()
    {
        foreach(GameObject curSphere in spheres)
        {
            Destroy(curSphere);
        }
        spheresSpawned = 0;
    }
}

public class Move : MonoBehaviour
{
    public float speed;
    public Vector3 endPos;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
    }
}
