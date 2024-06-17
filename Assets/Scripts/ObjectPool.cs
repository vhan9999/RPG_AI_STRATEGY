using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> _objectQueue = new Queue<T>();
    private GameObject _prefab;
    private Transform _parent;

    public ObjectPool()
    {
        
    }

    // Singleton
    public static ObjectPool<T> Instance { get; } = new ObjectPool<T>();

    public int queueCount => _objectQueue.Count;

    public void InitPool(GameObject prefab, int warmUpCount = 0, Transform parant = null)
    {
        _parent = parant == null ? GameObject.Find("ObjectPools").transform : parant;
        _prefab = prefab;

        List<T> warmUpList = new List<T>();
        for (int i = 0; i < warmUpCount; i++)
        {
            T t = Spawn(Vector3.zero, Quaternion.identity);
            warmUpList.Add(t);
            Recycle(warmUpList[i]);
        }
    }

    public T Spawn(Vector3 position, Quaternion quaternion, Transform transform = null)
    {
        if (queueCount <= 0)
        {
            GameObject g = Object.Instantiate(_prefab, position, quaternion);
            g.transform.SetParent(transform ?? _parent);
            T t = g.GetComponent<T>();
            if (t == null)
            {
                return default(T);
            }
            _objectQueue.Enqueue(t);
        }
        T obj = _objectQueue.Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = quaternion;
        obj.gameObject.SetActive(true);

        return obj;
    }

    public void Recycle(T obj)
    {
        _objectQueue.Enqueue(obj);
        obj.gameObject.SetActive(false);
    }
}