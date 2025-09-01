using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// »ç¿îµå ÃÑ°ý Å¬·¡½º
/// </summary>
public class SoundController : MonoBehaviour
{
    #region singleton
    private static SoundController instance;
    public static SoundController Instance => instance;
    #endregion

    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private Queue<AudioSource> sfxAudioSource;
    [SerializeField] private GameObject sfxObj;

    public AudioSource BgmAudioSource => bgmAudioSource;
    public Queue<AudioSource> SfxAudioSource => sfxAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        Initialize();
    }

    private void Initialize()
    {
        sfxAudioSource = new Queue<AudioSource>(sfxObj.GetComponents<AudioSource>());
    }

    public void PlayBgm(int bgmCode)
    {
        if (sfxAudioSource == null) return;

        AudioClip bgmClip = DataManager.Instance.
            TableDataTable.SoundTables.FindBgmWithCode(bgmCode).Clip;

        if (bgmClip == bgmAudioSource.clip)
            return;

        if (bgmClip != null)
        {
            bgmAudioSource.clip = bgmClip;
            bgmAudioSource.Play();
        }
    }


    public void PlayBgm(AudioClip clip)
    {
        if (clip != null)
        {
            bgmAudioSource.clip = clip;
            bgmAudioSource.Play();
        }
    }

    public void PlaySfx(int sfxCode)
    {
        if (sfxAudioSource.Count <= 0) return;

        AudioClip sfxClip = DataManager.Instance.
            TableDataTable.SoundTables.FindBgmWithCode(sfxCode).Clip;

        if (sfxClip != null)
        {
            AudioSource source = sfxAudioSource.Dequeue();
            source.PlayOneShot(sfxClip);
            sfxAudioSource.Enqueue(source);
        }
    }

    public void PlaySfx(AudioClip clip)
    {
        if (sfxAudioSource.Count <= 0) return;

        if (clip != null)
        {
            AudioSource source = sfxAudioSource.Dequeue();
            source.PlayOneShot(clip);
            sfxAudioSource.Enqueue(source);
        }
    }

    public void SetBgmVolume(float value)
    {
        bgmAudioSource.volume = value;
    }

    public void SetSfxVolume(float value)
    {
        foreach(AudioSource source in sfxAudioSource)
        {
            source.volume = value;
        }
    }
}