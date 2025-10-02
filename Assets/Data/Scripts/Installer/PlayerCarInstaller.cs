
using Zenject;
public class PlayerCarInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Container.Bind<PlayerPresenter>().AsSingle();
        Container.Bind<PlayerInputViewModel>().AsSingle();
        Container.Bind<PlayerInputController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<RaceControllerInput>().AsSingle();

        Container.BindInterfacesAndSelfTo<RaceCoordinator>().AsSingle().NonLazy();
        Container.Bind<CarSetterModel>().AsSingle().NonLazy();
        Container.Bind<CarSetterViewModel>().AsSingle().NonLazy();
        Container.Bind<CarSetterView>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<RaceUIViewModel>().AsSingle();
        Container.Bind<RaceUIView>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
