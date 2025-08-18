using UnityEngine;

public class PCIExpressPort : InternalSlot
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
        InternalHardware gpu = component.GetComponent<InternalHardware>();
        if (gpu != null)
        {
            isSlotOccupied = true;
            gpu.SnapToCorrectPosition(transform);
            gpu.DeactivateComponents();
            OnComponentAttached.Invoke();
        }
        else
        {
            Debug.LogError("Componente GPU no encontrado en el objeto adjuntado");
        }
    }

    protected override bool ValidateLatches()
    {
        if (latches != null)
        {
            for (int i = 0; i < latches.Length; ++i)
            {
                if (latches[i].IsLatchOpen == false)
                {
                    return false;
                }
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