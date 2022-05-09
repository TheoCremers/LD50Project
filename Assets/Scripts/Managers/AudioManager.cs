using UnityEngine;
using System.Linq;

public static class AudioManager
{
    public static void PlaySFX(SFXType sfxType)
    {
        // TODO: Implement object pooling for SFX
        var sfxGameObject = new GameObject("SFX");
        var audioSource = sfxGameObject.AddComponent<AudioSource>();
        var metadata = GetSFXMetadata(sfxType);
        audioSource.clip = metadata.AudioClip;

        // TODO: Implement volume setting
        audioSource.volume = metadata.BaseVolume;
        audioSource.Play();     
        Object.Destroy(sfxGameObject, audioSource.clip.length);
    }

    public static void PlayBGM(BGMType bgmType)
    {
        // TODO: Replace currently playing BGM
        var bgmGameObject = new GameObject("BGM");
        var audioSource = bgmGameObject.AddComponent<AudioSource>();
        var metadata =  GetBGMMetadata(bgmType);
        audioSource.clip = metadata.AudioClip;
        audioSource.volume = metadata.BaseVolume;
        audioSource.loop = true;
        audioSource.Play();  
    }

    public static void StopBGM() 
    {
        // TODO: Stop currently playing BGM
        throw new System.NotImplementedException();
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
            Debug.LogError("Missing SFX Metadata " + sfxType);
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
            Debug.LogError("Missing BGM Metadata " + bgmType);
            return null;
        }
    }
}
