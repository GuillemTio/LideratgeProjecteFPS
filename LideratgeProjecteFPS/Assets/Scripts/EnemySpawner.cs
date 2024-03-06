using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float timeToSpawn = 3f;
    private float spawnTimer;
    private float roundTimer;
    [SerializeField] float enemiesPerRound = 10f;
    float enemiesSpawned;
    bool roundActive = true;
    [SerializeField] float timeBetweenRounds = 10f;
    [SerializeField] float enemiesToAddEachRound = 3f;
    [SerializeField] LayerMask raycastLayerMask;

    public List<GameObject> enemyPrefabs;
    [SerializeField] TypesOfEnemyBehaviours enemyBehaviour;

    private void Start()
    {
        foreach (GameObject enemy in enemyPrefabs)
        {
            enemy.GetComponent<EnemyBehaviour>().thisEnemyBehaviour = enemyBehaviour;
        }
    }

    void Update()
    {
        if (roundActive)
        {
            Ray ray = new Ray(transform.position, Vector3.up);
            if (!Physics.Raycast(ray, 1, raycastLayerMask))
            {
                if (spawnTimer >= timeToSpawn)
                {
                    GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count - 1)];
                    GameObject.Instantiate(enemyPrefab, transform.position, transform.rotation);
                    enemiesSpawned++;
                    spawnTimer = 0;
                    if (enemiesSpawned == enemiesPerRound)
                    {
                        enemiesSpawned = 0;
                        roundActive = false;
                    }
                }
                else
                {
                    spawnTimer += Time.deltaTime;
                }
            }
        }
        else
        {
            roundTimer += Time.deltaTime;
            if(roundTimer > timeBetweenRounds)
            {
                roundTimer = 0;
                roundActive = true;
                enemiesPerRound += enemiesToAddEachRound;
            }
        }
    }
}
