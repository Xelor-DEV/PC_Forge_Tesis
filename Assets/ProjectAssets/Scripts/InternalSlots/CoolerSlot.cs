using UnityEngine;
using UnityEngine.Events;

public class CoolerSlot : InternalSlot
{
    [Header("References")]
    [SerializeField] private SocketLGAPort _cpuSocket;

    private ThermalPaste _cpuThermalPaste;
    [SerializeField] private bool _hasEnoughPaste = false;

    private void OnEnable()
    {
        if (_cpuSocket != null)
        {
            _cpuSocket.OnCPUAttached.AddListener(HandleCPUAttached);
        }
    }

    private void OnDisable()
    {
        if (_cpuThermalPaste != null)
        {
            _cpuThermalPaste.OnPasteFilled.RemoveListener(EnableCoolerInstallation);
        }
    }

    private void HandleCPUAttached(GameObject cpu)
    {
        _cpuThermalPaste = cpu.GetComponentInChildren<ThermalPaste>();
        if (_cpuThermalPaste != null)
        {
            _cpuThermalPaste.OnPasteFilled.AddListener(EnableCoolerInstallation);
        }
    }

    private void EnableCoolerInstallation()
    {
        _hasEnoughPaste = true;
    }

    protected override void CheckAssembly()
    {
        if (_hasEnoughPaste == true && ValidateLatches() == true && AllSlotsOccupied() == true)
        {
            AttachComponent(currentComponent);
        }
    }

    protected override void AttachComponent(GameObject component)
    {
        InternalHardware cooler = component.GetComponent<InternalHardware>();
        if (cooler != null)
        {
            cooler.SnapToCorrectPosition(transform);
            cooler.DeactivateComponents();
            OnComponentAttached?.Invoke();
        }
    }

    protected override bool ValidateLatches()
    {
        return true;
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
}