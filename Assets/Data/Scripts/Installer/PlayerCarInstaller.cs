using Unity.Cinemachine;
using UnityEngine;
using Zenject;
public class PlayerCarInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerPresenter>().AsSingle();
        Container.Bind<PlayerInputViewModel>().AsSingle();
        Container.Bind<PlayerInputController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<RaceController>().AsSingle();

        Container.Bind<CarSetterModel>().AsSingle().NonLazy();
        Container.Bind<CarSetterViewModel>().AsSingle().NonLazy();
        Container.Bind<CarSetterView>().FromComponentInHierarchy().AsSingle();
    }
}
