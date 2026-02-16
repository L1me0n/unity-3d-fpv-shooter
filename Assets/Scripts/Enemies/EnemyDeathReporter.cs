using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyDeathReporter : MonoBehaviour
{
    public enum EnemyKind { CircleMelee, SquareShooter }

    public static event Action<EnemyDeathReporter> AnyEnemyDied;

    private Health health;
    private EnemyReward reward;

    private void Awake()
    {
        health = GetComponent<Health>();
        reward = GetComponent<EnemyReward>();
        health.onDied.AddListener(HandleDied);
    }

    private void OnDestroy()
    {
        if (health != null)
            health.onDied.RemoveListener(HandleDied);
    }

    private void HandleDied()
    {
        AnyEnemyDied?.Invoke(this);

        if (reward != null && CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.Add(reward.rewardOnKill);
        }
    }
}
