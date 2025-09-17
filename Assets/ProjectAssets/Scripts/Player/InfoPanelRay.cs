using TMPro;
using UnityEngine;


public class InfoPanelRay : MonoBehaviour
{
    [SerializeField] GameObject _panel;

    [SerializeField] float _rayDistance;
    [SerializeField] LayerMask _layers;


    [SerializeField] TextMeshProUGUI _componentName;
    [SerializeField] TextMeshProUGUI _componentFeatures;

    private AssemblyComponent _lastComponent;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            SwitchPanelActive();
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _rayDistance, _layers))
        {
            if (hit.transform.TryGetComponent<AssemblyComponent>(out AssemblyComponent component))
            {
                if (_lastComponent != null && _lastComponent == component) return;

                ApplyUIInfo(component.Data);
                _lastComponent = component;
            }
        }
    }
    public void SwitchPanelActive()
    {
        _panel.SetActive(!_panel.activeSelf);
    }
    public void ApplyUIInfo(ComponentData componentData)
    {
        _componentName.text = componentData.Name;

        _componentFeatures.text = "";

        for (int i = 0; i < componentData.Features.Count; i++)
        {
            _componentFeatures.text = _componentFeatures.text +  "- " + componentData.Features[i] + System.Environment.NewLine;
        }
    }
}