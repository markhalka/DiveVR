using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Database : MonoBehaviour
{

    public GameObject quiz;
    public GameObject startPanel;
    public GameObject verticalScroll;

    public Sprite[] classificationSprites;
    public Sprite[] ecosystemSprites;
    public Sprite[] tectonicSprites;
    public Sprite[] animalSprites;
    public Sprite[] scientificNameSprites;


    public AudioSource source;
    public AudioClip swipe;
    public AudioClip click;

    public GameObject lessonGb;

    //ok, now get the start panel working well
    //than get the tectonic shit working 
    

    void Start()
    {
        quiz.GetComponent<QuizMenu>().startOffset = 1;
        Information.currentScene = "Database";
    }

    void OnEnable()
    {
        startPanel.transform.parent.gameObject.SetActive(true); // this should handle the start panels

        userButtons = new List<GameObject>();
        Vector2 offset = new Vector2(0, 0);

        //     finishStart = 0;

        ParseData.parseModel();

        Information.panelIndex = -1;
        Information.lableIndex = 0;

        initDatabase();
    }

    //this is the panel gameobject


    string[] currentNames;


    bool isHorizontalSnap = false;

    LessonDb currLesson;

    int tempStart = 0; // figure out how to add this 
    public GameObject InstructionAnimationGb;
    
    void initDatabase()
    {
        currLesson = lessonGb.GetComponent<LessonDb>();
        switch (Information.nextScene)
        {
            case 8: //classifcation
                currLesson = new ClassificationDb(startOffset, classificationSprites);
                break;
            case 9: //scientific names      
                currLesson = new ScientificNamesDb(startOffset, scientificNameSprites);
                break;
            case 10:
                currLesson = new animalLifeCycleDb(startOffset, animalSprites);
                break;
            case 38:
                currLesson = new tectonicDb(startOffset, tectonicSprites);
                break;

            case 41:
                currLesson = new identifyEcosystemDb(startOffset, ecosystemSprites);
                break;

        }
        var instructionAnimation = InstructionAnimationGb.GetComponent<InstructionAnimation>();
        StartCoroutine(instructionAnimation.swipeAnimation());
    }







    // this is the horizontal snap gameobject 

    List<GameObject> userButtons;

    Vector2 offsetAmount = new Vector2(1.5f, 0);
    public Canvas currentCanvas;

    public GameObject page1;
    public GameObject horizontalSnap;


    int startOffset = -1;
    int currentButtonIndex = -1;

    void showPanel()
    {

        Debug.LogError("showing informationp  panel");
        panel.SetActive(true);
    }

    #region replace with in quiz
    void startQuiz()
    {
        panel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(false);
        isQuiz = true;
        quiz.gameObject.SetActive(true);
    }

    void endQuiz()
    {
        isQuiz = false;
        Debug.LogError("ended quiz");
        if (Information.wasPreTest)
        {
            Information.panelIndex = -1;
            Information.lableIndex = 0;
            panel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(true);
            return;
        }


        quiz.GetComponent<QuizMenu>().endQuiz();
        Debug.LogError("quiz ended");

    }

    void initQuiz(Sprite[] currentSprites)
    {
        if (currentSprites != null)
        {
            Debug.LogError("added the current sprites ");
            quiz.GetComponent<QuizMenu>().images = currentSprites;
            quiz.GetComponent<QuizMenu>().useImage = true;
        }
        else
        {
            quiz.GetComponent<QuizMenu>().useImage = false;
        }
        quiz.GetComponent<QuizMenu>().startOffset = startOffset;

    }

    bool isQuiz = false;
    void checkQuiz()
    {
        if (!isQuiz)
        {
            if (Information.isQuiz == 1)
            {
                startQuiz();
            }

        }
        else
        {
            if (Information.isQuiz == 0)
            {
                endQuiz();
            }
        }
    }

    #endregion



    public GameObject vsChild;



    void Update()
    {
        if (!panel.activeSelf)
        {
            checkQuiz();
        }


        if (Information.doneLoading)
        {
            SceneManager.LoadScene("ScienceMain");
        }

        currLesson.update();
    } 
}
