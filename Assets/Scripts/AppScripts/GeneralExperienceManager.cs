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
    public string experienceSceneType;
    public string experienceControlType;
    public string userName;
    public string experienceFolderPath;
    public string VRquestionsPath; 

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
        experienceSceneType = EditorPrefs.GetString("ExperienceConfigScene", "Default");
        experienceControlType = EditorPrefs.GetString("ExperienceConfigControle", "Default");
        userName = EditorPrefs.GetString("ExperienceUserName", "Unknown");
        experienceFolderPath = EditorPrefs.GetString("ExperienceFolderPath", Application.dataPath);
#endif

        Debug.Log("Starting experience for user: " + userName);
        Debug.Log("Config: " + experienceSceneType + "  " + experienceControlType);

        // Build the scene queue based on config
        if (experienceControlType == "Scene imposee")
        {
            switch (experienceSceneType)
            {
                case "Scenographique":
                    sceneQueue.Enqueue("SubwaySceneFixe");
                    sceneQueue.Enqueue("Questionnaire");
                    break;

                case "Narratif":
                    sceneQueue.Enqueue("AmphiteatreSceneFixe");
                    sceneQueue.Enqueue("Questionnaire");
                    break;

                case "Abstrait":
                    sceneQueue.Enqueue("Fog Scene");
                    break;
            }
        }
        else
        {
           switch (experienceSceneType)
            {
                case "Scenographique":
                    sceneQueue.Enqueue("SubwaySceneModifiable");
                    sceneQueue.Enqueue("Questionnaire");
                    break;

                case "Narratif":
                    sceneQueue.Enqueue("AmphiteatreSceneModifiable");
                    sceneQueue.Enqueue("Questionnaire");
                    break;

                case "Abstrait":
                    sceneQueue.Enqueue("Fog Scene");
                    sceneQueue.Enqueue("Questionnaire");
                    break;
            } 
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

