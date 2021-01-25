using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Database : MonoBehaviour
{
    
    //todo:

    //scientific names: 9
    //ecosystem: 41
    //classification: 8
    //animal life cycle: 10


   //ok, so make the pretest have an animatino as well
   //remove the animation from the start panel


    //2. test all hs classes
    //3. test the quiz
    //4. get the vs classes working
    //5. test from home menu 
    //6. test pretest
    //7. fix any small bugs

    // you need to change all the start panels in data to be -1, do that at night when you're too tired to think
    //then you should get the 2 vertical scroll classes working 


    public GameObject quiz;
    public GameObject verticalScroll;

    public GameObject informationPanelGb;
    public GameObject currentPanel;

    public GameObject currLesson;
    public GameObject tectonic;
    public GameObject animalLifeCycle;
    public GameObject ecosystem;
    public GameObject classification;
    public GameObject scientificNames;

    public AudioSource source;
    public AudioClip swipe;
    public AudioClip click;

    public static Sprite[] currentSprites;


    //ok, now get the start panel working well
    //than get the tectonic shit working 

    bool instructionsShown = false;

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

        switch (Information.nextScene)
        {
            case 8: //classifcation
                currLesson = classification;
                break;
            case 9: //scientific names      
                currLesson = scientificNames;
                break;
            case 10:
                currLesson = animalLifeCycle;
                break;
            case 38:
                currLesson = tectonic;
                break;
            case 41:
                currLesson = ecosystem;
                break;
        }
        currLesson.SetActive(true);
        initQuiz(); // that should work
    }


    // this is the horizontal snap gameobject 

    List<GameObject> userButtons;

    Vector2 offsetAmount = new Vector2(1.5f, 0);
    public Canvas currentCanvas;

    public GameObject page1;
    public GameObject horizontalSnap;


 
    #region replace with in quiz

    void initQuiz()
    {
        var quizMenu = quiz.GetComponent<QuizMenu>();
        if (currentSprites != null)
        {
            Debug.LogError("added the current sprites ");
            quizMenu.images = currentSprites;
            quizMenu.useImage = true;
        }
        else
        {
            quizMenu.useImage = false;
        }
        quizMenu.startOffset = informationPanel.startOffset;

    }


    #endregion
 


    public GameObject vsChild;


    void showInstructions()
    {
        if (!instructionsShown)
        {
            InstructionAnimationGb.SetActive(true);
            instructionsShown = true;
        }
    }


    public GameObject pretestPanel;
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
        }

        if(instructionsShown && !InstructionAnimationGb.activeSelf && currLesson == null)
        {
            Debug.LogError("creating new lesson");
            initDatabase();
        }

        if (Information.doneLoading)
        {
            SceneManager.LoadScene("ScienceMain");
        }

      //  currLesson.update();
    } 
}
