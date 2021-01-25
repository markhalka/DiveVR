using PlayFab.Internal;
using System;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StudentMenu : MonoBehaviour
{

    public TMPro.TMP_Text divePoints;

    public Button Shop;
    public Button Coins;

    public Button learning;
    public Button aboutMe;

    public Button tutorialOk;
    public Button noTutorial;

    public GameObject leftImage;
    public GameObject rightImage;
    public GameObject tutorialPanel;
    public GameObject learningTypePanel;
    public GameObject firstTutorialPanel;


    public AudioSource source;
    public AudioClip buttonSound;

    Website network;
    Panel panel;

    int step = 0;
    void Start()
      {
          initButtons();
          panel = new Panel();
          network = new Website();

          Information.currentScene = "StudentMenu";

          if (Information.isParent)
          {
              learning.gameObject.SetActive(false);
          }

          if (Information.xmlDoc != null)
          {
              Information.doneLoadingDocuments = true;
              return;
          }
      }

    void loadLearningType()
    {

    }

    void loadLearningPlan()
    {

    }

    void displayPoints()
    {
        divePoints.text = Information.totalEarnedPoints.ToString();
    }

    void closeTutorial()
    {
        StartCoroutine(panel.panelAnimation(false, tutorialPanel.transform));
    }

    void openTutorial()
    {
        GameObject tutorial = GameObject.Find("TutorialCanvas");
        if (tutorial != null)
        {
            tutorial.GetComponent<Tutorial>().takeHelp();
        }
        //this is from after when the user saves their name 
        tutorialPanel.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = "Hi " + Information.name + "!";
        tutorialPanel.SetActive(true);

        tutorialPanel.SetActive(false);
    }

   

    void initButtons()
    {
        Shop.onClick.AddListener(delegate { openShop(); });
        Coins.onClick.AddListener(delegate { takeCoins(); });
        coinsPanelButton.onClick.AddListener(delegate { takeCoinsBack(); });


        leftImage.GetComponent<Button>().onClick.AddListener(delegate { takeLeftOption(); });
        rightImage.GetComponent<Button>().onClick.AddListener(delegate { takeRightOption(); });

        learning.onClick.AddListener(delegate { takeLearning(); });
        aboutMe.onClick.AddListener(delegate { takeAboutMe(); });

        tutorialOk.onClick.AddListener(delegate { openTutorial(); });
        noTutorial.onClick.AddListener(delegate { closeTutorial(); });
       
    }

    public GameObject coinsPanel;
    public Button coinsPanelButton;
    void takeCoins()
    {
        //here you would show the right info and all that 
        Debug.LogError(Information.totalEarnedPoints + " total earned points student menu");
        coinsPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = Information.totalEarnedPoints.ToString();
        coinsPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = Information.maxDivePoints.ToString();
        StartCoroutine(panel.panelAnimation(true, coinsPanel.transform));
    }

    void takeCoinsBack()
    {
        StartCoroutine(panel.panelAnimation(false, coinsPanel.transform));
    }

    void openShop()
    {
        source.clip = buttonSound;
        source.Play();

        SceneManager.LoadScene("Shop");
    }

    int menuIndex = 0;
    void takeLeftOption()
    {
        source.clip = buttonSound;
        source.Play();

        if (menuIndex == 0)
        {
            if (Information.lastSubject != null && Information.lastSubject.Length > 3 && Information.lastGrade != null && Information.lastGrade.Length > 3)
            {
                Information.subject = Information.lastSubject;
                Information.grade = Information.lastGrade;
                SceneManager.LoadScene("ModuleMenu");
            }
            else
            {
                Information.isStudentInfo = false;
                SceneManager.LoadScene("HomeMenu");
            }
        }
        else if (menuIndex == 1)
        {
            Information.isStudentInfo = true;
            SceneManager.LoadScene("HomeMenu");
        }
    }

    void takeRightOption()
    {
        source.clip = buttonSound;
        source.Play();

        if (menuIndex == 0)
        {
            Information.isStudentInfo = false;
            SceneManager.LoadScene("HomeMenu");
        }
        else if (menuIndex == 1)
        {
            //load the acheivment class 
            SceneManager.LoadScene("Acheivments");
        }
    }

    public GameObject secondPanel;
    public Sprite[] learningSprites; 
    public Sprite[] aboutMeSprites; 



    bool openLearning = false;
    void takeLearning()
    {
        if (!Information.doneLoadingDocuments)
        {
            Debug.LogError("not done loading yet");
            openLearning = true;
            return;
        }
        source.clip = buttonSound;
        source.Play();

        menuIndex = 0;
        secondPanel.gameObject.SetActive(true);
        //  secondPanel.transform.GetChild(1).GetComponent<Image>().sprite = learningSprites[0];

        Sprite firstSprite = learningSprites[0]; ;
        if (Information.lastSubject == "")
        {
            firstSprite = learningSprites[0];
        }
        else
        {
            switch (Information.lastSubject)
            {
                case "math":
                    firstSprite = learningSprites[1];
                    break;
                case "science":
                    firstSprite = learningSprites[2];
                    break;
                case "public speaking":
                    firstSprite = learningSprites[3];
                    break;
                case "social science":
                    firstSprite = learningSprites[4];
                    break;
            }
        }
        secondPanel.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = firstSprite;
        secondPanel.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = learningSprites[0];

        secondPanel.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Continue last lesson";
        secondPanel.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Learn something new";

        secondPanel.gameObject.SetActive(true);
    }

    bool shouldOpenAboutme = false;
    void takeAboutMe()
    {
        if (!Information.doneLoadingDocuments)
        {
            shouldOpenAboutme = true;
            return;
        }
        source.clip = buttonSound;
        source.Play();

        menuIndex = 1;
        secondPanel.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = aboutMeSprites[0];
        secondPanel.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = aboutMeSprites[1];

        secondPanel.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "See my results";
        secondPanel.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "See my achievements";
        secondPanel.gameObject.SetActive(true);
    }
    public GameObject namePanel;
    public GameObject streakPanel;
    
    void Update()
    {

        if (Information.firstTime && Information.doneLoadingDocuments && step == 0 && Information.name == "none")
        {
            
            step++;
            StartCoroutine(panel.panelAnimation(true, namePanel.transform));
            Information.firstTime = false;
                 
        }

        if(!namePanel.gameObject.activeSelf && step == 1)
        {
            step++;
            //  streakPanel.SetActive(true);
        }

        if (!streakPanel.activeSelf && step == 2)
        {
            step++;
            StartCoroutine(panel.panelAnimation(true, learningTypePanel.transform));
        }

        if(!learningTypePanel.activeSelf && step == 3)
        {
            step++;
            firstTutorialPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Hi " + Information.name + "!";
            StartCoroutine(panel.panelAnimation(true, firstTutorialPanel.transform));
        }



       /* if (openLearning)
        {
            if (doneLoading)
            {
                openLearning = false;
                takeLearning();
            }
        }
        else if (shouldOpenAboutme)
        {
            if (doneLoading)
            {
                shouldOpenAboutme = false;
                takeAboutMe();

            }
        } */
    }
}
