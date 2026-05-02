
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] items;
    [SerializeField] int maxItemsInMap, spawnChancePerIntervalPercentage;
    [SerializeField] float spawnItemIntervalDuration, minXSpawnRange, MaxXSpawnRange;

    float lastSpawnCheckTime;
    
    public int numItemsInMap = 0;

    public static ItemSpawner Instance { get; private set; }

    void Awake()
    {
        lastSpawnCheckTime = Time.time;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if(numItemsInMap < maxItemsInMap)
        {
            if(Time.time - spawnItemIntervalDuration > lastSpawnCheckTime)
            {
                SpawnItem();
            }
        }
    }

    private void SpawnItem()
    {
       lastSpawnCheckTime = Time.time;
        int randomInt = Random.Range(0, 100);
        if(randomInt < spawnChancePerIntervalPercentage)
        {
            GameObject randomItem = items[Random.Range(0, items.Length)];
            float xPos = Random.Range(minXSpawnRange,MaxXSpawnRange);
            Vector3 objectPos = new Vector3(xPos, transform.position.y, transform.position.z);
            Instantiate(randomItem, objectPos, Quaternion.identity);
            numItemsInMap++;
        }
    }
}
