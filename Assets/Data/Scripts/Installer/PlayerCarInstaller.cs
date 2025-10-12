
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static CheckpointModel;
using static CheckpointViewModel;
public class PlayerCarInstaller : MonoInstaller
{
    [SerializeField] private CheckpointView[] checkpointViews;
    public override void InstallBindings()
    {
        //Container.Bind<PlayerPresenter>().AsSingle();
        Container.Bind<PlayerInputViewModel>().AsSingle();
        Container.Bind<PlayerInputController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<RaceControllerInput>().AsSingle();
        Container.Bind<RaceController>().AsSingle();

        Container.BindInterfacesAndSelfTo<RaceCoordinator>().AsSingle().NonLazy();
        Container.Bind<CarSetterModel>().AsSingle().NonLazy();
        Container.Bind<CarSetterViewModel>().AsSingle().NonLazy();
        Container.Bind<CarSetterView>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<RaceUIViewModel>().AsSingle();
        Container.Bind<RaceUIView>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.BindFactory<int, CheckpointModel, CheckpointModelFactory>().AsSingle();
        Container.BindFactory<CheckpointModel, CheckpointView, CheckpointViewModel, CheckpointViewModelFactory>().AsSingle();

        var checkpointModels = new List<CheckpointModel>();
        var checkpointViewModels = new List<CheckpointViewModel>();

        var modelFactory = Container.Resolve<CheckpointModelFactory>();
        var viewModelFactory = Container.Resolve<CheckpointViewModelFactory>();

        foreach (var view in checkpointViews)
        {
            var model = modelFactory.Create(view.Id);
            var vm = viewModelFactory.Create(model, view);

            checkpointModels.Add(model);
            checkpointViewModels.Add(vm);
        }

        Container.BindInstance(checkpointModels).AsSingle();
        Container.BindInstance(checkpointViewModels).AsSingle();

        Container.Bind<RacePauseUIViewModel>().AsSingle();
        Container.Bind<RacePauseUIView>().FromComponentInHierarchy().AsSingle();

        Container.Bind<RaceFinishUIViewModel>().AsSingle();
        Container.Bind<RaceFinishUIView>().FromComponentInHierarchy().AsSingle();
    }
}
