using UnityEngine;

[CreateAssetMenu(menuName = "Save System/Level Unlock System")]
public class LevelUnlockSystem : ScriptableObject
{
    [SerializeField] private int currentUnlockedLevels = 1;

    public int CurrentUnlockedLevels
    {
        get
        {
            return currentUnlockedLevels;
        }
        private set
        {
            currentUnlockedLevels = value;
        }
    }

    public void Initialize(int unlockedLevels)
    {
        if (unlockedLevels < 1)
        {
            unlockedLevels = 1; // Asegura que al menos un nivel esté desbloqueado
        }

        CurrentUnlockedLevels = unlockedLevels;
    }


    public bool IsLevelUnlocked(int level)
    {
        // Compara el nivel recibido como argumento con el nivel máximo desbloqueado actualmente
        if (level <= CurrentUnlockedLevels)
        {
            // Si el nivel es menor o igual que el nivel desbloqueado actual, devuelve verdadero
            return true;
        }
        else
        {
            // Si el nivel es mayor que el nivel desbloqueado, devuelve falso
            return false;
        }
    }

    public void UnlockLevel(int level)
    {
        if (level > CurrentUnlockedLevels)
        {
            CurrentUnlockedLevels = level;
            SaveManager.Instance.SaveGame();
        }
    }
}