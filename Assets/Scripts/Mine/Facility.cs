using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Facility : MonoBehaviour, IExtraction
{
    [SerializeField] private Drone _dronePref;

    private List<IMineable> _minebles;
    private List<Drone> _miners = new();
    private Coroutine _mineCoroutine;

    public Vector3 Position => transform.position;
    public float Total { get; private set; }

    public UnityAction<float> TotalChanged { get; set; }

    public void Init(List<IMineable> mineables, int startDronCount)
    {
        _minebles = mineables;

        Spawn(startDronCount);
    }

    public void StartMine()
    {
        _mineCoroutine = StartCoroutine(MineJob());
    }

    public void Load()
    {
        Total++;
        TotalChanged?.Invoke(Total);
    }

    public void Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var dron = Instantiate(_dronePref, transform.position, Quaternion.identity);

            _miners.Add(dron);
        }
    }

    public void UnSpawnAll()
    {
        foreach (var item in _miners)
        {
            Destroy(item.gameObject);
        }
        _miners.Clear();
    }

    public void SetDroneSpeed(float speed)
    {
        foreach(var item in _miners)
            item.SetSpeed(speed);
    }

    public void EnablePathDrawer()
    {
        foreach (var item in _miners)
        {
            item.DrawPath();
        }
    }

    public void DisablePathDrawer()
    {
        foreach (var item in _miners)
        {   
            item.StopDrawPath();
        }
    }

    private void SendDroUpload(IMiner miner)
    {
        miner.MineEnded -= SendDroUpload;

        miner.SetUnloadTarget(this);
    }

    private IEnumerator MineJob()
    {
        while(true)
        {
            yield return null;

            foreach(var mine in _miners)
            {
                if (mine.IsBusy == true)
                    continue;

                foreach (var resurs in _minebles)
                {
                    if (resurs.MaxUsers > resurs.CurrentUsersCount && resurs.Volume > 0)
                    {
                        mine.SetMineTarget(resurs);
                        mine.MineEnded += SendDroUpload;
                        break;
                    }
                }
            }
        }
    }
}
