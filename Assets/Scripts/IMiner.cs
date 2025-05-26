using UnityEngine;
using UnityEngine.Events;

public interface IMiner
{
    public Vector3 Position { get; }
    public bool IsBusy { get; }

    public UnityAction<IMiner> MineStarted { get; set; }
    public UnityAction<IMiner> MineEnded { get; set; }
    public UnityAction<IMiner> UnloadStarted { get; set; }
    public UnityAction<IMiner> UnloadEnded { get; set; }

    public void SetMineTarget(IMineable target);
    public void SetUnloadTarget(IExtraction target);
    public void DrawPath();
    public void StopDrawPath();
}
