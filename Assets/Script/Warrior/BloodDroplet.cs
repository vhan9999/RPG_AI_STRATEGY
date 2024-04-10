using UnityEngine;

public class BloodDroplet : MonoBehaviour
{
    public GameObject bloodBlockPrefab; // Assign your blood block prefab in the inspector
    public int numberOfDroplets = 10; // Number of droplets to spawn
    public float spawnRadius = 1f; // Radius around the player to spawn the droplets

    // Method to call when you want to spawn blood droplets
    public void SpawnBloodDroplets()
    {
        for (int i = 0; i < numberOfDroplets; i++)
        {
            // Generate a random position around the player
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = transform.position.y; // Optional: keep the y position constant

            // Instantiate the blood block prefab at the calculated position
            Instantiate(bloodBlockPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void DestroyBloodDroplets(){
        Destroy(bloodBlockPrefab); // Destroy the blood block on collision
    }
}
