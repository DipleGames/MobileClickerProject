using UnityEngine;

public class AudioManager : SingleTon<AudioManager>
{
    [SerializeField] private AudioSource _audioSource;

    public void PlaySFX(AudioClip sfx)
    {
        _audioSource.PlayOneShot(sfx);
    }
}
