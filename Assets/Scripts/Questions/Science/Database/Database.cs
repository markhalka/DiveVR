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
    public GameObject informationPanelGb;
    public GameObject currentPanel;
    public GameObject page1;
    public GameObject horizontalSnap;

    public GameObject currLesson;
    public GameObject tectonic;
    public GameObject ecosystem;
    public GameObject scientificNames;
    public GameObject pretestPanel;

    public Canvas currentCanvas;

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

//        ParseData.parseModel();
       
        instructionsShown = false;
    //    quiz.GetComponent<QuizMenu>().startOffset = 1;
        Information.currentScene = "Database";
      //  informationPanelGb.SetActive(true);
        informationPanel = informationPanelGb.GetComponent<InformationPanel>();

        currLesson = null;
       
    }

    void OnEnable()
    {

        Information.panelIndex = -1;
        Information.lableIndex = 0;
    }


    public GameObject InstructionAnimationGb;
   
    void initDatabase()
    {

        switch (Information.nextScene)
        {    
            case 9: //scientific names      
                currLesson = scientificNames;
                break;
            case 38:
                currLesson = tectonic;
                break;
            case 41:
                currLesson = ecosystem;
                break;
        }

        informationPanel.initStartPanels(); // ?
        currLesson.SetActive(true);
        initQuiz(); // that should work
    }


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
 

    void showInstructions()
    {
        if (!instructionsShown)
        {
            InstructionAnimationGb.SetActive(true);
            instructionsShown = true;
        }
    }



    void Update()
    {
        /*  if (!panel.activeSelf)
          {
              checkQuiz();
          }
          */

        if (!instructionsShown)
        {
            showInstructions();
        }

        if(instructionsShown && !InstructionAnimationGb.activeSelf && currLesson == null)
        {
            Debug.LogError("creating new lesson");
            pretestPanel.SetActive(true);
            initDatabase();
        }

        if (Information.doneLoading)
        {
            SceneManager.LoadScene("ScienceMain");
        }

      //  currLesson.update();
    } 
}
