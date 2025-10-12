using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CarSelectModel
{
    private CarContainer _container;
    private int currentIndex = 0;
    public string CurrentCarId { get; private set; }
    public CarSelectModel(CarContainer container)
    {
        _container = container;
        CurrentCarId = _container.CarIds[currentIndex];
    }

    public void NextCarSelect()
    {
        currentIndex++;
        if(currentIndex >= _container.CarIds.Length)
            currentIndex = 0;
        CurrentCarId = _container.CarIds[currentIndex];
    }

    public void PreviousCarSelect()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = _container.CarIds.Length - 1;
        CurrentCarId = _container.CarIds[currentIndex];
    }

    public void LoadScene()
    {
        LoadSceneAsync().Forget();
        _container.ReleaseCars();
    }

    private async UniTaskVoid LoadSceneAsync()
    {
        var sceneLoadingInfo = Addressables.LoadSceneAsync("RaceScene");
        var awaitInfo = await sceneLoadingInfo.ToUniTask();
    }
}
