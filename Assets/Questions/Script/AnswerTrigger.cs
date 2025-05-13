using UnityEngine;

public class AnswerTrigger : MonoBehaviour
{
    public bool isYes; // Coche dans l'inspecteur : ce cube représente Oui ?

    private QuestionManager questionManager;

    void Start()
    {
        questionManager = FindObjectOfType<QuestionManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand")) // La main doit avoir ce tag
        {
            questionManager.OnAnswer(isYes);
        }
    }
}