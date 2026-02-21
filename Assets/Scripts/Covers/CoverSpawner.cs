using UnityEngine;
using System;

public class CoverSpawner : MonoBehaviour
{
    System.Random random = new System.Random();
    [Header("References")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private GameObject coverPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private int spawnOnWave = 5;
    [SerializeField] private int count;

    [SerializeField] private float arenaRadius = 28f;
    [SerializeField] private float spawnHeight = 18f;
    [SerializeField] private float groundRayLength = 100f;

    [SerializeField] private LayerMask groundMask = ~0;

    private bool spawnedThisRun;


    private void Awake()
    {
        count = random.Next(3, 7);
        if (waveManager == null)
            waveManager = FindAnyObjectByType<WaveManager>();
    }

    private void OnEnable()
    {
        if (waveManager != null)
            waveManager.OnWaveStarted += HandleWaveStarted;
            waveManager.OnWaveCleared += HandleWaveCleared;
    }

    private void OnDisable()
    {
        if (waveManager != null)
            waveManager.OnWaveStarted -= HandleWaveStarted;
            waveManager.OnWaveCleared += HandleWaveCleared;
    }

    private void HandleWaveStarted(int waveNumber)
    {
        if (spawnedThisRun) return;
        if (waveNumber < spawnOnWave) return;

        SpawnCovers();
        spawnedThisRun = true;
    }

    private void HandleWaveCleared(int waveNumber)
    {
        spawnedThisRun = false;
    }


    private void SpawnCovers()
    {
        if (coverPrefab == null) return;

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = GetRandomArenaPointOnGround();
            Vector3 spawnPos = pos + Vector3.up * spawnHeight;

            Instantiate(coverPrefab, spawnPos, UnityEngine.Random.rotation);
        }

        Debug.Log($"[COVERS] Spawned {count} covers🔺");
    }

    private Vector3 GetRandomArenaPointOnGround()
    {
        // pick random point in circle
        Vector2 p = UnityEngine.Random.insideUnitCircle * arenaRadius;
        Vector3 origin = new Vector3(p.x, spawnHeight + 5f, p.y) + transform.position;

        // raycast down to ground
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, groundRayLength, groundMask, QueryTriggerInteraction.Ignore))
        {
            return hit.point;
        }

        // fallback: just drop to y=0 plane if no ground hit
        return new Vector3(origin.x, 0f, origin.z);
    }
}
