using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    bool enemySpawned = false;
    [SerializeField] float renderDistance;
    [SerializeField] float despawnDistance;
    Vector3 playerPosition;
    Vector3 spawnerPosition;
    GameObject spawnedEnemy;

    void Awake()
    {
        spawnerPosition = transform.position;
    }

    void Update()
    {
        playerPosition = GameObject.FindWithTag("Player").transform.position; // gets the position of the player
        float distance = Vector3.Distance(spawnerPosition, playerPosition); // distance between the two
        if (distance < renderDistance && !enemySpawned)
        {
            spawnEnemy();
        }
        if (distance > despawnDistance && enemySpawned)
        {
            resetSpawner();
        }
    }

    private void spawnEnemy()
    {
        enemySpawned = true;
        spawnedEnemy = Instantiate(enemy);
        spawnedEnemy.transform.position = spawnerPosition;
        spawnedEnemy.GetComponent<Enemy>().spawnpoint = spawnerPosition; // set spawnpoint to here
    }

    private void resetSpawner()
    {
        if (spawnedEnemy != null)
            spawnedEnemy.GetComponent<Enemy>().EnemyDeath(); // run code in enemy which will destroy itself
        enemySpawned = false;
    }

    private void OnEnable()
    {
        Player.onDeath += resetSpawner; // subscribe to onDeath
    }
    private void OnDisable()
    {
        Player.onDeath -= resetSpawner; // unsubscribe from onDeath
    }
}
