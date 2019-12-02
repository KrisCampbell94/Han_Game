using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFirePooler : MonoBehaviour
{
    public List<GameObject> pooledBulletFire;
    public GameObject bulletFireToPool;
    public int amountToPool;
    public static BulletFirePooler sharedInstance;
    // Start is called before the first frame update
    void Start()
    {
        pooledBulletFire = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(bulletFireToPool);
            obj.SetActive(false);
            pooledBulletFire.Add(obj);
        }
    }
    public GameObject GetPooledBulletFires()
    {
        for (int i = 0; i < pooledBulletFire.Count; i++)
        {
            if (!pooledBulletFire[i].activeInHierarchy)
            {
                return pooledBulletFire[i];
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
