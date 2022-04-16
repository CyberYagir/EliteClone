using System;
using System.Collections;
using System.IO;
using Core.Galaxy;
using DG.Tweening;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.Init
{
    public class InitMenu : MonoBehaviour
    {
        public string hostURL;
        public CustomAnimate dowloadDialog;
        [SerializeField] private InitOptions options;
        [SerializeField] private DOTweenAnimation menuback;
        [SerializeField] private DOTweenAnimation loading;
        [SerializeField] private TMP_Text loadingText;

        private void Awake()
        {
            PlayerDataManager.CurrentSolarSystem = null;
            options.Awake();
        }

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
            ActiveLoading();
            loadingText.text = "Loading Galaxy";
            PlayerDataManager.Instance.LoadScene();
        }

        public void DowloadGalaxy()
        {
            WindowManager.Instance.OpenWindow(null);
            menuback.DOPlayForward();
            ActiveLoading();
            StartCoroutine(DowloadVersonFile());
        }

        public void ActiveLoading()
        {
            loading.gameObject.SetActive(true);
            loading.DOPlayForward();
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
                    try
                    {
                        File.WriteAllText(PlayerDataManager.GalaxyFile, webRequest.downloadHandler.text);
                    }
                    catch (Exception e)
                    {
                        GalaxyGenerator.ThrowLoadError("Dowloading failed, server error, generate galaxy manually.\nInfo: " + webRequest.error);
                    }
                    GenerateGalaxy();
                }
            }
        }


        public void OpenDXDiag(string url)
        {
            if (File.Exists(url))
            {
                Application.OpenURL(url);
            }
            else
            {
                GalaxyGenerator.ThrowLoadError($"DXDiag not found by path [{url}]");
            }
        }
    
        public void Quit()
        {
            Application.Quit();
        }
    }
}
