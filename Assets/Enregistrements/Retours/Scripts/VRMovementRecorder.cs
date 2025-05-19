using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMovementRecorder : MonoBehaviour
{
    public Transform playerHead; // Référence à la tête (caméra VR)
    public Transform leftHand;   // Référence à la main gauche
    public Transform rightHand;  // Référence à la main droite
    public Transform body;       // Référence au corps

    private bool isRecording = false;
    public List<PlayerFrameData> recordedFrames = new List<PlayerFrameData>();

    [System.Serializable]
    public class PlayerFrameData
    {
        public Vector3 headPosition;
        public Quaternion headRotation;
        public Vector3 leftHandPosition;
        public Quaternion leftHandRotation;
        public Vector3 rightHandPosition;
        public Quaternion rightHandRotation;
        public Vector3 bodyPosition;
        public Quaternion bodyRotation;

        public PlayerFrameData(Vector3 headPos, Quaternion headRot, Vector3 leftPos, Quaternion leftRot, Vector3 rightPos, Quaternion rightRot, Vector3 bodyPos, Quaternion bodyRot)
        {
            headPosition = headPos;
            headRotation = headRot;
            leftHandPosition = leftPos;
            leftHandRotation = leftRot;
            rightHandPosition = rightPos;
            rightHandRotation = rightRot;
            bodyPosition = bodyPos;
            bodyRotation = bodyRot;
        }
    }

    void Update()
    {
        if (isRecording)
        {
            recordedFrames.Add(new PlayerFrameData(
                playerHead.position, playerHead.rotation,
                leftHand.position, leftHand.rotation,
                rightHand.position, rightHand.rotation,
                body.position, body.rotation
            ));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            isRecording = !isRecording;
            Debug.Log(isRecording ? "Enregistrement démarré" : "Enregistrement arrêté");
        }
    }
}
