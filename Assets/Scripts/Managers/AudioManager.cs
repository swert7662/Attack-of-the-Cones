using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip[] _playlist;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _extendedEffect;
    [SerializeField] private GameObject audioSourcePrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1, float pitch = 1)
    {
        AudioSource effectSource = ObjectPoolManager.SpawnObject<AudioSource>(audioSourcePrefab, ObjectPoolManager.PoolType.AudioSource);
        effectSource.pitch = pitch;
        effectSource.PlayOneShot(clip, volume);
        ObjectPoolManager.DespawnComponent(this, effectSource, clip.length);
    }

    public void PlaySoundNoPitch(AudioClip clip, float volume = 1)
    {
        PlaySound(clip, volume, 1);
    }

    public void PlayClipPortion(AudioClip clip, float startAt, float duration, float pitch = 1)
    {
        StartCoroutine(PlayClipForDuration(clip, startAt, duration, pitch));
    }

    private IEnumerator PlayClipForDuration(AudioClip clip, float startAt, float duration, float pitch = 1)
    {
        _extendedEffect.pitch = pitch;
        _extendedEffect.clip = clip;
        _extendedEffect.time = startAt;
        _extendedEffect.Play();
        yield return new WaitForSeconds(duration);
        _extendedEffect.Stop();
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void ToggleEffects()
    {
        //_effectSource.mute = !_effectSource.mute;
    }

    public void ToggleMusic()
    {
        _musicSource.mute = !_musicSource.mute;
    }
}
