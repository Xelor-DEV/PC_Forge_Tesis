using UnityEngine;
using UnityEngine.Events;

public class Cooler : InternalHardware
{
    [Header("Bracket System")]
    [SerializeField] private Bracket[] _brackets;
    [SerializeField] private UnityEvent OnFullySecured = new UnityEvent();

    private bool _isCoolerSecured;
    private int _closedBracketsCount;

    private void OnEnable()
    {
        OnComponentInstalled.AddListener(ActivateBrackets);
    }

    private void Start()
    {
        InitializeBrackets();
    }

    private void InitializeBrackets()
    {
        for (int i = 0; i < _brackets.Length; ++i)
        {
            _brackets[i].Initialize(this);
        }
    }

    public void ReportBracketClosed()
    {
        _closedBracketsCount++;
        if (_closedBracketsCount >= _brackets.Length)
        {
            OnFullySecured?.Invoke();
            _isCoolerSecured = true; 
        }
    }

    private void ActivateBrackets()
    {
        for(int i = 0 ; i < _brackets.Length ; ++i)
        {
            _brackets[i].ActivateBracket();
        }
    }
}