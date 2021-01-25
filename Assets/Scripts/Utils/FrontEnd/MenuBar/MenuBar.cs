
using PlayFab.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour
{

    
      public GameObject mainMenu;
      public GameObject showQuiz;
      public GameObject showTutorial;


      public Button backButton;
      public Button quitButton;
      public Button settingsButton;
      public Button quizButton;
      public Button helpQuestion;
      public Button tutorialButton;
      public Button openMenu;
      public Button previousSceneButton;

      public Button backToHomeButton;

      public Button showQuizOk;
      public Button showQuizNo;

      public Button showTutorialOk;
      public Button showTutorialNo;



      public AudioClip buttonSound;
      public AudioClip menuOpen;
      public AudioClip menuClose;
      public AudioSource source;

      public Vector3 start;
      public Vector3 end;



     public  Panel panel; // this should be the current game object

      private void Start()
      {

        panel = new Panel();

          backButton.onClick.AddListener(delegate { back(); });
          quitButton.onClick.AddListener(delegate { takeQuitSave(); });
          settingsButton.onClick.AddListener(delegate { settings(); });
          //    quizButton.onClick.AddListener(delegate { startQuiz(); });
          previousSceneButton.onClick.AddListener(delegate { quitModule(); });
          // lessonButton.onClick.AddListener(delegate { takeLesson(); });
          helpQuestion.onClick.AddListener(delegate { takeHelpQuestion(); });
          tutorialButton.onClick.AddListener(delegate { takeTutorial(); });
          /*    tutorialPanelButton.onClick.AddListener(delegate { takeTutorialPanel(); });
              tutorialPanelButton.onClick.AddListener(delegate { takeTutorialBack(); });*/ 
    backToHomeButton.onClick.AddListener(delegate { takeBackToHome(); });

        showQuizOk.onClick.AddListener(delegate { takeQuizOk(); });
        showQuizNo.onClick.AddListener(delegate { showQuiz.SetActive(false); });
        showTutorialOk.onClick.AddListener(delegate { takeTutorial(); });
        showTutorialNo.onClick.AddListener(delegate { showTutorial.SetActive(false); });

        openMenu.onClick.AddListener(delegate { takeOpenMenu(); });

        SceneManager.sceneLoaded += OnSceneLoaded;

        start.x += transform.position.x;
        start.y += transform.position.y;

        end.x += transform.position.x;
        end.y += transform.position.y;

        if (!Information.menuLoaded)
        {
            DontDestroyOnLoad(this.gameObject);
          //  initTextToSpeech();
            Information.menuLoaded = true;
        }

    }

    void takeQuizOk()
    {
        Information.isQuiz = 1;
        showQuiz.SetActive(false);
    }



    public void takeQuitSave()
    {

        Debug.LogError("here");
        StartCoroutine(saveToWebsite());
    }

    //ok, so the points is handled well here, just make sure this gets called

    //topics looks ok, it should be handeld in save topics, but you should probably call it here as well

    //ahcievments
    private static string header = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
    IEnumerator saveToWebsite()
    {

        string data = header + Information.xmlDoc.ToString(SaveOptions.DisableFormatting);
        Debug.LogError(data);
        byte[] dataToPut = System.Text.Encoding.UTF8.GetBytes(data);

        CustomCertificateHandler certHandler = new CustomCertificateHandler();
        UnityWebRequest uwr = UnityWebRequest.Put(Information.saveFileUrl + Information.username, dataToPut);
        uwr.chunkedTransfer = false;
        uwr.certificateHandler = certHandler;

        yield return uwr.SendWebRequest();

        Debug.LogError("done saving file...");
     
        // Application.Quit();

    }

    void takeBackToHome()
    {
        SceneManager.LoadScene("StudentMenu");
        back();
    }


    //ok, so it actually doesn't really matter, so just leave this shit for now
    //well there are only two options, they go to a different scene from inbetween, or from the back button?
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        handleButtonsCalled = false;
        Information.minutesInScene = 0;
    }

    void handleButtons()
    {
        handleButtonsCalled = true;
        switch (Information.currentScene)
        {
            case "StudentMenu":
            case "Shop":
            case "StudentInfo":
            case "Curriculum":
            case "Breakdown":
            case "Achievement":
            case "DiveGame":
            case "ScienceTest":
                quizButton.gameObject.SetActive(false);
                helpQuestion.gameObject.SetActive(false);
                break;
            default:
                quizButton.gameObject.SetActive(true);
                helpQuestion.gameObject.SetActive(true);
                break;
        }
    }



    public void takeHelpQuestion()
    {
        source.clip = buttonSound;
        source.Play();

        GameObject curr = null;
        switch (Information.subject)
        {      
            case "science":

                if (Information.currentScene == "Models")
                {
                    curr = GameObject.Find("Main");
                   // curr.GetComponent<ScienceModels>().takeHelp();
                }
                else
                {
                    curr = GameObject.Find("Quiz");
                   // curr.GetComponent<QuizMenu>().takeHelp();
                }
                break;
            case "public speaking":
                break;
            case "other":
                break;
            default:
                Debug.LogError("could not find the current subject");
                break;

        }
        back();
    }

   

    void takeOpenMenu()
    {
        shouldOpen = true;
    }

    void takeTutorial()
    {
        source.clip = buttonSound;
        source.Play();
        showTutorial.SetActive(false);
        back();
        GameObject tutorial = GameObject.Find("TutorialCanvas");
        if (tutorial != null)
        {

            tutorial.GetComponent<Tutorial>().takeHelp();
        }

    }
    bool handleButtonsCalled = false;


    bool shouldOpen = false;

    private void Update()
    {
        if (!handleButtonsCalled)
        {
            handleButtonsCalled = true;
            //   handleButtons();
        }


        if (shouldOpen)
        {
            shouldOpen = false;
            Debug.LogError(mainMenu.activeSelf);
            if (!mainMenu.activeSelf)
            {

                StartCoroutine(panel.panelAnimation(true, mainMenu.transform));
            }
            else
            {
                back();
            }
        }
    }

    void takeHide()
    {
        Information.autoHide = !Information.autoHide;
    }


    void back()
    {
        StartCoroutine(panel.panelAnimation(false, mainMenu.transform));
    }


    public void settings()
    {
        source.clip = buttonSound;
        source.Play();

        transform.GetChild(1).gameObject.SetActive(true);
    }



    public void quitModule()
    {


        back();
        if (transform.GetChild(1).gameObject.activeSelf)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            return;
        }

        switch (Information.currentScene)
        {
            case "Shop":
                SceneManager.LoadScene("StudentMenu");
                break;
            case "StudentMenu":
                GameObject learnPanel = GameObject.Find("LearnPanel");
                if (learnPanel != null)
                {
                    learnPanel.gameObject.SetActive(false);
                }
                else
                {
                    Application.Quit();
                }

                break;
            case "StudentInfo":
                GameObject breakdown = GameObject.Find("LineChart");
                if (breakdown != null)
                {
                    breakdown.SetActive(false);

                }
                else
                {
                    SceneManager.LoadScene("StudentMenu");
                }

                break;
            case "HomeMenu":
                SceneManager.LoadScene("StudentMenu");
                break;
            case "Curriculum":

                GameObject curr = GameObject.Find("Main");
                if (curr != null)
                {
                    curr.GetComponent<Curriculum>().takeBack();
                }
                break;
            case "ModuleMenu":
                SceneManager.LoadScene("StudentMenu");
                break;

            default:
                SceneManager.LoadScene("ModuleMenu");
                break;
            case "Achievement":
                SceneManager.LoadScene("StudentMenu");
                break;
            case "ScienceTest":
                SceneManager.LoadScene("Curriculum");
                break;

        }
    }


}
