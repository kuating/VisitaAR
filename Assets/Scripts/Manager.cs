using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class Manager : MonoBehaviour
{
    public int currentScreen = 0;
    public int lastScreen = 0;

    public GameObject[] screens;
    public bool vuforiaCard;
    public GameObject imageTarget;
    [SerializeField]
    private VuforiaCamera vuforiaCamera;
    [SerializeField]
    private VideoLoader videoLoader;
    [SerializeField]
    private VideoPlayer videoPlayer;

    private bool gambiarra = false;

    // Start is called before the first frame update
    void Start()
    {
        string tempPath = Application.persistentDataPath + "/" + "DownloadTemp";
        if (Directory.Exists(tempPath))
        {
            Directory.Delete(tempPath);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScreen <= 2)
        {
            if (vuforiaCard) { vuforiaCard = false; videoPlayer.enabled = false; }
        }
        else
        {
            if (!vuforiaCard) { vuforiaCard = true; videoPlayer.enabled = true; }
        }

        if (currentScreen == 1)
        {
            if (vuforiaCamera.isDetecting && !gambiarra)
            {
                gambiarra = true;
                
            }
            if (!(vuforiaCamera.isDetecting) && gambiarra)
            {
                gambiarra = false;
                videoLoader.mUrl = vuforiaCamera.url;
                videoLoader.DownloadAndConfigure();
                ChangeScreen(2);

            }
        }
    }

    public void ChangeScreen(int newScreen)
    {
        if (newScreen == 1) vuforiaCamera.DetectQrCode();  
        lastScreen = currentScreen;
        screens[currentScreen].SetActive(false);
        screens[newScreen].SetActive(true);
        currentScreen = newScreen;
    }

    public void OpenSite(){
        Application.OpenURL("https://lab3d.coppe.ufrj.br/");
    }
}
