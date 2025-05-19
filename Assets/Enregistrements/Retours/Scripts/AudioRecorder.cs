using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    private AudioClip audioClip;
    private string microphoneDevice;
    private bool isRecording = false;

    // R�f�rence � l'objet auquel on veut attacher le son
    public GameObject audioSourceObject;

    private AudioSource audioSource;

    private void Start()
    {
        // Assure-toi qu'il y a un AudioSource sur l'objet cible
        if (audioSourceObject == null)
        {
            Debug.LogError("Aucun objet source audio assign� !");
            return;
        }

        // Ajoute un AudioSource sur l'objet sp�cifi�
        audioSource = audioSourceObject.AddComponent<AudioSource>();

        // Configure le composant AudioSource pour la spatialisation
        audioSource.spatialBlend = 1.0f;  // 1.0 signifie un son compl�tement 3D
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;  // Amortissement du son avec la distance
        audioSource.minDistance = 1f;  // Distance minimale pour entendre clairement
        audioSource.maxDistance = 15f;  // Distance maximale pour entendre

        // S�lectionne le premier micro disponible
        if (Microphone.devices.Length > 0)
        {
            microphoneDevice = Microphone.devices[0];
        }
        else
        {
            Debug.LogError("Aucun micro d�tect� !");
        }
    }

    private void Update()
    {
        // Appuie sur la touche "R" pour commencer ou arr�ter l'enregistrement
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

        // Appuie sur la touche "P" pour rejouer l'enregistrement
        if (Input.GetKeyDown(KeyCode.R) && audioClip != null && !isRecording)
        {
            PlayRecording();
        }
    }

    private void StartRecording()
    {
        if (microphoneDevice != null)
        {
            // Commence l'enregistrement avec une dur�e maximale de 60 secondes (modifiable)
            audioClip = Microphone.Start(microphoneDevice, false, 60, 44100);
            isRecording = true;
            Debug.Log("Enregistrement commenc�...");
        }
    }

    private void StopRecording()
    {
        if (isRecording)
        {
            // Arr�te l'enregistrement
            Microphone.End(microphoneDevice);
            isRecording = false;
            Debug.Log("Enregistrement termin�.");
        }
    }

    private void PlayRecording()
    {
        if (audioClip != null)
        {
            // Associe l'audio enregistr� � l'AudioSource et joue-le
            audioSource.clip = audioClip;
            audioSource.Play();
            Debug.Log("Lecture de l'enregistrement depuis l'objet.");
        }
        else
        {
            Debug.LogWarning("Aucun enregistrement disponible � lire.");
        }
    }
}
