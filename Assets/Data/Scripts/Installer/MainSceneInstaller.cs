using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<MainSceneModel>().AsSingle();
        Container.Bind<MainSceneView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<MainSceneViewModel>().AsSingle().NonLazy();
    }
}
