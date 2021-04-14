using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private Slider sfxSlider;



    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    // Start is called before the first frame update
    void Start()
    {

        //load all the audio clips in our Resources/Audio folder.
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];

        //load all audio clips into the dictionary.
        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }

        LoadVolume();

        musicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }


    public void PlaySFX(string name)
    {
        sfxSource.PlayOneShot(audioClips[name]);
    }

    /// <summary>
    /// Updates the volume based on the slider's position, and then saves them to player preferences.
    /// </summary>
    public void UpdateVolume()
    {
        musicSource.volume = musicSlider.value;

        sfxSource.volume = sfxSlider.value;

        PlayerPrefs.SetFloat("SFX:", sfxSlider.value);
        PlayerPrefs.SetFloat("Music:", musicSlider.value);
    }

    /// <summary>
    /// Load player preferences when starting the game.
    /// </summary>
    public void LoadVolume()
    {
        musicSource.volume = PlayerPrefs.GetFloat("Music:", 0.5f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFX:", 0.5f);

        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;

    }
}
