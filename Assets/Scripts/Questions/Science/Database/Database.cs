using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Database : MonoBehaviour
{
    
    // than, you should call tectonicDb, which creates the images in the right way, and calls the start panels
    // ok, so when tectonic is called, it should instantitate new children in horizontal scroll, and then gucci

    // todo next:
    // 1. create a struct (maybe static) that will have all the gameobjects needed, then that struct should be in lessondb, then initiate that struct here
    // 2. then, you can just create hs or vs in the individual lessons
    
    

    public GameObject quiz;
    public GameObject verticalScroll;

    public GameObject informationPanelGb;
    public GameObject currentPanel;

    public Sprite[] classificationSprites;
    public Sprite[] ecosystemSprites;
    public Sprite[] tectonicSprites;
    public Sprite[] animalSprites;
    public Sprite[] scientificNameSprites;


    public AudioSource source;
    public AudioClip swipe;
    public AudioClip click;


    //ok, now get the start panel working well
    //than get the tectonic shit working 

    bool instructionsShown = false;
    LessonDb currLesson;

    InformationPanel informationPanel;


    void Start()
    {

        instructionsShown = false;
    //    quiz.GetComponent<QuizMenu>().startOffset = 1;
        Information.currentScene = "Database";

        informationPanel = informationPanelGb.GetComponent<InformationPanel>();

        currLesson = null;
    }

    void tempLoad()
    {
        Information.grade = "Grade 3";
        Information.subject = "science";
        Information.nextScene = 38;

        TextAsset mytxtData = (TextAsset)Resources.Load("XML/General/UserData");
        string txt = mytxtData.text;
        Information.xmlDoc = XDocument.Parse(txt);

        mytxtData = (TextAsset)Resources.Load("XML/General/Data");
        txt = mytxtData.text;
        Information.loadDoc = XDocument.Parse(txt);

        ParseData.startXML();
    }

    void OnEnable()
    {
        tempLoad(); //TEMP STUFF

        userButtons = new List<GameObject>();
        Vector2 offset = new Vector2(0, 0);

        //     finishStart = 0;

        ParseData.parseModel();

        Information.panelIndex = -1;
        Information.lableIndex = 0;
    }

    //this is the panel gameobject


    string[] currentNames;

 

    int tempStart = 0; // figure out how to add this 
    public GameObject InstructionAnimationGb;
   
    void initDatabase()
    {
        if(currLesson != null)
        {
            return;
        }
        Debug.LogError("creating a new lesson...");
        
        switch (Information.nextScene)
        {
            case 8: //classifcation
                currLesson = new ClassificationDb(informationPanel.startOffset, classificationSprites);
                break;
            case 9: //scientific names      
                currLesson = new ScientificNamesDb(informationPanel.startOffset, scientificNameSprites);
                break;
            case 10:
                currLesson = new animalLifeCycleDb(informationPanel.startOffset, animalSprites);
                break;
            case 38:
                currLesson = new tectonicDb(informationPanel.startOffset, tectonicSprites);
                break;

            case 41:
                currLesson = new identifyEcosystemDb(informationPanel.startOffset, ecosystemSprites);
                break;

        }
    }


    // this is the horizontal snap gameobject 

    List<GameObject> userButtons;

    Vector2 offsetAmount = new Vector2(1.5f, 0);
    public Canvas currentCanvas;

    public GameObject page1;
    public GameObject horizontalSnap;


 /*
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
 */


    public GameObject vsChild;


    void showInstructions()
    {
        if (!instructionsShown)
        {
            InstructionAnimationGb.SetActive(true);
            instructionsShown = true;
            Debug.LogError("showing instructions");
        }
    }

    

    void Update()
    {
        /*  if (!panel.activeSelf)
          {
              checkQuiz();
          }
          */

        if (!currentPanel.activeSelf)
        {
            showInstructions();
            initDatabase();
        }

        if (Information.doneLoading)
        {
            SceneManager.LoadScene("ScienceMain");
        }

      //  currLesson.update();
    } 
}
