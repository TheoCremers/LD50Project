using UnityEngine;
using System.Linq;

public static class AudioManager
{   
    public static void PlaySFX(SFXType sfxType)
    {
        if (sfxType == SFXType.None) { return; }

        // TODO: Implement object pooling for SFX
        var sfxGameObject = new GameObject("SFX");
        var audioSource = sfxGameObject.AddComponent<AudioSource>();
        var metadata = GetSFXMetadata(sfxType);
        audioSource.clip = metadata.AudioClip;

        // TODO: Implement global volume setting
        audioSource.volume = metadata.BaseVolume;
        audioSource.Play();     
        Object.Destroy(sfxGameObject, audioSource.clip.length);
    }

    public static void PlaySFX (SFXType sfxType, GameObject target)
    {
        if (sfxType == SFXType.None) { return; }

        // TODO: Implement object pooling for SFX
        var audioSource = target.AddComponent<AudioSource>();
        var metadata = GetSFXMetadata(sfxType);
        audioSource.clip = metadata.AudioClip;

        // TODO: Implement global volume setting
        audioSource.volume = metadata.BaseVolume;
        audioSource.spatialBlend = 1f;
        audioSource.maxDistance = 30f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.Play();
        Object.Destroy(audioSource, audioSource.clip.length);
    }

    public static void PlaySFXVariation (SFXType sfxType)
    {
        if (sfxType == SFXType.None) { return; }

        // TODO: Implement object pooling for SFX
        var sfxGameObject = new GameObject("SFX");
        var audioSource = sfxGameObject.AddComponent<AudioSource>();
        var metadata = GetSFXMetadata(sfxType);
        audioSource.clip = metadata.AudioClip;

        // TODO: Implement global volume setting
        audioSource.volume = metadata.BaseVolume;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        Object.Destroy(sfxGameObject, audioSource.clip.length);
    }

    public static void PlaySFXVariation (SFXType sfxType, GameObject target)
    {
        if (sfxType == SFXType.None) { return; }

        // TODO: Implement object pooling for SFX
        var audioSource = target.AddComponent<AudioSource>();
        var metadata = GetSFXMetadata(sfxType);
        audioSource.clip = metadata.AudioClip;

        // TODO: Implement global volume setting
        audioSource.volume = metadata.BaseVolume;
        audioSource.spatialBlend = 1f;
        audioSource.maxDistance = 30f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        Object.Destroy(audioSource, audioSource.clip.length);
    }

    public static AudioSource AddAudioSource(SFXType sfxType, GameObject target)
    {
        var audioSource = target.AddComponent<AudioSource>();
        var metadata = GetSFXMetadata(sfxType);
        audioSource.clip = metadata.AudioClip;
        audioSource.volume = metadata.BaseVolume;
        audioSource.spatialBlend = 1f;
        return audioSource;
    }

    public static void PlayBGM(BGMType bgmType)
    {
        var metadata =  GetBGMMetadata(bgmType);
        var bgmGameObject = GameObject.Find("BGM");
        if (bgmGameObject != null) 
        {
            ChangeBGM(bgmGameObject, metadata);
        }
        else 
        {
            bgmGameObject = new GameObject("BGM");
            // This is a bit ugly ngl
            GameAssets.Instance.Persist(bgmGameObject);
            var audioSource = bgmGameObject.AddComponent<AudioSource>();            
            audioSource.clip = metadata.AudioClip;

            // TODO: Implement global volume setting
            audioSource.volume = metadata.BaseVolume;
            audioSource.loop = true;
            audioSource.Play();  
        }
    }

    public static void StopBGM() 
    {
        var bgmGameObject = GameObject.Find("BGM");
        if (bgmGameObject != null) 
        {
            var audioSource = bgmGameObject.GetComponent<AudioSource>();   
            if (audioSource != null) 
            {
                audioSource.Stop();
            }
        }
    }

    public static void TogglePauseBGM() 
    {
        var bgmGameObject = GameObject.Find("BGM");
        if (bgmGameObject != null) 
        {
            var audioSource = bgmGameObject.GetComponent<AudioSource>();   
            if (audioSource != null) 
            {
                if (audioSource.isPlaying) 
                {
                    audioSource.Pause();
                } 
                else
                {
                    audioSource.UnPause();
                }
            }
        }
    }

    private static void ChangeBGM(GameObject bgmGameObject, BGMMetadata bgmMetadata)
    {
        var audioSource = bgmGameObject.GetComponent<AudioSource>();   
        if (audioSource.clip == bgmMetadata.AudioClip) 
        {
            return;
        }
        audioSource.Stop();
        audioSource.clip = bgmMetadata.AudioClip;

        // TODO: Implement global volume setting
        audioSource.volume = bgmMetadata.BaseVolume;
        audioSource.loop = true;
        audioSource.Play();  
    }

    private static SFXMetadata GetSFXMetadata(SFXType sfxType) 
    {
        var sfxMetadata = GameAssets.Instance.SFXMetadata.FirstOrDefault(x => x.SfxType == sfxType);
        if (sfxMetadata != null) 
        {
            return sfxMetadata;
        } 
        else 
        {
            Debug.LogWarning("Missing SFX Metadata " + sfxType);
            return null;
        }
    }

    private static BGMMetadata GetBGMMetadata(BGMType bgmType) 
    {
        var bgmMetadata = GameAssets.Instance.BGMMetadata.FirstOrDefault(x => x.BGMType == bgmType);
        if (bgmMetadata != null) 
        {
            return bgmMetadata;
        } 
        else 
        {
            Debug.LogWarning("Missing BGM Metadata " + bgmType);
            return null;
        }
    }
}
