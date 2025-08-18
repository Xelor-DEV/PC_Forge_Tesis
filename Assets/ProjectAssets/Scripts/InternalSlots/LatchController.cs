using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LatchController : MonoBehaviour
{
    public enum RotationSpace { Local, World }
    public enum RotationAxis { X, Y, Z } // Nuevo enum para selección de eje

    [Header("Latch Configuration")]
    [SerializeField] RotationSpace rotationSpace = RotationSpace.Local;
    [SerializeField] RotationAxis rotationAxis = RotationAxis.Z; // Nuevo campo para eje
    [SerializeField] float targetAngle = 90f;
    [SerializeField] float closedAngle = 0f;
    [SerializeField] float angleThreshold = 1f;

    [Header("Closing Animation")]
    [SerializeField] float closeDuration = 0.5f;
    [SerializeField] AnimationCurve closeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Events")]
    public UnityEvent OnLatchOpened;
    public UnityEvent OnLatchClosed;

    [Header("Components to Lock")]
    [SerializeField] private Grabbable _grabbable;
    [SerializeField] private GrabInteractable _grabInteractable;
    [SerializeField] private HandGrabInteractable _handGrabInteractable;
    [SerializeField] private OneGrabRotateTransformer _oneGrabRotateTransformer;

    private float currentAngle;
    private bool isLocked = false;
    private bool isOpen = false;
    private Coroutine closeCoroutine;

    public bool IsLatchOpen => isOpen;
    public bool IsLocked => isLocked;

    void Update()
    {
        if (isLocked || closeCoroutine != null) return;

        UpdateRotation();
        CheckTargetAngle();
    }

    private void UpdateRotation()
    {
        // Obtener ángulo actual según eje seleccionado
        currentAngle = rotationSpace == RotationSpace.Local ?
            GetLocalRotationAxis() :
            GetWorldRotationAxis();

        // Eliminada la lógica de ajuste automático al mínimo/máximo
    }

    private float GetLocalRotationAxis()
    {
        return transform.localEulerAngles[(int)rotationAxis];
    }

    private float GetWorldRotationAxis()
    {
        return transform.eulerAngles[(int)rotationAxis];
    }

    private void CheckTargetAngle()
    {
        bool wasOpen = isOpen;
        isOpen = Mathf.Abs(currentAngle - targetAngle) < angleThreshold;

        if (!wasOpen && isOpen)
        {
            HandleLatchOpened();
        }
    }

    private void HandleLatchOpened()
    {
        SetRotation(targetAngle);
        LockLatch();
        DeactivateComponents();
        OnLatchOpened?.Invoke();
    }

    public void LockLatch()
    {
        isLocked = true;
    }

    public void CloseLatch()
    {
        if (closeCoroutine != null)
            StopCoroutine(closeCoroutine);

        closeCoroutine = StartCoroutine(SmoothClose());
    }

    private IEnumerator SmoothClose()
    {
        LockLatch();
        DeactivateComponents();

        float startAngle = GetCurrentAngle();
        float targetClosedAngle = closedAngle;
        float time = 0;

        while (time < closeDuration)
        {
            time += Time.deltaTime;
            float t = closeCurve.Evaluate(time / closeDuration);
            float angle = Mathf.LerpAngle(startAngle, targetClosedAngle, t);

            SetRotation(angle);
            yield return null;
        }

        SetRotation(targetClosedAngle);
        OnLatchClosed.Invoke();
        closeCoroutine = null;
    }

    private float GetCurrentAngle()
    {
        return rotationSpace == RotationSpace.Local ?
            GetLocalRotationAxis() :
            GetWorldRotationAxis();
    }

    private void SetRotation(float angle)
    {
        Vector3 newRotation = rotationSpace == RotationSpace.Local ?
            transform.localEulerAngles :
            transform.eulerAngles;

        // Aplicar al eje seleccionado
        newRotation[(int)rotationAxis] = angle;

        if (rotationSpace == RotationSpace.Local)
            transform.localEulerAngles = newRotation;
        else
            transform.eulerAngles = newRotation;
    }

    public void DeactivateComponents()
    {
        if (_grabbable != null) _grabbable.enabled = false;
        if (_grabInteractable != null) _grabInteractable.enabled = false;
        if (_handGrabInteractable != null) _handGrabInteractable.enabled = false;
        if (_oneGrabRotateTransformer != null) _oneGrabRotateTransformer.enabled = false;
    }
}