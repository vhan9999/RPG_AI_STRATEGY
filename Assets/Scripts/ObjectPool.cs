using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//ObjectPool<BerserkerAgent> BerserkerPool = ObjectPool<BerserkerAgent>.Instance;
//ObjectPool<MageAgent> MagePool = ObjectPool<MageAgent>.Instance;  
public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> _objectQueue;
    private GameObject _prefab;
    private Transform _parent;


    // Singleton 單例模式
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
    public ObjectPool()
    {
        _objectQueue = new Queue<T>();
    }

    public int queueCount
    {
        get
        {
            return _objectQueue.Count;
        }
    }

    public void InitPool(GameObject prefab, int warmUpCount = 0, Transform parent = null)
    {
        _prefab = prefab;
        _objectQueue = new Queue<T>();
        _parent = parent == null ? GameObject.Find("ObjectPools").transform : parent;
        // 物件池預熱。
        List<T> warmUpList = new List<T>();
        for (int i = 0; i < warmUpCount; i++)
        {
            T t = Spawn(prefab, Vector3.zero, Quaternion.identity, parent);
            warmUpList.Add(t);
        }
        for (int i = 0; i < warmUpList.Count; i++)
        {
            Recycle(warmUpList[i]);
        }
    }

    public T Spawn(GameObject prefab, Vector3 position, Quaternion quaternion, Transform parent = null)
    {
        if (queueCount <= 0)
        {
            _prefab = prefab;
          
            GameObject g = Object.Instantiate(_prefab, position, quaternion);
            _parent = parent == null ? GameObject.Find("ObjectPools").transform : parent;
            g.transform.SetParent(_parent);
            T t = g.GetComponent<T>();
            if (t == null)
            {
                //Debug.LogError(typeof(T).ToString() + " not found in prefab!");
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