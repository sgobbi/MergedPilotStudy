using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Fait en sorte que quand on passe dans le play mode dans l'editeur, c'est toujours la scene de chargement qui apparaisse
/// </summary>

[InitializeOnLoad]
public static class AutoBootstrapLoader
{
    private const string bootstrapScenePath = "Assets/Scenes/LoadScene.unity"; // Change if your path is different

    static AutoBootstrapLoader()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        // if (state == PlayModeStateChange.ExitingEditMode)
        // {
        //     if (EditorSceneManager.GetActiveScene().path != bootstrapScenePath)
        //     {
        //         bool switchScene = EditorUtility.DisplayDialog(
        //             "Load Bootstrap Scene?",
        //             "Play Mode is about to start, but the active scene is not the Intro scene.\n\nDo you want to switch to the Intro scene?",
        //             "Yes", "No");

        //         if (switchScene)
        //         {
        //             if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        //             {
        //                 EditorSceneManager.OpenScene(bootstrapScenePath);
        //                 EditorApplication.isPlaying = false;

        //                 // Delay Play Mode so the user can hit Play again after switching
        //                 Debug.Log("Bootstrap scene loaded. Press Play again to run.");
        //             }
        //         }
        //     }
        // }
    }
}
