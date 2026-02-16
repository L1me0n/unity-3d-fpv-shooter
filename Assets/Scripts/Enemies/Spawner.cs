using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform enemiesParent;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject circleEnemyPrefab; // melee circle
    [SerializeField] private GameObject cubeEnemyPrefab;   // ranged cube

    [Header("Spawn Points")]
    [SerializeField] private List<SpawnPoint> spawnPoints = new();

    public void SpawnEnemy(bool spawnCube)
    {
        GameObject prefab = spawnCube ? cubeEnemyPrefab : circleEnemyPrefab;

        if (prefab == null)
        {
            Debug.LogError("[Spawner] Missing prefab. Assign circleEnemyPrefab and cubeEnemyPrefab.");
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

        Instantiate(prefab, pos, rot, enemiesParent);
    }

    public void SetSpawnPoints(List<SpawnPoint> points) => spawnPoints = points;
}
