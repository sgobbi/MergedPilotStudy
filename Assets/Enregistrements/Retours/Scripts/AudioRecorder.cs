using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    private AudioClip audioClip;
    private string microphoneDevice;
    private bool isRecording = false;

    // Référence à l'objet auquel on veut attacher le son
    public GameObject audioSourceObject;

    private AudioSource audioSource;

    private void Start()
    {
        // Assure-toi qu'il y a un AudioSource sur l'objet cible
        if (audioSourceObject == null)
        {
            Debug.LogError("Aucun objet source audio assigné !");
            return;
        }

        // Ajoute un AudioSource sur l'objet spécifié
        audioSource = audioSourceObject.AddComponent<AudioSource>();

        // Configure le composant AudioSource pour la spatialisation
        audioSource.spatialBlend = 1.0f;  // 1.0 signifie un son complètement 3D
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;  // Amortissement du son avec la distance
        audioSource.minDistance = 1f;  // Distance minimale pour entendre clairement
        audioSource.maxDistance = 15f;  // Distance maximale pour entendre

        // Sélectionne le premier micro disponible
        if (Microphone.devices.Length > 0)
        {
            microphoneDevice = Microphone.devices[0];
        }
        else
        {
            Debug.LogError("Aucun micro détecté !");
        }
    }

    private void Update()
    {
        // Appuie sur la touche "R" pour commencer ou arrêter l'enregistrement
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
            // Commence l'enregistrement avec une durée maximale de 60 secondes (modifiable)
            audioClip = Microphone.Start(microphoneDevice, false, 60, 44100);
            isRecording = true;
            Debug.Log("Enregistrement commencé...");
        }
    }

    private void StopRecording()
    {
        if (isRecording)
        {
            // Arrête l'enregistrement
            Microphone.End(microphoneDevice);
            isRecording = false;
            Debug.Log("Enregistrement terminé.");
        }
    }

    private void PlayRecording()
    {
        if (audioClip != null)
        {
            // Associe l'audio enregistré à l'AudioSource et joue-le
            audioSource.clip = audioClip;
            audioSource.Play();
            Debug.Log("Lecture de l'enregistrement depuis l'objet.");
        }
        else
        {
            Debug.LogWarning("Aucun enregistrement disponible à lire.");
        }
    }
}
