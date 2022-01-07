using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class InitMenu : MonoBehaviour
{
    public string hostURL;
    public CustomAnimate dowloadDialog;
    [SerializeField] private DOTweenAnimation menuback;
    [SerializeField] private DOTweenAnimation loading;
    [SerializeField] private TMP_Text loadingText;
    

    public void Play()
    {
        if (File.Exists(PlayerDataManager.GalaxyFile))
        {
            GenerateGalaxy();
        }
        else
        {
            WindowManager.Instance.OpenWindow(dowloadDialog);
        }
    }

    public void GenerateGalaxy()
    {
        WindowManager.Instance.OpenWindow(null);
        menuback.DOPlayForward();
        loading.DOPlayForward();
        loadingText.text = "Loading Galaxy";
        PlayerDataManager.Instance.LoadScene();
    }

    public void DowloadGalaxy()
    {
        WindowManager.Instance.OpenWindow(null);
        menuback.DOPlayForward();
        loading.DOPlayForward();
        StartCoroutine(DowloadVersonFile());
    }

    IEnumerator DowloadVersonFile()
    {
        
        loadingText.text = "Getting data from server...";
        UnityWebRequest www = UnityWebRequest.Get(hostURL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            GalaxyGenerator.ThrowLoadError("Dowloading failed, generate galaxy manually.\nInfo: " + www.error);
        }
        else
        {
            print(JsonConvert.DeserializeObject<string>(www.downloadHandler.text));
            StartCoroutine(DowloadGalaxyFile(JsonConvert.DeserializeObject<string>(www.downloadHandler.text)));
        }
    }

    IEnumerator DowloadGalaxyFile(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://drive.google.com/uc?export=download&id=" + url))
        {
            webRequest.SendWebRequest();
            
            PlayerDataManager.GenerateProgress = 0;

            while (!webRequest.downloadHandler.isDone)
            {
                
                loadingText.text = $"Dowloaded galaxy [{(webRequest.downloadedBytes/1024f/1024f):F2} mb]";
                PlayerDataManager.GenerateProgress = webRequest.downloadProgress;
                yield return null;
            }
            
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                File.Delete(PlayerDataManager.GalaxyFile);
                GalaxyGenerator.ThrowLoadError("Dowloading failed, generate galaxy manually.\nInfo: " + webRequest.error);
            }
            else
            {
                File.WriteAllText(PlayerDataManager.GalaxyFile, webRequest.downloadHandler.text);
                GenerateGalaxy();
            }
        }
    }
}
