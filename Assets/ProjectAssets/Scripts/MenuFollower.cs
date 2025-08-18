using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menu; // Referencia al objeto menu

    // M�todo para activar/desactivar el men�
    public void ToggleMenu()
    {
        if (menu != null)
        {
            menu.SetActive(!menu.activeSelf);
        }
    }
}