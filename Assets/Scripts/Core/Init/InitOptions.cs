using Core.Core.Inject.FoldersManagerService;
using OmniSARTechnologies.LiteFPSCounter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Init
{
    public class InitOptions : MonoBehaviour
    {

        [SerializeField] private PlayerConfigSO playerConfig;
        [SerializeField] private TMP_Dropdown qualityD, apiD;
        [SerializeField] private Toggle showFpsTgToggle;
        [SerializeField] private GameObject fpsHolder;
        [SerializeField] private InitOptionsControlsDrawer controlsDrawer;
        
        
        private InputService inputService;
        private FolderManagerService folderManagerService;

        public void Awake()
        {
            if (fpsHolder.gameObject == null)
            {
                fpsHolder = FindObjectOfType<LiteFPSCounter>().gameObject;
            }
        }

        [Inject]
        public void Constructor(InputService input, FolderManagerService folderManagerService)
        {
            this.folderManagerService = folderManagerService;
            this.inputService = input;
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
        
        public void UILoad()
        {
            ChangeQuality(qualityD);

            showFpsTgToggle.isOn = playerConfig.showFPS;
            FPSCounterToggle(showFpsTgToggle);
        
            qualityD.value = playerConfig.quality;
            controlsDrawer.DrawControls();
        }

        public void SaveConfig()
        {
            
            playerConfig.axes = inputService.axes;
            playerConfig.quality = QualitySettings.GetQualityLevel();
            playerConfig.showFPS = showFpsTgToggle.isOn;

            playerConfig.SaveConfig();
        }
        
        public void LoadConfig()
        {
            inputService.SetAxesList(playerConfig.axes);
            QualitySettings.SetQualityLevel(playerConfig.quality);
            FPSCounterToggle(playerConfig.showFPS);
            
            UILoad(playerConfig);
        }

        public void ResetKeys()
        {
            var input = InputService.GetData();
            input.SetAxesList(input.startAxes);
            controlsDrawer.DrawControls();
        }


        public void RemoveSave() => folderManagerService.RemoveSave();
    }
}
