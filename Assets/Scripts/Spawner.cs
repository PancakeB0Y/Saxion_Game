using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] points;
    ArrayList initialPoints;
    Vector3[] positions;
    
    [SerializeField] Vector3[] directionVectors;
    [SerializeField] Vector3[] perpendicularVectors;
    [SerializeField] Vector3[] reversePerpendicularVectors;

    [SerializeField] GameObject npcAndHousePrefab;

    [SerializeField] float distance = 5f;

    ArrayList spawnedObjectPositions;
    [SerializeField] float minDistanceBetweenSpawns = 10f;

    void Start()
    {
        //Creating an array of all GameObjects that are children of this GameObject
        initialPoints = new ArrayList(GetComponentsInChildren<Transform>());
        initialPoints.RemoveAt(0); //GetComponentsInChildren() also returns the parent so it is removed from the ArrayList
        //An array is created from the ArrayList, because ArrayList isn't visualized in the Unity ispector
        points = new Transform[initialPoints.Count]; 
        int i = 0;
        foreach(Transform cur in initialPoints)
        {
            points[i] = cur;
            i++;
        }

        //Creating an array with all the positions of the GameObjects
        positions = new Vector3[points.Length];
        for(i = 0; i < points.Length; i++)
        {
            positions[i] = points[i].position;
        }

        //Creating an array with vectors that point from one GameObject to the next
        directionVectors = new Vector3[points.Length];
        for(i = 0; i < points.Length; i++)
        {
            int k = (i < points.Length - i) ? i : (i - 1);
            Debug.Log(k);
            directionVectors[i] = createVectorFromTwoPoints(points[k].position, points[k + 1].position);
        }

        /*
         * for(poin) -> len
         for(54) {
            for(100)
                r[0,len]
                for(poi)if(r> dp)r-=dp;else break;
                k = r / dp
                p = pi*(1-k) + pi+1*k
                rand (pos,neg)

                for closest
        }
         * */

        //Creating an array with the perpendicular vectors of the direction vectors 
        perpendicularVectors = new Vector3[directionVectors.Length];
        for (i = 0; i < directionVectors.Length; i++)
        {
            perpendicularVectors[i] = getPerpendicularVector(directionVectors[i]);
        }

        //Creating an array with the reverse of the perpendicular vectors
        reversePerpendicularVectors = new Vector3[directionVectors.Length];
        for (i = 0; i < directionVectors.Length; i++)
        {
            reversePerpendicularVectors[i] = reverseVector(perpendicularVectors[i]);
        }

        /*
        for (i = 0; i < points.Length; i++)
        {
            int j = (i < points.Length - 1) ? i : (i - 1);
            int whichVector = UnityEngine.Random.Range(0, 2);
            Vector3 curVector = whichVector == 0 ? perpendicularVectors[j] : reversePerpendicularVectors[j];
            float len = curVector.magnitude;
            double angle = (curVector.z > 0) ? Math.Acos(curVector.x / len) : (2 * Math.PI - Math.Acos(curVector.x / curVector.magnitude));
            angle = -angle;
            GameObject newElement = Instantiate(npcAndHousePrefab, points[i].position + curVector * distance, Quaternion.AngleAxis((float)((angle * 180 / Math.PI) - 90), new Vector3(0, 1, 0)));
        }
        */
        
        float lengthOfPoints = getLengthBetweenPoints(positions);
        Vector3[] newVectorArray;
        for (i = 0; i < 100; i++)
        {
            float randLength = UnityEngine.Random.Range(0, lengthOfPoints + 1);
            for(int j = 0; j < positions.Length; j++)
            {
                int k = (j < positions.Length - 1) ? j : (j - 1);
                newVectorArray = new Vector3[] { positions[k], positions[k + 1] };
                float curLength = getLengthBetweenPoints(newVectorArray);
                if(randLength > curLength)
                {
                    randLength -= curLength;
                }
                else
                {
                    Vector3 curDirectionVector = directionVectors[k] * randLength;
                    Vector3 newSpawnPos = positions[j] + curDirectionVector;
                    bool isCloseEnough = true;
                    foreach(Vector3 curSpawnedObjectPos in spawnedObjectPositions)
                    {
                        newVectorArray = new Vector3[] { newSpawnPos, curSpawnedObjectPos };
                        float newDistance = getLengthBetweenPoints(newVectorArray);
                        if(newDistance < minDistanceBetweenSpawns)
                        {
                            isCloseEnough = false;
                        }
                    }
                    if (isCloseEnough)
                    {
                        spawnedObjectPositions.Add(newSpawnPos);
                        int whichVector = UnityEngine.Random.Range(0, 2);
                        Vector3 curVector = whichVector == 0 ? perpendicularVectors[k] : reversePerpendicularVectors[k];
                        float len = curVector.magnitude;
                        double angle = (curVector.z > 0) ? Math.Acos(curVector.x / len) : (2 * Math.PI - Math.Acos(curVector.x / curVector.magnitude));
                        angle = -angle;
                        Instantiate(npcAndHousePrefab, newSpawnPos + curVector * distance, Quaternion.AngleAxis((float)((angle * 180 / Math.PI) - 90), new Vector3(0, 1, 0)));
                    }
                    else { break; }
                }
            }
        }
        
        
    }

    
    void Update()
    {
        
    }

    Vector3 createVectorFromTwoPoints(Vector3 point1, Vector3 point2)
    {
        Vector3 returnVector = new Vector3();
        returnVector.x = point2.x - point1.x;
        returnVector.y = 0;
        returnVector.z = point2.z - point1.z;

        return returnVector.normalized;
    }

    Vector3 reverseVector(Vector3 vector)
    {
        Vector3 returnVector = new Vector3();
        returnVector.x = -vector.x;
        returnVector.y = 0;
        returnVector.z = -vector.z;

        return returnVector.normalized;
    }

    Vector3 getPerpendicularVector(Vector3 vector)
    {
        Vector3 returnVector = new Vector3();
        returnVector.x = vector.z;
        returnVector.y = 0;
        returnVector.z = -vector.x;

        return returnVector.normalized;
    }

    float getLengthBetweenPoints(Vector3[] vectors)
    {
        float distance = 0f;
        for(int i = 0; i < vectors.Length - 1; i++)
        {
            distance += (float)Math.Sqrt((vectors[i+1].x - vectors[i].x) * (vectors[i + 1].x - vectors[i].x) + (vectors[i + 1].y - vectors[i].y) * (vectors[i + 1].y - vectors[i].y));
        }

        return distance;
    }
}
