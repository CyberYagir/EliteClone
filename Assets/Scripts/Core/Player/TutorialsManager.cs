using System.Collections;
using System.IO;
using Core.Game;
using Core.Location;
using Core.Systems;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.PlayerScripts
{
    public class TutorialsManager : MonoBehaviour
    {
        [SerializeField] private TutorialSO tutorialData;

        public TutorialSO TutorialData => tutorialData;

        public void LoadTutorial()
        {
            if (TutorialData == null)
            {
                tutorialData = ScriptableObject.CreateInstance<TutorialSO>();
            }
            
            if (File.Exists(PlayerDataManager.Instance.FSHandler.TutorialsFile))
            {
                JsonUtility.FromJsonOverwrite(File.ReadAllText(PlayerDataManager.Instance.FSHandler.TutorialsFile), TutorialData);
            }
            else
            {
                SaveTutorial();
            }
        }

        public void SaveTutorial()
        {
            File.WriteAllText(PlayerDataManager.Instance.FSHandler.TutorialsFile, JsonUtility.ToJson(TutorialData));
        }
    }
}
