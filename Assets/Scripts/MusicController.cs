
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip[] bgm;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(int songNumber)
    {
        audioSource.clip = bgm[songNumber];
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PlayClip2D(AudioClip clip)
    {
        GameObject tempAudioSource = new GameObject("TempAudio");
        AudioSource sfxSource = tempAudioSource.AddComponent<AudioSource>();

        sfxSource.clip = clip;
        sfxSource.spatialBlend = 0.0f;
        sfxSource.Play();

        Destroy(tempAudioSource, clip.length);

    }
}
