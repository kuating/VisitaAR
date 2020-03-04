using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

public class FileSelectInfo : MonoBehaviour
{
    public int videoIndex;
    public string videoName;
    public VideoClip videoClip;
    public GameObject display;
    private Manager manager;
    private VideoPlayer videoPlayer;

    public void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        display = manager.screens[3];
        videoPlayer = GameObject.FindGameObjectWithTag("VideoPlayer").GetComponent<VideoPlayer>();
    }

    public void Click()
    {
        display.GetComponentInChildren<TextMeshProUGUI>().text = videoName;
        display.GetComponent<Display>().currentIndex = videoIndex;
        videoPlayer.clip = videoClip;
        manager.ChangeScreen(3);
        videoPlayer.Stop();
    }
}
