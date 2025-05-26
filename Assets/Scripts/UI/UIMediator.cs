using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMediator : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Slider _sliderDronsCount;
    [SerializeField] private Slider _sliderDronsSpeed;
    [SerializeField] private TMP_InputField _resourceGeneration;
    [SerializeField] private Toggle _pathDraw;
    [SerializeField] private TMP_Text _Source1;
    [SerializeField] private TMP_Text _Source2;

    private void OnEnable()
    {
        _sliderDronsCount.onValueChanged.AddListener(_game.SetDronCount);
        _sliderDronsSpeed.onValueChanged.AddListener(_game.SetSpeedForAllDrons);
        _resourceGeneration.onValueChanged.AddListener(OnResourceGenerationChanged);
        _pathDraw.onValueChanged.AddListener(OnPathDrawTaggleChanged);

        _game.Facility[0].TotalChanged += OnSource1Changed;
        _game.Facility[1].TotalChanged += OnSource2Changed;
    }

    private void OnResourceGenerationChanged(string str)
    {
        _game.SetSpawnDeleyForResurce((int.Parse(str)));
    }

    private void OnPathDrawTaggleChanged(bool value)
    {
        if (value)
            _game.EnablePathRender();
        else
            _game.DisablePathRender();
    }

    private void OnSource1Changed(float value)
    {
        _Source1.text = value.ToString();
    }

    private void OnSource2Changed(float value)
    {
        _Source2.text = value.ToString();
    }
}
