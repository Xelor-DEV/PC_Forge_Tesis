using UnityEngine;

public class SocketTriggerForwarder : MonoBehaviour
{
    [SerializeField] private SocketLGAPort socketController;
    [SerializeField] private string targetTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            socketController?.HandleTriggerEnter();
        }
    }
}