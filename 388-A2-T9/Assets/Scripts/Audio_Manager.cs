using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    private static Audio_Manager instance;
    public static Audio_Manager Instance { get { return instance; } }
    //Fanfare Source
    public AudioSource fanfareAudioSource;
    //Main Audio Sources
    public bool playingMainAudio;
    public AudioSource mainAudioSource;
    private float mainAudioTime;
    public AudioSource combatAudioSource;
    private float combatAudioTime;

    public AudioClip mainAudioMusic;
    public AudioClip combatAudioMusic;
    [Range(0f, 1f)]
    public float audioMaxVolume = 1.0f;

    float fadeRate = 0.8f;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InitialSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitialSetup()
    {
        ResetToOriginalMusic(true, true);
        mainAudioSource.volume = audioMaxVolume;
        mainAudioSource.Play();
        combatAudioSource.volume = 0;
        combatAudioSource.Play();
    }
    public void EnterCombat()
    {
        playingMainAudio = false;
        if (combatAudioSource.clip != combatAudioMusic)
        {
            combatAudioTime = 0;
        }
        combatAudioSource.time = combatAudioTime;
        mainAudioTime = mainAudioSource.time;
        StartCoroutine(transitionMusic(mainAudioSource, combatAudioSource));
    }
    IEnumerator transitionMusic(AudioSource fromAudio, AudioSource toAudio)
    {
        StartCoroutine(fadeOut(fromAudio));
        StartCoroutine(fadeIn(toAudio));
        yield return null;
    }
    IEnumerator fadeOut(AudioSource fromAudio)
    {
        while (fromAudio.volume > 0.1)
        {
            fromAudio.volume = Mathf.Lerp(fromAudio.volume, 0.0f, fadeRate * Time.deltaTime);
            yield return null;
        }
        fromAudio.volume = 0.0f;
    }
    IEnumerator fadeIn(AudioSource toAudio)
    {
        while (toAudio.volume < (audioMaxVolume * 0.9f))
        {
            toAudio.volume = Mathf.Lerp(toAudio.volume, 1.0f, fadeRate * Time.deltaTime);
            yield return null;
        }
        toAudio.volume = audioMaxVolume;
    }
    public void LeaveCombat()
    {
        playingMainAudio = true;
        if (mainAudioSource.clip != mainAudioMusic)
        {
            mainAudioTime = 0;
        }
        mainAudioSource.time = mainAudioTime;
        combatAudioTime = combatAudioSource.time;
        StartCoroutine(transitionMusic(combatAudioSource, mainAudioSource));
    }

    public void PlayFanfare(AudioClip audioClip)
    {
        StartCoroutine(playFanfare(audioClip));
    }
    IEnumerator playFanfare(AudioClip audioClip)
    {
        float mainAudioVol = mainAudioSource.volume;
        float combatAudioVol = combatAudioSource.volume;
        fanfareAudioSource.clip = audioClip;
        fanfareAudioSource.Play();
        while (fanfareAudioSource.isPlaying)
        {
            mainAudioSource.mute = true;
            combatAudioSource.mute = true;
            yield return null;
        }

        mainAudioSource.mute = false;
        combatAudioSource.mute = false;
    }

    public void ChangeMainMusic(AudioClip audioClip)
    {
        mainAudioTime = 0;
        mainAudioSource.clip = audioClip;
        mainAudioSource.Play();
        //StartCoroutine(changeMusic(mainAudioSource, audioClip));
    }
    public void ChangeCombatMusic(AudioClip audioClip)
    {
        combatAudioTime = 0;
        combatAudioSource.clip = audioClip;
        combatAudioSource.Play();
        //StartCoroutine(changeMusic(combatAudioSource, audioClip));
    }
    IEnumerator changeMusic(AudioSource source, AudioClip music)
    {
        StartCoroutine(fadeOut(source));
        yield return new WaitForSeconds(fadeRate);
        source.clip = music;
        StartCoroutine(fadeIn(source));
    }
    public void ResetToOriginalMusic(bool main, bool combat)
    {
        if (main)
        {
            mainAudioSource.clip = mainAudioMusic;
            mainAudioSource.Play();
        }
        if (combat)
        {
            combatAudioSource.clip = combatAudioMusic;
            mainAudioSource.Play();
        }
    }
}
