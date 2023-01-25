using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeTrees : MonoBehaviour
{
    [SerializeField] GameObject[] treePrefabs;
    [SerializeField] Transform[] trees;
    [SerializeField] GameObject[] newTrees;

    void Start()
    {
        trees = gameObject.GetComponentsInChildren<Transform>();
        newTrees = new GameObject[trees.Length];
        for (int i = 0; i < trees.Length; i++)
        {
            newTrees[i] = trees[i].gameObject;
        }

        for(int i = 1; i < newTrees.Length; i++)
        {
            int whichTree = Random.Range(0, 4);
            if (whichTree != 0)
            {
                newTrees[i] = Replace(newTrees[i], treePrefabs[whichTree - 1]);
            }

            newTrees[i].transform.Rotate(0, Random.Range(0, 360), 0);
        }
    }

    GameObject Replace(GameObject obj1, GameObject obj2)
    {
        GameObject newTree = Instantiate(obj2, obj1.transform.position, Quaternion.identity);
        Destroy(obj1);

        return newTree;
    }
}
