using System.Collections;
using TMPro;
using UnityEngine;

public class WaveClearedUI : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;

    [Header("UI")]
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TMP_Text text;

    [Header("Timing")]
    [SerializeField] private float fadeInTime = 0.15f;
    [SerializeField] private float holdTime = 0.7f;
    [SerializeField] private float fadeOutTime = 0.2f;

    [Header("Text")]
    [SerializeField] private string format = "WAVE {0} CLEARED!";

    private Coroutine routine;

    private void Awake()
    {
        SetAlpha(0f);
        group.gameObject.SetActive(false);

        if (waveManager != null)
            waveManager.OnWaveCleared += HandleWaveCleared;
    }

    private void OnDestroy()
    {
        if (waveManager != null)
            waveManager.OnWaveCleared -= HandleWaveCleared;
    }

    private void HandleWaveCleared(int waveNumber)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShowRoutine(waveNumber));
    }

    private IEnumerator ShowRoutine(int waveNumber)
    {
        group.gameObject.SetActive(true);
        text.text = string.Format(format, waveNumber);

        yield return Fade(0f, 1f, fadeInTime);
        yield return new WaitForSeconds(holdTime);
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
