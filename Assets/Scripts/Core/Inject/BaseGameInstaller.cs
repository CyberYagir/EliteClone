using UnityEngine;
using Zenject;

namespace Core.Core.Inject
{
    [CreateAssetMenu(fileName = "BaseGameInstaller", menuName = "Installers/BaseGameInstaller")]
    public class BaseGameInstaller : ScriptableObjectInstaller<BaseGameInstaller>
    {
        public override void InstallBindings()
        {
        }
    }
}