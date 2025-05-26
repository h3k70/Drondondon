using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ResurceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _pref;
    [SerializeField] private float _spawnDistance = 18;
    [SerializeField] private float _spawnDeley = 1;
    [SerializeField] private int _max = 8;

    private List<IMineable> _minebls = new List<IMineable>();
    private Coroutine _spawnCoroutine;

    public List<IMineable> Minebls { get => _minebls; private set => _minebls = value; }
    public float SpawnDeley { get => _spawnDeley; set => _spawnDeley = value; }

    public void StartSpawn()
    {
        _spawnCoroutine = StartCoroutine(SpawnJob());
    }

    private void DeleteAtList(IMineable mineable)
    {
        mineable.Ended -= DeleteAtList;
        _minebls.Remove(mineable);
    }

    private IEnumerator SpawnJob()
    {
        while (true)
        {
            yield return null;

            if (_max <= _minebls.Count)
                continue;

            float spawnPositonX = Random.Range(-_spawnDistance, _spawnDistance);
            float spawnPositonZ = Random.Range(-_spawnDistance, _spawnDistance);
            Vector3 spawnPosition = new Vector3(spawnPositonX, transform.position.y, spawnPositonZ);

            var mineble = Instantiate(_pref, spawnPosition, Quaternion.identity);
            _minebls.Add(mineble);

            mineble.Ended += DeleteAtList;

            yield return SpawnDeley;
        }
    }
}
