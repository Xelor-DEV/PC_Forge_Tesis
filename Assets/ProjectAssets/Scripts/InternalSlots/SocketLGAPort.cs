using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SocketLGAPort : InternalSlot
{
    [Header("Socket Configuration")]
    [SerializeField] private Animator[] openAnimators; // Arreglo de animators para abrir
    [SerializeField] private Animator[] closeAnimators; // Arreglo de animators para cerrar

    [Header("Socket Events")]
    public UnityEvent OnSocketOpened;
    public UnityEvent OnSocketClosed;

    [Header("CPU Events")]
    public UnityEvent<GameObject> OnCPUAttached = new UnityEvent<GameObject>();

    [Header("External References")]
    [SerializeField] private GameObject triggerContainer; // Objeto con el collider
    [SerializeField] private BoxCollider triggerCollider;

    private bool isSocketOpen = false;
    private bool componentInstalled = false;

    protected override void Start()
    {
        base.Start();
        ConfigureTriggerCollider();
    }

    private void ConfigureTriggerCollider()
    {
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
    }

    public void HandleTriggerEnter()
    {
        if (componentInstalled == false)
        {
            OpenSocket();
        }
    }

    private void OpenSocket()
    {
        if (!isSocketOpen)
        {
            isSocketOpen = true;
            StartCoroutine(PlayOpenAnimations());
            OnSocketOpened?.Invoke();
        }
    }

    private IEnumerator PlayOpenAnimations()
    {
        for (int i = 0; i < openAnimators.Length; ++i)
        {
            Animator animator = openAnimators[i];
            if (animator != null)
            {
                animator.SetBool("OnOpenSocket", true);
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Esperar a que termine la animación
            }
        }
    }

    protected override void CheckAssembly()
    {
        if (isSlotOccupied) return;
        if (isSocketOpen == true && AllSlotsOccupied() == true)
        {
            AttachComponent(currentComponent);
            CloseSocket();
        }
    }

    private bool AllSlotsOccupied()
    {
        for (int i = 0; i < slotsOccupied.Length; ++i)
        {
            if (slotsOccupied[i] == false)
            {
                return false;
            }
        }
        return true;
    }

    protected override void AttachComponent(GameObject component)
    {
        InternalHardware cpu = component.GetComponent<InternalHardware>();
        if (cpu != null)
        {
            isSlotOccupied = true;
            cpu.SnapToCorrectPosition(transform);
            cpu.DeactivateComponents();
            componentInstalled = true;
            OnComponentAttached?.Invoke();
            OnCPUAttached?.Invoke(cpu.gameObject);
        }
    }

    private void CloseSocket()
    {
        isSocketOpen = false;
        StartCoroutine(PlayCloseAnimations());
        OnSocketClosed?.Invoke();
    }

    private IEnumerator PlayCloseAnimations()
    {
        for (int i = 0; i < closeAnimators.Length; ++i)
        {
            Animator animator = closeAnimators[i];
            if (animator != null)
            {
                animator.SetBool("OnClosedSocket", true);
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Esperar a que termine la animación
            }
        }
    }

    protected override bool ValidateLatches()
    {
        // No se usan latches en este socket
        return true;
    }
}