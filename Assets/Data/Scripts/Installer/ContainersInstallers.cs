using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CarContainerBinder", menuName = "Containers/Car Bind", order = 1)]
public class ContainersInstallers : ScriptableObjectInstaller
{
    [SerializeField] private CarContainer _carContainer;
    public override void InstallBindings()
    {
        Container.Bind<CarContainer>().FromInstance(_carContainer);
    }
}
