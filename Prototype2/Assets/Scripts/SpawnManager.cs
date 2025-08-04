using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] animalPrefabs; // Array to hold different animal prefabs
    private float spawnRangeX = 15.0f; // Range for spawning animals on the X-axis
    private float startDelay = 2.0f; // Delay before the first spawn
    private float spawnInterval = 1.5f; // Interval between spawns

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnRandomAnimal", startDelay, spawnInterval); // Start spawning animals every 2 seconds
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SpawnRandomAnimal()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, transform.position.z);
        int animalIndex = Random.Range(0, animalPrefabs.Length); // Randomly select an animal prefab index
        Instantiate(animalPrefabs[animalIndex], spawnPosition, animalPrefabs[animalIndex].transform.rotation);
    }
}
