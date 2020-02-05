using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

public class Display : MonoBehaviour
{
    public FileSelectInfo[] fileInfos;
    public int currentIndex;
    [SerializeField]
    private VideoPlayer videoPlayer;

    public void ShowNextFile()
    {
        videoPlayer.Stop();
        currentIndex = (currentIndex + 1) % fileInfos.Length;
        this.transform.GetComponentInChildren<TextMeshProUGUI>().text = fileInfos[currentIndex].videoName;
        videoPlayer.clip = fileInfos[currentIndex].videoClip;
        //Change video
        videoPlayer.Play();
    }

    public void ShowPreviousFile()
    {
        videoPlayer.Stop();
        currentIndex = (currentIndex - 1);
        if (currentIndex == -1) currentIndex = fileInfos.Length-1;
        this.transform.GetComponentInChildren<TextMeshProUGUI>().text = fileInfos[currentIndex].videoName;
        videoPlayer.clip = fileInfos[currentIndex].videoClip;
        //Change video
        videoPlayer.Play();
    }
}
