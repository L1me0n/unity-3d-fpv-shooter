using System;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private float maxHP = 80f;
    private float hp;

    public float HP => hp;
    public float MaxHP => maxHP;

    public event Action OnDied;

    private bool dead;

    private void Awake()
    {
        hp = maxHP;
    }

    public void TakeDamage(float amount)
    {
        if (dead) return;
        if (amount <= 0f) return;

        hp -= amount;
        if (hp <= 0f)
        {
            hp = 0f;
            Die();
        }
    }

    private void Die()
    {
        if (dead) return;
        dead = true;
        OnDied?.Invoke();
    }
}
