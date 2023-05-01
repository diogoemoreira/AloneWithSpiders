using UnityEngine;

public class SpiderSpawner : MonoBehaviour
{
    public static SpiderSpawner instance;

    public Transform[] spawnPositions;
    public float spawnRate;
    public GameObject spiderPrefab;

    private float lastTimeSpawn;

    private int maxSpiders;
    private int spawnedSpidersCount;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }
    }
    void Start()
    {
        lastTimeSpawn = 0f;
        maxSpiders = 0;
        spawnedSpidersCount = 0;
    }

    void Update()
    {
        if (spawnedSpidersCount >= maxSpiders)
        {
            return;
        }

        if (Time.time - lastTimeSpawn > spawnRate)
        {
            Instantiate(spiderPrefab, spawnPositions[Random.Range(0, spawnPositions.Length)]);
            lastTimeSpawn = Time.time;
            spawnedSpidersCount++;
        }
    }

    public void SpawnSpiders(int amount)
    {
        maxSpiders = amount;
    }
}
