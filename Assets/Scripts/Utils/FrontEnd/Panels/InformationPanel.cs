using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//ok, so for some reason in the tutorial it closes when you click next 

//ok, just do it here, just have a public function I guess, that shows the pretest before anything else
//than you can store information.was pretest 



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


    public Button quizButtonCenter;
    public Button quizButtonLeft;

    public Button soundButtonCenter;
    public Button soundButtonLeft;
    public Button soundButtonRight;

    public GameObject quizPanel;
    public Button startQuizButton;

    public GameObject hintPanel;
    public Button hintButton;

    public GameObject pretestPanel;
    public Button pretestNotOkButton;
    public Button pretestOkButton;
    public Button dontShowAgain;

    public GameObject postTest;
    public Button postTestOk;





    public bool shouldStayCenter = true;
    public bool shouldStayRight = true;

    //you also need to get rid of the center panel, in all cases



    //ok, so for some reason this shit isn't working right now

    //

    void Start()
    {

        //   quizButtonCenter.onClick.AddListener(delegate { startQuiz(); });
        //    quizButtonLeft.onClick.AddListener(delegate { startQuiz(); });

        closeOnEnd = true;

        shouldStayCenter = true;
        setCenter(shouldStayCenter);

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

    void takePostTestOk()
    {
        postTest.SetActive(false);
        Information.wasPreTest = false;
        panelContainer.SetActive(true); //??
    }

    //also, for the pretest, you shouldn't get hints 
    #region pretest stuff
    public bool isTutorialPanel = false;
    void showPreTest()
    {
        pretestPanel.transform.SetParent(panelContainer.transform);
        pretestPanel.SetActive(true);
        justTitle.transform.parent.gameObject.SetActive(false);
        shouldStayCenter = true;
        //   panelContainer.SetActive(false);

    }

    void pretestOk()
    {
        Debug.LogError("called again");
        Information.isQuiz = 1;
        Information.wasPreTest = true;
        //  StartCoroutine(enterAniamtion());
        panelContainer.SetActive(false);
        quizPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Not sure";
        StartCoroutine(exitAnimation()); //double check
    }

    void pretestNotOk()
    {
        //StartCoroutine(exitAnimation());
        pretestPanel.transform.SetParent(pretestPanel.transform.parent.parent); //get rid of that
        shouldStayCenter = true;
        setCenter(shouldStayCenter); //that should set it up for init staret panels 
        pretestPanel.SetActive(false); //that should allow the other things to show up

        //put everything back to normal
    }

    void takeDontShowAgain()
    {
        Information.showPreTest = false;
        XMLWriter.savePreTestConfig();
        pretestNotOk();
    }

    #endregion


    public void showHintPanel()
    {
        hintPanel.SetActive(true);
    }

    public void closeHintPanel()
    {
        hintPanel.SetActive(false);
    }




    public void takeHint()
    {
        //  source.clip = button;
        //    source.Play();

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
            Debug.LogError("hereee");
            quizPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Start Quiz";
            pretestNotOk();
            //  Information.wasPreTest = false;
            panelContainer.SetActive(false);

            postTest.SetActive(true);
        }

        if (!closeOnEnd)
        {
            if (!Information.panelClosed && !newModelLoaded && !pretestPanel.activeSelf)
            {
                Debug.LogError("this was good");
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

        StartCoroutine(exitAnimation());
    }

    public void setCenter(bool center)
    {
        panelContainer.transform.localPosition = new Vector3(0, 0, 0);
        shouldStayCenter = center;

        if (center)
        {
            centerContainer.transform.SetParent(panelContainer.transform);

            leftcontainer.transform.SetParent(transform);
            rightContainer.transform.SetParent(transform);

            centerContainer.SetActive(true);

            leftcontainer.SetActive(false);
            rightContainer.SetActive(false);


        }
        else
        {
            leftcontainer.transform.SetParent(panelContainer.transform);

            centerContainer.transform.SetParent(transform);
            rightContainer.transform.SetParent(transform);

            leftcontainer.SetActive(true);

            centerContainer.SetActive(false);
            rightContainer.SetActive(false);

            shouldStayRight = false;

        }

        simple = panelContainer.transform.GetChild(0).GetChild(3).GetComponent<TMPro.TMP_Text>();
        advanced = panelContainer.transform.GetChild(0).GetChild(2).GetComponent<TMPro.TMP_Text>();

        if (centerContainer.activeSelf)
        {
            //  panelContainer.transform.localPosition = centerExit;
        }
        else if (leftcontainer.activeSelf)
        {
            panelContainer.transform.localPosition = leftExit;
        }

        initPanelButtons();

    }


    public void setLeftorRight(bool right)
    {
        panelContainer.transform.localPosition = new Vector3(0, 0, 0);
        shouldStayCenter = false;
        if (right)
        {
            rightContainer.transform.SetParent(panelContainer.transform);

            leftcontainer.transform.SetParent(transform);
            centerContainer.transform.SetParent(transform);

            rightContainer.SetActive(true);

            leftcontainer.SetActive(false);
            centerContainer.SetActive(false);

            shouldStayRight = true;

        }
        else
        {
            leftcontainer.transform.SetParent(panelContainer.transform);

            rightContainer.transform.SetParent(transform);
            centerContainer.transform.SetParent(transform);

            leftcontainer.SetActive(true);

            rightContainer.SetActive(false);
            centerContainer.SetActive(false);

            shouldStayRight = false;

        }

        simple = panelContainer.transform.GetChild(0).GetChild(3).GetComponent<TMPro.TMP_Text>();
        advanced = panelContainer.transform.GetChild(0).GetChild(2).GetComponent<TMPro.TMP_Text>();
        if (rightContainer.activeSelf)
        {
            panelContainer.transform.localPosition = rightExit;
        }
        else if (leftcontainer.activeSelf)
        {
            panelContainer.transform.localPosition = leftExit;
        }
        initPanelButtons();
    }


    Model currentModel;

    public void loadNewModel()
    {
        Debug.LogError("neow model loaded");

        wasShowingTitle = justTitle.gameObject.activeSelf;
        Debug.LogError(wasShowingTitle + " was showing title");
        justTitle.transform.parent.gameObject.SetActive(false);
        simple.text = justTitle.text; //maybe?
        Debug.LogError("noew model loaded");
        source.clip = open;
        source.Play();

        if (closeOnEnd && !simpleClose)
        {
            StartCoroutine(enterAniamtion());
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

    IEnumerator enterAniamtion()
    {
        float count = 0;
        Vector3 start, end;

        if (shouldStayCenter)
        {
            //it comes from the bottom

            end = centerStart;
            start = centerExit;
        }
        else
        {
            if (shouldStayRight)
            {
                end = rightStart;
                start = rightExit;

            }
            else
            {
                end = leftStart;
                start = leftExit;
            }
            //it comes from the side 

        }

        while (count <= 1)
        {
            count += 0.1f;
            panelContainer.transform.localPosition = Vector3.Lerp(start, end, count);
            //   panelContainer.transform

            yield return new WaitForSeconds(0.02f);
        }

    }

    //i think that should work

    IEnumerator exitAnimation()
    {
        float count = 0;
        Vector3 start, end;

        if (shouldStayCenter)
        {
            //it comes from the bottom
            start = centerStart;
            end = centerExit;
        }
        else
        {

            //it comes from the side 
            if (shouldStayRight)
            {
                start = rightStart;
                end = rightExit;

            }
            else
            {
                start = leftStart;
                end = leftExit;
            }
        }

        while (count <= 1)
        {
            count += 0.1f;
            panelContainer.transform.localPosition = Vector3.Lerp(start, end, count);

            yield return new WaitForSeconds(0.02f);
        }
        // gameObject.SetActive(false);
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
        Debug.LogError(currentModel.advancedInfo.Count + " " + Information.lableIndex);
        if (Information.lableIndex >= currentModel.advancedInfo.Count)
        {
            Information.lableIndex = 0;
            output = 1;

        }

        if (Information.lableIndex < currentModel.advancedInfo.Count)
            advanced.text = currentModel.advancedInfo[Information.lableIndex];

        if (output == 1)
        {

            Debug.LogError("reached the end " + closeOnEnd);
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
                        StartCoroutine(exitAnimation());
                    }

                }
                //     StartCoroutine(exitAnimation()); //?


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
