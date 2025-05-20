using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class GazeTracker : MonoBehaviour
{
    public static GazeTracker Instance { get; private set; }
    public Camera playerCamera;
    public float gazeThreshold = 1f;
    public string userName = "Utilisateur";

    // 🏠 Liste d'exclusion par noms d'objets
    public List<string> excludedObjectNames = new List<string> { "Maison", "Mur", "Sol" };

    private RaycastHit hitInfo;
    private float gazeTime = 0f;
    private GameObject currentGazedObject = null;
    private List<GazeData> gazeDataList = new List<GazeData>();

    private LineRenderer lineRenderer;
    private string outputFolderPath;
    private string outputFilePath;

    private string timestamp;


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
    void Start()
    {
        timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        outputFolderPath = GeneralExperienceManager.Instance.experienceFolderPath + "/GazeTrackerFiles";
        userName = GeneralExperienceManager.Instance.userName;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    void Update()
    {
        if (playerCamera != null)
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            lineRenderer.SetPosition(0, playerCamera.transform.position);

            if (Physics.Raycast(ray, out hitInfo))
            {
                GameObject hitObject = hitInfo.collider.gameObject;

                if (currentGazedObject != hitObject)
                {
                    if (currentGazedObject != null && gazeTime >= gazeThreshold && !excludedObjectNames.Contains(currentGazedObject.name))
                    {
                    RecordGazeData(currentGazedObject, gazeTime);
                    }

                    currentGazedObject = hitObject;
                    gazeTime = 0f;
                }

                lineRenderer.SetPosition(1, hitInfo.point);
                gazeTime += Time.deltaTime;
            }
            else
            {
                lineRenderer.SetPosition(1, ray.origin + ray.direction * 10f);

                if (currentGazedObject != null && gazeTime >= gazeThreshold && !excludedObjectNames.Contains(currentGazedObject.name))
                {
                    RecordGazeData(currentGazedObject, gazeTime);
                }

                currentGazedObject = null;
                gazeTime = 0f;
            }
        }
        
    }

    private void RecordGazeData(GameObject gazedObject, float duration)
    {
        gazeDataList.Add(new GazeData
        {
            userName = userName,
            objectName = gazedObject.name,
            startTimestamp = Time.time - duration,
            endTimestamp = Time.time,
            gazeDuration = duration
        });
    }

    void OnApplicationQuit()
    {
        SaveGazeData("Last record before quit");
    }

    public void SaveGazeData(string title)
    {
        outputFolderPath = GeneralExperienceManager.Instance.experienceFolderPath + "/GazeTrackerFiles";

        string fileName = title + $"{userName}_gazeData.json";
        string filePath = Path.Combine(outputFolderPath, fileName);
        Debug.Log("output folder path: " + outputFolderPath + "  output file path: " + outputFilePath); 

        string json = JsonUtility.ToJson(new GazeDataList { Title = title, data = gazeDataList }, true);
        File.WriteAllText(filePath, json);

        Debug.Log($"Données de regard enregistrées dans : {filePath}");

        gazeDataList = new List<GazeData>(); // reset to empty after saving

    }

    public void ChangeCamera(Camera cam)
    {
        Debug.Log("camera changed"); 
        playerCamera = cam; 
    }

    [System.Serializable]
    public class GazeData
    {
        public string userName;
        public string objectName;
        public float startTimestamp;
        public float endTimestamp;
        public float gazeDuration;
    }

    [System.Serializable]
    public class GazeDataList
    {
        public string Title;
        public List<GazeData> data;
    }
    
}