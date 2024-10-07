using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> _objectQueue;
    private GameObject _prefab;
    private Transform _parent;

    public ObjectPool()
    {
        
    }

    // Singleton
    public static ObjectPool<T> instance = null;
    public static ObjectPool<T> Instance
    {
        get
        {

            if (instance == null) {
                instance = new ObjectPool<T>();
                Debug.Log("new objectpool");
            }
            return instance;
        }
    }
    public void InitPool(GameObject prefab, int warmUpCount = 0, Transform parent = null)
    {
        _objectQueue = new Queue<T>();
        _parent = parent ?? GameObject.Find("ObjectPools").transform;
        _prefab = prefab;

        List<T> warmUpList = new List<T>();
        for (int i = 0; i < warmUpCount; i++)
        {
            T t = Spawn(Vector3.zero, Quaternion.identity, parent);
            warmUpList.Add(t);
        }
        warmUpList.ForEach(obj => Recycle(obj));
    }

    public T Spawn(Vector3 position, Quaternion quaternion, Transform parent = null)
    {
        if (_objectQueue.Count <= 0)
        {
            GameObject g = Object.Instantiate(_prefab, position, quaternion);
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
        obj.transform.parent = parent ?? _parent;
        obj.gameObject.SetActive(true);

        return obj;
    }

    public void Recycle(T obj)
    {
        _objectQueue.Enqueue(obj);
        obj.gameObject.SetActive(false);
        obj.transform.parent = _parent;
    }

    
}
