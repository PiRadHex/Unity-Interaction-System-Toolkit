using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : Interactable
{
    public bool isPauseButton = false;
    public new AudioClip audio;
    public List<AudioSource> sources;

    [Header("Audio Source Setting")]
    public bool applySetting = false;
    public float volume = 1f;
    public float pitch = 1f;
    public bool loop = false;

    private bool isPaused = false;

    public override void Interact(Transform interactingObjectTransform)
    {
        base.Interact(interactingObjectTransform);

        isPaused = !isPaused;

        foreach (AudioSource source in sources)
        {
            if (applySetting)
            {
                source.volume = volume;
                source.pitch = pitch;
                source.loop = loop;
            }
            

            if (isPauseButton)
            {
                if (source.isPlaying)
                {
                    source.Pause();
                }
                else
                {
                    isPaused = true;
                }
            }
            else
            {
                if (source.isPlaying)
                {
                    source.UnPause();
                }
                else
                {
                    source.clip = audio;
                    source.Play();
                }
            }
        }
        
    }
}
