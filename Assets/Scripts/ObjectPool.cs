using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> _objectQueue;
    private GameObject _prefab;
    private GameObject _parent;

    // Singleton ��ҼҦ�
    private static ObjectPool<T> _instance = null;
    public static ObjectPool<T> Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ObjectPool<T>();
            }
            return _instance;
        }
    }

    public int queueCount
    {
        get
        {
            return _objectQueue.Count;
        }
    }

    public void InitPool(GameObject prefab, int warmUpCount = 0)
    {
        _parent = GameObject.Find("magicMissilePool");
        _prefab = prefab;
        _objectQueue = new Queue<T>();

        // ������w���C
        List<T> warmUpList = new List<T>();
        for (int i = 0; i < warmUpCount; i++)
        {
            T t = Instance.Spawn(Vector3.zero, Quaternion.identity);
            warmUpList.Add(t);
        }
        for (int i = 0; i < warmUpList.Count; i++)
        {
            Instance.Recycle(warmUpList[i]);
        }
    }

    public T Spawn(Vector3 position, Quaternion quaternion)
    {
        if (_prefab == null)
        {
            Debug.LogError(typeof(T).ToString() + " prefab not set!");
            return default(T);
        }
        if (queueCount <= 0)
        {
            GameObject g = Object.Instantiate(_prefab, position, quaternion);
            g.transform.SetParent(_parent.transform);
            T t = g.GetComponent<T>();
            if (t == null)
            {
                Debug.LogError(typeof(T).ToString() + " not found in prefab!");
                return default(T);
            }
            _objectQueue.Enqueue(t);
        }
        T obj = _objectQueue.Dequeue();
        obj.gameObject.transform.position = position;
        obj.gameObject.transform.rotation = quaternion;
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Recycle(T obj)
    {
        _objectQueue.Enqueue(obj);
        obj.gameObject.SetActive(false);
    }
}