using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathRenderer
{
    private MonoBehaviour _parent;
    private NavMeshAgent _agent;
    private LineRenderer _lineRenderer;

    private List<Vector3> _pathPoints = new();
    private Coroutine _drawCoroutine;

    public PathRenderer(MonoBehaviour parent, NavMeshAgent agent, LineRenderer lineRenderer)
    {
        _parent = parent;
        _agent = agent;
        _lineRenderer = lineRenderer;
    }

    public void Draw()
    {
        Stop();
        _drawCoroutine = _parent.StartCoroutine(DrawJob());
    }

    public void Stop()
    {
        if (_drawCoroutine != null)
            _parent.StopCoroutine(_drawCoroutine);
    }

    private void PathDraw()
    {
        NavMeshPath path = _agent.path;

        _pathPoints.Clear();

        _pathPoints.Add(_parent.transform.position);

        for (int i = 0; i < path.corners.Length; i++)
        {
            _pathPoints.Add(path.corners[i]);
        }
        _lineRenderer.positionCount = _pathPoints.Count;
        _lineRenderer.SetPositions(_pathPoints.ToArray());
    }

    private IEnumerator DrawJob()
    {
        while (true)
        {
            PathDraw();
            yield return null;
        }
    }
}
