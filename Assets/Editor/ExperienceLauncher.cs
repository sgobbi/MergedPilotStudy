using UnityEditor;
using UnityEngine;
using System.IO;

public class ExperienceLauncher : EditorWindow
{
    private int selectedOptionScene = 0;
    private int selectedOptionControle = 0;
    private string[] optionsScene = new string[] { "Scenographique", "Narratif", "Abstrait" };
    private string[] optionsControle = new string[] { "Scene imposée", "Scene modulable" };
    private string userName; 
    private string projectFolderPath = "";
    private string specificExperienceFolderPath = "";


    [MenuItem("Tools/Experience Launcher")]
    public static void ShowWindow()
    {
        GetWindow<ExperienceLauncher>("Experience Launcher");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Experience", EditorStyles.boldLabel);
        
        selectedOptionScene = EditorGUILayout.Popup("Regime esthetique", selectedOptionScene, optionsScene);
        selectedOptionControle = EditorGUILayout.Popup("Niveau de controle", selectedOptionControle, optionsControle);

        GUILayout.Space(10);
        GUILayout.Label("Select Save Folder", EditorStyles.boldLabel);

        GUILayout.Space(10);
        userName = EditorGUILayout.TextField("User Name", userName);

        EditorGUILayout.BeginHorizontal();
        projectFolderPath = EditorGUILayout.TextField("Folder Path", projectFolderPath);

       if (GUILayout.Button("Browse", GUILayout.MaxWidth(80)))
        {
            string path = EditorUtility.OpenFolderPanel("Select Folder to Save Files", "", "");
            if (!string.IsNullOrEmpty(path))
            {
                projectFolderPath = path;
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        if (GUILayout.Button("Launch Experience"))
        {
            if (string.IsNullOrEmpty(projectFolderPath) || !Directory.Exists(projectFolderPath))
            {
                EditorUtility.DisplayDialog("Invalid Path", "Please select a valid folder path before launching.", "OK");
                return;
            }

            specificExperienceFolderPath = projectFolderPath + "_" + optionsScene[selectedOptionScene] + "_" + optionsControle[selectedOptionControle] + "_" + userName; 

            LaunchSelectedExperience();
        }
    }

    private void LaunchSelectedExperience()
    {
        Debug.Log("Launching: " + optionsScene[selectedOptionScene] + " : " + optionsControle[selectedOptionControle]);

        // Example: Set up arguments or configurations
        switch (selectedOptionScene)
        {
            case 0:
                // Set ici les options pour la scene scenographique 
                EditorPrefs.SetString("ExperienceConfig", "A");
                break;
            case 1:
                // Set ici les options pour la scene narrative 
                EditorPrefs.SetString("ExperienceConfig", "B");
                break;
            case 2:
                // Set ici les options pour la scene abstraite 
                EditorPrefs.SetString("ExperienceConfig", "C");
                break;
        }

        // Optionally enter Play mode
        if (!EditorApplication.isPlaying)
        {
            EditorApplication.EnterPlaymode();
        }
    }
}
