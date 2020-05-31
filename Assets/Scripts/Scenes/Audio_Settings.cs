using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Audio_Settings : MonoBehaviour
{
    public Slider Sounds, AudioFX, AudioFXOther;
    public AudioMixerGroup mixer;
    public bool SetAllSlidersbool = false;
    public void SliderSounds()
    {
        float value = Sounds.value;
        mixer.audioMixer.SetFloat("SoundVolume", value);
        PlayerPrefs.SetFloat("Sound", value);
    }    
    public void SliderAudioFX()
    {
        float value = AudioFX.value;
        mixer.audioMixer.SetFloat("FXVolume", value);
        PlayerPrefs.SetFloat("AudioFX", value);
    }    
    public void SliderAudioFXOthers()
    {
        float value = AudioFXOther.value;
        mixer.audioMixer.SetFloat("FXOthersVolume", value);
        PlayerPrefs.SetFloat("AudioFXOthers", value);
    }
    public void SetAllSliders()
    {
        Sounds.value = PlayerPrefs.GetFloat("Sound");
        AudioFX.value = PlayerPrefs.GetFloat("AudioFX");
        AudioFXOther.value = PlayerPrefs.GetFloat("AudioFXOthers");
    }
    public void SetAllAudioEffects()
    {
        if (PlayerPrefs.HasKey("Sound")) mixer.audioMixer.SetFloat("SoundVolume", PlayerPrefs.GetFloat("Sound"));
        else
        {
            PlayerPrefs.SetFloat("Sound", 0);
            mixer.audioMixer.SetFloat("SoundVolume", PlayerPrefs.GetFloat("Sound"));
        }

        if (PlayerPrefs.HasKey("AudioFX")) mixer.audioMixer.SetFloat("FXVolume", PlayerPrefs.GetFloat("AudioFX"));
        else
        {
            PlayerPrefs.SetFloat("AudioFX", 0);
            mixer.audioMixer.SetFloat("FXVolume", PlayerPrefs.GetFloat("AudioFX"));
        }

        if (PlayerPrefs.HasKey("AudioFXOthers")) mixer.audioMixer.SetFloat("FXOthersVolume", PlayerPrefs.GetFloat("AudioFXOthers"));
        else
        {
            PlayerPrefs.SetFloat("AudioFXOthers", 0);
            mixer.audioMixer.SetFloat("FXOthersVolume", PlayerPrefs.GetFloat("AudioFXOthers"));
        }
    }
    void Start()
    {
        if (SetAllSlidersbool)
        {
            SetAllSliders();
        }
        SetAllAudioEffects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
