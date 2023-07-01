using Content.Scripts.Game;
using Core.Core.Inject.FoldersManagerService;
using Core.Core.Inject.GlobalDataService;
using Core.Init;
using UnityEngine;
using Zenject;

namespace Core.Core.Inject
{
    public class BaseGameInstaller : MonoBinder
    {
        [SerializeField] private PlayerConfigSO playerConfig;
        [SerializeField] private SolarSystemService solarSystemHandler;
        public override void InstallBindings()
        {
            BindService<FolderManagerService>();
            BindService<InputService>();
            BindService<PlayerConfigSO>();
            BindService<SolarSystemService>();
        }
    }
}