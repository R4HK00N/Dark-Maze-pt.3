using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAudio : MonoBehaviour
{
    public AudioClip[] playerAudioClips;
    public AudioClip[] enemyAudioClips;

    public AudioSource playerAudioSource;
    public AudioSource enemyAudioSource;
    public AudioSource gateAudioSource;
    public void SwitchPlayerAudioClip(int index)
    {
        playerAudioSource.clip = playerAudioClips[index];
        playerAudioSource.Play();
    }
    public void SwitchEnemyAudioClip(int index)
    {
        enemyAudioSource.clip = enemyAudioClips[index];
        enemyAudioSource.Play();
    }
    public void PlayGateSound()
    {
        gateAudioSource.Play();
    }
    public void StopPlayerAudio()
    {
        playerAudioSource.Stop();
    }
    public void StopEnemyAudio()
    {
        enemyAudioSource.Stop();
    }
}
