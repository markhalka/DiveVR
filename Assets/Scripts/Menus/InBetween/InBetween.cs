using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InBetween : MonoBehaviour
{

    public GameObject inquiry;
    public GameObject survey;

    public Button retry;
    public Button exit;
    public Button skipLoad;

    public Image LoadingBar;

    Panel surveyPanelAnimation;

    public TMPro.TMP_Text title;

    float currentValue;
    float speed = 15;

    public bool showInquiry = false;
    bool showedSurvey = false;

    public AudioSource source;
    public AudioClip buttonSound;


    void Start()
    {
        surveyPanelAnimation = new Panel();
        surveyPanelAnimation.start = new Vector3(0, 450, 0);
        surveyPanelAnimation.end = new Vector3(0, 0, 0);

        Time.timeScale = 1;
        Information.panelClosed = true; //?
        Information.pretestScore = -1;
        Information.wasPreTest = false;

        exit.onClick.AddListener(delegate { takeBack(); });
        skipLoad.onClick.AddListener(delegate { takeSkipLoad(); });
        retry.onClick.AddListener(delegate { takeRetry(); });


        Information.totalEarnedPoints += 15;
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
            retry.onClick.AddListener(delegate { takeRetry(); });
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

    void takeSkipLoad()
    {
        source.clip = buttonSound;
        source.Play();
        SceneManager.LoadScene("ModuleMenu");
    }

    void takeRetry()
    {
        source.clip = buttonSound;
        source.Play();
     //   Information.nextScene--; // i dont know why this is needed but oh well
        SceneManager.LoadScene("ScienceMain");
    }

    void takeBack()
    {
        source.clip = buttonSound;
        source.Play();

        SceneManager.LoadScene("ModuleMenu");
    }

    void takeSkip()
    {
        doneLoading = true;
        //Information.doneLoading = true;
       // Information.skip = true;
    }

    public GameObject certificate;
    bool checkCertificate()
    {
        Information.socialMediaMessage = "";

        int currScore = (int) Information.score;
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
            int pastScore;
            int improvment;
            if (tests.Count > 0 && int.TryParse(tests[tests.Count - 1].score, out pastScore))
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

                improvment = currScore - (int) Information.pretestScore;
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

    bool submitedDAta = false;

    bool doneLoading = false;
    void Update()
    {
        if (wasCertificat && !certificate.activeSelf)
        {
            Debug.LogError("it was certificate");
            handleInquiry();
            wasCertificat = false;
        }
        if (!inquiry.activeSelf && !certificate.activeSelf && !survey.activeSelf)
        {
            if (!survey.activeSelf && !showedSurvey && Information.shouldShowSurvey)
            {
                showedSurvey = true;
                survey.SetActive(true); 
                StartCoroutine(surveyPanelAnimation.panelAnimation(true, survey.transform));
                Debug.LogError("started survey animation");
                return;
            }
            if (!submitedDAta)
            {
                submitedDAta = true;
                XMLWriter.savePoints();
                XMLWriter.saveCurrentSubjectAndGrade();
             //   GameObject.Find("Menu").GetComponent<MenuBar>().takeQuitSave();
            }
            if (currentValue < 100)
            {
                currentValue += speed * Time.deltaTime;
            }
            else
            {
                doneLoading = true;
            }
        }

        if (doneLoading)
        {
            SceneManager.LoadScene("ModuleMenu"); 
        }

        LoadingBar.fillAmount = currentValue / 100;
    }
}

