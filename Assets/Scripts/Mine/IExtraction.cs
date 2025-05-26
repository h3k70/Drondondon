using UnityEngine;
using UnityEngine.Events;

public interface IExtraction
{
    public Vector3 Position { get; }
    public float Total { get; }

    public UnityAction<float> TotalChanged { get; set; }

    public void Load();
}
