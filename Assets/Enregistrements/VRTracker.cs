using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class VRTracker : MonoBehaviour
{
    // Ajout du nom de l'utilisateur
    public string userName = "Utilisateur";

    // Objets de remplacement (placeholders)
    public Transform headObject;  // Remplacer par votre cam�ra VR une fois la configuration termin�e
    public Transform leftHandObject;  // Remplacer par le contr�leur gauche VR
    public Transform rightHandObject;  // Remplacer par le contr�leur droit VR

    private Vector3 lastHeadPos;
    private Vector3 lastLeftHandPos;
    private Vector3 lastRightHandPos;

    private List<MovementData> movementDataList = new List<MovementData>();

    // Pour ajuster la fr�quence d'enregistrement (en secondes)
    public float recordInterval = 0.1f;  // Enregistrer les donn�es toutes les 0.1 secondes
    private float lastRecordTime = 0f;

    void Start()
    {
        // Initialisation des derni�res positions
        lastHeadPos = headObject.position;
        lastLeftHandPos = leftHandObject.position;
        lastRightHandPos = rightHandObject.position;
    }

    void Update()
    {
        // V�rifier si c'est le bon moment pour enregistrer
        if (Time.time - lastRecordTime >= recordInterval)
        {
            // R�cup�rer les positions des objets (placeholders pour le moment)
            Vector3 headPosition = headObject.position;
            Vector3 leftHandPosition = leftHandObject.position;
            Vector3 rightHandPosition = rightHandObject.position;

            // Calcul de la vitesse et de l'acc�l�ration
            Vector3 headVelocity = (headPosition - lastHeadPos) / Time.deltaTime;
            Vector3 leftHandVelocity = (leftHandPosition - lastLeftHandPos) / Time.deltaTime;
            Vector3 rightHandVelocity = (rightHandPosition - lastRightHandPos) / Time.deltaTime;

            Vector3 headAcceleration = (headVelocity - (lastHeadPos - headObject.position) / Time.deltaTime) / Time.deltaTime;
            Vector3 leftHandAcceleration = (leftHandVelocity - (lastLeftHandPos - leftHandObject.position) / Time.deltaTime) / Time.deltaTime;
            Vector3 rightHandAcceleration = (rightHandVelocity - (lastRightHandPos - rightHandObject.position) / Time.deltaTime) / Time.deltaTime;

            // Ajouter les donn�es � la liste avec un timestamp ajust�
            movementDataList.Add(new MovementData
            {
                userName = userName,  // Ajouter le nom de l'utilisateur
                timestamp = Time.time,  // Utiliser Time.time comme timestamp
                headPosition = headPosition,
                leftHandPosition = leftHandPosition,
                rightHandPosition = rightHandPosition,
                headVelocity = headVelocity,
                leftHandVelocity = leftHandVelocity,
                rightHandVelocity = rightHandVelocity,
                headAcceleration = headAcceleration,
                leftHandAcceleration = leftHandAcceleration,
                rightHandAcceleration = rightHandAcceleration
            });

            // Sauvegarder les anciennes positions pour les calculs futurs
            lastHeadPos = headPosition;
            lastLeftHandPos = leftHandPosition;
            lastRightHandPos = rightHandPosition;

            // Mettre � jour le dernier enregistrement de donn�es
            lastRecordTime = Time.time;
        }
    }

    void OnApplicationQuit()
    {
        // D�finir le chemin du dossier o� vous voulez enregistrer le fichier JSON
        string folderPath = @"E:\Monologue_Koltes\Assets\Enregistrements\EnregistrementsHH";

        // V�rifier si le dossier existe et le cr�er s'il n'existe pas
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // D�finir le chemin complet du fichier
        string filePath = Path.Combine(folderPath, $"{userName}_movementData.json");

        // Convertir la liste en format JSON
        string json = JsonUtility.ToJson(new MovementDataList { data = movementDataList }, true);

        // Sauvegarder dans le fichier
        File.WriteAllText(filePath, json);

        Debug.Log($"Donn�es enregistr�es dans : {filePath}");
    }

    [System.Serializable]
    public class MovementData
    {
        public string userName;  // Nom de l'utilisateur
        public float timestamp;
        public Vector3 headPosition;
        public Vector3 leftHandPosition;
        public Vector3 rightHandPosition;
        public Vector3 headVelocity;
        public Vector3 leftHandVelocity;
        public Vector3 rightHandVelocity;
        public Vector3 headAcceleration;
        public Vector3 leftHandAcceleration;
        public Vector3 rightHandAcceleration;
    }

    [System.Serializable]
    public class MovementDataList
    {
        public List<MovementData> data;
    }
}