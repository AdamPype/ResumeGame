using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioClip[] _sounds;
    [SerializeField] private string[] _names;
    private List<AudioSource> _audioSources = new List<AudioSource>();
    
    /// <summary>
    /// Plays a sound. If there are more with the same name it will choose one at random.
    /// </summary>
    /// <param name="soundName">Name of the sound.</param>
    /// <param name="loop">Does the sound loop?</param>
    /// <param name="spatial">Is the sound 2D or 3D? (0 - 1)</param>
    public void Play(string soundName, bool loop = false, float spatial = 1, float volume = 1)
        {
        //if there are more sounds with the same name, it will choose one at random
        int[] indexes = GetAllIndexes(_names, soundName);
        int indexToChoose = indexes[UnityEngine.Random.Range(0, indexes.Length)];
        
        //makes a new audiosource component on the object and gives it the right settings
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.clip = _sounds[indexToChoose];
        audio.spatialBlend = spatial;
        audio.volume = volume;
        if (loop)
            {
            audio.loop = true;
            }
        
        //plays the audio
        audio.Play();
        //add it to the active audio list
        _audioSources.Add(audio);

        }

    private int[] GetAllIndexes(string[] array, string val)
        {
        List<int> indexes = new List<int>();
        for (int i = 0; i < array.Length; i++)
            {
            if (array[i] == val)
                {
                indexes.Add(i);
                }
            }
        return indexes.ToArray();
        }

    //this is for UI
    public void PlaySimple(string soundName)
        {
        //makes a new audiosource component on the object and gives it the right settings
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.clip = _sounds[Array.IndexOf(_names, soundName)];
        audio.spatialBlend = 0;
        //plays the audio
        audio.Play();
        //add it to the active audio list
        _audioSources.Add(audio);
        }

    /// <summary>
    /// Stops all playing instances of a sound.
    /// </summary>
    /// <param name="soundName">The name of the sound to stop.</param>
    public void Stop(string soundName)
        {
        //check if there is audio playing
        if (_audioSources.Count > 0)
            {
            //then stop all the sounds that match
            for (int i = 0; i < _audioSources.Count; i++)
                {
                if (_audioSources[i].clip == _sounds[Array.IndexOf(_names, soundName)])
                    {
                    _audioSources[i].Stop();
                    Destroy(_audioSources[i]);
                    _audioSources.RemoveAt(i);
                    }
                }
            }
        }

    /// <summary>
    /// Plays a random sound from a given array of sounds.
    /// </summary>
    /// <param name="soundNames">An array with all the names of the sounds to choose from.</param>
    public void PlayRandom(string[] soundNames, bool loop = false, float spatial = 1, float volume = 1)
        {
        Play(soundNames[UnityEngine.Random.Range(0, soundNames.Length)], loop, spatial, volume);
        }

    /// <summary>
    /// Stops all sounds.
    /// </summary>
    public void StopAll()
        {
        //stop every sound
        foreach (AudioSource audio in _audioSources)
            {
            audio.Stop();
            Destroy(audio);
            }
        //clear list
        _audioSources.Clear();
        }

    /// <summary>
    /// Checks whether the sound is currently being played.
    /// </summary>
    /// <param name="soundName">Name of the sound.</param>
    /// <returns></returns>
    public bool IsPlaying(string soundName)
        {
        bool isPlaying = false;
        foreach (AudioSource audio in _audioSources)
            {
            //check if sound matches
            if (audio.clip == _sounds[Array.IndexOf(_names, soundName)])
                {
                isPlaying = audio.isPlaying;
                }
            }
        return isPlaying;
        }

    private void Update()
        {
        //if an audio has stopped playing, remove the component
        if (_audioSources.Count > 0)
            {
            for (int i = 0; i < _audioSources.Count; i++)
                {
                AudioSource audio = _audioSources[i];
                if (!audio.isPlaying)
                    {
                    _audioSources.Remove(audio);
                    Destroy(audio);
                    }
                }
            }
        }
    }
