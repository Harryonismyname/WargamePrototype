using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] bool reqInit;
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int numberOfItems;
    }

    #region Singleton
    public static ObjectPool Instance;
    private void Awake()
    {
        Instance = this;
        if (!reqInit)
        {
            Initialize();
        }
    }
    #endregion
    [SerializeField] Transform world;
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public bool init;

    public bool IsInitialized => init;

    //  change this later
    /*private void Start()
    {
        Initialize();
    }*/

    private void OnDestroy()
    {
        Instance = null;
        foreach (KeyValuePair<string, Queue<GameObject>> kv in poolDictionary)
        {
            foreach (var obj in kv.Value)
            {
                Destroy(obj);
            }
        }
    }
    public void Initialize()
    {
        StartCoroutine(BootUp());
    }
    public void Activate()
    {
    }
    IEnumerator BootUp()
    {
        init = false;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new();
            for (int i = 0; i < pool.numberOfItems; i++)
            {
                GameObject obj = Instantiate(pool.prefab, world, true);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
            yield return new WaitForEndOfFrame();
        }
        init = true;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(false);
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);
        objectToSpawn.SetActive(true);

        if (objectToSpawn.TryGetComponent(out IPooledObject pooledObject))
        {
            pooledObject.OnObjectSpawn();
        }


        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
    public GameObject SpawnFromPool(string tag, Transform target)
    {
        return SpawnFromPool(tag, target.position, target.rotation);
    }
}
