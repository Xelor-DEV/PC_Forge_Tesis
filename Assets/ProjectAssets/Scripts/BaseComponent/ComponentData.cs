using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComponentData", menuName = "Scriptable Objects/ComponentData")]
public class ComponentData : ScriptableObject
{
    public string Name;
    public List<string> Features;
}