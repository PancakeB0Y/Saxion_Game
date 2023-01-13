using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    public static bool isInMinigame = false;

    [SerializeField] Camera mainCamera;
    Camera curCamera;

    [SerializeField] GameObject goodSphere;
    [SerializeField] GameObject badSphere;
    [SerializeField] float distanceFromCam = 2f;
    int goodSpheresDestroyed = 0;
    int badSpheresDestroyed = 0;

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
        if (spheres.Count >= maxSphereCount)
        {
            if (areSpheresOut())
            {
                curCamera.enabled = false;
                mainCamera.enabled = true;
                Cursor.visible = !Cursor.visible;
            }
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = curCamera.ScreenPointToRay(mousePos);

            if(Physics.Raycast(ray, out RaycastHit hitData, 100, 1 << badSphere.layer))
            {
                GameObject sphereHit = hitData.transform.gameObject;
                if (sphereHit.GetComponent<MeshRenderer>().sharedMaterial == goodSphere.GetComponent<MeshRenderer>().sharedMaterial)
                { 
                    goodSpheresDestroyed++;
                }
                else{ badSpheresDestroyed++; }
                spheres.Remove(sphereHit);
                Destroy(sphereHit); 
            }
        }
        
        timeBetweenSpawns += Time.deltaTime;
        if (timeBetweenSpawns >= spawnRate)
        {
            timeBetweenSpawns = 0;
            SpawnSphere();
        }

    }

    void SpawnSphere()
    {
        float x = Random.Range(0, 2);
        float endX = (x == 1) ? -0.2f : 1.2f;
        float y = Random.Range(1, 10) / 10f;
        float sphereSpeed = Random.Range(5, 11) / 10f;

        Vector3 startPos = curCamera.ViewportToWorldPoint(new Vector3(x, y, curCamera.nearClipPlane + distanceFromCam));
        Vector3 endPos = curCamera.ViewportToWorldPoint(new Vector3(endX, y, curCamera.nearClipPlane + distanceFromCam));

        int whichSphere = Random.Range(0, 2);
        GameObject sphere = Instantiate((whichSphere == 0) ? badSphere : goodSphere, startPos, Quaternion.identity);
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

    bool areSpheresOut()
    {
        bool allOut = true;
        foreach (GameObject sphere in spheres)
        {
            if(sphere.transform.position.x != sphere.GetComponent<Move>().endPos.x)
            {
                allOut = false;
            }
        }
        return allOut;
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
