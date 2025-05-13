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
        public string texteOui;
        public string texteNon;
    }

    [System.Serializable]
    public class AnswerData
    {
        public string utilisateur;
        public string question;
        public string reponse;
        public string horodatage;
    }

    [Header("Références UI")]
    public TextMeshProUGUI questionTextUI;
    public TextMeshProUGUI texteOuiUI;
    public TextMeshProUGUI texteNonUI;

    [Header("Paramètres du questionnaire")]
    public string nomUtilisateur = "Participant";
    public Question[] questions;

    private int currentQuestionIndex = 0;
    private bool isWaiting = false;
    private List<AnswerData> answers = new List<AnswerData>();

    private string outputDirectory = @"E:\Monologue_Koltes\Assets\Questions\Réponses\";
    private string outputFilePath;

    void Start()
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string safeUser = nomUtilisateur.Replace(" ", "_");
        outputFilePath = Path.Combine(outputDirectory, $"reponses_{safeUser}_{timestamp}.json");

        ShowQuestion();
    }

    public void OnAnswer(bool isYes)
    {
        if (isWaiting) return;

        string reponseText = isYes ? "Oui" : "Non";

        var data = new AnswerData
        {
            utilisateur = nomUtilisateur,
            question = questions[currentQuestionIndex].questionText,
            reponse = reponseText,
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
            questionTextUI.text = "Merci pour vos réponses !";
            texteOuiUI.text = "";
            texteNonUI.text = "";
            SaveAnswersToFile();
        }
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex < questions.Length)
        {
            var q = questions[currentQuestionIndex];
            questionTextUI.text = q.questionText;
            texteOuiUI.text = q.texteOui;
            texteNonUI.text = q.texteNon;
        }
    }

    System.Collections.IEnumerator NextQuestionDelay()
    {
        isWaiting = true;
        yield return new WaitForSeconds(1f);
        ShowQuestion();
        isWaiting = false;
    }

    void SaveAnswersToFile()
    {
        try
        {
            string json = JsonHelper.ToJson(answers.ToArray(), true);
            File.WriteAllText(outputFilePath, json);
            Debug.Log("Réponses enregistrées dans : " + outputFilePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("Erreur d'enregistrement des réponses : " + ex.Message);
        }
    }
}
