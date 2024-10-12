using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RickAndMemory.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        private AudioSource source;

        private void Awake()
        {
            if(Instance != null) 
            {
                Destroy(this);
                return;
            }

            Instance = this;
            source = GetComponent<AudioSource>();
        }

        public void PlayClip(AudioClip clip, float pinch = 1, float delay = 0) 
        {
            source.clip = clip;
            source.pitch = pinch;

            if (delay > 0)
                source.PlayDelayed(delay);
            else
                source.Play();
        }
    }
}
