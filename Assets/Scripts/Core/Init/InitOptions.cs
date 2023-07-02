using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OmniSARTechnologies.LiteFPSCounter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Init
{
    public class InitOptions : MonoBehaviour
    {
        public class PlayerConfig
        {
            public List<InputService.Axis> axes = new List<InputService.Axis>();
            public int quality;
            public bool showFPS;

        }

        [SerializeField] private TMP_Dropdown qualityD, apiD;
        [SerializeField] private Toggle showFpsTgToggle;
        [SerializeField] private GameObject fpsHolder;
        [SerializeField] private InitOptionsControlsDrawer controlsDrawer;

        public void Awake()
        {
            if (fpsHolder.gameObject == null)
            {
                fpsHolder = FindObjectOfType<LiteFPSCounter>().gameObject;
            }
        }

        private void Start()
        {
            LoadConfig();
        }

        public void ChangeQuality(TMP_Dropdown dropdown)
        {
            QualitySettings.SetQualityLevel(dropdown.value);
        }

        public void FPSCounterToggle(Toggle toggle)
        {
            fpsHolder.gameObject.SetActive(toggle.isOn);
        }
        public void FPSCounterToggle(bool bl)
        {
            fpsHolder.gameObject.SetActive(bl);
        }

        public void SaveConfig()
        {
            PlayerConfig cfg = new PlayerConfig();
            cfg.axes = FindObjectOfType<InputService>().axes;
            cfg.quality = QualitySettings.GetQualityLevel();
            cfg.showFPS = showFpsTgToggle.isOn;
            File.WriteAllText(PlayerDataManager.Instance.FSHandler.ConfigFile, JsonConvert.SerializeObject(cfg));
            PlayerDataManager.PlayerConfig = cfg;
        }

        public void UILoad(PlayerConfig cfg)
        {
            qualityD.value = cfg.quality;
            ChangeQuality(qualityD);

            showFpsTgToggle.isOn = cfg.showFPS;
            FPSCounterToggle(showFpsTgToggle);
        
            controlsDrawer.DrawControls();
        }

        public void LoadConfig()
        {
            if (!File.Exists(PlayerDataManager.Instance.FSHandler.ConfigFile))
            {
                QualitySettings.SetQualityLevel(0);
                SaveConfig();
            }

            PlayerConfig cfg = JsonConvert.DeserializeObject<PlayerConfig>(File.ReadAllText(PlayerDataManager.Instance.FSHandler.ConfigFile));
            FindObjectOfType<InputService>().SetAxesList(cfg.axes);
            QualitySettings.SetQualityLevel(cfg.quality);
            FPSCounterToggle(cfg.showFPS);
            PlayerDataManager.PlayerConfig = cfg;
            UILoad(PlayerDataManager.PlayerConfig);
        }

        public void ResetKeys()
        {
            var input = InputService.GetData();
            input.SetAxesList(input.startAxes);
            controlsDrawer.DrawControls();
        }

        public void RemoveSave() => PlayerDataManager.Instance.FSHandler.RemoveSave();
    }
}
