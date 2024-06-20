using BurglarGame;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BurglarSceneInstaller : MonoInstaller
    {
        private BurglarGameSettings _gameSettings;

        public override void InstallBindings()
        {
            //Container.Bind<IBurglarGameSettings>().To<BurglarGameSettings>().AsSingle();
            //Container.Bind<PinsModel>();
        }
    }
}