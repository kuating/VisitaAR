using UnityEngine;
using System;
using System.Collections;

//using Vuforia;

using System.Threading;

using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using Vuforia;

[AddComponentMenu("System/VuforiaCamera")]
public class VuforiaCamera : MonoBehaviour
{
    [SerializeField]
    private VideoLoader videoLoader;
    [SerializeField]
    private Manager manager;

    public string url;

    private bool cameraInitialized;
    public bool isDetecting = false;

    private BarcodeReader barCodeReader;
    private bool isDecoding = false;

    void Start()
    {        
        barCodeReader = new BarcodeReader();
        StartCoroutine(InitializeCamera());
    }

    private IEnumerator InitializeCamera()
    {
        // Waiting a little seem to avoid the Vuforia's crashes.
        yield return new WaitForSeconds(1.25f);

        var isFrameFormatSet = CameraDevice.Instance.SetFrameFormat(PIXEL_FORMAT.GRAYSCALE, true);
        Debug.Log(String.Format("FormatSet : {0}", isFrameFormatSet));

        // Force autofocus.
        var isAutoFocus = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        if (!isAutoFocus)
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
        }
        Debug.Log(String.Format("AutoFocus : {0}", isAutoFocus));
        cameraInitialized = true;
    }

    private void Update()
    {
        if (isDetecting)
        {
            Debug.Log("camera init = " + cameraInitialized + " and is decoding = " + isDecoding);
            if (cameraInitialized && !isDecoding)
            {
                try
                {
                    var cameraFeed = CameraDevice.Instance.GetCameraImage(PIXEL_FORMAT.GRAYSCALE);

                    if (cameraFeed == null)
                    {
                        Debug.Log("cameraFeed = null");
                        return;
                    }
                    ThreadPool.QueueUserWorkItem(new WaitCallback(DecodeQr), cameraFeed);

                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
    }

    private void DecodeQr(object state){
        isDecoding = true;
        var cameraFeed = (Image)state;
        Debug.Log("a");
        if (barCodeReader.Decode(cameraFeed.Pixels, cameraFeed.BufferWidth, cameraFeed.BufferHeight, RGBLuminanceSource.BitmapFormat.RGB24) == null) ;
        var data = barCodeReader.Decode(cameraFeed.Pixels, cameraFeed.BufferWidth, cameraFeed.BufferHeight, RGBLuminanceSource.BitmapFormat.RGB24);
        Debug.Log("b");
        if (data != null)
        {
            // QRCode detected.
            Debug.Log(data.BarcodeFormat);
            Debug.Log(data.Text);
            this.url = "https://drive.google.com/uc?export=download&id=" + data.Text;
            isDecoding = false;
            isDetecting = false;
        }
        else
        {
            isDecoding = false;
            Debug.Log("No QR code detected !");
        }
        Debug.Log("c");

    }

    public void DetectQrCode()
    {
        isDetecting = true;
    }

    public void CancelDetection()
    {
        isDetecting = false;
    }
}