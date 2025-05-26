using UnityEngine;
using UnityEngine.Events;

public interface IMineable
{
    public Vector3 Position { get; }
    public int MaxUsers { get; }
    public int CurrentUsersCount { get; }
    public float Volume { get; }

    public UnityAction<IMineable> Ended { get; set; }

    public bool TrySetUser(IMiner miner);
    public bool TryRemoveUser(IMiner miner);
    public float Mine(float strength);
}
