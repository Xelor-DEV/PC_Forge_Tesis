using UnityEngine;

public class TutorialSceneController : MonoBehaviour
{
    [Header("Main Menu Scene Name")]
    [SerializeField, Tooltip("The name of the scene to be placed in this variable is that of the main menu.")] private string sceneName = "MenuPrincipal";
    [Header("References")]
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private int sceneUnlocked; 

    void Start()
    {
        CheckTutorialCompletion();
    }

    private void CheckTutorialCompletion()
    {
        bool isManual = SceneLoader.isManualTutorialLoad;
        SceneLoader.isManualTutorialLoad = false; // Resetear inmediatamente después de verificar

        if (SaveManager.Instance.UnlockSystem.IsLevelUnlocked(sceneUnlocked) && isManual == false)
        {
            sceneLoader.LoadSceneWithoutFade(sceneName);
        }
    }
}