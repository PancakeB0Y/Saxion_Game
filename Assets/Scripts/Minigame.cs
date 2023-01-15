using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Minigame : MonoBehaviour
{
    Camera curCamera;
    float distanceFromCam = 2f;

    [SerializeField] GameObject goodSphere;
    [SerializeField] GameObject badSphere;

    [SerializeField] TextMeshProUGUI sphereCountText;
    [SerializeField] TextMeshProUGUI resultsText;

    ArrayList goodSpheres = new ArrayList();
    ArrayList badSpheres = new ArrayList();
    [SerializeField] int maxGoodSpheres = 20;
    [SerializeField] int maxBadSpheres = 20;
    int goodSpheresSpawned = 0;
    int badSpheresSpawned = 0;
    int goodSpheresDestroyed = 0;
    int badSpheresDestroyed = 0;

    [SerializeField] float spawnRate = 0.5f;
    float timeBetweenSpawns;

    [SerializeField] StopwatchScript stopwatchScript;
    bool isTimeOut = false;

    bool gameOver = false;

    void Start()
    {
        curCamera = gameObject.GetComponentInChildren<Camera>();
        timeBetweenSpawns = spawnRate;
        sphereCountText.text = "Good Spheres: 0/" + maxGoodSpheres + "\n" + "Bad Spheres: 0/" + maxBadSpheres;
    }

    void Update()
    {
        sphereCountText.enabled = false;
        resultsText.enabled = false;
        isTimeOut = stopwatchScript.isTimeOut;
        if (isTimeOut) { gameOver = true; }
        if (gameOver)
        {
            KillSpheres();
            resultsText.text = "Your results \n" + "Good Spheres: " + goodSpheresDestroyed + "/" + maxGoodSpheres + "\n"
                    + "Bad Spheres: " + badSpheresDestroyed + "/" + maxBadSpheres; ;
            if (curCamera.enabled) { resultsText.enabled = true; }
            return;
        }
        if (!curCamera.enabled)
        {
            KillSpheres();
            sphereCountText.text = "Good Spheres: 0/" + maxGoodSpheres + "\n" + "Bad Spheres: 0/" + maxBadSpheres;
            goodSpheresDestroyed = 0;
            badSpheresDestroyed = 0;
            goodSpheresSpawned = 0;
            badSpheresSpawned = 0;
            return;
        }
        sphereCountText.enabled = true;
        if (goodSpheresSpawned >= maxGoodSpheres && badSpheresSpawned >= maxBadSpheres)
        {
            if (areAllSpheresOut())
            {
                resultsText.enabled = true;
                gameOver = true;
                return;
            }
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
                    goodSpheres.Remove(sphereHit);
                }
                else{ 
                    badSpheresDestroyed++; 
                    badSpheres.Remove(sphereHit); 
                }
                sphereCountText.text = "Good Spheres: " + goodSpheresDestroyed + "/" + maxGoodSpheres + "\n"
                    + "Bad Spheres: " + badSpheresDestroyed + "/" + maxBadSpheres;
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
        if(goodSpheresSpawned >= maxGoodSpheres && badSpheresSpawned >= maxBadSpheres) { return; }
        float x = Random.Range(0, 2);
        float endX = (x == 1) ? -0.1f : 1.1f;
        float y = Random.Range(1, 9) / 10f;
        float sphereSpeed = Random.Range(7, 10) / 10f;

        Vector3 startPos = curCamera.ViewportToWorldPoint(new Vector3(x, y, curCamera.nearClipPlane + distanceFromCam));
        Vector3 endPos = curCamera.ViewportToWorldPoint(new Vector3(endX, y, curCamera.nearClipPlane + distanceFromCam));

        int whichSphere = Random.Range(0, 2);
        if(goodSpheresSpawned >= maxGoodSpheres) { whichSphere = 1; }
        if(badSpheresSpawned >= maxBadSpheres) { whichSphere = 0; }
        GameObject sphere = Instantiate((whichSphere == 0) ? goodSphere : badSphere, startPos, Quaternion.identity);
        sphere.AddComponent<Move>();
        sphere.GetComponent<Move>().speed = sphereSpeed;
        sphere.GetComponent<Move>().endPos = endPos;

        if(whichSphere == 0)
        {
            goodSpheres.Add(sphere);
            goodSpheresSpawned++;
        }
        else { 
            badSpheres.Add(sphere);
            badSpheresSpawned++;
        }
        
    }

    void KillSpheres()
    {
        foreach (GameObject sphere in goodSpheres)
        {
            Destroy(sphere);
        }
        goodSpheres.Clear();

        foreach (GameObject sphere in badSpheres)
        {
            Destroy(sphere);
        }
        badSpheres.Clear();
    }

    bool areAllSpheresOut()
    {
        bool allOut = true;
        foreach (GameObject sphere in goodSpheres)
        {
            if(sphere.transform.position.z != sphere.GetComponent<Move>().endPos.z)
            {
                allOut = false;
            }
        }
        foreach (GameObject sphere in badSpheres)
        {
            if (sphere.transform.position.z != sphere.GetComponent<Move>().endPos.z)
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
