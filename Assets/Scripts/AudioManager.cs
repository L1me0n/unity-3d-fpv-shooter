using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("2D SFX Source")]
    [SerializeField] private AudioSource sfx2D;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // optional
    }

    public void Play2D(AudioClip clip, float volume = 1f, float pitchMin = 1f, float pitchMax = 1f)
    {
        if (clip == null || sfx2D == null) return;

        sfx2D.pitch = Random.Range(pitchMin, pitchMax);
        sfx2D.PlayOneShot(clip, volume);
    }

    public void Play3D(AudioClip clip, Vector3 pos, float volume = 1f, float pitchMin = 1f, float pitchMax = 1f, float minDist = 4f, float maxDist = 25f)
    {
        if (clip == null) return;

        GameObject go = new GameObject("OneShot3D");
        go.transform.position = pos;

        var src = go.AddComponent<AudioSource>();
        src.clip = clip;
        src.volume = volume;
        src.pitch = Random.Range(pitchMin, pitchMax);
        src.spatialBlend = 1f;            // 3D
        src.minDistance = minDist;
        src.maxDistance = maxDist;
        src.rolloffMode = AudioRolloffMode.Logarithmic;

        if (sfxGroup != null) src.outputAudioMixerGroup = sfxGroup;

        src.Play();
        Destroy(go, clip.length / Mathf.Max(0.01f, src.pitch));
    }
}
