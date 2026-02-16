using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private TMP_Text countdownText;

    private void Awake()
    {
        if (waveManager != null)
            waveManager.OnWaveDataChanged += Refresh;
    }

    private void OnDestroy()
    {
        if (waveManager != null)
            waveManager.OnWaveDataChanged -= Refresh;
    }

    private void Start()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (waveManager == null || countdownText == null) return;

        if (waveManager.CurrentState == WaveManager.State.Break)
        {
            // Ceil so "0.2s" still shows as 1, then flips to 0 when done
            int seconds = Mathf.CeilToInt(waveManager.BreakTimeRemaining);
            countdownText.gameObject.SetActive(true);
            countdownText.text = $"Next wave in {seconds}";
        }
        else
        {
            countdownText.gameObject.SetActive(false);
        }
    }
}
