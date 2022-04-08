using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioClip[] musicList;
    private int currentClip;
    private AudioSource source;

    public TextMeshPro ClipTime;
    public TextMeshPro SongTitle;
    public TextMeshPro MuteTitle;

    private int fulllength;
    private int playTime;
    private int seconds;
    private int minutes;


    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        //PlayMusic();
    }

    public void PlayMusic()
    {
        Debug.Log("Play Music");
        if (source.isPlaying)
        {
            return;
        }

        currentClip -= 1;

        if(currentClip < 0)
        {
            currentClip = musicList.Length - 1;
        }

        StartCoroutine("WaitForClipEnd");

    }

    public void PauseMusic()
    {
        Debug.Log("Pause Music");
        source.Stop();
        StopCoroutine("WaitForClipEnd");
    }
    public void MuteMusic()
    {
        Debug.Log("Mute Music");
        source.mute = !source.mute;
        if (source.mute)
        {
            MuteTitle.text = "unmute";
        }
        else
        {
            MuteTitle.text = "mute";
        }
    }

    IEnumerator WaitForClipEnd()
    {
        while (source.isPlaying)
        {
            playTime = (int)source.time;
            ShowTime();
            yield return null;
        }

        nextTitle();

    }

    public void nextTitle()
    {
        source.Stop();
        currentClip += 1;
        if(currentClip >= musicList.Length)
        {
            currentClip = 0;
        }

        source.clip = musicList[currentClip];
        
        source.Play();
        ShowTitle();

        StartCoroutine("WaitForClipEnd");
    }

    public void ShowTitle()
    {
        SongTitle.text = source.clip.name;
        fulllength = (int)source.clip.length;
    }

    public void ShowTime()
    {
        seconds = playTime % 60;
        minutes = (playTime / 60) % 60;
        ClipTime.text = minutes + ":" + seconds + "/" + (fulllength / 60) % 60 + ":" + fulllength % 60;
    }


    public void previousTitle()
    {
        Debug.Log("Prev");
        source.Stop();
        currentClip -= 1;
        if (currentClip < 0)
        {
            currentClip = musicList.Length - 1;
        }

        source.clip = musicList[currentClip];
        source.Play();

        ShowTitle();

        StartCoroutine("WaitForClipEnd");
    }

}
