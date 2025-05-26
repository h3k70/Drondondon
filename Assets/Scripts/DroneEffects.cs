using UnityEngine;
using UnityEngine.UI;

public class DroneEffects : MonoBehaviour
{
    [SerializeField] private Drone _miner;
    [SerializeField] private ParticleSystem _dropResurceParticle;
    [SerializeField] private Image _statusImage;

    [SerializeField] private Color _mineColor;
    [SerializeField] private Color _unloadColor;
    [SerializeField] private Color _flyColor;

    private void Start()
    {
        var main = _dropResurceParticle.main;
        main.duration = _miner.UnloadDeley;

        _statusImage.color = _flyColor;
    }

    private void OnEnable()
    {
        _miner.MineStarted += OnMineStarted;
        _miner.MineEnded += OnMineEnded;
        _miner.UnloadStarted += OnUnloadStarted;
        _miner.UnloadEnded += OnUnloadEnded;
    }

    private void OnDisable()
    {
        _miner.MineStarted -= OnMineStarted;
        _miner.MineEnded -= OnMineEnded;
        _miner.UnloadStarted -= OnUnloadStarted;
        _miner.UnloadEnded -= OnUnloadEnded;
    }

    private void OnMineStarted(IMiner miner)
    {
        _statusImage.color = _mineColor;
    }
    
    private void OnMineEnded(IMiner miner)
    {
        _statusImage.color = _flyColor;
    }

    private void OnUnloadStarted(IMiner miner)
    {
        _statusImage.color = _unloadColor;

        _dropResurceParticle.Play();
    }

    private void OnUnloadEnded(IMiner miner)
    {
        _statusImage.color = _flyColor;
    }
}
