using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static FinishGame;

public class Minigame : MonoBehaviour
{
    Camera curCamera;
    float distanceFromCam = 2f;

    [SerializeField] GameObject[] goodElementPrefabs;
    [SerializeField] GameObject[] badElementPrefabs;
    [SerializeField] LayerMask elementLayer;

    [SerializeField] TextMeshProUGUI elementCountText;
    [SerializeField] TextMeshProUGUI resultsText;

    ArrayList allElements = new ArrayList();

    int maxGoodElements;
    int maxBadElements;

    int goodElementsSpawned = 0;
    int badElementsSpawned = 0;
    int goodElementsDestroyed = 0;
    int badElementsDestroyed = 0;

    [SerializeField] float spawnRate = 0.5f;
    float timeBetweenSpawns;

    [SerializeField] StopwatchScript stopwatchScript;
    bool isTimeOut = false;

    [HideInInspector] public bool isMinigameOver = false;
    public bool isMinigameWon = false;

    void Start()
    {
        curCamera = gameObject.GetComponentInChildren<Camera>();
        timeBetweenSpawns = spawnRate;

        maxGoodElements = Random.Range(10, 21);
        maxBadElements = Random.Range(10, 21);

        RandomizeGameObjectArray(goodElementPrefabs);
        RandomizeGameObjectArray(badElementPrefabs);

        elementCountText.text = "Bad Elements: 0";
        minigamesCount++;
    }

    void FinishMinigame()
    {
        if (!isMinigameOver) {
            isMinigameOver = true;
            DestroyAllElements();
            resultsText.text = "Your results \n" + "Bad Elements: " + badElementsDestroyed + "/" + maxBadElements + "\n"
                    + "Good Elements: " + goodElementsDestroyed + "/" + maxGoodElements;
            if (goodElementsDestroyed <= 5 && badElementsDestroyed >= maxBadElements - 5)
            {
                isMinigameWon = true;
                minigamesWon++;
            }
            minigamesOverCount++;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = curCamera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hitData, 100, elementLayer))
            {
                GameObject elementHit = hitData.transform.gameObject;
                bool isElementGood = false;
                foreach (GameObject goodElementPrefab in goodElementPrefabs)
                {
                    if (elementHit.GetComponent<MeshRenderer>().sharedMaterial == goodElementPrefab.GetComponent<MeshRenderer>().sharedMaterial)
                    {
                        goodElementsDestroyed++;
                        isElementGood = true;
                    }
                }
                if (!isElementGood)
                {
                    badElementsDestroyed++;
                }
                allElements.Remove(elementHit);
                elementCountText.text = "Bad Elements: " + badElementsDestroyed + "\n";
                Destroy(elementHit);
            }
        }

        isTimeOut = stopwatchScript.isTimeOut;
        if (isTimeOut) FinishMinigame();

        if (goodElementsSpawned >= maxGoodElements && badElementsSpawned >= maxBadElements)
        {
            if (areAllElementsOut())
            {
                FinishMinigame();
            }
        }

        if(!isMinigameOver)
        {
            timeBetweenSpawns += Time.deltaTime;
            if (timeBetweenSpawns >= spawnRate)
            {
                timeBetweenSpawns = 0;
                SpawnElement();
            }
        }

        elementCountText.enabled = !isMinigameOver && curCamera.enabled;
        resultsText.enabled = isMinigameOver && curCamera.enabled;

        if (!curCamera.enabled)
        {
            if (allElements.Count != 0)
            {
                DestroyAllElements();
                elementCountText.text = "Bad Elements: 0";
                goodElementsDestroyed = 0;
                badElementsDestroyed = 0;
                goodElementsSpawned = 0;
                badElementsSpawned = 0;
            }
        }
    }

    void SpawnElement()
    {
        if(goodElementsSpawned >= maxGoodElements && badElementsSpawned >= maxBadElements) { return; }
        float x = Random.Range(0, 2);
        float endX = (x == 1) ? -0.1f : 1.1f;
        float y = Random.Range(1, 9) / 10f;
        float elementSpeed = Random.Range(7, 10) / 10f;

        Vector3 startPos = curCamera.ViewportToWorldPoint(new Vector3(x, y, curCamera.nearClipPlane + distanceFromCam));
        Vector3 endPos = curCamera.ViewportToWorldPoint(new Vector3(endX, y, curCamera.nearClipPlane + distanceFromCam));

        GameObject prefabToSpawn;
        int elementAffinity = Random.Range(0, 2);
        if(goodElementsSpawned >= maxGoodElements) { elementAffinity = 1; }
        if(badElementsSpawned >= maxBadElements) { elementAffinity = 0; }
        if(elementAffinity == 0)
        {
            int whichElement = Random.Range(0, goodElementPrefabs.Length - 1);
            prefabToSpawn = goodElementPrefabs[whichElement];
        }else
        {
            int whichElement = Random.Range(0, badElementPrefabs.Length - 1);
            prefabToSpawn = badElementPrefabs[whichElement];
        }

        GameObject newElement = Instantiate(prefabToSpawn, startPos, Quaternion.identity);
        newElement.AddComponent<Move>();
        newElement.GetComponent<Move>().speed = elementSpeed;
        newElement.GetComponent<Move>().endPos = endPos;

        if(elementAffinity == 0)
        {
            goodElementsSpawned++;
        }
        else {
            badElementsSpawned++;
        }
        allElements.Add(newElement);
    }

    void DestroyAllElements()
    {
        foreach (GameObject curElement in allElements)
        {
            Destroy(curElement);
        }
        allElements.Clear();

    }

    bool areAllElementsOut()
    {
        bool allOut = true;
        foreach (GameObject curElement in allElements)
        {
            if(curElement.transform.position.z != curElement.GetComponent<Move>().endPos.z)
            {
                allOut = false;
            }
        }

        return allOut;
    }

    void RandomizeGameObjectArray(GameObject[] array)
    {
        int index1;
        int index2;
        for (int i = 0; i < array.Length * 10; i++)
        {
            index1 = Random.Range(0, array.Length);
            index2 = Random.Range(0, array.Length);
            GameObject temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }
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
