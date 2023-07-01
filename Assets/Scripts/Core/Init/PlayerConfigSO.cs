using System.Collections.Generic;
using System.IO;
using Core.Core.Inject.FoldersManagerService;
using UnityEngine;

namespace Core.Init
{
    [CreateAssetMenu(menuName = "BaseGame", fileName = "PlayerConfig", order = 0)]
    public class PlayerConfigSO: ScriptableObject
    {
        public List<InputService.Axis> axes = new List<InputService.Axis>();
        public int quality = 0;
        public bool showFPS = false;
        
        private InputService input;
        private FolderManagerService folderManagerService;

        public void Init(InputService input, FolderManagerService folderManagerService)
        {
            this.folderManagerService = folderManagerService;
            this.input = input;

            LoadConfig();
        }
        
        public void SaveConfig()
        {
            File.WriteAllText(folderManagerService.ConfigFile, JsonUtility.ToJson(this));
        }

        public void LoadConfig()
        {
            if (!File.Exists(folderManagerService.ConfigFile))
            {
                QualitySettings.SetQualityLevel(0);
                SaveConfig();
            }

            JsonUtility.FromJsonOverwrite(File.ReadAllText(folderManagerService.ConfigFile), this);
        }
    }
}