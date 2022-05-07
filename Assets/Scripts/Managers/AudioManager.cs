using UnityEngine;
using System.Linq;

public static class AudioManager
{
    public static void PlaySFX(SFXType sfxType)
    {
        // TODO: Implement object pooling for SFX
        var sfxGameObject = new GameObject("SFX");
        var audioSource = sfxGameObject.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(sfxType);
        audioSource.Play();     
        Object.Destroy(sfxGameObject, audioSource.clip.length);
    }

    public static void PlayBGM(BGMType bgmType)
    {
        // TODO: Replace currently playing BGM
        var bgmGameObject = new GameObject("BGM");
        var audioSource = bgmGameObject.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(bgmType);
        audioSource.loop = true;
        audioSource.Play();  
    }

    public static void StopBGM() 
    {
        // TODO: Stop currently playing BGM
        throw new System.NotImplementedException();
    }

    private static AudioClip GetAudioClip(SFXType sfxType) 
    {
        var sfxAudioClip = GameAssets.Instance.SFXAudioClips.FirstOrDefault(x => x.SfxType == sfxType);
        if (sfxAudioClip != null) 
        {
            return sfxAudioClip.AudioClip;
        } 
        else 
        {
            Debug.LogError("Missing SFX AudioClip " + sfxType);
            return null;
        }
    }

    private static AudioClip GetAudioClip(BGMType bgmType) 
    {
        var sfxAudioClip = GameAssets.Instance.BGMAudioClips.FirstOrDefault(x => x.BGMType == bgmType);
        if (sfxAudioClip != null) 
        {
            return sfxAudioClip.AudioClip;
        } 
        else 
        {
            Debug.LogError("Missing BGM AudioClip " + bgmType);
            return null;
        }
    }
}
