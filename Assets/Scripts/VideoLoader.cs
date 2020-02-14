﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class VideoLoader : MonoBehaviour
{

    //https://drive.google.com/file/d//view?usp=sharing

    //1LKfWcpm1jP6XzcpA8LsxqWX7qMYrThXW
    //https://drive.google.com/uc?export=download&id=
    //https://drive.google.com/uc?export=download&id=1LKfWcpm1jP6XzcpA8LsxqWX7qMYrThXW

    public string mUrl = ""; //https://drive.google.com/uc?export=download&id=14DX2nQuyaiyeV3SguE2D82TkGta7oVjJ
    public string fileName = "";
    public bool mClearChache = false;
    private float mLoadFill = 0f;
    private double progressCheck;
    private int frameCounter;
    private int timeoutFrameLimit = 500;
    private Dictionary<string,string> loadedUrls;

    [SerializeField]
    private Image mDisk = null;
    private VideoPlayer mVideoPlayer = null;
    private AssetBundle mBundle = null;
    [SerializeField]
    private GameObject selectPrefab = null;
    [SerializeField]
    private GameObject scrollContent = null;
    [SerializeField]
    private GameObject loadScreen = null;
    [SerializeField]
    private GameObject errorScreen = null;
    private Display display;

    private UnityWebRequest request = null;


    void Awake()
    {
        display = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().screens[3].GetComponent<Display>();
        mVideoPlayer = GetComponent<VideoPlayer>();
        Caching.compressionEnabled = false;
        if (mClearChache) Caching.ClearCache();
        loadedUrls = new Dictionary<string, string>();
    }

    void Update()
    {
        
        mDisk.fillAmount = mLoadFill;
    }

    public void DownloadAndConfigure()
    {
        loadScreen.SetActive(true);
        StartCoroutine(DownloadAndPlay());
    }

    private IEnumerator DownloadAndPlay()
    {
        if (loadedUrls.ContainsKey(mUrl.Split('=')[2]))
        {
            Debug.Log(mUrl.Split('=')[2]);
            Debug.Log("Ja contem a url " + mUrl.Split('=')[2]);

            mBundle = AssetBundle.LoadFromFile(loadedUrls[mUrl.Split('=')[2]]);
            loadScreen.SetActive(false);
        }
        else
        {
            yield return GetBundle();

            if (!mBundle)
            {
                Debug.Log("A url " + mUrl + " tem um mBundle nulo");
                Debug.Log("Falha no Download do Bundle");
                errorScreen.SetActive(true);
                yield break;
            }
            SaveToFolder(request, mUrl.Split('=')[2]);
            //loadedUrls.Add(mUrl.Split('=')[2], mBundle);
        }

        
        display.fileInfos = new FileSelectInfo[mBundle.GetAllAssetNames().Length];

        for (int i = 0; i < mBundle.GetAllAssetNames().Length; i++)
        {
            Debug.Log((i+1) + " : " + mBundle.GetAllAssetNames()[i]);
            GameObject newFileSelect = Instantiate(selectPrefab, scrollContent.transform);
            newFileSelect.GetComponent<RectTransform>().localPosition = new Vector2(0,  - 45 - (100 * i));

            string[] temp = mBundle.GetAllAssetNames()[i].Split('/');
            //for (int j = 0; j < temp.Length; j++) { Debug.Log(temp[j]); }
            newFileSelect.GetComponentInChildren<Button>().transform.GetComponentInChildren<TextMeshProUGUI>().text = temp[temp.Length-1].Split('.')[0];
            newFileSelect.GetComponent<FileSelectInfo>().videoName = temp[temp.Length - 1].Split('.')[0];
            newFileSelect.GetComponent<FileSelectInfo>().videoIndex = i;
            newFileSelect.GetComponent<FileSelectInfo>().videoClip = mBundle.LoadAsset<VideoClip>(mBundle.GetAllAssetNames()[i]);
            
            display.fileInfos[i] = newFileSelect.GetComponent<FileSelectInfo>();
        }
        /*
        VideoClip newVideoClip = mBundle.LoadAsset<VideoClip>(mBundle.GetAllAssetNames()[0]);
        mVideoPlayer.clip = newVideoClip;
        mVideoPlayer.Play();
        Debug.Log("Saiu!");//
        //while (true) if(mVideoPlayer.isPaused) break;
        Debug.Log("Parou");//

        mVideoPlayer.targetTexture.Release();*/
    }

    private IEnumerator GetBundle()
    {
        //WWW request = WWW.LoadFromCacheOrDownload(mUrl, 0);
        /*UnityWebRequest*/ request = UnityWebRequestAssetBundle.GetAssetBundle(mUrl);
        request.downloadHandler = new CustomAssetBundleDownloadHandler(Application.persistentDataPath + "/" + mUrl.Split('=')[2] + ".assetbundle");
        request.chunkedTransfer = false;
        Debug.Log("Download Queued");
        /*yield return*/ request.SendWebRequest();
        Debug.Log("Download Started");
        progressCheck = 0; frameCounter = 0;
        while (!request.isDone)
        {
            Debug.Log(request.GetResponseHeaders());
            //Debug.Log(((CustomAssetBundleDownloadHandler)request.downloadHandler).ShowProgress());
            mLoadFill = request.downloadProgress;
            if (request.downloadedBytes == progressCheck) frameCounter++;
            else frameCounter = 0;
            progressCheck = request.downloadedBytes;

            if(frameCounter > timeoutFrameLimit) { Debug.LogError("Timeout"); break; }

            yield return null;
        }
        Debug.Log("Download Ended");
        mLoadFill = 0f;
        loadScreen.SetActive(false);

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            mBundle = null;
        }

        else
        {
            mBundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + mUrl.Split('=')[2] + ".assetbundle"); //((DownloadHandlerAssetBundle)request.downloadHandler).assetBundle;
            //Debug.Log("Download sucedido");
        } 

        yield return null;

    }
    public void ClearCache() {
        Caching.ClearCache();
    }

    private void SaveToFolder(UnityWebRequest request, string bundleName)
    {
        string cachedAssetBundle = Application.persistentDataPath + "/" + bundleName + ".assetbundle";
        loadedUrls.Add(bundleName, cachedAssetBundle);
        //request.downloadHandler = new CustomAssetBundleDownloadHandler(cachedAssetBundle);
        //System.IO.FileStream cache = new System.IO.FileStream(cachedAssetBundle, System.IO.FileMode.Create);
        //cache.Write(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        //cache.Close();
        //iOS.Device.SetNoBackupFlag(cachedAssetBundle);

        Debug.Log("Cache saved: " + cachedAssetBundle);
    }
}

/*
 UnityWebRequest www = UnityWebRequest.Get(bundleURL + ".manifest");

// create empty hash string
Hash128 hashString = (default(Hash128));
     
     if (www.downloadHandler.text.Contains ("ManifestFileVersion"))  {
     var hashRow = www.downloadHandler.text.ToString().Split("\n".ToCharArray())[5];
hashString = Hash128.Parse(hashRow.Split (':') [1].Trim());
     
     if (hashString.isValid == true) {
     if (Caching.IsVersionCached (bundleURL, hashString) == true) {
     Debug.Log("Bundle with this hash is already cached!... " + hashString);
     } else {
     string writepath = Application.persistentDataPath + "/" + hashString;
www.downloadHandler = new CustomAssetBundleDownloadHandler(writepath);
Debug.Log("No cached version founded for this hash.." + hashString);
     }
    */
