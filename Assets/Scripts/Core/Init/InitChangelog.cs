using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core.Galaxy;
using TMPro;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

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


        [SerializeField] private TMP_Text text;


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
                    File.Delete(PlayerDataManager.GalaxyFile);
                    GalaxyGenerator.ThrowLoadError("Error getting data.\nInfo: " + webRequest.error);
                }
                else
                {
                    versions = JsonConvert.DeserializeObject<List<Version>>(webRequest.downloadHandler.text);
                    UpdateText();
                }
            }
        }

        public void UpdateText()
        {
            text.text = "";
            
            foreach (var v in versions)
            {
                var vers = "Version " + v.version + ": \n";

                for (int i = 0; i < v.changes.Count; i++)
                {
                    vers += " -" + v.changes[i].type.ToString() + "\n";
                    vers += DrawArray(v.changes[i].Gameplay, "Gameplay");
                    vers += DrawArray(v.changes[i].Location, "Locations");
                    vers += DrawArray(v.changes[i].Menu, "Menu");
                    vers += DrawArray(v.changes[i].System, "Systems");
                }

                text.text += vers + "\n";
            }
        }

        public string DrawArray(List<string> list, string mn)
        {
            if (list.Count == 0)
            {
                return "";
            }
            else
            {
                var str = "  + " + mn + "\n";
                for (int i = 0; i < list.Count; i++)
                {
                    str += "    " +  list[i] + "\n";
                }

                return str;
            }
        }
    }
}
