using PlayFab.Internal;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShutdownSave : MonoBehaviour
{
    public float minutes = 0;
    bool isPublicSpeaking = false;

    public GameObject endOfSession;
    public GameObject speakTimeOveruse;
    public GameObject startQuiz;
    public GameObject startTutorial;



    public Button speakingContinue;
    public Button sessionContinue;
    public Button sessionExit;



    void Start()
    {

        StartCoroutine(checkTime());
        initButtons();
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void initButtons()
    {
        speakingContinue.onClick.AddListener(delegate { takeSpeakingContinue(); });
        sessionContinue.onClick.AddListener(delegate { takeSessionContinue(); });
        sessionExit.onClick.AddListener(delegate { takeSessionEnd(); });
    }


    void takeSpeakingContinue()
    {
        SceneManager.LoadScene("HomeMenu");
    }

    //reset the timer 
    void takeSessionContinue()
    {
        endOfSession.gameObject.SetActive(false);
    }


    void takeSessionEnd()
    {
        Application.Quit();
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Information.subject != null)
            if (Information.subject == "public speaking")
            {
                isPublicSpeaking = true;
                StartCoroutine(handlePublicSpeakingTime());
            }
            else
            {
                isPublicSpeaking = false;
            }
    }



    int currentSpeakingTime = 0;
    DateTime startTime;


    IEnumerator handlePublicSpeakingTime()
    {
        CustomCertificateHandler certHandler = new CustomCertificateHandler();
        UnityWebRequest uwr = UnityWebRequest.Get(Information.loadDocUrl);
        uwr.chunkedTransfer = false;
        uwr.certificateHandler = certHandler;

        yield return uwr.SendWebRequest();



        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received");
        }

        string[] output = uwr.downloadHandler.text.Split('&');

        int timeUsed = 0;
        if (!int.TryParse(output[1], out timeUsed))
        {
            Debug.LogError("could not parse the time from website");
            yield break;
        }

        startTime = DateTime.Parse(ParseData.decodeDate(output[0]));
        var difference = DateTime.Today - startTime;
        if (difference.Days > 7)
        {
            //  Website.GET(Information.setSpeakingTimeUrl + Information.username + "/" + DateTime.Today + "&0"); //reset the time
            uwr = UnityWebRequest.Get(Information.setSpeakingTimeUrl + Information.username + "/" + DateTime.Today + "&0");
            yield return uwr.SendWebRequest();
        }
        else
        {
            currentSpeakingTime = timeUsed;
            if (currentSpeakingTime > Information.maxSpeakinTime)
            {
                showSpeakingTimeError();
                yield break;
            }
        }

    }

    void showSpeakingTimeError()
    {
        speakTimeOveruse.gameObject.SetActive(true);
        speakTimeOveruse.transform.GetChild(1).GetComponent<Text>().text = "Your speaking time resets at: " + startTime.AddDays(7).ToString();
    }


    IEnumerator handleSavePublicSpeaking()
    {
        CustomCertificateHandler certHandler = new CustomCertificateHandler();


        UnityWebRequest uwr = UnityWebRequest.Get(Information.setSpeakingTimeUrl + Information.username + "/" + ParseData.encodeDate() + "&" + currentSpeakingTime);
        uwr.chunkedTransfer = false;
        uwr.certificateHandler = certHandler;

        yield return uwr.SendWebRequest();



        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received");
        }
    }


    void savePublicSpeakingTime()
    {
        StartCoroutine(handleSavePublicSpeaking());
    }

    //ok, here just check the scene, if its in science you can show the tutorial, or the quiz after like 5 minutes
    public bool wasModels = false;
    public bool wasDatabase = false;

    int minuteForQuiz = 5;
    int minuteForTutorial;

    IEnumerator checkTime()
    {
        while (true)
        {
            if (isPublicSpeaking)
            {
                currentSpeakingTime++;
                if (currentSpeakingTime > Information.maxSpeakinTime)
                {
                    //SceneManager.LoadScene("HomeMenu");
                    showSpeakingTimeError();

                }
                else
                {
                    savePublicSpeakingTime();
                }
            }
            minutes++;

            if (Information.firstTime)
            {
                Debug.LogError("it is first time");
                if (Information.currentScene == "Models" && !wasModels)
                {
                    wasModels = true;
                    startTutorial.SetActive(true);
                }
                else if (Information.currentScene == "Database" && !wasDatabase)
                {
                    wasDatabase = true;
                    startTutorial.SetActive(true);
                }
            }

            Information.minutesInScene += 0.25f;
            if (Information.minutesInScene > minuteForQuiz && Information.currentScene == "Mdoels" || Information.currentScene == "Database")
            {
                startQuiz.SetActive(true);
                Information.minutesInScene = 0;
            }

            yield return new WaitForSeconds(15);
        }
    }



    void OnApplicationQuit()
    {

    }
}
