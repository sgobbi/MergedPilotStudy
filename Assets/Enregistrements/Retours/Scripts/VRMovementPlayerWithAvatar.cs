using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMovementPlayerWithAvatar : MonoBehaviour
{
    public Transform avatarHead;     // Référence à la tête de l'avatar
    public Transform avatarLeftHand; // Référence à la main gauche de l'avatar
    public Transform avatarRightHand; // Référence à la main droite de l'avatar
    public Transform avatarBody;     // Référence au corps de l'avatar

    private List<VRMovementRecorder.PlayerFrameData> recordedFrames;
    private bool isPlaying = false;
    private int currentFrame = 0;

    void Start()
    {
        recordedFrames = FindObjectOfType<VRMovementRecorder>().recordedFrames;
    }

    void Update()
    {
        if (isPlaying && recordedFrames != null && recordedFrames.Count > 0)
        {
            var frame = recordedFrames[currentFrame];
            avatarHead.position = frame.headPosition;
            avatarHead.rotation = frame.headRotation;
            avatarLeftHand.position = frame.leftHandPosition;
            avatarLeftHand.rotation = frame.leftHandRotation;
            avatarRightHand.position = frame.rightHandPosition;
            avatarRightHand.rotation = frame.rightHandRotation;
            avatarBody.position = frame.bodyPosition;
            avatarBody.rotation = frame.bodyRotation;

            currentFrame++;

            if (currentFrame >= recordedFrames.Count)
            {
                isPlaying = false;
                currentFrame = 0;
                Debug.Log("Lecture terminée");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isPlaying = true;
            currentFrame = 0;
            Debug.Log("Lecture démarrée");
        }
    }
}
