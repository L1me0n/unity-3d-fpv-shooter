using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<SpawnPoint> spawnPoints = new();

    public void SetEnemyPrefab(GameObject prefab) => enemyPrefab = prefab;

    public void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("[Spawner] Enemy Prefab is not assigned.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("[Spawner] No spawn points assigned.");
            return;
        }

        SpawnPoint chosen = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Vector3 pos = chosen.transform.position;
        Quaternion rot = chosen.transform.rotation;

        GameObject enemy = Instantiate(enemyPrefab, pos, rot, enemiesParent);
    }

    public void SetSpawnPoints(List<SpawnPoint> points)
    {
        spawnPoints = points;
    }
}
