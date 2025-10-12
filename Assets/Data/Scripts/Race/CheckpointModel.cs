using System;
using Zenject;

public class CheckpointModel
{
    public int Id { get; }
    public bool IsPassed { get; private set; }

    public event Action<int> OnPassed;

    [Inject]
    public CheckpointModel(int id)
    {
        Id = id;
    }

    public void MarkPassed()
    {
        if (IsPassed) return;

        IsPassed = true;
        OnPassed?.Invoke(Id);
    }

    public void Reset() => IsPassed = false;

    public class CheckpointModelFactory : PlaceholderFactory<int, CheckpointModel> { }
}