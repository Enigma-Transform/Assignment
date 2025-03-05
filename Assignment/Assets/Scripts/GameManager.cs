using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public List<LevelScriptableObject> levels; // List of levels stored in scriptable objects
    private int currentLevelIndex = 0; // Tracks the current level
    private int currentQuestionIndex = 0; // Tracks the current question within a level

    [Header("UI Elements")]
    public TMP_Text questionText; // UI text element for displaying questions
    public Image questionImage; // UI image element for displaying question images
    public Button[] buttons; // Buttons for selecting answers
    public Image[] buttonImages; // Images assigned to answer buttons
    public Animator levelAnimator; // Animator for level transitions and wrong answer animations

    private Dictionary<Button, int> buttonValues = new Dictionary<Button, int>(); // Stores answer values for buttons

    void Start()
    {
        LoadLevel(); // Load the first level when the game starts
    }

    void LoadLevel()
    {
        // Check if all levels are completed
        if (currentLevelIndex >= levels.Count)
        {
            Debug.Log("You Win!"); // Display win message in console
            return;
        }
        
        LevelScriptableObject currentLevel = levels[currentLevelIndex];

        // Play level intro animation if available
        if (currentLevel.levelAnimation != null)
        {
            levelAnimator.runtimeAnimatorController = currentLevel.levelAnimation;
            levelAnimator.Play("Intro");
        }
        
        currentQuestionIndex = 0; // Reset question index
        LoadQuestion(); // Load the first question of the level
    }

    void LoadQuestion()
    {
        // Check if all levels are completed
        if (currentLevelIndex >= levels.Count)
        {
            Debug.Log("You Win!");
            return;
        }
        
        LevelScriptableObject currentLevel = levels[currentLevelIndex];
        
        // Check if all questions in the current level are completed
        if (currentQuestionIndex >= currentLevel.questionTexts.Count)
        {
            currentLevelIndex++; // Move to the next level
            LoadLevel(); // Load the next level
            return;
        }

        // Assign question text
        questionText.text = currentLevel.questionTexts[currentQuestionIndex];
        
        // Assign question image if available
        if (currentLevel.questionSprite.Count > currentQuestionIndex && currentLevel.questionSprite[currentQuestionIndex] != null)
        {
            questionImage.sprite = currentLevel.questionSprite[currentQuestionIndex];
            questionImage.gameObject.SetActive(true);
        }
        else
        {
            questionImage.gameObject.SetActive(false);
        }
        
        AssignAnswers(); // Assign answer choices
    }

    void AssignAnswers()
    {
        LevelScriptableObject currentLevel = levels[currentLevelIndex];
        
        // Assign correct answer data
        int correctValue = currentLevel.correctAnswerSequence[currentQuestionIndex];
        string correctText = currentLevel.answersText[currentQuestionIndex];
        Sprite correctSprite = currentLevel.answerSprite.Count > currentQuestionIndex ? currentLevel.answerSprite[currentQuestionIndex] : null;

        // Select a wrong answer that is not the correct one
        List<int> possibleWrongIndices = new List<int>();
        for (int i = 0; i < currentLevel.wrongAnswersText.Count; i++)
        {
            if (i != correctValue)
            {
                possibleWrongIndices.Add(i);
            }
        }

        int wrongIndex = possibleWrongIndices[Random.Range(0, possibleWrongIndices.Count)];
        string wrongText = currentLevel.wrongAnswersText[wrongIndex];
        Sprite wrongSprite = currentLevel.wrongSprite.Count > wrongIndex ? currentLevel.wrongSprite[wrongIndex] : null;

        // Randomly assign correct answer to one of the buttons
        int correctButtonIndex = Random.Range(0, buttons.Length);
        buttons[correctButtonIndex].GetComponentInChildren<TMP_Text>().text = correctText;
        buttonValues[buttons[correctButtonIndex]] = correctValue;
        
        // Assign correct answer image if available
        if (correctSprite != null)
        {
            buttonImages[correctButtonIndex].sprite = correctSprite;
            buttonImages[correctButtonIndex].gameObject.SetActive(true);
        }
        else
        {
            buttonImages[correctButtonIndex].gameObject.SetActive(false);
        }
        
        // Assign wrong answer to the remaining button
        int wrongButtonIndex = 1 - correctButtonIndex;
        buttons[wrongButtonIndex].GetComponentInChildren<TMP_Text>().text = wrongText;
        buttonValues[buttons[wrongButtonIndex]] = wrongIndex;
        
        // Assign wrong answer image if available
        if (wrongSprite != null)
        {
            buttonImages[wrongButtonIndex].sprite = wrongSprite;
            buttonImages[wrongButtonIndex].gameObject.SetActive(true);
        }
        else
        {
            buttonImages[wrongButtonIndex].gameObject.SetActive(false);
        }

        // Add event listeners for button clicks
        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();
        buttons[0].onClick.AddListener(() => OnAnswerSelected(buttons[0]));
        buttons[1].onClick.AddListener(() => OnAnswerSelected(buttons[1]));
    }

    void OnAnswerSelected(Button clickedButton)
    {
        LevelScriptableObject currentLevel = levels[currentLevelIndex];
        
        // Check if selected answer is correct
        if (buttonValues[clickedButton] == currentLevel.correctAnswerSequence[currentQuestionIndex])
        {
            currentQuestionIndex++; // Move to the next question
            LoadQuestion(); // Load the next question
        }
        else
        {
            // Play wrong answer animation if available
            if (currentLevel.wrongAnswerAnimations.Count > 0)
            {
                levelAnimator.runtimeAnimatorController = currentLevel.wrongAnswerAnimations[Random.Range(0, currentLevel.wrongAnswerAnimations.Count)];
                levelAnimator.Play("Wrong");
            }
        }
    }
}
