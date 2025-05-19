using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class VRTracker : MonoBehaviour
{
    public string userName = "Utilisateur";

    public Transform headObject;
    public Transform leftHandObject;
    public Transform rightHandObject;

    private Vector3 lastHeadPos;
    private Vector3 lastLeftHandPos;
    private Vector3 lastRightHandPos;

    private List<MovementData> movementDataList = new List<MovementData>();

    public float recordInterval = 0.1f;
    private float lastRecordTime = 0f;

    private string timestamp;

    void Start()
    {
        timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");

        lastHeadPos = headObject.position;
        lastLeftHandPos = leftHandObject.position;
        lastRightHandPos = rightHandObject.position;
    }

    void Update()
    {
        float delta = Time.deltaTime;
        if (delta <= Mathf.Epsilon) return;

        if (Time.time - lastRecordTime >= recordInterval)
        {
            Vector3 headPosition = headObject.position;
            Vector3 leftHandPosition = leftHandObject.position;
            Vector3 rightHandPosition = rightHandObject.position;

            Vector3 headVelocity = (headPosition - lastHeadPos) / delta;
            Vector3 leftHandVelocity = (leftHandPosition - lastLeftHandPos) / delta;
            Vector3 rightHandVelocity = (rightHandPosition - lastRightHandPos) / delta;

            Vector3 headAcceleration = (headVelocity - (lastHeadPos - headPosition) / delta) / delta;
            Vector3 leftHandAcceleration = (leftHandVelocity - (lastLeftHandPos - leftHandPosition) / delta) / delta;
            Vector3 rightHandAcceleration = (rightHandVelocity - (lastRightHandPos - rightHandPosition) / delta) / delta;

            movementDataList.Add(new MovementData
            {
                userName = userName,
                timestamp = Time.time,
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

            lastHeadPos = headPosition;
            lastLeftHandPos = leftHandPosition;
            lastRightHandPos = rightHandPosition;

            lastRecordTime = Time.time;
        }
    }

    void OnApplicationQuit()
    {
        string folderPath = @"E:\Monologue_Koltes\Assets\Enregistrements\EnregistrementsHH";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fileName = $"{userName}_movementData_{timestamp}.json";
        string filePath = Path.Combine(folderPath, fileName);

        string json = JsonUtility.ToJson(new MovementDataList { data = movementDataList }, true);
        File.WriteAllText(filePath, json);

        Debug.Log($"Données de mouvement enregistrées dans : {filePath}");
    }

    [System.Serializable]
    public class MovementData
    {
        public string userName;
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