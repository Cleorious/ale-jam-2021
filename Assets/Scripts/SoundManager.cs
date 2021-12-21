using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    MainMenu,
    Gameplay,
    Instructions,
    Cooking,
    
    BGM_Count
}

public enum SFX
{
    Cooking,
    Clock,
    Lose,
    Correct,
    Win,
    Male1,
    Male2,
    Male3,
    
    SFX_Count
}

public class SoundManager
{
    static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new SoundManager();
            }

            return _instance;
        }
        
    }
    GameManager gameManager;
    
    AudioSource[] bgmSources;
    AudioSource[] sfxSources;
    Dictionary<SFX, AudioSource> sfxSourceDict;
    Dictionary<BGM, AudioSource> bgmSourceDict;

    const float BGM_VOLUME = 0.4f;
    const string BGM_PATH = "BGM/";
    const string SFX_PATH = "SFX/";

    GameObject audioSourceHolder;
    
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        
        audioSourceHolder = new GameObject("Audio");
        InitBGM();
        InitSFX();
    }

    public void InitBGM()
    {
        int bgmCount = (int) BGM.BGM_Count;
        
        bgmSources = new AudioSource[bgmCount];
        bgmSourceDict = new Dictionary<BGM, AudioSource>();
        
        for(int i = 0; i < bgmCount; i++)
        {
            bgmSources[i] = audioSourceHolder.AddComponent<AudioSource>();
            bgmSources[i].playOnAwake = false;
            bgmSources[i].loop = true;
            bgmSources[i].spatialBlend = 0;
            bgmSources[i].volume = BGM_VOLUME;
            bgmSources[i].clip = GetAudioClip(BGM_PATH + "BGM_" + ((BGM)i));

            bgmSourceDict[(BGM)i] = bgmSources[i];
        }
    }

    public void InitSFX()
    {
        int sfxCount = (int)SFX.SFX_Count;
        
        sfxSources = new AudioSource[sfxCount];
        sfxSourceDict = new Dictionary<SFX, AudioSource>();
        
        for(int i = 0; i < sfxCount; i++)
        {
            sfxSources[i] = audioSourceHolder.AddComponent<AudioSource>();
            sfxSources[i].playOnAwake = false;
            sfxSources[i].loop = false;
            sfxSources[i].spatialBlend = 0;
            sfxSources[i].clip = GetAudioClip(SFX_PATH + "SFX_" + ((SFX)i));

            sfxSourceDict[(SFX)i] = sfxSources[i];
        }
    }

    public void PlayBGM(BGM bgm, float volume = 0.2f, bool turnOffOthers = true)
    {
        foreach(KeyValuePair<BGM,AudioSource> audioSource in bgmSourceDict)
        {
            if(audioSource.Key == bgm)
            {
                audioSource.Value.volume = volume;
                audioSource.Value.Play();
            }
            else
            {
                if(turnOffOthers)
                {
                    audioSource.Value.Stop();
                }
            }
        }
    }
    
    public void StopAllBGM()
    {
        foreach(KeyValuePair<BGM,AudioSource> audioSource in bgmSourceDict)
        {
            audioSource.Value.Stop();
        }
    }
    
    public void PlaySfx(SFX sfx, float volume = 1)
    {
        AudioSource source = sfxSourceDict[sfx];
        source.volume = volume;
        source.Play();
    }
    
    AudioClip GetAudioClip(string path)
    {
        Debug.Log("Audio/" + path);
        AudioClip result = Resources.Load<AudioClip>("Audio/" + path);
        return result;
    }
    
    public SFX GetRandomMaleSFX()
    {
        SFX ret = SFX.Male1;

        int offset = Random.Range(0, 3);

        int enumIndex = (int)SFX.Male1 + offset;

        ret = (SFX)enumIndex;

        return ret;
    }
}
