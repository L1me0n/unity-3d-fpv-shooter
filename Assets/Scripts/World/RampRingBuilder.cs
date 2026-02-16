using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class RampRingBuilder : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the object that has FenceRingBuilder here (your FenceRing).")]
    [SerializeField] private FenceRingBuilder fenceBuilder;

    [Tooltip("Prefab to spawn for each fence segment.")]
    [SerializeField] private GameObject rampPrefab;

    [Header("Ramp Geometry")]
    [Min(0.1f)] [SerializeField] private float rampWidth = 2f;
    [Min(0.01f)] [SerializeField] private float rampThickness = 0.2f;

    [Tooltip("Length ALONG the ramp mesh (not horizontal).")]
    [Min(0.1f)] [SerializeField] private float rampLength = 6f;

    [Header("Placement")]
    [Tooltip("How much the ramp extends inside the fence (so enemies get 'over' the barrier).")]
    [Min(0f)] [SerializeField] private float insideInset = 0.5f;

    [Tooltip("Extra distance outside the fence before the ramp starts.")]
    [Min(0f)] [SerializeField] private float outsideOffset = 0.5f;

    [Tooltip("How high above the fence top the ramp should clear.")]
    [Min(0f)] [SerializeField] private float topClearance = 0.2f;

    [Header("Rebuild")]
    [SerializeField] private bool rebuildOnValidate = true;

    private const string GeneratedTag = "__RampGenerated";

    private void OnValidate()
    {
        if (!rebuildOnValidate) return;
        Rebuild();
    }

    [ContextMenu("Rebuild Ramps")]
    public void Rebuild()
    {
        if (fenceBuilder == null || rampPrefab == null)
            return;

        ClearGenerated();

        // Read ring settings from the fence builder
        int count = fenceBuilder.SegmentCount;
        float radius = fenceBuilder.Radius;

        // Fence height is just the fence segment scale Y (because our segment is a cube scaled in Y).
        float fenceHeight = fenceBuilder.SegmentScale.y;

        // We want the inner end of ramp to be above the fence top (plus a bit).
        float rise = fenceHeight + topClearance;

        // Convert rise + rampLength into an incline angle.
        // If rise is too big for the length, clamp to prevent NaN.
        float sin = Mathf.Clamp(rise / rampLength, 0f, 0.999f);
        float angleRad = Mathf.Asin(sin);
        float angleDeg = angleRad * Mathf.Rad2Deg;

        // Horizontal projection of the ramp length
        float horiz = rampLength * Mathf.Cos(angleRad);

        float step = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = i * step * Mathf.Deg2Rad;

            // direction from center toward this segment (outward)
            Vector3 dirOut = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)).normalized;
            Vector3 dirIn = -dirOut;

            // We place the ramp so that:
            // - The "high end" is near the fence line (slightly inside)
            // - The "low end" starts outside
            //
            // Center distance from origin:
            // fence radius + outside offset + half horizontal length - inside inset
            float centerDistance = radius + outsideOffset + (horiz * 0.5f) - insideInset;

            // Center height is half the rise (outer end near ground, inner end near fence top)
            float centerY = rise * 0.5f;

            GameObject ramp = InstantiatePrefab();
            ramp.name = $"{GeneratedTag}_{i:00}";
            ramp.transform.SetParent(transform, false);

            // Position it around the ring
            ramp.transform.localPosition = dirOut * centerDistance + Vector3.up * centerY;

            // Yaw: make the ramp face inward (so its forward points toward center)
            Quaternion yaw = Quaternion.LookRotation(dirIn, Vector3.up);

            // Pitch: tilt upward along forward (inward).
            // Unity: negative X pitches forward UP.
            Quaternion pitch = Quaternion.Euler(-angleDeg, 0f, 0f);

            ramp.transform.localRotation = yaw * pitch;

            // Scale the ramp
            ramp.transform.localScale = new Vector3(rampWidth, rampThickness, rampLength);
        }
    }

    private void ClearGenerated()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.name.StartsWith(GeneratedTag))
            {
#if UNITY_EDITOR
                if (!Application.isPlaying) DestroyImmediate(child.gameObject);
                else Destroy(child.gameObject);
#else
                Destroy(child.gameObject);
#endif
            }
        }
    }

    private GameObject InstantiatePrefab()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            var obj = (GameObject)PrefabUtility.InstantiatePrefab(rampPrefab);
            return obj != null ? obj : Instantiate(rampPrefab);
        }
#endif
        return Instantiate(rampPrefab);
    }
}
