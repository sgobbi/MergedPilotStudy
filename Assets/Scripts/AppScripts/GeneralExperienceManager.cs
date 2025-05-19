#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

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
        string experienceType = EditorPrefs.GetString("ExperienceConfig", "Default");
        string userName = EditorPrefs.GetString("ExperienceUserName", "Unknown");
        string folderPath = EditorPrefs.GetString("ExperienceFolderPath", Application.dataPath);
        #endif
        
        Debug.Log("Starting experience for user: " + userName);
        Debug.Log("Config: " + experienceType);

        // Build the scene queue based on config
        switch (experienceType)
        {
            case "Experience A":
                sceneQueue.Enqueue("IntroScene");
                sceneQueue.Enqueue("TrainingScene");
                sceneQueue.Enqueue("TestScene");
                sceneQueue.Enqueue("OutroScene");
                break;

            case "Experience B":
                sceneQueue.Enqueue("IntroScene");
                sceneQueue.Enqueue("TestScene");
                sceneQueue.Enqueue("OutroScene");
                break;

            case "Experience C":
                sceneQueue.Enqueue("IntroScene");
                sceneQueue.Enqueue("FreeExploreScene");
                sceneQueue.Enqueue("OutroScene");
                break;
        }

        LoadNextScene();
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

