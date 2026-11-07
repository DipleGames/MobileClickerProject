using UnityEngine;

public class AudioManager : SingleTon<AudioManager>
{
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;

    void Start()
    {
        PlayBGM();
    }

    public void PlaySFX(AudioClip sfx)
    {
        _sfxSource.PlayOneShot(sfx);
    }

    public void PlayBGM()
    {
        _bgmSource.Play();
    }
}
