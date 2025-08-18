using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private LevelUnlockSystem unlockSystem;

    [Header("UI Elements")]
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private GameObject[] panels;

    private int currentPanel = 0;

    private void Start()
    {
        InitializePanels();
        RefreshMenu();
    }

    private void InitializePanels()
    {
        for (int i = 0; i < panels.Length; ++i)
        {
            panels[i].SetActive(i == 0);
        }
        currentPanel = 0;
    }

    public void ChangePanel(int panelIndex)
    {
        if (panelIndex < 0 || panelIndex >= panels.Length || panelIndex == currentPanel)
            return;

        panels[currentPanel].SetActive(false);
        panels[panelIndex].SetActive(true);
        currentPanel = panelIndex;
    }

    public void RefreshMenu()
    {
        for (int i = 0; i < levelButtons.Length; ++i)
        {
            int levelIndex = i + 1;
            levelButtons[i].interactable = unlockSystem.IsLevelUnlocked(levelIndex);
        }
    }
}
