using UnityEditor;
using UnityEngine;
using System.IO;

/// <summary>
/// Gestion du menu in Edit mode pour lancer l'experience
/// </summary>

public class ExperienceLauncher : EditorWindow
{
    private int selectedOptionScene = 0;
    private int selectedOptionControle = 0;
    private string[] optionsScene = new string[] { "Scenographique", "Narratif", "Abstrait" };
    private string[] optionsControle = new string[] { "Scene imposée", "Scene modulable" };
    private string userName;
    private string projectFolderPath = "";
    private string specificExperienceFolderPath = "";

    private string GazeTrackerFolder = "";
    private string PositionTrackerFolder = "";


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
                Debug.Log("folder path: " + projectFolderPath); 
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
            Debug.Log("folder path: " + projectFolderPath);
            specificExperienceFolderPath = projectFolderPath + "/" + optionsScene[selectedOptionScene] + "_" + optionsControle[selectedOptionControle] + "_" + userName;

            if (!Directory.Exists(specificExperienceFolderPath))
            {
                Directory.CreateDirectory(specificExperienceFolderPath);
                Debug.Log("Created folder: " + specificExperienceFolderPath);
            }
            else
            {
                Debug.Log("Attention! Le dossier dans lequel vous voulez sauvegarder existe deja, les fichiers risquent d'etre ecrasés"); 
            }

            GazeTrackerFolder = specificExperienceFolderPath + "/GazeTrackerFiles";
            PositionTrackerFolder = specificExperienceFolderPath + "/VRPositionTrackerFiles";

            if (!Directory.Exists(GazeTrackerFolder))
            {
                Directory.CreateDirectory(GazeTrackerFolder);
            }

            if (!Directory.Exists(PositionTrackerFolder))
            {
                Directory.CreateDirectory(PositionTrackerFolder);
            }

            LaunchSelectedExperience();
        }
    }

    private void LaunchSelectedExperience()
    {
        Debug.Log("Launching: " + optionsScene[selectedOptionScene] + " : " + optionsControle[selectedOptionControle]);
        EditorPrefs.SetString("ExperienceUserName", userName);
        EditorPrefs.SetString("ExperienceFolderPath", specificExperienceFolderPath);

        // Example: Set up arguments or configurations
        switch (selectedOptionScene)
        {
            case 0:
                // Set ici les options pour la scene scenographique 
                EditorPrefs.SetString("ExperienceConfigScene", "Scenographique");
                Debug.Log("Changed ExperienceConfigScene to Scenogrpahique"); 
                break;
            case 1:
                // Set ici les options pour la scene narrative 
                EditorPrefs.SetString("ExperienceConfigScene", "Narratif");
                Debug.Log("Changed ExperienceConfigScene to Narratif"); 
                break;
            case 2:
                // Set ici les options pour la scene abstraite 
                EditorPrefs.SetString("ExperienceConfigScene", "Abstrait");
                break;
        }

        switch (selectedOptionControle)
        {
            case 0:
                // Set ici les options pour la scene scenographique 
                EditorPrefs.SetString("ExperienceConfigControle", "Imposé");
                break;
            case 1:
                // Set ici les options pour la scene narrative 
                EditorPrefs.SetString("ExperienceConfigControle", "Modifiable");
                break;
        }

        // Optionally enter Play mode
        if (!EditorApplication.isPlaying)
        {
            EditorApplication.EnterPlaymode();
        }
    }
}
