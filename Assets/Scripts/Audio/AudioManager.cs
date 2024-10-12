using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private List<AudioSource> playingSources = new List<AudioSource>();
        private Queue<AudioSource> notPlayingSources = new Queue<AudioSource>();

        private void Awake()
        {
            if(Instance != null) 
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        public void PlayClip(AudioClip clip, float pinch = 1, float delay = 0) 
        {
            AudioSource source = GetSource();
            playingSources.Add(source);

            source.clip = clip;
            source.pitch = pinch;

            if (delay > 0)
                source.PlayDelayed(delay);
            else
                source.Play();

            StartCoroutine(CheckForAudioFinished(source));
        }

        private AudioSource GetSource()
        {
            if(notPlayingSources.Count > 0)
            {
                return notPlayingSources.Dequeue();
            }

            GameObject sourceObject = new GameObject("source", typeof(AudioSource));
            sourceObject.transform.SetParent(transform);

            return sourceObject.GetComponent<AudioSource>();
        }

        private IEnumerator CheckForAudioFinished(AudioSource source) 
        {
            while (source.isPlaying) 
            {
                yield return null;
            }

            playingSources.Remove(source);
            notPlayingSources.Enqueue(source);
        }
    }
}
