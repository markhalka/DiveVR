using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InformationPanel : MonoBehaviour
{

    TMP_Text simple;
    TMP_Text advanced;
    public TMP_Text justTitle;

    public bool closeOnEnd = true;
    bool newModelLoaded = false;
    public bool simpleClose = false;

    public AudioSource source;
    public AudioClip open;
    public AudioClip close;
    public AudioClip flip;


    public GameObject panelContainer;
    public GameObject centerContainer;
    public GameObject leftcontainer;
    public GameObject rightContainer;

    public GameObject quizPanel;
    public GameObject hintPanel;
    public GameObject pretestPanel;
    public GameObject postTest;


    public Button startQuizButton;
    public Button quizButtonCenter;
    public Button quizButtonLeft;

    public Button soundButtonCenter;
    public Button soundButtonLeft;
    public Button soundButtonRight;

    public Button hintButton;

    public Button pretestNotOkButton;
    public Button pretestOkButton;
    public Button dontShowAgain;

    public Button postTestOk;

    public enum MenuPosition { RIGHT, LEFT, CENTER };
    MenuPosition currentPosition = MenuPosition.CENTER;


    int startOffset = 0;
    List<int> startPanels;
    void Start()
    {
        closeOnEnd = true;
        startOffset = 0;
        startPanels = new List<int>();

        setPosition(currentPosition);

        hintButton.onClick.AddListener(delegate { takeHint(); });
        startQuizButton.onClick.AddListener(delegate { startQuiz(); });

        pretestNotOkButton.onClick.AddListener(delegate { pretestNotOk(); });
        pretestOkButton.onClick.AddListener(delegate { pretestOk(); });
        dontShowAgain.onClick.AddListener(delegate { takeDontShowAgain(); });

        postTestOk.onClick.AddListener(delegate { takePostTestOk(); });

        if (Information.showPreTest && !isTutorialPanel && Information.isQuiz == 0)
        {
            showPreTest();
        }
    }

    void initStartPanels()
    {
        startPanels = new List<int>();
        startOffset = 0;
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

    }

    void takePostTestOk()
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


    void Update()
    {

        if (buttonPause < buttonThres)
        {
            buttonPause++;
        }

        if (Information.wasPreTest && Information.isQuiz == 0 && pretestPanel.activeSelf && !isTutorialPanel && !postTest.activeSelf)
        {
            quizPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Start Quiz";
            pretestNotOk();
            panelContainer.SetActive(false);
            postTest.SetActive(true);
        }

        if (!closeOnEnd)
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
        }
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

        StartCoroutine(moveAnimation(false));
    }

    public void setPosition(MenuPosition position)
    {

        currentPosition = position;

        panelContainer.transform.localPosition = new Vector3(0, 0, 0);

        if (position == MenuPosition.CENTER)
        {
            centerContainer.transform.SetParent(panelContainer.transform);

            leftcontainer.transform.SetParent(transform);
            rightContainer.transform.SetParent(transform);

            centerContainer.SetActive(true);

            leftcontainer.SetActive(false);
            rightContainer.SetActive(false);
        }
        else if (position == MenuPosition.LEFT)
        {
            leftcontainer.transform.SetParent(panelContainer.transform);

            centerContainer.transform.SetParent(transform);
            rightContainer.transform.SetParent(transform);

            leftcontainer.SetActive(true);

            centerContainer.SetActive(false);
            rightContainer.SetActive(false);
        } else
        {
            rightContainer.transform.SetParent(panelContainer.transform);

            leftcontainer.transform.SetParent(transform);
            centerContainer.transform.SetParent(transform);

            rightContainer.SetActive(true);

            leftcontainer.SetActive(false);
            centerContainer.SetActive(false);
        }

        simple = panelContainer.transform.GetChild(0).GetChild(3).GetComponent<TMPro.TMP_Text>();
        advanced = panelContainer.transform.GetChild(0).GetChild(2).GetComponent<TMPro.TMP_Text>();

        if (centerContainer.activeSelf)
        {
            panelContainer.transform.localPosition = centerExit;
        }
        else if (leftcontainer.activeSelf)
        {
            panelContainer.transform.localPosition = leftExit;
        } else if (rightContainer.activeSelf)
        {
            panelContainer.transform.localPosition = rightExit;
        }
        initPanelButtons();
    }


    Model currentModel;

    public void loadNewModel()
    {
        wasShowingTitle = justTitle.gameObject.activeSelf;
        justTitle.transform.parent.gameObject.SetActive(false);
        simple.text = justTitle.text; //maybe?
        source.clip = open;
        source.Play();

        if (closeOnEnd && !simpleClose)
        {
            StartCoroutine(moveAnimation(true));
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

        simple.text = currentModel.simpleInfo[0];

        if (currentModel.advancedInfo.Count > 0)
        {
            advanced.text = currentModel.advancedInfo[0].Trim();
        }
        else
        {
            advanced.text = "";
        }
    }

    public Vector3 rightStart;
    public Vector3 rightExit;

    public Vector3 leftStart;
    public Vector3 leftExit;

    public Vector3 centerStart;
    public Vector3 centerExit;

    bool wasShowingTitle = false;

    IEnumerator moveAnimation(bool enter)
    {
        float count = 0;
        Vector3 start, end;
        start = end = new Vector3(0, 0, 0);

        switch (currentPosition)
        {
            case MenuPosition.CENTER:
                end = centerStart;
                start = centerExit;
                break;
            case MenuPosition.LEFT:
                end = leftStart;
                start = leftExit;
                break;
            case MenuPosition.RIGHT:
                end = rightStart;
                start = rightExit;
                break;
        }

        if (!enter)
        {
            var temp = start;
            start = end;
            end = temp;
        }


        while (count <= 1)
        {
            count += 0.1f;
            panelContainer.transform.localPosition = Vector3.Lerp(start, end, count);
            yield return new WaitForSeconds(0.02f);
        }

        if (!enter)
        {
            panelContainer.gameObject.SetActive(false);
            if (Information.isQuiz == 1)
            {
                Debug.LogError("ok now its true");
                wasShowingTitle = true;
            }
            if (wasShowingTitle)
            {

                justTitle.transform.parent.gameObject.SetActive(true);
            }
            newModelLoaded = false;
        }
    }


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
        GameObject.Find("Menu").GetComponent<MenuBar>().setText(advanced.text);
    }

    public int buttonPause = 0;
    public int buttonThres = 15;

    //you need to double check this method 
    public int takeInformationClick(bool next)
    {
        if (buttonPause < buttonThres)
        {
            Debug.LogError("button problem");
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
            advanced.text = currentModel.advancedInfo[Information.lableIndex];

        if (output == 1)
        {
            if (closeOnEnd)
            {
                source.clip = close;
                source.Play();
                if (closeOnEnd)
                {
                    if (simpleClose)
                    {
                        newModelLoaded = false;
                        panelContainer.SetActive(false);
                    }
                    else
                    {
                        StartCoroutine(moveAnimation(false));
                    }
                }
            }
            else
            {
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
