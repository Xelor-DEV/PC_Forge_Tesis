using Oculus.Interaction.HandGrab;
using Oculus.Interaction;
using UnityEngine;

public class GrabbableObjectDetector : MonoBehaviour
{
    [SerializeField] private GrabInteractable _grabInteractable;
    [SerializeField] private HandGrabInteractable _handGrabInteractable;

    private void Update()
    {
        if (_grabInteractable.State == InteractableState.Select || _handGrabInteractable.State == InteractableState.Select)
        {
            HandleGrab();
        }
    }

    private void HandleGrab()
    {
        LastGrabbedTracker.Instance.RegisterGrab(this.gameObject);
    }
}
