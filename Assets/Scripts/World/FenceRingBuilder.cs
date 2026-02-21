using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class FenceRingBuilder : MonoBehaviour
{
    public int SegmentCount => segmentCount;
    public float Radius => radius;
    public Vector3 SegmentScale => segmentScale;

    [Header("Prefab")]
    [SerializeField] private GameObject segmentPrefab;

    [Header("Ring Settings")]
    [Min(3)] [SerializeField] private int segmentCount = 16;
    [Min(0.1f)] [SerializeField] private float radius = 4f;

    [Header("Optional Overrides (applied to each spawned segment)")]
    [SerializeField] private Vector3 segmentScale = new Vector3(1f, 2f, 0.2f);

    [Header("Orientation")]
    [Tooltip("If true, segments face outward; if false, inward.")]
    [SerializeField] private bool faceOutward = true;

    [Tooltip("Extra rotation around Y, if you want to align a 'flat side' to an axis.")]
    [SerializeField] private float yawOffsetDegrees = 0f;

    [Header("Rebuild")]
    [SerializeField] private bool rebuildOnValidate = true;

    private const string GeneratedTag = "__FenceGenerated";

    private void OnValidate()
    {
        if (!rebuildOnValidate) return;
        Rebuild();
    }

    [ContextMenu("Rebuild Fence Ring")]
    public void Rebuild()
    {
        if (segmentPrefab == null)
        {
            // No prefab assigned, nothing to build
            return;
        }

        ClearGenerated();

        float step = 360f / segmentCount;

        for (int i = 0; i < segmentCount; i++)
        {
            float angleDeg = i * step;
            float angleRad = angleDeg * Mathf.Deg2Rad;

            // Position around ring in XZ plane
            Vector3 localPos = new Vector3(
                Mathf.Sin(angleRad) * radius,
                0f,
                Mathf.Cos(angleRad) * radius
            );

            GameObject seg = InstantiateSegment();
            seg.name = $"{GeneratedTag}_{i:00}";
            seg.transform.SetParent(transform, false);
            seg.transform.localPosition = localPos;

            // Face center/outside
            Vector3 directionFromCenter = localPos.normalized;
            if (!faceOutward) directionFromCenter = -directionFromCenter;

            // Look in that direction
            Quaternion lookRot = Quaternion.LookRotation(directionFromCenter, Vector3.up);
            seg.transform.localRotation = lookRot * Quaternion.Euler(0f, yawOffsetDegrees, 0f);

            // Apply scale override
            seg.transform.localScale = segmentScale;
        }
    }

    private void ClearGenerated()
    {
        // Remove only the generated children
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.name.StartsWith(GeneratedTag))
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    DestroyImmediate(child.gameObject);
                else
                    Destroy(child.gameObject);
#else
                Destroy(child.gameObject);
#endif
            }
        }
    }

    private GameObject InstantiateSegment()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            var obj = (GameObject)PrefabUtility.InstantiatePrefab(segmentPrefab);
            return obj != null ? obj : Instantiate(segmentPrefab);
        }
#endif
        return Instantiate(segmentPrefab);
    }
}
