using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GazeTracker : MonoBehaviour
{
    public Camera playerCamera; // La caméra VR (représentant la tête)
    public float gazeThreshold = 1f; // Le temps minimum (en secondes) pour considérer qu'un objet a été regardé pendant un certain temps
    public string userName = "Utilisateur";

    private RaycastHit hitInfo;
    private float gazeTime = 0f;
    private GameObject currentGazedObject = null;
    private List<GazeData> gazeDataList = new List<GazeData>();

    private LineRenderer lineRenderer;  // Le LineRenderer pour afficher le rayon

    void Start()
    {
        // Ajouter un LineRenderer à cet objet pour dessiner le rayon
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    void Update()
    {
        // Créer un rayon partant du centre de la caméra (la tête du joueur)
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        // Mettre à jour la position du LineRenderer pour afficher le rayon
        lineRenderer.SetPosition(0, playerCamera.transform.position);

        if (Physics.Raycast(ray, out hitInfo))  // Si le rayon touche un objet
        {
            GameObject hitObject = hitInfo.collider.gameObject;

            // Si le rayon touche un objet différent de l'objet précédent
            if (currentGazedObject != hitObject)
            {
                // Si un objet était précédemment regardé, on enregistre sa durée
                if (currentGazedObject != null && gazeTime >= gazeThreshold)
                {
                    RecordGazeData(currentGazedObject, gazeTime);
                }

                // Réinitialiser le temps de regard pour le nouvel objet
                currentGazedObject = hitObject;
                gazeTime = 0f;
            }

            // Mettre à jour la position du LineRenderer (fin du rayon)
            lineRenderer.SetPosition(1, hitInfo.point);  // Positionner la fin du rayon au point d'impact

            // Incrémenter la durée de regard
            gazeTime += Time.deltaTime;
        }
        else
        {
            // Si aucun objet n'est touché, afficher un rayon jusqu'à 10 unités de distance
            lineRenderer.SetPosition(1, ray.origin + ray.direction * 10f);  // Dessiner jusqu'à 10 unités

            // Si aucun objet n'est touché, on réinitialise les variables
            if (currentGazedObject != null && gazeTime >= gazeThreshold)
            {
                RecordGazeData(currentGazedObject, gazeTime);
            }

            currentGazedObject = null;
            gazeTime = 0f;
        }
    }

    // Enregistre les données de l'objet regardé dans une liste
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
        // Définir le chemin du dossier pour enregistrer les données JSON
        string folderPath = @"E:\Monologue_Koltes\Assets\Enregistrements\EnregistrementsHH";

        // Créer le dossier s'il n'existe pas déjà
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Définir le chemin du fichier de données de regard
        string filePath = Path.Combine(folderPath, $"{userName}_gazeData.json");

        // Convertir la liste de données en JSON
        string json = JsonUtility.ToJson(new GazeDataList { data = gazeDataList }, true);

        // Sauvegarder les données dans le fichier
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