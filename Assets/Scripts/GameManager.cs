using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] bigAsteroidsPrefab;
    [SerializeField] private GameObject[] mediumAsteroidPrefab;
    [SerializeField] private GameObject[] smallAsteroidPrefab;

    public Dictionary<Asteroid.Type, GameObject[]> asteroidsPrefab = new Dictionary<Asteroid.Type, GameObject[]>();
    [SerializeField] private int initialAsteroidCount = 5;
    [SerializeField] private float spawnRadius = 10f;
    
    private List<GameObject> activeAsteroids = new List<GameObject>();

    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
         if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }
    
    Instance = this;
    DontDestroyOnLoad(gameObject);

    }
    void Start()
    {
        asteroidsPrefab.Add(Asteroid.Type.Big, bigAsteroidsPrefab);
        asteroidsPrefab.Add(Asteroid.Type.Medium, mediumAsteroidPrefab);
        asteroidsPrefab.Add(Asteroid.Type.Small, smallAsteroidPrefab);
        
        SpawnInitialAsteroids();
        
    }       

    void Update()
    {
        // Controlla se tutti gli asteroidi sono stati distrutti
        if (activeAsteroids.Count == 0)
        {
            SpawnInitialAsteroids();
        }
    }
    
    private void SpawnInitialAsteroids()
    {
        for (int i = 0; i < initialAsteroidCount; i++)
        {
            SpawnAsteroid(GetRandomSpawnPosition(), 0); // size 3 = grande
        }
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        // Spawna gli asteroidi ai bordi dello schermo
        float randomAngle = Random.Range(0f, 360f);
        Vector3 spawnPos = new Vector3(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad) * spawnRadius,
            Mathf.Sin(randomAngle * Mathf.Deg2Rad) * spawnRadius,
            0
        );
        return spawnPos;
    }
    
    public void SpawnAsteroid(Vector3 position, Asteroid.Type type)
    {
        GameObject asteroid = Instantiate(asteroidsPrefab[type][Random.Range(0,asteroidsPrefab[type].Length)], position, Quaternion.identity);
        activeAsteroids.Add(asteroid);
        
    }
    
    public void RemoveAsteroid(GameObject asteroid)
    {
        activeAsteroids.Remove(asteroid);
    }
}
