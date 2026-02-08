using TMPro;
using UnityEngine;

public class WaveHUDUI : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text aliveText;
    [SerializeField] private TMP_Text stateText;

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
        if (waveManager == null) return;

        waveText.text = $"Wave: {waveManager.CurrentWave}";
        aliveText.text = $"Alive: {waveManager.AliveEnemies}";
        stateText.text = waveManager.CurrentState.ToString();
    }
}
