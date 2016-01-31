using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Audio : MonoBehaviour
{

    private Dictionary<string, AudioSource> AudioSources;

	// Use this for initialization
	void Start () 
	{
		if (AudioSources == null)
		{
			AudioSources = new Dictionary<string, AudioSource>();
			
			AudioSource[] sources = gameObject.GetComponentsInChildren<AudioSource>();
			for (int i = 0; i < sources.Length; i++)
			{
				GameManager.AudioSources[sources[i].gameObject.name] = sources[i];
				DontDestroyOnLoad(sources[i]);
			}
		}
    }

    public static AudioSource GetRandomBubbles()
    {
        return Random.value < 0.5 ? GameManager.AudioSources["Bubbles1"] : GameManager.AudioSources["Bubbles2"];
    }

    public static AudioSource GetRandomFillingSound()
    {
        int randomSelector = Random.Range(0, 5);
        List<AudioSource> FillingSounds = new List<AudioSource>
                                              {
                                                  GameManager.AudioSources["Bubbles1"],
                                                  GameManager.AudioSources["Bubbles2"],
                                                  GameManager.AudioSources["GlassClink"],
                                                  GameManager.AudioSources["GlassClink"],   // want more often
                                                  GameManager.AudioSources["GlassDown"],
                                                  GameManager.AudioSources["GlassDown"]    // want more often
                                              };
        return FillingSounds[randomSelector];
    }

    public static void StartGameMusic()
    {
        GameManager.AudioSources["BackgroundPiano"].Stop();
        GameManager.AudioSources["Score"].Play();
    }
}
