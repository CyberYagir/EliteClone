using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core.Galaxy;
using TMPro;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Core.Init
{
    public class InitChangelog : MonoBehaviour
    {
        [System.Serializable]
        public class Version
        {
            public string version;

            [System.Serializable]
            public class Change
            {
                public enum ChangeType
                {
                    Fix,
                    Added
                }

                public ChangeType type;
                public List<string> Menu;
                public List<string> Location;
                public List<string> Gameplay;
                public List<string> System;
            }

            public List<Change> changes;
        }

        public List<Version> versions;
        
        [SerializeField] private InitChangeLogItem item;


        private void Start()
        {
            GetComponentInParent<InitMenu>().StartCoroutine(GetChangeLog());
        }


        IEnumerator GetChangeLog()
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get("https://drive.google.com/uc?export=download&id=" + "1b6U78L4RwVESRcVyHHhQqMb0s6mMP6Ro"))
            {
                webRequest.SendWebRequest();

                PlayerDataManager.GenerateProgress = 0;

                while (!webRequest.downloadHandler.isDone)
                {
                    yield return null;
                }


                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    File.Delete(PlayerDataManager.Instance.FSHandler.GalaxyFile);
                    GalaxyGenerator.ThrowLoadError("Error getting data.\nInfo: " + webRequest.error);
                }
                else
                {
                    versions = JsonConvert.DeserializeObject<List<Version>>(webRequest.downloadHandler.text);
                    UpdateText();
                }
            }
        }

        private bool spawned = false;
        public void UpdateText()
        {
            if (!spawned)
            {
                item.gameObject.SetActive(true);
                versions.Reverse();
                foreach (var v in versions)
                {
                    var newItem = Instantiate(item, item.transform.parent);
                    newItem.Init(v);
                }

                item.gameObject.SetActive(false);
                spawned = true;
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponentInParent<Canvas>().GetComponent<RectTransform>());
        }
    }
}
