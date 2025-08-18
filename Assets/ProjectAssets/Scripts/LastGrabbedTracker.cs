using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class LastGrabbedTracker : MonoBehaviour
{
    public static LastGrabbedTracker Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private string _motherboardTag = "Motherboard";
    [SerializeField] private string _coolerTag = "Cooler";

    [SerializeField] private GameObject _lastMotherboard;
    [SerializeField] private GameObject _lastCooler;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void RegisterGrab(GameObject grabbedObject)
    {
        if (grabbedObject.CompareTag(_motherboardTag))
        {
            _lastMotherboard = grabbedObject;
        }
        else if (grabbedObject.CompareTag(_coolerTag))
        {
            _lastCooler = grabbedObject;
        }
    }
}