using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerScript : MonoBehaviour
{
    // WITH ASSISTANCE FROM
    // https://www.raywenderlich.com/847-object-pooling-in-unity#toc-anchor-008
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    public static ObjectPoolerScript sharedInstance;

    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        } 
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake()
    {
        sharedInstance = this;
    }
}
