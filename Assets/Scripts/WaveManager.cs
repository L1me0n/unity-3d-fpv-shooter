using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public enum State
    {
        WaitingToStart,
        Spawning,
        Fighting,
        Break
    }

    [Header("References")]
    [SerializeField] private Spawner spawner;

    [Header("Wave Settings")]
    [SerializeField] private float timeBetweenWaves = 5f;

    [Tooltip("Wave 1 enemy count.")]
    [SerializeField] private int baseEnemyCount = 5;

    [Tooltip("How many more enemies each wave adds.")]
    [SerializeField] private int enemiesAddedPerWave = 3;

    [Tooltip("Seconds between spawns inside a wave (lower = more intense).")]
    [SerializeField] private float spawnInterval = 0.6f;

    [Header("Spawn Randomization")]
    [Tooltip("Adds +/- randomness to spawn interval. Example 0.2 means 0.6 becomes 0.4..0.8")]
    [SerializeField] private float spawnIntervalJitter = 0.15f;

    // Runtime
    public int CurrentWave { get; private set; } = 0;
    public int AliveEnemies { get; private set; } = 0;
    public State CurrentState { get; private set; } = State.WaitingToStart;

    // Break countdown (UI reads this)
    public float BreakTimeRemaining { get; private set; } = 0f;

    // UI events
    public Action OnWaveDataChanged;
    public Action<int> OnWaveStarted;        // wave number
    public Action<int> OnWaveCleared;        // wave number

    private void OnEnable()
    {
        EnemyDeathReporter.AnyEnemyDied += HandleEnemyDied;
    }

    private void OnDisable()
    {
        EnemyDeathReporter.AnyEnemyDied -= HandleEnemyDied;
    }

    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        // Initial short delay so the scene can settle.
        yield return new WaitForSeconds(1f);

        while (true)
        {
            // Break / countdown before wave starts
            CurrentState = State.Break;
            BreakTimeRemaining = timeBetweenWaves;
            OnWaveDataChanged?.Invoke();

            while (BreakTimeRemaining > 0f)
            {
                BreakTimeRemaining -= Time.deltaTime;
                OnWaveDataChanged?.Invoke(); // lets countdown update smoothly
                yield return null;
            }

            BreakTimeRemaining = 0f;

            // Start wave
            CurrentWave++;
            OnWaveStarted?.Invoke(CurrentWave);

            int targetSpawnCount = baseEnemyCount + enemiesAddedPerWave * (CurrentWave - 1);

            CurrentState = State.Spawning;
            OnWaveDataChanged?.Invoke();

            // Spawn enemies over time
            for (int i = 0; i < targetSpawnCount; i++)
            {
                SpawnEnemy();
                AliveEnemies++;
                OnWaveDataChanged?.Invoke();

                float delay = GetRandomSpawnDelay();
                yield return new WaitForSeconds(spawnInterval);
            }

            // After spawning, we enter fighting state until AliveEnemies becomes 0
            CurrentState = State.Fighting;
            OnWaveDataChanged?.Invoke();

            while (AliveEnemies > 0)
            {
                yield return null;
            }

            // Wave cleared, loop continues -> Break again
            OnWaveCleared?.Invoke(CurrentWave);
        }
    }

    private float GetRandomSpawnDelay()
    {
        float min = Mathf.Max(0.05f, spawnInterval - spawnIntervalJitter);
        float max = spawnInterval + spawnIntervalJitter;
        return UnityEngine.Random.Range(min, max);
    }

    private void SpawnEnemy()
    {
        spawner.SpawnEnemy();
    }

    private void HandleEnemyDied(EnemyDeathReporter enemy)
    {
        // Only count down if we're in a wave (Spawning or Fighting)
        if (CurrentState == State.Spawning || CurrentState == State.Fighting)
        {
            AliveEnemies = Mathf.Max(0, AliveEnemies - 1);
            OnWaveDataChanged?.Invoke();
        }
    }
}
