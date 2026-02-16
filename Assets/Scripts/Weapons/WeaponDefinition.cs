using UnityEngine;

public enum FireType { Hitscan, Projectile }

[CreateAssetMenu(menuName = "Weapon Definition")]
public class WeaponDefinition : ScriptableObject
{
    [Header("Identity")]
    public string weaponId = "pistol";
    public string displayName = "Pistol";

    [Header("Stats")]
    public FireType fireType = FireType.Hitscan;
    public float damage = 10f;
    public float fireRate = 3f;          // shots per second (pistol slow-ish)
    public int magazineSize = 12;
    public float reloadTime = 1.4f;
    public float range = 70f;

    [Header("Projectile (if used)")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 25f;

    [Header("Presentation")]
    public GameObject weaponViewPrefab;   // model shown in hands
    public AudioClip shootSfx;
    public AudioClip reloadSfx;
}
