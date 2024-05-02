using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BloodDropletPoolManager : MonoBehaviour
{
    public GameObject bloodBlockPrefab;
    static public BloodDropletPoolManager Instance {  get; private set; }

    public int poolSize = 2000;
    public Queue<GameObject> pool = new Queue<GameObject>();

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
        droplet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        droplet.SetActive(false);
        //pool.Enqueue(droplet);
    }

    public void SpawnBloodDroplets(Vector3 position)
    {
        for (int i = 0; i < numberOfDroplets; i++)
        {
            Vector3 spawnPosition = position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = position.y;

            // Get a droplet from the pool instead of instantiating
            GetFromPool(spawnPosition);
        }
    }
}
