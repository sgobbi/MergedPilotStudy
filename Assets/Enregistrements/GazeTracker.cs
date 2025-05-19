using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GazeTracker : MonoBehaviour
{
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

    private string timestamp;

    void Start()
    {
        timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    void Update()
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
        string folderPath = @"E:\Monologue_Koltes\Assets\Enregistrements\EnregistrementsHH";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fileName = $"{userName}_gazeData_{timestamp}.json";
        string filePath = Path.Combine(folderPath, fileName);

        string json = JsonUtility.ToJson(new GazeDataList { data = gazeDataList }, true);
        File.WriteAllText(filePath, json);

        Debug.Log($"Données de regard enregistrées dans : {filePath}");
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
        public List<GazeData> data;
    }
}