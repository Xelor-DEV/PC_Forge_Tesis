using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine.Events;

public class InternalHardware : MonoBehaviour
{
    [Header("Components to Deactivate")]
    [SerializeField] public Rigidbody _compRigidbody;
    [SerializeField] public BoxCollider _compBoxCollider;
    [SerializeField] public Grabbable _compGrabbable;
    [SerializeField] public GrabInteractable _grabInteractable;
    [SerializeField] public HandGrabInteractable _handGrabInteractable;

    [Header("Attachment Settings")]
    [SerializeField] public Vector3 correctPosition; // Posici�n relativa a la motherboard
    [SerializeField] public Vector3 correctRotation; // Rotaci�n en �ngulos de Euler

    [Header("Events")]
    public UnityEvent OnComponentInstalled; // El evento p�blico

    public void SnapToCorrectPosition(Transform motherboard)
    {
        // Configurar transformaci�n relativa
        transform.SetParent(motherboard);
        transform.localPosition = correctPosition;
        transform.localRotation = Quaternion.Euler(correctRotation);
    }

    [ContextMenu("Set Correct Position And Rotation")]
    public void SetCorrectPositionAndRotation()
    {
        correctPosition = transform.transform.localPosition;
        correctRotation = transform.transform.localRotation.eulerAngles;
    }


    public void DeactivateComponents()
    {
        if (_compGrabbable != null)
        {
            _compGrabbable.enabled = false;
        }

        if (_grabInteractable != null)
        {
            _grabInteractable.enabled = false;
        }

        if (_handGrabInteractable != null)
        {
            _handGrabInteractable.enabled = false;
        }

        if (_compBoxCollider != null)
        {
            _compBoxCollider.enabled = false;
        }
        

        if (_compRigidbody != null)
        {
            // Desactivar la gravedad
            _compRigidbody.useGravity = false;

            // Congelar la posici�n y rotaci�n
            _compRigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

            // Hacer el Rigidbody cinem�tico
            _compRigidbody.isKinematic = true;

            // Detener el movimiento y la rotaci�n (aunque est� congelado por los constraints)
            _compRigidbody.linearVelocity = Vector3.zero;
            _compRigidbody.angularVelocity = Vector3.zero;
        }
        OnComponentInstalled?.Invoke();
    }
}
