using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System;
using Unity.Mathematics;

public class QuestionManagerSalome : MonoBehaviour
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

    [System.Serializable]
    public class QuestionList
    {
        public Question[] questions;
    }

    [Header("R�f�rences UI")]
    public TextMeshProUGUI questionTextUI;
    public TextMeshProUGUI libelleMinUI;
    public TextMeshProUGUI libelleMaxUI;

    [Header("Param�tres du questionnaire")]
    public string nomUtilisateur = "Participant";
    private Question[] questions;
    private string questionFolderPath = @"C:\Users\Mines\Desktop\Salome\Projets Unity\MergedPilotStudy\Assets\Questions\QuestionsJSON"; 
    private string questionFilePath = @"C:\Users\Mines\Desktop\Salome\Projets Unity\MergedPilotStudy\Assets\Questions\QuestionsJSON\test.json"; 
    private int currentQuestionIndex = 0;
    private bool isWaiting = false;
    private List<AnswerData> answers = new List<AnswerData>();

    private string outputDirectory;
    private string outputFilePath;

    void Start()
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string safeUser = nomUtilisateur.Replace(" ", "_");
        nomUtilisateur = GeneralExperienceManager.Instance.userName; 
        outputDirectory = GeneralExperienceManager.Instance.experienceFolderPath + "/QuestionnaireAnswers";
        outputFilePath = Path.Combine(outputDirectory, $"reponses_{safeUser}_{timestamp}.json");

        if (GeneralExperienceManager.Instance.experienceControlType == "Impose")
        {
            Debug.Log("finding the question file path, control type = " + GeneralExperienceManager.Instance.experienceControlType);
            switch (GeneralExperienceManager.Instance.experienceSceneType)
            {
                
                case "Scenographique":
                    Debug.Log("finding the question file path, scene type = " + GeneralExperienceManager.Instance.experienceSceneType);
                    questionFilePath = questionFolderPath + @"\QuestionsScenographiqueSceneImposee.json";
                    Debug.Log("question file path intermediaire: " + questionFilePath); 
                    break;

                case "Narratif":
                    Debug.Log("finding the question file path, scene type = " + GeneralExperienceManager.Instance.experienceSceneType);
                    questionFilePath = questionFolderPath + @"\QuestionsNarratifSceneImposee.json";
                    Debug.Log("question file path intermediaire: " + questionFilePath); 
                    break;

                case "Abstrait":
                    Debug.Log("finding the question file path, scene type = " + GeneralExperienceManager.Instance.experienceSceneType);
                    questionFilePath = questionFolderPath + @"\QuestionsAbstraitSceneImposee.json";
                    Debug.Log("question file path intermediaire: " + questionFilePath); 
                    break;
            }
        }
        else
        {
           Debug.Log("finding the question file path, control type = " + GeneralExperienceManager.Instance.experienceControlType);
           switch (GeneralExperienceManager.Instance.experienceSceneType)
            {
                case "Scenographique":
                    Debug.Log("finding the question file path, scene type = " + GeneralExperienceManager.Instance.experienceSceneType);
                    questionFilePath = questionFolderPath + @"\QuestionsScenographiqueSceneModifiable.json";
                    Debug.Log("question file path intermediaire: " + questionFilePath); 
                    break;

                case "Narratif":
                    Debug.Log("finding the question file path, scene type = " + GeneralExperienceManager.Instance.experienceSceneType);
                    questionFilePath = questionFolderPath + @"\QuestionsNarratifSceneModifiable.json";
                    Debug.Log("question file path intermediaire: " + questionFilePath); 
                    break;

                case "Abstrait":
                    Debug.Log("finding the question file path, scene type = " + GeneralExperienceManager.Instance.experienceSceneType);
                    questionFilePath = questionFolderPath + @"\QuestionsAbstraitSceneModifiable.json";
                    Debug.Log("question file path intermediaire: " + questionFilePath); 
                    break;
            } 
        }

        Debug.Log("questions file path = " + questionFilePath); 
        questions = LoadQuestionsFromJson(questionFilePath);

        foreach (Question q in questions)
        {
            Debug.Log("question: " + q.questionText);
            Debug.Log("min: " + q.libelleMin);
            Debug.Log("max: " + q.libelleMax); 
        }

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
            questionTextUI.text = "Merci pour vos reponses !";
            libelleMinUI.text = "";
            libelleMaxUI.text = "";
            SaveAnswersToFile();
        }
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex < questions.Length)
        {
            Debug.Log("la question d'index " + currentQuestionIndex);
            Debug.Log("les questions: " + questions); 
            var q = questions[currentQuestionIndex];
            questionTextUI.text = q.questionText;
            libelleMinUI.text = q.libelleMin;
            libelleMaxUI.text = q.libelleMax;
            Debug.Log("est : " + q.questionText); 
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
            Debug.Log("Reponses enregistrees dans : " + outputFilePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("Erreur d'enregistrement des reponses : " + ex.Message);
        }
    }


    public Question[] LoadQuestionsFromJson(string path)
    {
        Question[] questions;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            QuestionList loaded = JsonUtility.FromJson<QuestionList>(json);
            questions = loaded.questions;

            Debug.Log($"Loaded {questions.Length} questions.");
            return questions;
        }
        else
        {
            Debug.LogError("JSON file not found at path: " + path);
            return new Question[0]; 
        }
    }
}