using System.Collections.Generic;
using UnityEngine;

public class BloodDropletPoolManager : MonoBehaviour
{
    public static BloodDropletPoolManager Instance { get; private set; }
    public GameObject bloodBlockPrefab;

    public int poolSize = 20;
    private Queue<GameObject> pool = new Queue<GameObject>();

    public int numberOfDroplets = 10; // Number of droplets to spawn
    public float spawnRadius = 1f; // Radius around the player to spawn the droplets

    void Awake()
    {
        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject droplet = Instantiate(bloodBlockPrefab);
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

    public void SpawnBloodDroplets()
    {
        for (int i = 0; i < numberOfDroplets; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = transform.position.y;

            // Get a droplet from the pool instead of instantiating
            Instance.GetFromPool(spawnPosition);
        }
    }
}
