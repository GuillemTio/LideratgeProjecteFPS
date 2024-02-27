using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float timeToSpawn = 3f;
    private float timer;
    [SerializeField] LayerMask raycastLayerMask;

    public GameObject enemyPrefab;
    [SerializeField] TypesOfEnemyBehaviours enemyBehaviour;

    private void Start()
    {
        enemyPrefab.GetComponent<EnemyBehaviour>().thisEnemyBehaviour = enemyBehaviour;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.up);
        if (!Physics.Raycast(ray, 1, raycastLayerMask))
        {
            if (timer >= timeToSpawn)
            {
                GameObject.Instantiate(enemyPrefab, transform.position, transform.rotation);
                timer = 0;

            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }
}
