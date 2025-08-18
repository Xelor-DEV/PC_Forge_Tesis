using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalPasteApplicator : MonoBehaviour
{
    [SerializeField] private PlungerSyringe _plunger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ThermalPaste"))
        {
            _plunger.CurrentPaste = other.GetComponentInChildren<ThermalPaste>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ThermalPaste"))
        {
            _plunger.CurrentPaste = null;
        }
    }
}