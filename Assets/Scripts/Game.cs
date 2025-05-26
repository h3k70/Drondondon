using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private ResurceSpawner _resurceSpawner;
    [SerializeField] private List<Facility> _facility;
    [SerializeField] private int _dronCount = 3;

    public List<Facility> Facility { get => _facility; }

    private void Awake()
    {
        _resurceSpawner.StartSpawn();

        foreach (Facility facility in _facility)
        {
            facility.Init(_resurceSpawner.Minebls, _dronCount);
            facility.StartMine();
        }
    }

    public void SetDronCount(float dronCount)
    {
        foreach (Facility facility in _facility)
        {
            facility.UnSpawnAll();
            facility.Spawn((int)dronCount);
        }
    }

    public void SetSpeedForAllDrons(float speedForAllDrons)
    {
        foreach (Facility facility in _facility)
            facility.SetDroneSpeed(speedForAllDrons);
    }

    public void EnablePathRender()
    {
        foreach (var item in _facility)
            item.EnablePathDrawer();
    }

    public void DisablePathRender()
    {
        foreach (var item in _facility)
            item.DisablePathDrawer();
    }

    public void SetSpawnDeleyForResurce(int spawnDeleyForResurce)
    {
        _resurceSpawner.SpawnDeley = spawnDeleyForResurce;
    }
}
