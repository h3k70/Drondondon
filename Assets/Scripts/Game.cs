using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private ResurceSpawner _resurceSpawner;
    [SerializeField] private Facility _facility1;
    [SerializeField] private Facility _facility2;
    [SerializeField] private int _dronCount = 3;

    private void Start()
    {
        _resurceSpawner.StartSpawn();

        _facility1.Init(_resurceSpawner.Minebls, _dronCount);
        _facility2.Init(_resurceSpawner.Minebls, _dronCount);

        _facility1.StartMine();
        _facility2.StartMine();
    }
}
