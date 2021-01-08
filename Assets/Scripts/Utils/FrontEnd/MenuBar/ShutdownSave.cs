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

    public GameObject endOfSession;
    public GameObject speakTimeOveruse;
    public GameObject startQuiz;
    public GameObject startTutorial;

    public Button speakingContinue;
    public Button sessionContinue;
    public Button sessionExit;


    Website network;
    void Start()
    {
        network = new Website();
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

    }



    //ok, here just check the scene, if its in science you can show the tutorial, or the quiz after like 5 minutes
    public bool wasModels = false;
    public bool wasDatabase = false;

    int minuteForQuiz = 5;

    IEnumerator checkTime()
    {
        while (true)
        {
           
            minutes++;

            if (Information.firstTime)
            {
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
