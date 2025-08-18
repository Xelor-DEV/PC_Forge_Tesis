using UnityEngine;

public class Bracket : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _animator;
    [SerializeField] private BoxCollider _triggerCollider;

    [Header("Settings")]
    [SerializeField] private string _playerTag = "Player";

    private Cooler _cooler;
    private bool _isClosed;

    public void Initialize(Cooler cooler)
    {
        _cooler = cooler;
        _triggerCollider.enabled = false;
    }

    public void ActivateBracket()
    {
        _triggerCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isClosed == false && other.tag == _playerTag)
        {
            CloseBracket();
        }
    }

    private void CloseBracket()
    {
        _isClosed = true;
        _animator.SetBool("OnClosed", true);
        _cooler.ReportBracketClosed();
        _triggerCollider.enabled = false; // Desactivar después de cerrar
    }
}