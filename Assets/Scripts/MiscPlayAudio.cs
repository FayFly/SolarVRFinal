using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscPlayAudio : MonoBehaviour
{
    AudioSource audioSource;
    bool isPlaying = false;
    // Start is called before the first frame update
    void Start() {
        audioSource=transform.parent.parent.GetComponent<AudioSource>();
    }
    public void ToggleAudio() {
        if(!isPlaying) {
            PlayAudio();
            isPlaying = true;
        }
        else if (isPlaying) {
            PauseAudio();
            isPlaying = false;
        }
    }
    public void PlayAudio() {
        audioSource.Play();
    }
    public void PauseAudio() {
        audioSource.Pause();
    }
    public void StopAudio() {
        audioSource.Stop();
    }
}
