using UnityEngine;
using UnityEngine.Events;

public enum ScaleAxis { XZ, XY }

public class ThermalPaste : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private ScaleAxis _scaleAxis = ScaleAxis.XZ;
    [SerializeField] private Vector2 _scaleLimits = new Vector2(0, 1);
    [SerializeField] private float _currentScale = 0f;

    [Header("Events")]
    public UnityEvent OnPasteApplied = new UnityEvent();
    public UnityEvent OnPasteFilled = new UnityEvent();

    private Vector3 _initialScale;
    private bool _isActive = false;
    private bool _filledTriggered = false;

    private void Start()
    {
        _initialScale = transform.localScale;
        SetScale(0);
        gameObject.SetActive(false);
    }

    public void ActivatePaste()
    {
        gameObject.SetActive(true);
        _isActive = true;
    }

    public void ApplyPaste(float amount)
    {
        if (!_isActive) return;

        _currentScale = Mathf.Clamp(_currentScale + amount, 0f, 1f);
        SetScale(_currentScale);
        OnPasteApplied?.Invoke();
        if (_currentScale == _scaleLimits.y)
        {
            OnPasteFilled?.Invoke();
            _filledTriggered = true;
        }
    }

    private void SetScale(float normalizedValue)
    {
        Vector3 newScale = _initialScale;

        switch (_scaleAxis)
        {
            case ScaleAxis.XZ:
                newScale.x = Mathf.Lerp(_scaleLimits.x, _scaleLimits.y, normalizedValue);
                newScale.z = newScale.x;
                break;

            case ScaleAxis.XY:
                newScale.x = Mathf.Lerp(_scaleLimits.x, _scaleLimits.y, normalizedValue);
                newScale.y = newScale.x;
                break;
        }

        transform.localScale = newScale;
    }

    // Llamar este método cuando se instale el CPU
    public void HandleCPUAttached()
    {
        ActivatePaste();
    }
}
