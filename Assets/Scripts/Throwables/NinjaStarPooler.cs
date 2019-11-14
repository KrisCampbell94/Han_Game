using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaStarPooler : MonoBehaviour
{
    public List<GameObject> pooledNinjaStar;
    public GameObject ninjaStarToPool;
    public int amountToPool;
    public static NinjaStarPooler sharedInstance;
    // Start is called before the first frame update
    void Start()
    {
        pooledNinjaStar = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(ninjaStarToPool);
            obj.SetActive(false);
            pooledNinjaStar.Add(obj);
        }
    }
    public GameObject GetPooledNinjaStars()
    {
        for (int i = 0; i < pooledNinjaStar.Count; i++)
        {
            if (!pooledNinjaStar[i].activeInHierarchy)
            {
                return pooledNinjaStar[i];
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
