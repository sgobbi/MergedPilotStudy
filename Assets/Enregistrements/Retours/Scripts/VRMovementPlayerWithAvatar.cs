using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMovementPlayerWithAvatar : MonoBehaviour
{
    public Transform avatarHead;     // R�f�rence � la t�te de l'avatar
    public Transform avatarLeftHand; // R�f�rence � la main gauche de l'avatar
    public Transform avatarRightHand; // R�f�rence � la main droite de l'avatar
    public Transform avatarBody;     // R�f�rence au corps de l'avatar

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
                Debug.Log("Lecture termin�e");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isPlaying = true;
            currentFrame = 0;
            Debug.Log("Lecture d�marr�e");
        }
    }
}
