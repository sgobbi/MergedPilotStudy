#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;
using System;

/// <summary>
/// Gestionnaire du déroulé de l'experience 
/// </summary>

public class GeneralExperienceManager : MonoBehaviour
{

    public static GeneralExperienceManager Instance { get; private set; }
    private Stopwatch stopwatch;
    private List<TimedEvent> timedEvents = new List<TimedEvent>();
    public string experienceSceneType;
    public string experienceControlType;
    public string userName;
    public string experienceFolderPath;
    public string VRquestionsPath;
    private string ExplorationTimesFilePath;


    [System.Serializable]
    public class TimedEvent
    {
        public string label;
        public float timeInSeconds;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        stopwatch = new Stopwatch();
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
        if (experienceControlType == "Impose")
        {
            switch (experienceSceneType)
            {
                case "Scenographique":
                    sceneQueue.Enqueue("SubwaySceneFixe");
                    sceneQueue.Enqueue("Questionnaire");
                    break;

                case "Narratif":
                    sceneQueue.Enqueue("AmphitheatreSceneFixe");
                    sceneQueue.Enqueue("Questionnaire");
                    break;

                case "Abstrait":
                    sceneQueue.Enqueue("FogSceneFixe");
                    sceneQueue.Enqueue("Questionnaire");
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
                    sceneQueue.Enqueue("AmphitheatreSceneModifiable");
                    sceneQueue.Enqueue("Questionnaire");
                    break;

                case "Abstrait":
                    sceneQueue.Enqueue("FogSceneModifiable");
                    sceneQueue.Enqueue("Questionnaire");
                    break;
            }
        }

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string ExplorationTimesFolder = experienceFolderPath + "/ExplorationTimes";
        ExplorationTimesFilePath = Path.Combine(ExplorationTimesFolder, $"TimerLog_{userName}_{timestamp}.json");

        StartNewTimer();

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

    public void StartNewTimer()
    {
        stopwatch.Restart();
        Debug.Log("Timer started.");
    }

    public void LogEvent(string label)
    {
        stopwatch.Stop();
        float elapsedSeconds = (float)stopwatch.Elapsed.TotalSeconds;
        timedEvents.Add(new TimedEvent { label = label, timeInSeconds = elapsedSeconds });

        Debug.Log($"Logged event '{label}' at {elapsedSeconds} seconds.");

        SaveEventsToFile();
        stopwatch.Restart(); // start new timing cycle
    }

    private void SaveEventsToFile()
    {
        string json = JsonUtility.ToJson(new TimedEventListWrapper { events = timedEvents }, true);
        File.WriteAllText(ExplorationTimesFilePath, json);
        Debug.Log($"Timer log saved at {ExplorationTimesFilePath}");
    }

    [System.Serializable]
    private class TimedEventListWrapper
    {
        public List<TimedEvent> events;
    }

}

