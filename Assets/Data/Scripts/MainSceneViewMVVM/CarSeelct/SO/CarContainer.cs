using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using Zenject;

[CreateAssetMenu(fileName = "CarContainer", menuName = "Containers/Car", order = 1)]
public class CarContainer : ScriptableObject
{
    [SerializeField] private List<string> _carIds;
    private readonly Dictionary<string, AsyncOperationHandle<GameObject>> _cache = new();
    public string[] CarIds => _carIds.ToArray();
    
    private async UniTask<Transform> LoadCarInternal(string id)
    {
        if (_cache.TryGetValue(id, out var handle))
        {
            return handle.Result.transform;
        }

        var handleNew = Addressables.LoadAssetAsync<GameObject>(id);
        var prefab = await handleNew.ToUniTask();

        if (prefab != null)
            _cache[id] = handleNew;

        return prefab.transform;
    }

    public void LoadCar(string id, Action<Transform> onSuccess)
    {
        if (string.IsNullOrEmpty(id))
            id = _carIds[0];
        LoadCarAsync(id, onSuccess).Forget();
    }

    private async UniTaskVoid LoadCarAsync(string id, Action<Transform> onSuccess)
    {
        var prefab = await LoadCarInternal(id);
        onSuccess?.Invoke(prefab.transform);
    }

    public void ReleaseCars()
    {
        foreach (var kvp in _cache)
        {
            Addressables.Release(kvp.Value);
        }
        _cache.Clear();
    }
}
