using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiPooler : MonoBehaviour
{
    public List<GameObject> pooledKunais;
    public GameObject kunaisToPool;
    public int amountToPool;
    public static KunaiPooler sharedInstance;
    // Start is called before the first frame update
    void Start()
    {
        pooledKunais = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(kunaisToPool);
            obj.SetActive(false);
            pooledKunais.Add(obj);
        }
    }
    public GameObject GetPooledKunais()
    {
        for (int i = 0; i < pooledKunais.Count; i++)
        {
            if (!pooledKunais[i].activeInHierarchy)
            {
                return pooledKunais[i];
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
