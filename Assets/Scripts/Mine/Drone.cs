using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LineRenderer))]
public class Drone : MonoBehaviour, IMiner
{
    [SerializeField] private float _mineDistance = 2f;
    [SerializeField] private float _mineDeley = 2f;
    [SerializeField] private float _mineStrength = 1f;
    [SerializeField] private float _UnloadDistance = 2f;
    [SerializeField] private float _UnloadDeley = 2f;

    private NavMeshAgent _agent;
    private LineRenderer _lineRenderer;
    private PathRenderer _pathRenderer;
    private IMineable _targetForMine;
    private IExtraction _targetForUpload;
    private bool _isBusy;

    private Coroutine _mineCoroutine;
    private Coroutine _UnloadCoroutine;

    public Vector3 Position => transform.position;
    public bool IsBusy { get { return _isBusy; } private set { _isBusy = value; BusyStatusChanged?.Invoke(_isBusy); } }
    public float UnloadDeley { get => _UnloadDeley; private set => _UnloadDeley = value; }

    public UnityAction<IMiner> MineStarted { get; set; }
    public UnityAction<IMiner> MineEnded { get; set; }
    public UnityAction<IMiner> UnloadStarted { get; set; }
    public UnityAction<IMiner> UnloadEnded { get; set; }
    public UnityAction<bool> BusyStatusChanged { get; set; }
    public UnityAction<IMiner> Destroyed { get; set; }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _lineRenderer = GetComponent<LineRenderer>();
        _pathRenderer = new(this, _agent, _lineRenderer);
    }

    private void OnDestroy()
    {
        Destroyed?.Invoke(this);
    }

    public void SetMineTarget(IMineable target)
    {
        IsBusy = true;

        _targetForMine = target;
        MoveTo(_targetForMine.Position);
        target.TrySetUser(this);

        StartCoroutine(MineJob());
    }

    public void SetUnloadTarget(IExtraction target)
    {
        _targetForUpload = target;
        MoveTo(_targetForUpload.Position);
        StartCoroutine(UnloadJob());
    }

    public void DrawPath()
    {
        _pathRenderer.Draw();
    }

    public void StopDrawPath()
    {
        _pathRenderer.Stop();
    }

    public void SetSpeed(float speed)
    {
        _agent.speed = speed;
    }

    private void StopMine()
    {
        if(_mineCoroutine != null)
            StopCoroutine(_mineCoroutine);

        if(_targetForMine != null)
        {
            _targetForMine.TryRemoveUser(this);
            _targetForMine = null;
        }
        MineEnded?.Invoke(this);
    }

    private void StopUnload()
    {
        if(_UnloadCoroutine != null)
            StopCoroutine(_UnloadCoroutine);

        _targetForUpload = null;

        UnloadEnded?.Invoke(this);
        IsBusy = false;
    }

    private void MoveTo(Vector3 vector3)
    {
        _agent.SetDestination(vector3);
    }

    private void StopMove()
    {
        _agent.ResetPath();
    }

    private IEnumerator MineJob()
    {
        while (Vector3.Distance(_targetForMine.Position, transform.position) >= _mineDistance)
            yield return null;

        StopMove();
        MineStarted?.Invoke(this);

        while (_targetForMine.Volume > 0)
        {
            yield return new WaitForSeconds(_mineDeley);

            _targetForMine.Mine(_mineStrength);
        }
        StopMine();
    }

    private IEnumerator UnloadJob()
    {
        while (Vector3.Distance(_targetForUpload.Position, transform.position) >= _UnloadDistance)
            yield return null;

        StopMove();
        UnloadStarted?.Invoke(this);

        yield return new WaitForSeconds(_UnloadDeley);

        _targetForUpload.Load();
        StopUnload();
    }
}
