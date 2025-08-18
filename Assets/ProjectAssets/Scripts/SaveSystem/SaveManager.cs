using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [SerializeField] private LevelUnlockSystem unlockSystem;
    private LevelProgress _currentProgress;

    public LevelUnlockSystem UnlockSystem
    {
        get
        {
            return unlockSystem;
        }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            LoadGame();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        _currentProgress.unlockedLevels = unlockSystem.CurrentUnlockedLevels;
        SaveSystem.SaveGame(_currentProgress);
    }

    public void LoadGame()
    {
        _currentProgress = SaveSystem.LoadGame();
        unlockSystem.Initialize(_currentProgress.unlockedLevels);
    }

    public void DeleteSave()
    {
        SaveSystem.DeleteSave();
        LoadGame();
    }

    public void UnlockLevel(int level)
    {
        SaveManager.Instance.UnlockSystem.UnlockLevel(level);
    }
}