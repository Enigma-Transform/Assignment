using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Quiz/LevelData")]
public class LevelScriptableObject : ScriptableObject
{
    // =====================================
    // Level Settings
    // =====================================
    
    [Header("Level Animation")]
    [Tooltip("Animation that plays at the start of the level.")]
    public RuntimeAnimatorController levelAnimation;

    // =====================================
    // Question Data
    /* You can enter how many ever questions you want. There is no limit.*/
    // =====================================
    
    [Header("Questions")]
    [Tooltip("List of question to be displayed to the player for this level.")]
    [TextArea] public List<string> questionTexts = new List<string>();
    
    [Tooltip("List of correct answers for each question.")]
    public List<string> answersText = new List<string>();
    
    [Tooltip("List of wrong answer options.")]
    public List<string> wrongAnswersText = new List<string>();

    [Tooltip("The correct sequence value for each question. Here you can specify the order in which the answers are correct eg,[1,4,3,5]")]
    public List<int> correctAnswerSequence = new List<int>();

    // =====================================
    /* Visual Elements Make sure you add the images in the correct order*/ 
    // =====================================
    
    [Header("Images")]
    [Tooltip("Images corresponding to each question.")]
    public List<Sprite> questionSprite = new List<Sprite>();
    
    [Tooltip("Images corresponding to the correct answers.")]
    public List<Sprite> answerSprite = new List<Sprite>();

    [Tooltip("Images corresponding to the wrong answers.")]
    public List<Sprite> wrongSprite = new List<Sprite>();

    // =====================================
    // Wrong Answer Animation
    // =====================================
    
    [Header("Wrong Answer Animations")]
    [Tooltip("Animations that play when the player selects a wrong answer.")]
    public List<RuntimeAnimatorController> wrongAnswerAnimations = new List<RuntimeAnimatorController>();
}
