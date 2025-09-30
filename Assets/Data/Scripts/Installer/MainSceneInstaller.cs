using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<MainSceneModel>().AsSingle();
        Container.Bind<MainSceneViewModel>().AsSingle().NonLazy();
        Container.Bind<MainSceneView>().FromComponentInHierarchy().AsSingle();

        Container.Bind<CarSelectModel>().AsSingle().NonLazy();
        Container.Bind<CarSelectViewModel>().AsSingle().NonLazy();
        Container.Bind<CarSelectView>().FromComponentInHierarchy().AsSingle();

        Container.Bind<UIStateModel>().AsSingle();
        Container.Bind<UIStateViewModel>().AsSingle();
    }
}
