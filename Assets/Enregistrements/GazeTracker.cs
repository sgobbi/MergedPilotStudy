using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GazeTracker : MonoBehaviour
{
    public Camera playerCamera; // La cam�ra VR (repr�sentant la t�te)
    public float gazeThreshold = 1f; // Le temps minimum (en secondes) pour consid�rer qu'un objet a �t� regard� pendant un certain temps
    public string userName = "Utilisateur";

    private RaycastHit hitInfo;
    private float gazeTime = 0f;
    private GameObject currentGazedObject = null;
    private List<GazeData> gazeDataList = new List<GazeData>();

    private LineRenderer lineRenderer;  // Le LineRenderer pour afficher le rayon

    void Start()
    {
        // Ajouter un LineRenderer � cet objet pour dessiner le rayon
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    void Update()
    {
        // Cr�er un rayon partant du centre de la cam�ra (la t�te du joueur)
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        // Mettre � jour la position du LineRenderer pour afficher le rayon
        lineRenderer.SetPosition(0, playerCamera.transform.position);

        if (Physics.Raycast(ray, out hitInfo))  // Si le rayon touche un objet
        {
            GameObject hitObject = hitInfo.collider.gameObject;

            // Si le rayon touche un objet diff�rent de l'objet pr�c�dent
            if (currentGazedObject != hitObject)
            {
                // Si un objet �tait pr�c�demment regard�, on enregistre sa dur�e
                if (currentGazedObject != null && gazeTime >= gazeThreshold)
                {
                    RecordGazeData(currentGazedObject, gazeTime);
                }

                // R�initialiser le temps de regard pour le nouvel objet
                currentGazedObject = hitObject;
                gazeTime = 0f;
            }

            // Mettre � jour la position du LineRenderer (fin du rayon)
            lineRenderer.SetPosition(1, hitInfo.point);  // Positionner la fin du rayon au point d'impact

            // Incr�menter la dur�e de regard
            gazeTime += Time.deltaTime;
        }
        else
        {
            // Si aucun objet n'est touch�, afficher un rayon jusqu'� 10 unit�s de distance
            lineRenderer.SetPosition(1, ray.origin + ray.direction * 10f);  // Dessiner jusqu'� 10 unit�s

            // Si aucun objet n'est touch�, on r�initialise les variables
            if (currentGazedObject != null && gazeTime >= gazeThreshold)
            {
                RecordGazeData(currentGazedObject, gazeTime);
            }

            currentGazedObject = null;
            gazeTime = 0f;
        }
    }

    // Enregistre les donn�es de l'objet regard� dans une liste
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
        // D�finir le chemin du dossier pour enregistrer les donn�es JSON
        string folderPath = @"E:\Monologue_Koltes\Assets\Enregistrements\EnregistrementsHH";

        // Cr�er le dossier s'il n'existe pas d�j�
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // D�finir le chemin du fichier de donn�es de regard
        string filePath = Path.Combine(folderPath, $"{userName}_gazeData.json");

        // Convertir la liste de donn�es en JSON
        string json = JsonUtility.ToJson(new GazeDataList { data = gazeDataList }, true);

        // Sauvegarder les donn�es dans le fichier
        File.WriteAllText(filePath, json);

        Debug.Log($"Donn�es de regard enregistr�es dans : {filePath}");
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