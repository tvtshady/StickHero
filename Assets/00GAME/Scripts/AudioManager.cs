using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource gameMusic;
    [SerializeField] AudioSource[] chanels;
    int chanelID = 0;

    [SerializeField] AudioClip[] audioClips;


    private void Start()
    {
        gameMusic.Play();
    }

    public void PlaySound(AUDIO_TYPE type)
    {
        if (type != AUDIO_TYPE.GROW)
        {
            chanelID++;
            if (chanelID >= chanels.Length) chanelID = 0;
            chanels[chanelID].clip = audioClips[(int)type];
            chanels[chanelID].Play();
        }
        else
        {
            chanels[chanelID].clip = audioClips[(int)type];
            if (!chanels[chanelID].isPlaying)
                chanels[chanelID].Play();
        }
    }

    public enum AUDIO_TYPE
    {
        GROW,SCORE,PERFECT,WOODHIT,DIE,BUTTON,FALL,KICK,HIT,DIEENEMY
    }

}
