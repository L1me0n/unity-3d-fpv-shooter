using System.Collections;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    [SerializeField] private WeaponController weapon;
    [SerializeField] private Transform viewSlot;

    private GameObject currentView;

    private void Awake()
    {
        if (!weapon) weapon = GetComponent<WeaponController>();
    }

    private IEnumerator Start()
    {
        yield return null; // wait 1 frame
        Refresh();
    }

    public void Refresh()
    {
        if (!weapon)
        {
            Debug.LogError($"[WeaponView] Missing WeaponController on {name}");
            return;
        }

        if (!viewSlot)
        {
            Debug.LogError($"[WeaponView] viewSlot not assigned on {name}");
            return;
        }

        if (weapon.Definition == null)
        {
            Debug.LogError($"[WeaponView] WeaponDefinition is null on {name}");
            return;
        }

        if (currentView) Destroy(currentView);

        GameObject prefab = weapon.Definition.weaponViewPrefab;
        if (!prefab)
        {
            Debug.LogWarning($"[WeaponView] No weaponViewPrefab set for {weapon.Definition.name}");
            return;
        }

        currentView = Instantiate(prefab, viewSlot);
        currentView.transform.localPosition = Vector3.zero;
        currentView.transform.localRotation = Quaternion.identity;
        currentView.transform.localScale = Vector3.one;
    }

    public void Apply(WeaponDefinition def)
    {
        if (viewSlot == null || def == null) return;

        if (currentView != null)
            Destroy(currentView);

        if (def.weaponViewPrefab != null)
            currentView = Instantiate(def.weaponViewPrefab, viewSlot);
    }
}
