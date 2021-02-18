using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class CustomTrack : MonoBehaviour
{
    public AudioClip Track1;

    public bool clipLoaded;

    public bool playingClip;
    
    // Start is called before the first frame update
    async void Start()
    {
        var path = Path.Combine(Application.dataPath, "Your_Music", "Track1.wav");
        Track1 = await LoadClip(path);
        
    }

    async Task<AudioClip> LoadClip(string path)
    {
        AudioClip audioClip = null;
        // Placehodler
        using (UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
        {
            unityWebRequest.SendWebRequest();

            try
            {
                while (!unityWebRequest.isDone)
                {
                    await Task.Delay(5);
                }

                if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
                {
                    Debug.Log("Request error " + unityWebRequest.error + " " + unityWebRequest.url);
                    
                }
                else
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(unityWebRequest);
                    audioClip = clip;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        if (audioClip != null)
        {
            clipLoaded = true;
        }
        return audioClip;
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (clipLoaded && !playingClip)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                var audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.clip = Track1;
                playingClip = true;
                audioSource.Play();
            }
        }
    }
}
