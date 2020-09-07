using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InBetween : MonoBehaviour
{



    public Image LoadingBar;
    public Button retry;
    public Button exit;
    public Button skipLoad;

    public GameObject redoPanel;

    public GameObject inquiry;

    public TMPro.TMP_Text title;

    float currentValue;
    float speed = 15;
    public bool showInquiry = false;


    public AudioSource source;
    public AudioClip buttonSound;

    public GameObject survey;
    public Button submitSurvey;
    public Button cancelSurvey;
    public Button dontShowSurvey;

    //add the survye animation as well



    bool showedSurvey = false;




    void Start()
    {
        Time.timeScale = 1;
        Information.panelClosed = true; //?
        Information.pretestScore = -1;
        Information.wasPreTest = false;

        retry.onClick.AddListener(delegate { openRetry(); });
        exit.onClick.AddListener(delegate { takeBack(); });
        skipLoad.onClick.AddListener(delegate { takeSkipLoad(); });

        dontShowSurvey.onClick.AddListener(delegate { takeDontShowSurvey(); });
        cancelSurvey.onClick.AddListener(delegate { takeCancelSurvey(); });
        submitSurvey.onClick.AddListener(delegate { takeSubmitSurvey(); });


        redoPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { takeWrongAnswer(); });
        redoPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { takeRetry(); });
        redoPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { takeContinue(); });

        Information.totalEarnedPoints += 15;
    }


    #region surveyshit
    public void takeSubmitSurvey()
    {
        Transform t = survey.transform.GetChild(0);
        XMLWriter.submitSurvey((int)t.GetChild(0).GetComponent<Slider>().value, (int)t.GetChild(1).GetComponent<Slider>().value, t.GetChild(2).GetComponent<TMP_InputField>().text, t.GetChild(3).GetComponent<TMP_InputField>().text);
        StartCoroutine(closeSurvey(true));
    }

    public void takeCancelSurvey()
    {
        StartCoroutine(closeSurvey(false));
    }

    void takeDontShowSurvey()
    {
        Information.shouldShowSurvey = false;
        XMLWriter.saveSurveyConfig();
        StartCoroutine(closeSurvey(false));
    }

    Vector2 surveyStart = new Vector2(0, 450.86f);
    Vector2 surveyEnd = new Vector2(0, 0);


    IEnumerator openSurvey()
    {
        float count = 0;
        survey.SetActive(true);
        while (count <= 1)
        {
            count += 0.1f;
            survey.transform.localPosition = Vector3.Lerp(surveyStart, surveyEnd, count);
            //   panelContainer.transform

            yield return new WaitForSeconds(0.02f);
        }

    }

    IEnumerator closeSurvey(bool submited)
    {

        float count = 0;
        if (submited)
        {
            survey.transform.GetChild(0).gameObject.SetActive(false);
            survey.transform.GetChild(1).gameObject.SetActive(true);

            while (count < 2)
            {
                count++;
                yield return new WaitForSeconds(0.5f);
            }
        }
        count = 0;

        while (count <= 1)
        {
            count += 0.1f;
            survey.transform.localPosition = Vector3.Lerp(surveyEnd, surveyStart, count);
            //   panelContainer.transform

            yield return new WaitForSeconds(0.02f);
        }

        survey.SetActive(false);

    }

    #endregion

    void openRetry()
    {
        source.clip = buttonSound;
        source.Play();

        redoPanel.gameObject.SetActive(true);
    }

    void handleInquiry()
    {
        if (Information.inquire != null && Information.inquire != "")
        {
            showInquiry = true;
            inquiry.SetActive(true);
        }
        else
        {
            inquiry.SetActive(false);
        }
    }

    bool wasCertificat;
    private void OnEnable()
    {
        wasCertificat = false;
        Time.timeScale = 1;
        currentValue = 0;
        if (Information.isGame)
        {
            retry.onClick.AddListener(delegate { takeSkip(); });
            retry.transform.GetChild(0).GetComponent<TMP_Text>().text = "Skip Game";
            Information.isGame = false;
        }
        else
        {
            retry.onClick.AddListener(delegate { openRetry(); });
            retry.transform.GetChild(0).GetComponent<TMP_Text>().text = "Retry";
        }
        if (Information.wasWrongAnswer)
        {
            Information.wasWrongAnswer = false;

        }
        else
        {
            if (checkCertificate())
            {
                wasCertificat = true;
            }
            else
            {
                handleInquiry();
            }
        }
    }

    void OnDisable()
    {
        redoPanel.gameObject.SetActive(false); //make sure it doesn't pop up again
    }



    void takeWrongAnswer()
    {
        source.clip = buttonSound;
        source.Play();

        GameObject curr = null;
        if (Information.subject == "math")
        {

            curr = GameObject.Find("Main");
            curr.GetComponent<MathScript>().redoWrongAnswers();
        }
        else if (Information.subject == "science")
        {
            if (Information.currentScene == "Models")
            {
                curr = GameObject.Find("Main");
                curr.GetComponent<ScienceModels>().startWrongQuiz();
                gameObject.SetActive(false);
            }
            else
            {
                curr = GameObject.Find("Quiz");
                if (curr == null)
                {
                    Debug.LogError("could not find quiz in the current thing ");
                    return;
                }
                curr.GetComponent<QuizMenu>().redoWrongAnswers();
                gameObject.SetActive(false);

            }
        }
        redoPanel.SetActive(false);
        gameObject.SetActive(false);
        Information.wasWrongAnswer = true;
    }

    void takeContinue()
    {
        source.clip = buttonSound;
        source.Play();

        redoPanel.SetActive(false);
        currentValue = 0;
    }

    void takeSkipLoad()
    {
        source.clip = buttonSound;
        source.Play();
        Information.doneLoading = true;
    }

    void takeRetry()
    {
        source.clip = buttonSound;
        source.Play();

        if (Information.subject == "science")
        {
            Information.retry = true;
            //    Information.nextScene--;
            SceneManager.LoadScene("ScienceMain");
            return;
        }
        else
        {
            Information.retry = true;
            Information.doneLoading = true;
        }

    }

    void takeBack()
    {
        source.clip = buttonSound;
        source.Play();

        SceneManager.LoadScene("ModuleMenu");
    }

    void takeSkip()
    {
        Information.doneLoading = true;
        Information.skip = true;
    }

    public GameObject certificate;
    bool checkCertificate()
    {


        Information.socialMediaMessage = "";

        float currScore = Information.score;
        Information.score = 0;


        if (Information.subject == "public speaking")
        {
            if (currScore > 80)
            {
                Information.acheivment = "Master speaker";
            }
            else if (currScore > 70)
            {
                Information.acheivment = "Influencer";
            }
            else if (currScore > 60)
            {
                Information.acheivment = "Great speaker";
            }
            else
            {
                return false;
            }

            certificate.SetActive(true);

            return true;
        }

        int currentScene = Information.nextScene;
        if (Information.subject == "math")
        {
            currentScene--;
        }
        else if (Information.subject == "science")
        {
            currentScene = ParseData.getScienceScene();
            if (currentScene == -1)
            {
                Debug.LogError("could not find science scene");
                return false;
            }
        }


        string topicName = Information.topics[currentScene].name;
        if (currScore >= 80)
        {
            Information.acheivment = "scoring " + currScore + "% in " + topicName;
            certificate.SetActive(true);
        }
        else
        {
            var tests = Information.topics[currentScene].tests;
            float pastScore;
            float improvment;
            if (tests.Count > 0 && float.TryParse(tests[tests.Count - 1].score, out pastScore))
            {
                improvment = currScore - pastScore;
                if (improvment > 20)
                {
                    Information.acheivment = "improving " + improvment + "% in " + topicName;
                    certificate.SetActive(true);
                    return true;
                }
            }

            if (Information.pretestScore > 0)
            {

                improvment = currScore - Information.pretestScore;
                if (improvment > 20)
                {
                    Information.acheivment = "improving " + improvment + "% in " + topicName;
                    certificate.SetActive(true);
                }
                else
                {
                    return false;
                }


            }
            else
            {
                return false;
            }
        }
        return true;
    }

    string getPretest()
    {
        foreach (var subject in Information.xmlDoc.Descendants("grades"))
        {
            foreach (var lesson in subject.Elements("lesson"))
            {
                if (lesson.Attribute("pretest") != null)
                {
                    string[] temp = lesson.Attribute("pretest").Value.Split(' ');
                    return temp[temp.Length - 1];
                }
            }
        }
        return "";
    }

    bool submitedDAta = false;
    void Update()
    {
        if (wasCertificat && !certificate.activeSelf)
        {
            Debug.LogError("it was certificate");
            handleInquiry();
            wasCertificat = false;
        }



        if (!inquiry.activeSelf && !certificate.activeSelf && !redoPanel.activeSelf && !survey.activeSelf)
        {
            if (!survey.activeSelf && !showedSurvey && Information.shouldShowSurvey)
            {
                showedSurvey = true;
                StartCoroutine(openSurvey());

                return;
            }
            if (!submitedDAta)
            {
                submitedDAta = true;
                XMLWriter.savePoints();
                XMLWriter.saveCurrentSubjectAndGrade();
                GameObject.Find("Menu").GetComponent<MenuBar>().takeQuitSave();
            }
            if (currentValue < 100)
            {
                currentValue += speed * Time.deltaTime;
            }
            else
            {
                Information.doneLoading = true;
            }
        }

        LoadingBar.fillAmount = currentValue / 100;
    }
}

