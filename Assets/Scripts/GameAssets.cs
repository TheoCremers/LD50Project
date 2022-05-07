using UnityEngine;
using System.Collections.Generic;

public class GameAssets : MonoBehaviour
{
    [SerializeField] public List<SFXAudioClip> SFXAudioClips = new List<SFXAudioClip>();

    [SerializeField] public List<BGMAudioClip> BGMAudioClips = new List<BGMAudioClip>();

    private static GameAssets _instance;

    public static GameAssets Instance 
    {
        get 
        {
            if (_instance == null) 
            {
                _instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }
            return _instance;
        }
    }
}
