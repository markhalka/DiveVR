using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InformationPanel : MonoBehaviour
{
    //the only thing that should be here, is the code that gets and decides what the text on the panel should be
    //ok you can manipulate closeonend to not close for the start panels

    // ok so right now, comment out everything that doesn't have to do with pretest stuff 

    //ok, so now let's make sure the right model is loaded

    // ok, everythings going well, you got the start panels, now its time to mofuckin display it in the panel

    // so to do that, first get the panel to show up, and then go away
    //then make sure the buttons work (you should have code so that if its start panels, you don't close on end, you just go to the next one)

    // ok, so now the start panels work
    // so now get the tectonic shit working 



    public TMP_Text justTitle;

    public bool closeOnEnd = true;
    bool newModelLoaded = false;
    public bool simpleClose = false;

    public AudioSource source;
    public AudioClip open;
    public AudioClip close;
    public AudioClip flip;


    public GameObject panelContainer;

    public GameObject quizPanel;
    public GameObject hintPanel;
    public GameObject pretestPanel; // this has the pretest panel script 
    public GameObject postTest;


    public Button startQuizButton;
    public Button quizButtonCenter;
    public Button quizButtonLeft;

    public Button soundButtonCenter;
    public Button soundButtonLeft;
    public Button soundButtonRight;

    public Button hintButton;

    public Button postTestOk;

    public int startOffset = 0;
    List<int> startPanels;
    int startPanelIndex = 0;

    public LocationPanel locationPanel; //this should be attached to it
    public PretestPanel pretest;

   
    void Start()
    {
        
        closeOnEnd = true;
        startOffset = 0;
        startPanels = new List<int>();
        initStartPanels();
        initPanelButtons();

    /*    hintButton.onClick.AddListener(delegate { takeHint(); });
        startQuizButton.onClick.AddListener(delegate { startQuiz(); });

     

        postTestOk.onClick.AddListener(delegate { takePostTestOk(); });*/


    }

    public void showPanel(int index)
    {

        Information.panelIndex = index + startOffset;
        loadNewModel();
    }

    public void showTitle(int index)
    {
        justTitle.gameObject.SetActive(true);
        justTitle.text = Information.userModels[index + startOffset].simpleInfo[0];
    }

    #region startPanels
    void initStartPanels()
    {
        startPanels = new List<int>();
        startOffset = 0;
        startPanelIndex = 0;
        
        ParseData.parseModel();
        for (int i = 0; i < Information.userModels.Count; i++)
        {
            if (Information.userModels[i].section == -1)
            {
                startOffset++;
                startPanels.Add(i);
            }
        }
        if (startOffset > 0)
        {
            changeStart();
        }
    }

    //this method should not close it, but it should just show the next text 
    //you need to redefine what the buttons do...
    void changeStart()
    {
        closeOnEnd = false;
        Debug.LogError("got the start panels: " + startPanels.Count);
        StartCoroutine(locationPanel.moveAnimation(true));
        nextStart();
    }

    void nextStart()
    {
        if(startPanelIndex >= startPanels.Count)
        {
            return;
        }

        if(startPanelIndex > startPanels.Count - 2)
        {
            closeOnEnd = true;
        }
        currentModel = Information.userModels[startPanels[startPanelIndex]];
        locationPanel.simple.text = currentModel.simpleInfo[0];
        locationPanel.advanced.text = currentModel.advancedInfo[0]; //?
        startPanelIndex++;
    
    }

    #endregion

  /*  void takePostTestOk()
    {
        postTest.SetActive(false);
        Information.wasPreTest = false;
        panelContainer.SetActive(true); //??
    }


    public void takeHint()
    {
        GameObject curr = null;
        switch (Information.subject)
        {
            case "math":
                curr = GameObject.Find("HelpContainer");
                if (curr != null)
                {
                    curr.GetComponent<HelpPanel>().callHelp();
                }
                break;
            case "science":

                if (Information.currentScene == "Models")
                {
                    curr = GameObject.Find("Main");
                    curr.GetComponent<ScienceModels>().takeHelp();
                }
                else
                {
                    curr = GameObject.Find("Quiz");
                    curr.GetComponent<QuizMenu>().takeHelp();
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
    }
  */

    void Update()
    {

        if (buttonPause < buttonThres)
        {
            buttonPause++;
        }

   /*     if (Information.wasPreTest && Information.isQuiz == 0 && pretestPanel.activeSelf && !isTutorialPanel && !postTest.activeSelf)
        {
            quizPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Start Quiz";
            pretest.pretestNotOk();
            panelContainer.SetActive(false);
            postTest.SetActive(true);
        }
   */
   /*     if (!closeOnEnd)
        {
            if (!Information.panelClosed && !newModelLoaded && !pretestPanel.activeSelf)
            {
                loadNewModel();
            }
        }
        else
        {
            if (panelContainer.gameObject.activeSelf && !newModelLoaded && !pretestPanel.activeSelf)
            {
                loadNewModel();
            }
        } */
    }
/*
    void startQuiz()
    {
        Information.isQuiz = 1;
        closeOnEnd = true;
        if (Information.wasPreTest)
        {
            Information.isIncorrect = true;
            Information.notSure = true;
            Debug.LogError("here and setting incorrect to be true");
        }
        else
        {
            quizPanel.SetActive(false);
        }

        StartCoroutine(locationPanel.moveAnimation(false));
    }

    */

    Model currentModel;

    public void loadNewModel()
    {
        wasShowingTitle = justTitle.gameObject.activeSelf;
        justTitle.transform.parent.gameObject.SetActive(false);
        locationPanel.simple.text = justTitle.text; //maybe?
        source.clip = open;
        source.Play();

       // Debug.Log(closeOnEnd )
        if (closeOnEnd)
        {
            Debug.LogError("showing panel...");
            StartCoroutine(locationPanel.moveAnimation(true));
        }

        newModelLoaded = true;
        if (Information.userModels == null && Information.tutorialModel == null)
        {
            ParseData.parseModel();
            if (Information.userModels.Count < 1)
            {
                return;
            }
        }
        currentModel = getModel();

       locationPanel. simple.text = currentModel.simpleInfo[0];

        if (currentModel.advancedInfo.Count > 0)
        {
            locationPanel.advanced.text = currentModel.advancedInfo[0].Trim();
        }
        else
        {
            locationPanel.advanced.text = "";
        }
    }


    bool wasShowingTitle = false;



    void OnDisable()
    {
        Information.tutorialModel = null;
    }

    Model getModel()
    {
        if (Information.tutorialModel != null)
        {
            return Information.tutorialModel;
        }
        else
        {
            Debug.LogError(Information.panelIndex + " panelindex");
            return Information.userModels[Information.panelIndex];
        }
    }


    void initPanelButtons()
    {
        Button next = panelContainer.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        Button back = panelContainer.transform.GetChild(0).GetChild(0).GetComponent<Button>();

        next.onClick.AddListener(delegate { takeInformationClick(true); });
        back.onClick.AddListener(delegate { takeInformationClick(false); });

        soundButtonCenter.onClick.AddListener(delegate { takeSound(); });
        soundButtonLeft.onClick.AddListener(delegate { takeSound(); });
        soundButtonRight.onClick.AddListener(delegate { takeSound(); });
    }

    //here you would use the microsoft azure shit
    void takeSound()
    {
      //  GameObject.Find("Menu").GetComponent<MenuBar>().setText(advanced.text);
    }

    int buttonPause = 0;
    int buttonThres = 15;

    //you need to double check this method 
    public int takeInformationClick(bool next)
    {
        if (buttonPause < buttonThres)
        {
            return 0;
        }

        buttonPause = 0;
        int direction = -1;
        if (next)
        {
            direction = 1;
        }
        int output = 0;

        Information.lableIndex += direction;

        if (Information.lableIndex < 0)
        {
            Information.lableIndex = currentModel.advancedInfo.Count - 1;
            output = -1;
        }

        if (currentModel == null)
        {
            Debug.LogError("it is null for some reason");
            return -1;
        }

        if (Information.lableIndex >= currentModel.advancedInfo.Count)
        {
            Information.lableIndex = 0;
            output = 1;

        }

        if (Information.lableIndex < currentModel.advancedInfo.Count)
            locationPanel.advanced.text = currentModel.advancedInfo[Information.lableIndex];

        if (output == 1)
        {
            if (closeOnEnd)
            {
                source.clip = close;
                source.Play();
                Debug.LogError("closing...");
                if (simpleClose)
                {
                    newModelLoaded = false;
                    panelContainer.SetActive(false);
                }
                else
                {
                    StartCoroutine(locationPanel.moveAnimation(false));
                }

            }
            else
            {
                nextStart();
                newModelLoaded = false;
                Information.panelClosed = true;
            }
        }

        if (!closeOnEnd || closeOnEnd && output != 1)
        {
            source.clip = flip;
            source.Play();
        }

        return output;
    }

}
