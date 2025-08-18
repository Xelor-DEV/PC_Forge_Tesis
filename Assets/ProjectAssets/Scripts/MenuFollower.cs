using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menu; // Referencia al objeto menu

    // Método para activar/desactivar el menú
    public void ToggleMenu()
    {
        if (menu != null)
        {
            menu.SetActive(!menu.activeSelf);
        }
    }
}