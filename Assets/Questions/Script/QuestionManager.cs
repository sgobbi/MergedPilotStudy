using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System;

public class QuestionManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string libelleMin;
        public string libelleMax;
    }

    [System.Serializable]
    public class AnswerData
    {
        public string utilisateur;
        public string question;
        public int valeur;  // 1 � 8
        public string horodatage;
    }

    [Header("R�f�rences UI")]
    public TextMeshProUGUI questionTextUI;
    public TextMeshProUGUI libelleMinUI;
    public TextMeshProUGUI libelleMaxUI;

    [Header("Param�tres du questionnaire")]
    public string nomUtilisateur = "Participant";
    public Question[] questions;

    private int currentQuestionIndex = 0;
    private bool isWaiting = false;
    private List<AnswerData> answers = new List<AnswerData>();

    private string outputDirectory = @"E:\Monologue_Koltes\Assets\Questions\";
    private string outputFilePath;

    void Start()
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string safeUser = nomUtilisateur.Replace(" ", "_");
        outputFilePath = Path.Combine(outputDirectory, $"reponses_{safeUser}_{timestamp}.json");

        ShowQuestion();
    }

    public void OnAnswer(int valeur)
    {
        if (isWaiting || valeur < 1 || valeur > 8) return;

        var data = new AnswerData
        {
            utilisateur = nomUtilisateur,
            question = questions[currentQuestionIndex].questionText,
            valeur = valeur,
            horodatage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        answers.Add(data);

        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Length)
        {
            StartCoroutine(NextQuestionDelay());
        }
        else
        {
            questionTextUI.text = "Merci pour vos r�ponses !";
            libelleMinUI.text = "";
            libelleMaxUI.text = "";
            SaveAnswersToFile();
        }
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex < questions.Length)
        {
            var q = questions[currentQuestionIndex];
            questionTextUI.text = q.questionText;
            libelleMinUI.text = q.libelleMin;
            libelleMaxUI.text = q.libelleMax;
        }
    }

    System.Collections.IEnumerator NextQuestionDelay()
    {
        isWaiting = true;

        // Masquer les textes pour cr�er une pause visuelle
        questionTextUI.text = "";
        libelleMinUI.text = "";
        libelleMaxUI.text = "";

        yield return new WaitForSeconds(1.5f); // Dur�e de la pause

        ShowQuestion();
        isWaiting = false;
    }

    void SaveAnswersToFile()
    {
        try
        {
            string json = JsonHelper.ToJson(answers.ToArray(), true);
            File.WriteAllText(outputFilePath, json);
            Debug.Log("R�ponses enregistr�es dans : " + outputFilePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("Erreur d'enregistrement des r�ponses : " + ex.Message);
        }
    }
}