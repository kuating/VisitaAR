﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CustomAssetBundleDownloadHandler : DownloadHandlerScript
 {
     private string _targetFilePath;
     private Stream _fileStream;
 
     public CustomAssetBundleDownloadHandler(string targetFilePath)
         //: base(new byte[4096]) // use pre-allocated buffer for better performance
     {
         _targetFilePath = targetFilePath;
     }
 
     protected override bool ReceiveData(byte[] data, int dataLength)
     {
         // create or open target file
         if (_fileStream == null)
         {
             _fileStream = File.OpenWrite(_targetFilePath);
         }
 
         // write data to file
         _fileStream.Write(data, 0, dataLength);
 
         return true;
     }

    protected override void ReceiveContentLength(int contentLength)
    {
        Debug.LogError("ReceiveContentLength: " + contentLength);
    }

    protected override void CompleteContent()
     {
         // close and save
         _fileStream.Close();
     }

     public float ShowProgress()
    {
        return GetProgress();
    }

 }