using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RaceController
{
    private readonly int _lapsToComplete;
    private readonly List<CheckpointModel> _checkpoints;

    private RaceState _raceState = RaceState.Prepare;
    private int _currentLap = 0;
    private float _raceTimer = 0f;
    private int _lastCheckpointId = 1;
    private bool _isLapFinished;
    private bool _isRunningTimer;

    public RaceState RaceState => _raceState;
    public int CurrentLap => _currentLap;
    public int LapsToComplete => _lapsToComplete;
    public float RaceTimer => _raceTimer;
    public bool IsLapFinished => _isLapFinished;

    public event Action<RaceState> OnRaceStateChanged;
    public event Action<int> OnLapCompleted;
    public event Action<int> OnCheckpointPassedAction;
    public event Action OnRaceStarted;
    public event Action OnRaceFinished;
    public event Action<float> OnRaceTimerUpdated;

    public RaceController(List<CheckpointModel> checkpoints)
    {
        _lapsToComplete = 3;
        _checkpoints = checkpoints;
        Init();
    }
    public void Init()
    {
        foreach (var cp in _checkpoints)
            cp.OnPassed += OnCheckpointTriggered;
    }
    
    public void StartRace()
    {
        if (_raceState != RaceState.Prepare)
            return;

        _raceState = RaceState.Running;
        _currentLap = 1;
        _lastCheckpointId = 0;
        _raceTimer = 0f;
        _isLapFinished = false;
        _isRunningTimer = true;

        ResetAllCheckpoints();

        OnRaceStarted?.Invoke();
        OnRaceStateChanged?.Invoke(_raceState);
    }

    public void PauseRace()
    {
        if (_raceState == RaceState.Running)
        {
            _raceState = RaceState.Paused;
            _isRunningTimer = false;
            OnRaceStateChanged?.Invoke(_raceState);
        }
    }

    public void ResumeRace()
    {
        if (_raceState == RaceState.Paused)
        {
            _raceState = RaceState.Running;
            _isRunningTimer = true;
            OnRaceStateChanged?.Invoke(_raceState);
        }
    }

    public void FinishRace()
    {
        _raceState = RaceState.Finished;
        _isRunningTimer = false;

        OnRaceFinished?.Invoke();
        OnRaceStateChanged?.Invoke(_raceState);
    }

    public void Update(float deltaTime)
    {
        if (_isRunningTimer)
        {
            _raceTimer += deltaTime;
            OnRaceTimerUpdated?.Invoke(_raceTimer);
        }
    }

    public void OnCheckpointTriggered(int checkpointId)
    {
        if (_raceState != RaceState.Running)
            return;

        if (checkpointId == _lastCheckpointId + 1)
        {
            _lastCheckpointId = checkpointId;
            _checkpoints[checkpointId - 1].MarkPassed();
            OnCheckpointPassedAction?.Invoke(checkpointId);

            if (_lastCheckpointId == _checkpoints.Count)
                CompleteLap();
        }
        else
        {
            Debug.LogWarning($"Checkpoint {checkpointId} skipped or out of order!");
        }
    }

    private void CompleteLap()
    {
        _isLapFinished = true;

        if (_currentLap >= _lapsToComplete)
        {
            FinishRace();
        }
        else
        {
            _currentLap++;
            OnLapCompleted?.Invoke(_currentLap);
            _isLapFinished = false;
            _lastCheckpointId = 0;
            ResetAllCheckpoints();
        }
    }

    private void ResetAllCheckpoints()
    {
        foreach (var cp in _checkpoints)
            cp.Reset();
    }
}
