using UnityEngine;

public class DIMMPort : InternalSlot
{
    protected override void CheckAssembly()
    {
        if (isSlotOccupied) return;
        if (ValidateLatches() == true && AllSlotsOccupied() == true)
        {
            AttachComponent(currentComponent);
            CloseLatches();
        }
    }

    protected override void AttachComponent(GameObject component)
    {
        InternalHardware ram = component.GetComponent<InternalHardware>();
        if (ram != null)
        {
            isSlotOccupied = true;
            ram.SnapToCorrectPosition(transform);
            ram.DeactivateComponents();
            OnComponentAttached?.Invoke();
        }
    }

    protected override bool ValidateLatches()
    {
        for (int i = 0; i < latches.Length; ++i)
        {
            if (latches[i].IsLatchOpen == false)
            {
                return false;
            }
        }
        return true; // Requiere todos los latches abiertos
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

    private void CloseLatches()
    {
        for (int i = 0; i < latches.Length; ++i)
        {
            latches[i].CloseLatch();
        }
    }
}
