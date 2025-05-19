using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMovementRecorder : MonoBehaviour
{
    public Transform playerHead; // R�f�rence � la t�te (cam�ra VR)
    public Transform leftHand;   // R�f�rence � la main gauche
    public Transform rightHand;  // R�f�rence � la main droite
    public Transform body;       // R�f�rence au corps

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
            Debug.Log(isRecording ? "Enregistrement d�marr�" : "Enregistrement arr�t�");
        }
    }
}
