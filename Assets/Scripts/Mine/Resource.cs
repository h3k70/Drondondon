using System;
using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour, IMineable
{
    [SerializeField] private float _volume = 1f;
    [SerializeField] private int _maxUsers = 1;

    public Vector3 Position => transform.position;
    public int MaxUsers => _maxUsers;
    public int CurrentUsersCount { get; private set; }
    public float Volume => _volume;

    public UnityAction<IMineable> Ended { get; set; }

    public float Mine(float strength)
    {
        if (_volume <= 0)
            return 0;

        float total = 0;

        _volume -= strength;

        if (_volume <= 0)
        {
            total = strength - _volume;
            Ended?.Invoke(this);
            Destroy(gameObject);
        }
        else
        {
            total = strength;
        }       

        return total;
    }

    public bool TryRemoveUser(IMiner miner)
    {
        if (CurrentUsersCount == 0)
            return false;

        CurrentUsersCount--;
        return true;
    }

    public bool TrySetUser(IMiner miner)
    {
        if (CurrentUsersCount >= MaxUsers)
            return false;

        miner.Destroyed += OnUserMinerDestroyed;
        CurrentUsersCount++;
        return true;
    }

    private void OnUserMinerDestroyed(IMiner miner)
    {
        TryRemoveUser(miner);
    }
}
