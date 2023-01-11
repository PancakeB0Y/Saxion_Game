using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    public static bool isInMinigame = false;

    Camera curCamera;

    [SerializeField] GameObject spherePrefab;
    [SerializeField] float sphereDistanceFromCam = 2f;
    [SerializeField] float sphereSpeed = 1f;

    ArrayList spheres = new ArrayList();
    [SerializeField] int maxSphereCount = 20;

    [SerializeField] float spawnRate = 0.5f;
    float timeBetweenSpawns;

    void Start()
    {
        curCamera = gameObject.GetComponentInChildren<Camera>();
        timeBetweenSpawns = spawnRate;
    }

    void Update()
    {
        if (!curCamera.enabled)
        {
            if (spheres.Count != 0)
            {
                KillSpheres();
            }
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = curCamera.ScreenPointToRay(mousePos);

            if(Physics.Raycast(ray, out RaycastHit hitData, 100, 1 << spherePrefab.layer))
            {
                spheres.Remove(hitData.transform.gameObject);
                Destroy(hitData.transform.gameObject);
            }
        }
        
        if (spheres.Count < maxSphereCount)
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
            KillSpheres();
        }

    }

    void SpawnSphere()
    {
        float x = Random.Range(0, 2);
        float y = Random.Range(1, 10) / 10f;

        Vector3 startPos = curCamera.ViewportToWorldPoint(new Vector3(x, y, curCamera.nearClipPlane + sphereDistanceFromCam));
        Vector3 endPos = curCamera.ViewportToWorldPoint(new Vector3(1 - x, y, curCamera.nearClipPlane + sphereDistanceFromCam));

        GameObject sphere = Instantiate(spherePrefab, startPos, Quaternion.identity);
        sphere.AddComponent<Move>();
        sphere.GetComponent<Move>().speed = sphereSpeed;
        sphere.GetComponent<Move>().endPos = endPos;

        spheres.Add(sphere);
    }

    void KillSpheres()
    {
        foreach (GameObject sphere in spheres)
        {
            Destroy(sphere);
        }
        spheres.Clear();
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
