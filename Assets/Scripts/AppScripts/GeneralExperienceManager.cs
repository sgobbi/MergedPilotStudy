#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Gestionnaire du déroulé de l'experience 
/// </summary>

public class GeneralExperienceManager : MonoBehaviour
{

    public static GeneralExperienceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private Queue<string> sceneQueue = new Queue<string>();

    private void Start()
    {
#if UNITY_EDITOR
        string experienceSceneType = EditorPrefs.GetString("ExperienceConfigScene", "Default");
        string experienceControlType = EditorPrefs.GetString("ExperienceConfigControle", "Default");
        string userName = EditorPrefs.GetString("ExperienceUserName", "Unknown");
        string folderPath = EditorPrefs.GetString("ExperienceFolderPath", Application.dataPath);
#endif

        Debug.Log("Starting experience for user: " + userName);
        Debug.Log("Config: " + experienceSceneType + "  " + experienceControlType);

        // Build the scene queue based on config
        switch (experienceSceneType)
        {
            case "Scenographique":
                sceneQueue.Enqueue("Subway Scene");
                break;

            case "Narratif":
                sceneQueue.Enqueue("Amphiteatre");
                break;

            case "Abstrait":
                sceneQueue.Enqueue("Fog Scene");
                break;
        }

        //LoadNextScene();
    }

    public void LoadNextScene()
    {
        if (sceneQueue.Count > 0)
        {
            string nextScene = sceneQueue.Dequeue();
            Debug.Log("Loading next scene: " + nextScene);
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.Log("Experience complete.");
        }
    }
}

