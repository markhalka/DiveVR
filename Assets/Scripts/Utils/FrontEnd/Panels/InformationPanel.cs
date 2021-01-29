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
    public bool isTutorial = false;

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
    public Panel panel;


    void OnEnable()
    {
        tookPostTest = false;
        panel = new Panel();
        closeOnEnd = true;
        startOffset = 0;
        startPanels = new List<int>();


        initPanelButtons();

        if (isTutorial)
        {
            justTitle.gameObject.SetActive(false);
        }

        hintButton.onClick.AddListener(delegate { takeHint(); });
        startQuizButton.onClick.AddListener(delegate { startQuiz(); });
        postTestOk.onClick.AddListener(delegate { takePostTestOk(); });

        soundButtonCenter.onClick.AddListener(delegate { playSound(); });
        soundButtonLeft.onClick.AddListener(delegate { playSound(); });
        soundButtonRight.onClick.AddListener(delegate { playSound(); });

    }

    void playSound()
    {
        Information.tts = locationPanel.advanced.text;
    }

    public void showPanel(int index)
    {

        Information.panelIndex = index + startOffset;
        loadNewModel();
    }

    public void showTitle(int index)
    {
        justTitle.transform.parent.gameObject.SetActive(true);
        justTitle.text = Information.userModels[index + startOffset].simpleInfo[0];
    }

    public void showPostTest()
    {
        StartCoroutine(panel.panelAnimation(true, postTest.transform));
    }

    #region startPanels
    // call this in every lesson
    public void initStartPanels()
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
            justTitle.transform.parent.gameObject.SetActive(false);
            changeStart();
        }      
    }

    //this method should not close it, but it should just show the next text 
    //you need to redefine what the buttons do...
    void changeStart()
    {
        closeOnEnd = false;
        panelContainer.transform.localPosition = locationPanel.centerStart;
        panelContainer.SetActive(true);
        
        //       StartCoroutine(locationPanel.moveAnimation(true));
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
            wasShowingTitle = true;//SUPER TEMP
            justTitle.text = Information.userModels[startPanels[0]].simpleInfo[0];
            closeOnEnd = true;
        }
        currentModel = Information.userModels[startPanels[startPanelIndex]];
        locationPanel.simple.text = currentModel.simpleInfo[0];
        locationPanel.advanced.text = currentModel.advancedInfo[0]; //?
        startPanelIndex++;
    
    }

    #endregion

    public void closePanel()
    {
      /*  if (panelContainer.activeSelf)
        {
            StartCoroutine(panel.panelAnimation(false, panelContainer.transform));
        }      */
    }

    void takePostTestOk()
    {
        // Information.wasPreTest = false;
        StartCoroutine(panel.panelAnimation(false, postTest.transform));
        initStartPanels();

    }
    


    public void takeHint()
    {
        GameObject curr = null;

        if (Information.currentScene == "Models")
        {
            curr = GameObject.Find("Main");
          //  curr.GetComponent<ScienceModels>().takeHelp();
        }
        else
        {
            curr = GameObject.Find("Quiz");
         //   curr.GetComponent<QuizMenu>().takeHelp();
        }
    }

    bool tookPostTest = false;
    void Update()
    {

        if (buttonPause < buttonThres)
        {
            buttonPause++;
        }   

        if (Information.wasPreTest && Information.isQuiz == 0 && !postTest.activeSelf && !tookPostTest)
        {
            quizPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Start Quiz";
           // pretest.pretestNotOk();
            panelContainer.SetActive(false);
            StartCoroutine(panel.panelAnimation(true, postTest.transform));
            tookPostTest = true;
        }
   
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
        closePanel();
    }

    

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

       locationPanel.simple.text = currentModel.simpleInfo[0];

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
        if (isTutorial)
        {
            return Information.tutorialModel;
        }
        else
        {
            return Information.userModels[Information.panelIndex];
        }
    }


    void initPanelButtons()
    {
        GameObject[] panles = new GameObject[] { locationPanel.centerContainer, locationPanel.leftcontainer, locationPanel.rightContainer };
        foreach(var panel in panles)
        {
            Button next = panel.transform.GetChild(1).GetComponent<Button>();
            Button back = panel.transform.GetChild(0).GetComponent<Button>();

            next.onClick.AddListener(delegate { takeInformationClick(true); });
            back.onClick.AddListener(delegate { takeInformationClick(false); });
        }


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

                if (wasShowingTitle)
                {
                    justTitle.transform.parent.gameObject.SetActive(true);
                }
                

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
