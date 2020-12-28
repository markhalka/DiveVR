using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#pragma warning disable 0649

//ideas for teaching public speaking
//1. show ted talks, and highlight certain things about the speach
//2. then have the user try and do it as well (e.g equal eye contact for each person)
//3. teach them basic gestures
//4. just look up public speaking tips online
//5. then maybe make an improve club (where people can meet, there will be an instructor, and you can host a virtual improve club, and help kids 

//ok, then take a look at media pipe, run it on the desktop, and see if you can keep track of the users movments
//then try and program in some parameters for what counts as "good" hand gestures and "bad" handgestures 
//then you can start to add better people, and make the the whole thing more interactive 
/*

public class Presentation : MonoBehaviour
{

    string[] commonFillerWords = new string[] { "so", "you know", "like", "right", "I mean", "or something", "believe me", "basically", "sort of", "%HESITATION" };
    int[] fillerWordsCount;


    float[] time;
    float lastTime;
    float currentTime;
    GameObject currentStudent;

    public Button endSpeech;

    public GameObject models;
    public GameObject cursor;
    public GameObject lowerBound;
    public GameObject upperBound;
    GameObject people;
    public Button endPresentationClassroom;
    public Button endPresentationOffice;

    public GameObject buttons;



    private void Start()
    {


        Information.isVrMode = false;
        Information.lowerBoundary = lowerBound;
        Information.upperBoundary = upperBound;



#if UNITY_IPHONE
     Information.isVrMode = true;
#elif UNITY_ANDROID
    // Information.isVrMode = true;
#endif
        loadModel();


        if (!Information.isVrMode)
        {
            cam.GetComponent<CameraMove>().enabled = true;
            UnityEngine.XR.XRSettings.enabled = false;
        }
        else
        {
            UnityEngine.XR.XRSettings.enabled = true;
        }

        fillerWordsCount = new int[commonFillerWords.Length];
        time = new float[people.transform.childCount + 1];
        lastTime = Time.realtimeSinceStartup;
        currentTime = Time.realtimeSinceStartup;
        endPresentationClassroom.onClick.AddListener(delegate { loadBar(); });
        endPresentationOffice.onClick.AddListener(delegate { loadBar(); });

        //  Permission.RequestUserPermission(Permission.Microphone);
        initAurora();

        startPage.SetActive(true);
        initButtons();

        chairsToHide.SetActive(false);
        Information.currentScene = "publicSpeaking";
        takeStartNext();
    }

    public GameObject LoadingBar;
    float currentValue = 0;
    float speed = 25; //test this

    void loadBar()
    {
        // SceneManager.LoadScene("ModuleMenu");
        startToneAnalysis();
        buttons.gameObject.SetActive(true);
        results.SetActive(true);
        initButtons();

    }




    GameObject pastSelected;
    void updateLoadBar()
    {
        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        if (UnityEngine.Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.GetComponent<Button>() != null || hit.transform.gameObject.GetComponent<Toggle>() != null)
            {
                //ok then do the check
                if (hit.transform.gameObject.name.Contains("EndPresentation"))
                {
                    if (startPage.activeSelf || results.activeSelf)
                    {
                        pastSelected = null;
                        currentValue = 0;
                        LoadingBar.gameObject.SetActive(false);
                        return;
                    }
                    else
                    {
                        LoadingBar.transform.localScale = new Vector3(5, 5, 5);
                        LoadingBar.SetActive(true);
                    }
                }
                else
                {
                    LoadingBar.transform.localScale = new Vector3(1, 1, 1);
                }

                if (pastSelected == null)
                {
                    currentValue = 0;
                    LoadingBar.transform.localPosition = hit.transform.localPosition;

                }
                else
                {
                    if (pastSelected.transform == hit.transform)
                    {

                    }
                    else
                    {
                        currentValue = 0;
                    }
                }
                pastSelected = hit.transform.gameObject;
                LoadingBar.gameObject.SetActive(true);
                if (currentValue < 100)
                {
                    currentValue += speed * Time.deltaTime;
                }
                else
                {
                    if (hit.transform.gameObject.GetComponent<Button>() != null)
                    {
                        hit.transform.gameObject.GetComponent<Button>().onClick.Invoke();
                        currentValue = 0;
                    }
                    else
                    {
                        hit.transform.gameObject.GetComponent<Toggle>().isOn = !hit.transform.gameObject.GetComponent<Toggle>().isOn;
                        currentValue = 0;
                    }
                }
                LoadingBar.transform.GetChild(0).GetComponent<Image>().fillAmount = currentValue / 100;
            }
            else
            {
                pastSelected = null;
                LoadingBar.gameObject.SetActive(false);
                currentValue = 0;
            }
        }
        else
        {
            pastSelected = null;
            LoadingBar.gameObject.SetActive(false);
            currentValue = 0;
        }
    }




    void initButtons()
    {
        buttons.gameObject.SetActive(true);
        buttons.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        buttons.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        if (startPage.activeSelf)
        {
            buttons.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { takeStartNext(); });
            buttons.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { takeStartBack(); });
        }
        else if (results.activeSelf)
        {
            buttons.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { takeNext(); });
            buttons.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { takeBack(); });
        }
    }

    void loadModel()
    {
        models.transform.GetChild(Information.nextScene).gameObject.SetActive(true);
        people = models.transform.GetChild(Information.nextScene).GetChild(0).gameObject;
        if (Information.nextScene == 0)
        {
            endPresentationOffice.gameObject.SetActive(false);
        }
        else
        {
            endPresentationClassroom.gameObject.SetActive(false);
        }

    }

    public GameObject particleSystem;
    void initAurora()
    {
        for (int i = 0; i < people.transform.childCount; i++)
        {
            GameObject ps = Instantiate(particleSystem, people.transform.GetChild(i).transform, false);
            ps.transform.localPosition = new Vector3(0, 0.8f, 0);
        }
    }


    void startToneAnalysis()
    {
        StopRecording();
        initToneAnalyzer();
        for (int i = 0; i < commonFillerWords.Length; i++)
        {
            fillerWordsCount[i] = Regex.Matches(outputText, commonFillerWords[i]).Count;
        }

        results.SetActive(true);
        initButtons();
    }

    private void Update()
    {
        updateLoadBar();

        if (!results.activeSelf)
            checkEyeContact();
    }




    #region eye contact
    RaycastHit hit;
    public Camera cam;
    void checkEyeContact()
    {
        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        currentTime = Time.realtimeSinceStartup;
        float timePassed = currentTime - lastTime;
        if (UnityEngine.Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Player")
            {
                updateEyeContactCount(hit.collider.gameObject, timePassed);

            }
        }
        else
        {
            time[time.Length - 1] += timePassed;
            if (currentStudent != null && currentStudent.transform.childCount > 0)
            {
                currentStudent.transform.GetChild(0).gameObject.SetActive(false);

            }
        }

        lastTime = currentTime;


    }



    void updateEyeContactCount(GameObject curr, float timePassed)
    {
        if (currentStudent != curr && currentStudent != null)
        {
            currentStudent.transform.GetChild(0).gameObject.SetActive(false);
        }

        currentStudent = curr;

        for (int i = 0; i < people.transform.childCount; i++)
        {
            if (people.transform.GetChild(i).gameObject == curr)
            {
                time[i] += timePassed;
                currentStudent.transform.GetChild(0).gameObject.SetActive(true);

            }
        }
    }

    #endregion

    #region results

    int currentPanel = 0;
    public GameObject wedge;
    public GameObject review;
    public GameObject finalPage;
    public GameObject results;
    public GameObject tone;
    public GameObject startPage;
    public GameObject chairsToHide;
    void nextPanel()
    {
        if (chairsToHide.activeSelf)
        {
            chairsToHide.SetActive(false);
        }
        clearAll();
        switch (currentPanel)
        {
            case 0: //tone checkbox
                results.transform.GetChild(1).GetComponent<Text>().text = "Select the tones you wanted to convey!";

                tone.SetActive(true);
                break;
            case 1: // bar graph
                loadBarGraph();

                break;
            case 2: //pie graph
                wedge.transform.parent.gameObject.SetActive(true);
                loadPieGraph();

                break;
            case 3:
                includedTones.transform.parent.gameObject.SetActive(true);
                loadToneGraph();

                break;
            case 4://review

                review.SetActive(true);
                loadReview();

                break;
            case 5: //last page
                buttons.gameObject.SetActive(false);
                finalPage.SetActive(true);
                loadLast();

                break;
        }
    }

    void startNextPanel()
    {
        Debug.Log("showing: " + currentPanel);
        clearAllStart();
        startPage.transform.GetChild(currentPanel).gameObject.SetActive(true);
    }
    int currColor = 0;
    int maxLength = 100;

    GameObject scrollObject = null;
    public GameObject barGraph;
    void loadBarGraph()
    {
        AwesomeCharts.BarChart script = barGraph.GetComponent<AwesomeCharts.BarChart>();
        AwesomeCharts.BarData thing = new AwesomeCharts.BarData();
        AwesomeCharts.BarDataSet set1 = new AwesomeCharts.BarDataSet();

        List<string> lables = new List<string>();

        results.transform.GetChild(1).GetComponent<Text>().text = "Filler word use";
        int maxCount = fillerWordsCount.Max();
        int index = 0;
        for (int i = 0; i < fillerWordsCount.Length; i++)
        {
            if (fillerWordsCount[i] == 0)
            {
                continue;
            }

            if (currColor > Information.colors.Length - 1)
            {
                currColor = 0;
            }

            lables.Add(commonFillerWords[i]);
            set1.AddEntry(new AwesomeCharts.BarEntry(index++, fillerWordsCount[i]));
            set1.BarColors.Add(Information.colors[index % Information.colors.Length]);

        }
        thing.CustomLabels = lables;
        thing.DataSets.Add(set1);

        script.data = thing;
        script.SetDirty();

        barGraph.gameObject.SetActive(true); //that should work 

    }


    void loadPieGraph()
    {

        barGraph.SetActive(false);

        float rotationAmount = 0;
        results.transform.GetChild(1).GetComponent<Text>().text = "Eye contact";
        float maxCount = 0;
        foreach (var curr in time)
        {
            maxCount += curr;
        }

        for (int i = 0; i < time.Length; i++)
        {
            GameObject currWedge = Instantiate(wedge.gameObject, wedge.transform, true);
            currWedge.transform.SetParent(currWedge.transform.parent.parent);
            currWedge.GetComponent<Image>().color = Information.colors[currColor++];
            if (currColor > Information.colors.Length - 1)
            {
                currColor = 0;
            }
            if (i == time.Length - 1)
            {
                currWedge.GetComponent<Image>().color = Color.grey;
            }
            float percentage = (float)time[i] / (float)maxCount;
            currWedge.GetComponent<Image>().fillAmount = percentage;
            rotationAmount += percentage * 360.0f;

            currWedge.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, rotationAmount)); //rotate z negative
        }

    }



    void clearAllStart()
    {
        for (int i = 1; i < startPage.transform.childCount - 1; i++)
        {
            startPage.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void clearAll()
    {
        barGraph.SetActive(false);
        wedge.transform.parent.gameObject.SetActive(false);
        review.SetActive(false);
        finalPage.SetActive(false);
        tone.SetActive(false);
        includedTones.transform.parent.gameObject.SetActive(false);
        scrollObject = null;

    }

    List<int> scores = new List<int>();


    void loadReview()
    {
        calculateScores();

        for (int i = 0; i < userScores.Count; i++)
        {
            GameObject newCircular = review.transform.GetChild(i).gameObject;
            newCircular.GetComponent<Image>().fillAmount = userScores[i] / 100;
            newCircular.transform.GetChild(2).GetComponent<Text>().text = Mathf.Round(userScores[i]) + "%";
            Color color = Color.green;
            if (userScores[i] < 40)
            {
                color = Color.red;
            }
            else if (userScores[i] < 70)
            {
                color = new Color(1, 0.7f, 0, 1);
            }
            else
            {
                color = Color.green;
            }
            newCircular.GetComponent<Image>().color = color;
            newCircular.transform.GetChild(2).GetComponent<Text>().color = color;

            newCircular.gameObject.SetActive(true);

        }

        //ok, so here you should save it like this:
        //date,fillerwords,eyecontact,tone,overallscore
        //then in student menu dispaly the overall score, but when they click on the breakdown than show each of the indiviual scores (not the overall) than that should be good '

    }

    void saveToFile()
    {
        float totalScore = 0;
        Topic.Test currTest = new Topic.Test();

        currTest.score = DateTime.Today + ",0,";
        for (int i = 0; i < userScores.Count; i++)
        {
            totalScore += userScores[i];

        }

        totalScore /= userScores.Count;
        currTest.score += totalScore;

        for (int i = 0; i < userScores.Count; i++)
        {
            currTest.score += "&" + userScores[i];
        }

        Debug.LogError("the current score is: " + currTest.score);
        currTest.date = DateTime.Today.ToString("MM/dd/yyy");
        Information.topics[Information.nextScene].tests.Add(currTest); //that should save it 

    }


    public GameObject circularGraph;
    public GameObject includedTones;
    public GameObject notIncludedTones;


    void loadToneGraph()
    {
        float goodToneTotal = 0;
        float toneTotal = 0;

        for (int i = 0; i < userTones.Length; i++)
        {
            float toneScore = 0;
            int userScore = 0;
            foreach (var tone in tones)
            {
                if (tone.ToneName == userTones[i].name)
                {
                    toneScore = (float)tone.Score;
                    break;
                }
            }
            if (toneScore < 0.2f)
            {
                if (userTones[i].isOn)
                {
                    toneScore = 0.2f;

                }
            }

            if (toneScore < 0.5)
            {
                userScore = 0;
            }
            else if (toneScore < 0.75)
            {
                userScore = 1;
            }
            else
            {
                userScore = 2;
            }

            if (userTones[i].isOn)
            {
                goodToneTotal += 2;
                toneTotal += userScore;

            }
            else
            {
                toneTotal -= userScore;
            }

            int shownScore = (int)toneScore * 100;

            GameObject newCircular = Instantiate(circularGraph, circularGraph.transform, true);
            newCircular.GetComponent<Image>().fillAmount = toneScore;
            newCircular.transform.GetChild(1).GetComponent<Text>().text = userTones[i].name;
            newCircular.transform.GetChild(2).GetComponent<Text>().text = shownScore + "%";
            newCircular.gameObject.SetActive(true);

            if (userTones[i].isOn)
            {
                newCircular.transform.SetParent(includedTones.transform);

            }
            else
            {
                newCircular.GetComponent<Image>().color = Color.red;
                newCircular.transform.GetChild(2).GetComponent<Text>().color = Color.red;

                newCircular.transform.SetParent(notIncludedTones.transform);
            }
        }

        float goodToneScore = toneTotal / goodToneTotal;
        goodToneScore *= 100;
        userScores.Add(goodToneScore);

        results.transform.GetChild(1).GetComponent<Text>().text = "Tone of speech";

    }

    public List<float> userScores;
    public Toggle[] userTones;

    void calculateScores()
    {
        userScores = new List<float>();

        float totalTime = 0;
        foreach (var sec in time)
        {
            totalTime += sec;
        }

        float perfectScore = totalTime / (people.transform.childCount);
        float badTime = 0;
        for (int i = 0; i < time.Length - 1; i++)
        {
            badTime += Mathf.Abs(time[i] - perfectScore);
        }

        float goodTime = 1 - badTime / totalTime;
        goodTime *= 100;
        userScores.Add(goodTime);

        float totalFiller = 0;
        foreach (var filler in fillerWordsCount)
        {
            totalFiller += filler;
        }

        int wordcount = outputText.Split(' ').Length - 1;
        float percentFiller = totalFiller / wordcount;
        percentFiller *= 100;
        userScores.Add(percentFiller);



        float goodToneTotal = 0;
        float toneTotal = 0;

        for (int i = 0; i < userTones.Length; i++)
        {
            float toneScore = 0;
            int userScore = 0;
            foreach (var tone in tones)
            {
                if (tone.ToneName == userTones[i].name)
                {
                    toneScore = (float)tone.Score;
                    break;
                }
            }


            if (toneScore < 0.5)
            {
                userScore = 0;
            }
            else if (toneScore < 0.75) //it was most likely precieved
            {
                userScore = 1;
            }
            else //it was very likely precieved 
            {
                userScore = 2;
            }

            if (userTones[i].isOn) //its a good onte
            {
                goodToneTotal += 2;
                toneTotal += userScore;

            }
            else
            {
                toneTotal -= userScore;
            }
        }

        float goodToneScore = toneTotal / goodToneTotal;
        goodToneScore *= 100;
        userScores.Add(goodToneScore);

        XMLWriter.savePresnetation(Information.nextScene, userScores);
    }

    void loadLast()
    {
        results.transform.GetChild(1).GetComponent<Text>().text = "";
        finalPage.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { takeAgain(); });
        finalPage.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { takeExit(); });
        buttons.gameObject.SetActive(false);
    }

    #endregion

    void takeExit()
    {
        SceneManager.LoadScene("OpenModule");
    }

    void takeAgain()
    {
        SceneManager.LoadScene("Presenetation");
    }

    void saveResults()
    {
        XMLWriter.savePresnetation(Information.nextScene, userScores);
    }

    void takeStartNext()
    {
        currentPanel++;

        if (currentPanel > startPage.transform.childCount - 1)
        {
            chairsToHide.SetActive(true);
            startPage.SetActive(false);
            buttons.gameObject.SetActive(false);
            initSpeechToText();
            cursor.GetComponent<Image>().enabled = false;
            currentPanel = -1;
            if (!Information.isVrMode)
            {
                cam.GetComponent<CameraMove>().enabled = true;
            }
        }
        else
        {
            startNextPanel();
        }
    }

    void takeStartBack()
    {
        currentPanel--;
        startNextPanel();
    }

    void takeNext()
    {
        if (currentPanel == 0)
        {
            saveResults();
        }
        currentPanel++;

        nextPanel();
    }

    void takeBack()
    {
        currentPanel--;
        if (currentPanel < 0)
        {
            currentPanel = 0;
        }
        nextPanel();
    }


    #region speech to text
    private string textAPIkey = "8-7aBhsDDA71z4UMNFjNzf1YgfRdMySMThe0wbM7rd1h";
    private string textURL = "https://api.us-south.speech-to-text.watson.cloud.ibm.com/instances/511fba9d-8e66-401f-b57e-7484a68867eb";

    private string outputText = "";

    private string _recognizeModel;

    private int _recordingRoutine = 0;
    private string _microphoneID = null;
    private AudioClip _recording = null;
    private int _recordingBufferSize = 1;
    private int _recordingHZ = 22050;

    private SpeechToTextService _service;

    void initSpeechToText()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateTextService());
    }

    private IEnumerator CreateTextService()
    {
        if (string.IsNullOrEmpty(textAPIkey))
        {
            throw new IBMException("Plesae provide IAM ApiKey for the service.");
        }

        IamAuthenticator authenticator = new IamAuthenticator(apikey: textAPIkey);

        //  Wait for tokendata
        while (!authenticator.CanAuthenticate())
            yield return null;

        _service = new SpeechToTextService(authenticator);
        if (!string.IsNullOrEmpty(textURL))
        {
            _service.SetServiceUrl(textURL);
        }
        _service.StreamMultipart = true;

        Active = true;
        StartRecording();
    }

    public bool Active
    {
        get { return _service.IsListening; }
        set
        {
            if (value && !_service.IsListening)
            {
                _service.RecognizeModel = (string.IsNullOrEmpty(_recognizeModel) ? "en-US_BroadbandModel" : _recognizeModel);
                _service.DetectSilence = true;
                _service.EnableWordConfidence = true;
                _service.EnableTimestamps = true;
                _service.SilenceThreshold = 0.01f;
                _service.MaxAlternatives = 1;
                _service.EnableInterimResults = true;
                _service.OnError = OnError;
                _service.InactivityTimeout = -1;
                _service.ProfanityFilter = false;
                _service.SmartFormatting = true;
                _service.SpeakerLabels = false;
                _service.WordAlternativesThreshold = null;
                _service.EndOfPhraseSilenceTime = null;
                _service.StartListening(OnRecognize, OnRecognizeSpeaker);
            }
            else if (!value && _service.IsListening)
            {
                _service.StopListening();
            }
        }
    }

    private void StartRecording()
    {
        if (_recordingRoutine == 0)
        {
            UnityObjectUtil.StartDestroyQueue();
            _recordingRoutine = Runnable.Run(RecordingHandler());
        }
    }

    private void StopRecording()
    {
        if (_recordingRoutine != 0)
        {
            // Microphone.End(_microphoneID);
            Runnable.Stop(_recordingRoutine);
            _recordingRoutine = 0;
        }
    }

    private void OnError(string error)
    {
        Active = false;

        Log.Debug("ExampleStreaming.OnError()", "Error! {0}", error);
    }

    private IEnumerator RecordingHandler()
    {
        /*     Log.Debug("ExampleStreaming.RecordingHandler()", "devices: {0}", Microphone.devices);
         //    _recording = Microphone.Start(_microphoneID, true, _recordingBufferSize, _recordingHZ);
             yield return null;  

             if (_recording == null)
             {
                 StopRecording();
                 yield break;
             }

             bool bFirstBlock = true;
             int midPoint = _recording.samples / 2;
             float[] samples = null;

             while (_recordingRoutine != 0 && _recording != null)
             {
                 int writePos = Microphone.GetPosition(_microphoneID);
                 if (writePos > _recording.samples || !Microphone.IsRecording(_microphoneID))
                 {
                     Log.Error("ExampleStreaming.RecordingHandler()", "Microphone disconnected.");
                     Debug.LogError("no microphone");
                     StopRecording();
                     yield break;
                 }

                 if ((bFirstBlock && writePos >= midPoint)
                   || (!bFirstBlock && writePos < midPoint))
                 {
                     samples = new float[midPoint];
                     _recording.GetData(samples, bFirstBlock ? 0 : midPoint);

                     AudioData record = new AudioData();
                     record.MaxLevel = Mathf.Max(Mathf.Abs(Mathf.Min(samples)), Mathf.Max(samples));
                     record.Clip = AudioClip.Create("Recording", midPoint, _recording.channels, _recordingHZ, false);
                     record.Clip.SetData(samples, 0);
                     _service.OnListen(record);
                     bFirstBlock = !bFirstBlock;
                 }
                 else
                 {

                     int remaining = bFirstBlock ? (midPoint - writePos) : (_recording.samples - writePos);
                     float timeRemaining = (float)remaining / (float)_recordingHZ;

                     yield return new WaitForSeconds(timeRemaining);
                 }
             }*/ /*
        yield break;
    }

    private void OnRecognize(SpeechRecognitionEvent result)
    {

        if (result != null && result.results.Length > 0)
        {
            foreach (var res in result.results)
            {
                foreach (var alt in res.alternatives)
                {
                    string text = string.Format("{0} ({1}, {2:0.00})\n", alt.transcript, res.final ? "Final" : "Interim", alt.confidence);
                    Log.Debug("ExampleStreaming.OnRecognize()", text);

                    if (res.final)
                    {
                        outputText += " " + alt.transcript;
                    }



                }

                if (res.keywords_result != null && res.keywords_result.keyword != null)
                {
                    foreach (var keyword in res.keywords_result.keyword)
                    {
                        Log.Debug("ExampleStreaming.OnRecognize()", "keyword: {0}, confidence: {1}, start time: {2}, end time: {3}", keyword.normalized_text, keyword.confidence, keyword.start_time, keyword.end_time);
                    }
                }

                if (res.word_alternatives != null)
                {
                    foreach (var wordAlternative in res.word_alternatives)
                    {
                        Log.Debug("ExampleStreaming.OnRecognize()", "Word alternatives found. Start time: {0} | EndTime: {1}", wordAlternative.start_time, wordAlternative.end_time);
                        foreach (var alternative in wordAlternative.alternatives)
                            Log.Debug("ExampleStreaming.OnRecognize()", "\t word: {0} | confidence: {1}", alternative.word, alternative.confidence);
                    }
                }
            }
        }
    }

    private void OnRecognizeSpeaker(SpeakerRecognitionEvent result)
    {
        if (result != null)
        {
            foreach (SpeakerLabelsResult labelResult in result.speaker_labels)
            {
                Log.Debug("ExampleStreaming.OnRecognizeSpeaker()", string.Format("speaker result: {0} | confidence: {3} | from: {1} | to: {2}", labelResult.speaker, labelResult.from, labelResult.to, labelResult.confidence));
            }
        }
    }


    #endregion

    #region tone analyzer

    private string toneAPIKey = "EFvuqgyZZQVKfyBUFA29cP2XdGaU2o3f3prPsAVXmT90";
    private string toneURL = "https://api.eu-gb.tone-analyzer.watson.cloud.ibm.com/instances/0fad1f56-fafa-4676-bd74-2aaeafac322c";
    private string versionDate = "2019-09-16";
    private ToneAnalyzerService service;


    private bool toneTested = false;
    private bool toneChatTested = false;

    private void initToneAnalyzer()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
    }

    private IEnumerator CreateService()
    {
        if (string.IsNullOrEmpty(toneAPIKey))
        {
            throw new IBMException("Plesae provide IAM ApiKey for the service.");
        }

        //  Create credential and instantiate service
        IamAuthenticator authenticator = new IamAuthenticator(apikey: toneAPIKey);

        //  Wait for tokendata
        while (!authenticator.CanAuthenticate())
            yield return null;

        service = new ToneAnalyzerService(versionDate, authenticator);
        if (!string.IsNullOrEmpty(toneURL))
        {
            service.SetServiceUrl(toneURL);
        }

        Runnable.Run(Examples());
    }

    private IEnumerator Examples()
    {
        ToneInput toneInput = new ToneInput()
        {
            Text = outputText
        };

        List<string> tones = new List<string>()
            {
                "emotion",
                "language",
                "social"
            };
        service.Tone(callback: OnTone, toneInput: toneInput, sentences: true, tones: tones, contentLanguage: "en", acceptLanguage: "en", contentType: "application/json");


        while (!toneTested)
        {
            yield return null;
        }


        List<Utterance> utterances = new List<Utterance>()
            {
                new Utterance()
                {
                    Text = outputText,
                    User = "User 1"
                }
            };
        service.ToneChat(callback: OnToneChat, utterances: utterances, contentLanguage: "en", acceptLanguage: "en");

        while (!toneChatTested)
        {
            yield return null;
        }

        Log.Debug("ExampleToneAnalyzerV3.Examples()", "Examples complete!");
    }
    List<ToneScore> tones;
    private void OnTone(DetailedResponse<ToneAnalysis> response, IBMError error)
    {
        tones = new List<ToneScore>();
        if (error != null)
        {
            Log.Debug("ExampleToneAnalyzerV3.OnTone()", "Error: {0}: {1}", error.StatusCode, error.ErrorMessage);
        }

        tones = response.Result.DocumentTone.Tones;
        toneTested = true;
    }

    private void OnToneChat(DetailedResponse<UtteranceAnalyses> response, IBMError error)
    {
        if (error != null)
        {
            Log.Debug("ExampleToneAnalyzerV3.OnToneChat()", "Error: {0}: {1}", error.StatusCode, error.ErrorMessage);
        }
        else
        {
            Log.Debug("ExampleToneAnalyzerV3.OnToneChat()", "{0}", response.Response);
        }

        toneChatTested = true;
    }

    #endregion

    #region triggers

    GameObject currGameObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        currGameObject = collision.gameObject;
        if (collision.GetComponent<Button>() != null)
        {
            collision.GetComponent<Button>().Select();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (currGameObject == collision.gameObject)
            return;
        currGameObject = collision.gameObject;
        if (collision.GetComponent<Button>() != null)
        {
            collision.GetComponent<Button>().Select();
        }
    }

    #endregion
}
*/