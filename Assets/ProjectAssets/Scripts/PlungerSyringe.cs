using UnityEngine;
using Oculus.Interaction;

public class PlungerSyringe : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OneGrabTranslateTransformer _transformer;

    [Header("Configuration")]
    [SerializeField] private Vector2 _zLimits = new Vector2(0.1776f, 0.4041412f);

    private float _currentNormalizedPosition;
    private float _previousPosition;
    private ThermalPaste _currentPaste;
    public ThermalPaste CurrentPaste
    {
        set
        {
            _currentPaste = value;
        }
    }

    private void Start()
    {
        GetTransformerConstraints();
        _previousPosition = transform.localPosition.z;
    }

    [ContextMenu("Get Constraints From Transformer")]
    private void GetTransformerConstraints()
    {
        if (_transformer != null)
        {
            _zLimits = new Vector2(
                _transformer.Constraints.MinZ.Value,
                _transformer.Constraints.MaxZ.Value
            );
        }
    }

    private void Update()
    {
        UpdatePlungerPosition();
    }

    private void UpdatePlungerPosition()
    {
        float currentZ = transform.localPosition.z;
        _currentNormalizedPosition = Mathf.InverseLerp(_zLimits.x, _zLimits.y, currentZ);

        // Solo aplicar cambios si nos movemos hacia abajo (hacia el mínimo)
        float delta = currentZ - _previousPosition;
        if (delta < 0 && _currentPaste != null)
        {
            float progress = Mathf.Abs(delta) / (_zLimits.y - _zLimits.x);
            _currentPaste.ApplyPaste(progress);
        }

        _previousPosition = currentZ;
    }
}
