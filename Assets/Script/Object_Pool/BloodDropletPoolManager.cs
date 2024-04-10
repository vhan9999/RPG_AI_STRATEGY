using System.Collections.Generic;
using UnityEngine;

public class BloodDropletPool : MonoBehaviour
{
    public static BloodDropletPool Instance { get; private set; }

    public GameObject bloodDropletPrefab;
    public int poolSize = 20;
    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject droplet = Instantiate(bloodDropletPrefab);
            droplet.SetActive(false);
            pool.Enqueue(droplet);
            droplet.transform.SetParent(this.transform); // Organize them under the pool manager
        }
    }

    public GameObject GetFromPool(Vector3 position)
    {
        if (pool.Count == 0) // Optional: Instantiate more if needed
        {
            InitializePool(); // Reinitialize or handle as needed
        }

        GameObject droplet = pool.Dequeue();
        droplet.SetActive(true);
        droplet.transform.position = position;

        return droplet;
    }

    public void ReturnToPool(GameObject droplet)
    {
        droplet.SetActive(false);
        pool.Enqueue(droplet);
    }
}
