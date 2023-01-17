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
    [SerializeField] Vector3[] oppositeDirectionVectors;
    [SerializeField] Vector3[] perpenticularVectors;
    [SerializeField] Vector3[] oppositePerpenticularVectors;

    [SerializeField] GameObject npcAndHousePrefab;

    [SerializeField] float distance = 5f;

    void Start()
    {
        initialPoints = new ArrayList(GetComponentsInChildren<Transform>());
        initialPoints.RemoveAt(0);
        points = new Transform[initialPoints.Count];
        int i = 0;
        foreach(Transform cur in initialPoints)
        {
            points[i] = cur;
            i++;
        }

        positions = new Vector3[points.Length];
        for(i = 0; i < points.Length; i++)
        {
            positions[i] = points[i].position;
        }

        directionVectors = new Vector3[points.Length];
        for(i = 0; i < points.Length - 1; i++)
        {
            directionVectors[i] = createVectorFromTwoPoints(points[i].position, points[i + 1].position);
        }
        oppositeDirectionVectors = new Vector3[directionVectors.Length];
        for (i = 0; i < directionVectors.Length; i++)
        {
            oppositeDirectionVectors[i] = reverseVector(directionVectors[i]);
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
        perpenticularVectors = new Vector3[directionVectors.Length];
        for (i = 0; i < points.Length; i++)
        {
            int j = (i < points.Length - 1) ? i : (i - 1);
            perpenticularVectors[j] = getPerpendicularVector(directionVectors[j]);
        }
        oppositePerpenticularVectors = new Vector3[directionVectors.Length];
        for (i = 0; i < points.Length; i++)
        {
            int j = (i < points.Length - 1) ? i : (i - 1);
            oppositePerpenticularVectors[j] = getPerpendicularVector(oppositeDirectionVectors[j]);
        }
        for (i = 0; i < points.Length; i++)
        {
            int j = (i < points.Length - 1) ? i : (i - 1);
            int whichVector = UnityEngine.Random.Range(0, 2);
            Vector3 curVector = whichVector == 0 ? perpenticularVectors[j] : oppositePerpenticularVectors[j];
            float len = curVector.magnitude;
            double angle = (curVector.z > 0) ? Math.Acos(curVector.x / len) : (2 * Math.PI - Math.Acos(curVector.x / curVector.magnitude));
            angle = -angle;
            GameObject newElement = Instantiate(npcAndHousePrefab, points[i].position + curVector * distance, Quaternion.AngleAxis((float)((angle * 180 / Math.PI) - 90), new Vector3(0, 1, 0)));
        }

        float lenghtOfPoints = getLengthBetweenPoints(positions);
        for(i = 0; i < points.Length; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                float randLength = UnityEngine.Random.Range(0, lenghtOfPoints + 1);
                for(int k = 0; k < points.Length - 1; k++)
                {
                    Vector3[] newVector = new Vector3[] { positions[k], positions[k + 1] };
                    float curLength = getLengthBetweenPoints(newVector)
                    if(lenghtOfPoints > curLength)
                    {
                        lenghtOfPoints -= curLength;
                    }
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
