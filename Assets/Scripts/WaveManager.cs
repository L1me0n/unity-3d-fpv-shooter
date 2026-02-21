using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    System.Random random = new System.Random();

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
    [SerializeField] private int baseEnemyCount;

    [Tooltip("How many more enemies each wave adds.")]
    [SerializeField] private int enemiesAddedPerWave = 3;

    [Tooltip("Seconds between spawns inside a wave (lower = more intense).")]
    [SerializeField] private float spawnInterval = 0.6f;

    [Header("Spawn Randomization")]
    [Tooltip("Adds +/- randomness to spawn interval. Example 0.2 means 0.6 becomes 0.4..0.8")]
    [SerializeField] private float spawnIntervalJitter = 0.15f;

    [Header("Cube Scaling (Guaranteed)")]
    [SerializeField] private int cubeStartWave = 1;
    [SerializeField] private float cubeRatioStart = 0.10f;     // 10% cubes on start wave
    [SerializeField] private float cubeRatioAddPerWave = 0.08f; // +8% ratio per wave
    [SerializeField] private float cubeRatioMax = 0.60f;        // cap 60%

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
        baseEnemyCount = random.Next(5, 9);
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

            CurrentState = State.Spawning;
            OnWaveDataChanged?.Invoke();

            int totalToSpawn = baseEnemyCount + enemiesAddedPerWave * (CurrentWave - 1);

            float cubeRatio = GetCubeRatioForWave(CurrentWave);
            int cubeToSpawn = Mathf.FloorToInt(totalToSpawn * cubeRatio);
            int circleToSpawn = totalToSpawn - cubeToSpawn;

            CurrentState = State.Spawning;
            OnWaveDataChanged?.Invoke();

            // Spawn cubes first
            for (int i = 0; i < cubeToSpawn; i++)
            {
                spawner.SpawnEnemy(true);
                AliveEnemies++;
                OnWaveDataChanged?.Invoke();
                yield return new WaitForSeconds(GetRandomSpawnDelay());
            }

            for (int i = 0; i < circleToSpawn; i++)
            {
                spawner.SpawnEnemy(false);
                AliveEnemies++;
                OnWaveDataChanged?.Invoke();
                yield return new WaitForSeconds(GetRandomSpawnDelay());
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

    private float GetCubeRatioForWave(int wave)
    {
        if (wave < cubeStartWave) return 0f;

        int steps = wave - cubeStartWave;
        float ratio = cubeRatioStart + cubeRatioAddPerWave * steps;
        return Mathf.Clamp(ratio, 0f, cubeRatioMax);
    }

    private float GetRandomSpawnDelay()
    {
        float min = Mathf.Max(0.05f, spawnInterval - spawnIntervalJitter);
        float max = spawnInterval + spawnIntervalJitter;
        return UnityEngine.Random.Range(min, max);
    }

    private void SpawnEnemy()
    {
        float cubeChance = GetCubeRatioForWave(CurrentWave);
        bool spawnCube = UnityEngine.Random.value < cubeChance;
        spawner.SpawnEnemy(spawnCube);
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
