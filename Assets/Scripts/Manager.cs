using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool gambiarra = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentScreen <= 2)
        {
            //if (vuforiaCard) { vuforiaCard = false; imageTarget.SetActive(false); }
        }
        else
        {
            //if (!vuforiaCard) { vuforiaCard = true; imageTarget.SetActive(true); }
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
}
