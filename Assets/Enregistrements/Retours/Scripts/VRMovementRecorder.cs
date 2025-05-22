using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMovementRecorder : MonoBehaviour
{
    public Transform playerHead; // Référence à la tête (caméra VR)
    public Transform leftHand;   // Référence à la main gauche
    public Transform rightHand;  // Référence à la main droite

    [Header("Microphone Settings")]
    public string selectedMicrophone = "";
    private string microphoneDevice;
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

        public PlayerFrameData(Vector3 headPos, Quaternion headRot, Vector3 leftPos, Quaternion leftRot, Vector3 rightPos, Quaternion rightRot)
        {
            headPosition = headPos;
            headRotation = headRot;
            leftHandPosition = leftPos;
            leftHandRotation = leftRot;
            rightHandPosition = rightPos;
            rightHandRotation = rightRot;
        }
    }

    private AudioSource audioSource;
    public AudioClip audioClip; // Clip global à utiliser

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            if (string.IsNullOrEmpty(selectedMicrophone))
            {
                microphoneDevice = Microphone.devices[0];
            }
            else
            {
                if (System.Array.Exists(Microphone.devices, device => device == selectedMicrophone))
                {
                    microphoneDevice = selectedMicrophone;
                }
                else
                {
                    Debug.LogError($"Le micro '{selectedMicrophone}' n'est pas disponible !");
                    microphoneDevice = Microphone.devices[0];
                }
            }
        }
        else
        {
            Debug.LogError("Aucun micro détecté !");
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 15f;
    }

    void Update()
    {
        if (isRecording)
        {
            recordedFrames.Add(new PlayerFrameData(
                playerHead.position, playerHead.rotation,
                leftHand.position, leftHand.rotation,
                rightHand.position, rightHand.rotation
            ));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isRecording)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }
        }
    }

    private void StartRecording()
    {
        if (microphoneDevice != null)
        {
            if (Microphone.IsRecording(microphoneDevice))
            {
                Microphone.End(microphoneDevice);
            }

            recordedFrames.Clear();
            audioClip = Microphone.Start(microphoneDevice, false, 999, 44100);
            isRecording = true;
            Debug.Log("Enregistrement du mouvement et du son commencé...");
        }
    }

    private void StopRecording()
    {
        if (isRecording)
        {
            Microphone.End(microphoneDevice);
            isRecording = false;
            Debug.Log("Enregistrement du mouvement et du son terminé.");
        }
    }
}