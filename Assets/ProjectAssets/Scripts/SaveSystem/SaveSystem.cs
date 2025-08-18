using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string savePath = Path.Combine(Application.persistentDataPath, "gameSave.data");

    public static void SaveGame(LevelProgress data)
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                formatter.Serialize(stream, data);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving game: {e.Message}");
        }
    }

    public static LevelProgress LoadGame()
    {
        if (File.Exists(savePath))
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(savePath, FileMode.Open))
                {
                    return (LevelProgress)formatter.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading game: {e.Message}");
                return new LevelProgress();
            }
        }
        return new LevelProgress();
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            try
            {
                File.Delete(savePath);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error deleting save: {e.Message}");
            }
        }          
    }
    public static string GetSavePath()
    {
        return savePath;
    }
}

[Serializable]
public class LevelProgress
{
    public int unlockedLevels = 1;
}