using UnityEngine;
using Zenject;
public class PlayerCarInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerPresenter>().AsSingle();
        Container.Bind<PlayerInputViewModel>().AsSingle();
        Container.Bind<PlayerInputController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<RaceController>().AsSingle();
    }
}
