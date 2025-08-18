#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class SaveSystemEditor : EditorWindow
{
    private LevelProgress loadedProgress;
    private Vector2 scrollPosition;
    private string statusMessage = "";
    private Color statusColor = Color.white;

    [MenuItem("Tools/Save System Manager")]
    public static void ShowWindow()
    {
        GetWindow<SaveSystemEditor>("Save Manager");
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Sección de estado del archivo
        GUILayout.Label("Save File Status", EditorStyles.boldLabel);
        bool saveExists = File.Exists(SaveSystem.GetSavePath());

        GUILayout.Label($"Save File Exists: {saveExists}");
        if (saveExists)
        {
            GUILayout.Label($"File Name: {Path.GetFileName(SaveSystem.GetSavePath())}");
            GUILayout.Label($"Location: {SaveSystem.GetSavePath()}");
        }

        // Sección de carga/edición
        GUILayout.Space(10);
        GUILayout.Label("Load & Edit", EditorStyles.boldLabel);

        if (GUILayout.Button("Load Save File"))
        {
            LoadSaveData();
        }

        if (loadedProgress != null)
        {
            loadedProgress.unlockedLevels = EditorGUILayout.IntField(
                "Unlocked Levels",
                loadedProgress.unlockedLevels
            );

            if (GUILayout.Button("Save Modified Data"))
            {
                SaveSystem.SaveGame(loadedProgress);
                SaveManager.Instance.LoadGame();
                ShowStatus("Save modified successfully!", Color.green);
            }
        }

        // Sección de operaciones
        GUILayout.Space(20);
        GUILayout.Label("Save Operations", EditorStyles.boldLabel);

        if (GUILayout.Button("Create New Save"))
        {
            SaveSystem.SaveGame(new LevelProgress());
            LoadSaveData();
            ShowStatus("New save created!", Color.green);
        }

        if (GUILayout.Button("Delete Save"))
        {
            SaveSystem.DeleteSave();
            loadedProgress = null;
            ShowStatus("Save deleted!", Color.yellow);
        }

        // Mensaje de estado
        GUILayout.Space(20);
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
        GUI.contentColor = statusColor;
        GUILayout.Label(statusMessage);
        GUI.contentColor = Color.white;

        EditorGUILayout.EndScrollView();
    }

    private void LoadSaveData()
    {
        try
        {
            loadedProgress = SaveSystem.LoadGame();
            ShowStatus("Save loaded successfully!", Color.green);
        }
        catch (System.Exception e)
        {
            loadedProgress = null;
            ShowStatus($"Load failed: {e.Message}", Color.red);
        }
    }

    private void ShowStatus(string message, Color color)
    {
        statusMessage = message;
        statusColor = color;
    }
}
#endif