using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMovementPlayerWithAvatar : MonoBehaviour
{
    public Transform avatarHead;
    public Transform avatarLeftHand;
    public Transform avatarRightHand;

    private List<VRMovementRecorder.PlayerFrameData> recordedFrames;
    private VRMovementRecorder recorder;

    private bool isPlaying = false;
    private int currentFrame = 0;
    private AudioSource audioSource;

    void Start()
    {
        recorder = FindObjectOfType<VRMovementRecorder>();
        recordedFrames = recorder.recordedFrames;
        audioSource = gameObject.AddComponent<AudioSource>();
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
            if (recorder.audioClip != null)
            {
                audioSource.Stop();
                audioSource.clip = recorder.audioClip;
                audioSource.Play();
            }

            recordedFrames = recorder.recordedFrames;
            isPlaying = true;
            currentFrame = 0;
            Debug.Log("Lecture démarrée");
        }
    }
}