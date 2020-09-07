using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Watson.TextToSpeech.V1;
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





    private void Start()
    {
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
            initTextToSpeech();
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
        Debug.LogError("it is now saving...");
        /*   if(Information.username == "test@email.com")
           {
               Information.xmlDoc.Save("C:/users/hithe/Desktop/tempxml.xml");
               return;
           }*/

        Debug.LogError("here");
        /*  if (Information.topics != null)
              XMLWriter.saveTopics(Information.grade, Information.subject);
              */
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

        /*   using (UnityWebRequest www = UnityWebRequest.Put(Information.saveFileUrl + Information.username, dataToPut))
               {
                   yield return www.Send();

                   if (www.isNetworkError || www.isHttpError)
                   {
                       Debug.Log(www.error);
                   }
                   else
                   {
                       Debug.Log("Upload complete!");
                   }
               }
           */


        yield return uwr.SendWebRequest();

        Debug.LogError("done saving file...");
        /*
                if (uwr.isNetworkError)
                {
                    Debug.Log("Error While Sending: " + uwr.error);
                }
                else
                {
                    Debug.Log("Received: " + uwr.downloadHandler.text);
                }

                //that should save the points as well

                uwr = UnityWebRequest.Get(Information.saveEarnedPointsUrl + Information.username + "/" + Information.pastPointsDate + "&" + Information.totalEarnedPoints + "&" + Information.maxDivePoints); //date, amount of points, max points 
                yield return uwr.SendWebRequest();

                Debug.LogError("don sending"); */

        // Application.Quit();

    }

    void takeBackToHome()
    {
        SceneManager.LoadScene("StudentMenu");
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
        back();
    }

    /*
        bool wasChecked = false;
        public void checkTutorial()
        {
            wasChecked = true;
            foreach (var scene in Information.loadDoc.Descendants("scene"))
            {
                if (scene.Attribute("name").Value.Contains(Information.currentScene))
                {
                    if (scene.Attribute("date") == null)
                    {
                        scene.Add(new XAttribute("date", DateTime.Today.ToString("MM/dd/yyyy")));
                        tutorialPanel.transform.GetChild(1).GetComponent<Text>().text = "It looks like it's your first time, would you like a tutorial?";
                        XMLWriter.saveLoadFile();
                        tutorialPanel.SetActive(true);
                        return;
                    }
                    else
                    {
                        DateTime lastDate = DateTime.Parse(scene.Attribute("date").Value);
                        var diff = DateTime.Today - lastDate;
                        if (diff.Days > 14) //its been two weeks since their last tutorial
                        {
                            tutorialPanel.transform.GetChild(1).GetComponent<Text>().text = "It looks like it's been a while, would you like a tutorial?";
                            scene.Attribute("date").Value = DateTime.Today.ToString("MM/dd/yyyy");
                            tutorialPanel.SetActive(true);
                            XMLWriter.saveLoadFile();
                            return;
                        }
                    }
                }
            }
        }

        void takeTutorialPanel()
        {
            tutorialPanel.SetActive(false);
            takeTutorial();
        }

        void takeTutorialBack()
        {
            source.clip = buttonSound;
            source.Play();

            tutorialPanel.SetActive(false);
            //update the day here so they don't get the message everytime 

            foreach (var scene in Information.loadDoc.Descendants("scene"))
            {
                if (scene.Attribute("date") == null)
                {
                    scene.Add(new XAttribute("date", DateTime.Today.ToString("MM/dd/yyyy")));
                    XMLWriter.saveLoadFile();
                }
                else
                {
                    scene.Attribute("date").Value = DateTime.Today.ToString("MM/dd/yyyy");
                    XMLWriter.saveLoadFile();
                }
            }
        }
        */

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

        if (!Information.isVrMode)
        {
            if (shouldOpen)
            {
                shouldOpen = false;
                if (!mainMenu.activeSelf)
                {

                    StartCoroutine(openAnimation());
                }
                else
                {
                    back();
                }

            }
        }

        updateTextToSpeech();
    }



    IEnumerator openAnimation()
    {
        float count = 0;
        mainMenu.SetActive(true);
        // mainMenu
        source.clip = menuOpen;
        source.Play();
        while (count <= 1)
        {
            count += 0.1f;
            mainMenu.transform.position = Vector3.Lerp(start, end, count);

            yield return new WaitForSeconds(0.02f);
        }



        Information.isInMenu = true;
    }

    IEnumerator closeAnimation()
    {
        float count = 0;

        // mainMenu
        while (count <= 1)
        {
            count += 0.1f;
            mainMenu.transform.position = Vector3.Lerp(end, start, count);

            yield return new WaitForSeconds(0.02f);
        }

        source.clip = menuClose;
        source.Play();

        Information.isInMenu = false;
        mainMenu.SetActive(false);

    }




    void takeHide()
    {
        Information.autoHide = !Information.autoHide;
    }


    Vector3 prevAxisLock;
    Vector3 prevPosition;

    void back()
    {

        StartCoroutine(closeAnimation());

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

    #region textToSpeech


    private string iamApikey = "sjmQdyZUZerntfn5GV5n1c8VT0dW9nN2tIdv3rP5ZN60";

    private string serviceUrl = "https://api.us-south.text-to-speech.watson.cloud.ibm.com/instances/4d3a7b39-1101-4d8e-83fc-952f00fc57a4";
    private TextToSpeechService service;
    private string allisionVoice = "en-US_AllisonV3Voice";
    private string synthesizeText = "Hello, welcome to the Watson Unity SDK!";
    private string placeholderText = "Please type text here and press enter.";
    private string waitingText = "Watson Text to Speech service is synthesizing the audio!";
    private string synthesizeMimeType = "audio/wav";

    public string textToRead;

    private bool _textEntered = false;
    private AudioClip _recording = null;
    private byte[] audioStream = null;
    #endregion

    private void initTextToSpeech()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
    }

    public void setText(string text)
    {
        service.SynthesizeUsingWebsockets(text);
        //     textInput.text = waitingText;
    }

    void updateTextToSpeech()
    {


        while (service != null && !service.IsListening)
        {
            if (audioStream != null && audioStream.Length > 0)
            {
                Log.Debug("ExampleTextToSpeech", "Audio stream of {0} bytes received!", audioStream.Length.ToString()); // Use audioStream and play audio
                _recording = WaveFile.ParseWAV("myClip", audioStream);
                PlayClip(_recording);
            }
            //  textInput.text = placeholderText;

            audioStream = null;
            StartListening(); // need to connect because service disconnect websocket after transcribing https://cloud.ibm.com/docs/services/text-to-speech?topic=text-to-speech-usingWebSocket#WSsend
        }
    }

    private IEnumerator CreateService()
    {
        if (string.IsNullOrEmpty(iamApikey))
        {
            throw new IBMException("Please add IAM ApiKey to the Iam Apikey field in the inspector.");
        }

        IamAuthenticator authenticator = new IamAuthenticator(apikey: iamApikey);

        while (!authenticator.CanAuthenticate())
        {
            yield return null;
        }

        service = new TextToSpeechService(authenticator);
        if (!string.IsNullOrEmpty(serviceUrl))
        {
            service.SetServiceUrl(serviceUrl);
        }

        Active = true;
    }

    private void OnError(string error)
    {
        Active = false;

        Log.Debug("ExampleTextToSpeech.OnError()", "Error! {0}", error);
    }

    private void StartListening()
    {
        Log.Debug("ExampleTextToSpeech", "start-listening");
        service.Voice = allisionVoice;
        service.OnError = OnError;
        service.StartListening(OnSynthesize);
    }

    public bool Active
    {
        get { return service.IsListening; }
        set
        {
            if (value && !service.IsListening)
            {
                StartListening();
            }
            else if (!value && service.IsListening)
            {
                Log.Debug("ExampleTextToSpeech", "stop-listening");
                service.StopListening();
            }
        }
    }

    private void OnSynthesize(byte[] result)
    {
        Log.Debug("ExampleTextToSpeechV1", "Binary data received!");
        audioStream = ConcatenateByteArrays(audioStream, result);
    }

    #region Synthesize Without Websocket Connection
    private IEnumerator ExampleSynthesize()
    {
        byte[] synthesizeResponse = null;
        AudioClip clip = null;
        service.Synthesize(
            callback: (DetailedResponse<byte[]> response, IBMError error) =>
            {
                synthesizeResponse = response.Result;
                Log.Debug("ExampleTextToSpeechV1", "Synthesize done!");
                clip = WaveFile.ParseWAV("myClip", synthesizeResponse);
                PlayClip(clip);
            },
            text: synthesizeText,
            voice: allisionVoice,
            accept: synthesizeMimeType
        );

        while (synthesizeResponse == null)
            yield return null;

        yield return new WaitForSeconds(clip.length);
    }
    #endregion

    #region PlayClip
    private void PlayClip(AudioClip clip)
    {
        if (Application.isPlaying && clip != null)
        {
            GameObject audioObject = new GameObject("AudioObject");
            AudioSource source = audioObject.AddComponent<AudioSource>();
            source.spatialBlend = 0.0f;
            source.loop = false;
            source.clip = clip;
            source.Play();

            GameObject.Destroy(audioObject, clip.length);
        }
    }
    #endregion

    #region Concatenate Byte Arrays
    private byte[] ConcatenateByteArrays(byte[] a, byte[] b)
    {
        if (a == null || a.Length == 0)
        {
            return b;
        }
        else if (b == null || b.Length == 0)
        {
            return a;
        }
        else
        {
            List<byte> list1 = new List<byte>(a);
            List<byte> list2 = new List<byte>(b);
            list1.AddRange(list2);
            byte[] result = list1.ToArray();
            return result;
        }
    }
    #endregion



}
