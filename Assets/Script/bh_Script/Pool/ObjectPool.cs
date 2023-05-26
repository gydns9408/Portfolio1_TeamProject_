using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : PoolObjectShape
{
    // Start is called before the first frame update
    public GameObject copyPrefab;
    public int poolSize = 128;

    T[] pool;
    Queue<T> objectQueue;

    public void MakeObjectPool() {
        if (pool == null)
        {
            pool = new T[poolSize];
            objectQueue = new Queue<T>(poolSize);
            GenerateObject(0, poolSize, pool);
        }
        else 
        {
            foreach (T obj in pool) 
            { 
                obj.gameObject.SetActive(false);
            }
        }
    }

    void GenerateObject(int start, int end, T[] poolArray) {
        for (int i = start; i < end; i++) {
            GameObject obj = Instantiate(copyPrefab, transform);
            obj.name = $"{copyPrefab.name}_{i}";
            T objType = obj.GetComponent<T>();
            poolArray[i] = objType;
            objType.onDisable += (() => objectQueue.Enqueue(objType));
            obj.SetActive(false);
        }
    }

    public T GetObject() {
        if (objectQueue.Count > 0)
        {
            T objType = objectQueue.Dequeue();
            objType.gameObject.SetActive(true);
            return objType;
        }
        else {
            ExtendPool();
            return GetObject();
        }
    }

    public T GetObject(Transform goalTransfrom)
    {
        if (objectQueue.Count > 0)
        {
            T objType = objectQueue.Dequeue();
            objType.gameObject.SetActive(true);
            objType.gameObject.transform.position = goalTransfrom.position;
            return objType;
        }
        else
        {
            ExtendPool();
            return GetObject(goalTransfrom);
        }
    }

    void ExtendPool() {
        int newSize = poolSize * 2;
        T[] newPool = new T[newSize];
        for (int i = 0; i < poolSize; i++)
        {
            newPool[i] = pool[i];
        }
        GenerateObject(poolSize, newSize, newPool);
        pool = newPool;
        poolSize = newSize;
    }

}
