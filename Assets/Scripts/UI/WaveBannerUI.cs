using System.Collections;
using TMPro;
using UnityEngine;

public class WaveBannerUI : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;

    [Header("UI")]
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TMP_Text bannerText;

    [Header("Timing")]
    [SerializeField] private float fadeInTime = 0.2f;
    [SerializeField] private float holdTime = 0.6f;
    [SerializeField] private float fadeOutTime = 0.25f;

    private Coroutine routine;

    private void Awake()
    {
        SetAlpha(0f);
        group.gameObject.SetActive(false);

        if (waveManager != null)
            waveManager.OnWaveStarted += HandleWaveStarted;
    }

    private void OnDestroy()
    {
        if (waveManager != null)
            waveManager.OnWaveStarted -= HandleWaveStarted;
    }

    private void HandleWaveStarted(int waveNumber)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShowBannerRoutine(waveNumber));
    }

    private IEnumerator ShowBannerRoutine(int waveNumber)
    {
        group.gameObject.SetActive(true);
        bannerText.text = $"WAVE {waveNumber}";

        // Fade in
        yield return Fade(0f, 1f, fadeInTime);

        // Hold
        yield return new WaitForSeconds(holdTime);

        // Fade out
        yield return Fade(1f, 0f, fadeOutTime);

        group.gameObject.SetActive(false);
        routine = null;
    }

    private IEnumerator Fade(float from, float to, float time)
    {
        if (time <= 0f)
        {
            SetAlpha(to);
            yield break;
        }

        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / time);
            SetAlpha(a);
            yield return null;
        }

        SetAlpha(to);
    }

    private void SetAlpha(float a)
    {
        group.alpha = a;
        group.interactable = false;
        group.blocksRaycasts = false;
    }
}
