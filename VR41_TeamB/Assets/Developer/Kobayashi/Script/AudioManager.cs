using UnityEngine;
using System.Collections.Generic;

public class AudioManager : B_SingletonMonoBehaviour<AudioManager>
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSourcePrefab;

    [Header("Volume")]
    [Range(0f, 1f)] [SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float bgmVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float seVolume = 1f;

    private List<AudioSource> seSources = new List<AudioSource>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // BGM Source が未設定なら自動生成
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
        }
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.volume = bgmVolume * masterVolume;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume * masterVolume;
    }

    public void PlaySE(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSESource();
        source.clip = clip;
        source.volume = seVolume * masterVolume;
        source.Play();
    }

    public void PlayApplause3D(
    AudioClip clip,
    Transform[] characters
)
    {
        if (clip == null || characters == null || characters.Length == 0)
            return;

        int count = characters.Length;

        // 人数が増えても爆音にならないよう正規化
        float volumePerSound =
            seVolume * masterVolume / Mathf.Sqrt(count);

        foreach (var ch in characters)
        {
            if (ch == null) continue;

            AudioSource src = GetAvailableSESource();

            // 位置をキャラに合わせる
            src.transform.position = ch.position;

            // 3Dサウンド設定
            src.spatialBlend = 1.0f;   // 完全3D
            src.minDistance = 2.0f;    // 近距離
            src.maxDistance = 15.0f;   // 遠距離
            src.rolloffMode = AudioRolloffMode.Logarithmic;

            // 同じ音感を避ける
            src.pitch = Random.Range(0.95f, 1.05f);

            src.clip = clip;
            src.volume = volumePerSound;

            // 少しだけズラすと自然
            src.PlayDelayed(Random.Range(0f, 0.05f));
        }
    }

    public void PlayApplause(AudioClip clip, int count)
    {
        if (clip == null || count <= 0) return;

        float volumePerSound = seVolume * masterVolume / Mathf.Sqrt(count);

        for (int i = 0; i < count; i++)
        {
            AudioSource src = GetAvailableSESource();
            src.clip = clip;
            src.pitch = Random.Range(0.95f, 1.05f);
            src.volume = volumePerSound;

            // ほんの少しだけ再生タイミングをズラすと自然
            src.PlayDelayed(UnityEngine.Random.Range(0f, 0.05f));
        }
    }

    private AudioSource GetAvailableSESource()
    {
        foreach (var src in seSources)
        {
            if (!src.isPlaying)
                return src;
        }

        // なければ新しく作る
        AudioSource newSource = Instantiate(seSourcePrefab, transform);
        seSources.Add(newSource);
        return newSource;
    }

    public void SetSEVolume(float volume)
    {
        seVolume = Mathf.Clamp01(volume);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume * masterVolume;
    }
}
