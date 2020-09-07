using PlayFab.Internal;
using System;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//FF9A76
//150, 150, 32, 32

//for selected: FFF2CB

//for text color: 0E4255

//F9D155
//button color

//for blue is text color:
//27507D


//for text color title:
//0E3E55

//for text color white:
//EBEBEB

//another thing you can do is:
//1. when you save it to topics, save it to the file at the same time, than you can get rid of xml.save topics and all that 



//science test doens really work (maybe something to do with the delay, especially for quiz menu (the 3 choices), the table sometimes looks fucked up (like itsm issing random parts, or maybe its just the wrong shape)

//reseting the hint for the table during swipe doesn't work

//for the ahcievments, make sure there is an image to scroll
//update the files to update the tutorial in science 


//log in still not working the first time?/

//start redoing data
//get rid of all mentions of the expirments on dive for now 


//also, when you earn points and come back, it is always wayy to high




namespace PlayFab.Internal
{
    public class CustomCertificateHandler : CertificateHandler
    {
        // Encoded RSAPublicKey
        private static readonly string PUB_KEY = "";


        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

}

public class StudentMenu : MonoBehaviour
{


    //ok, so just take a look at what you do in math, copy the code to write to the file, then thats pretty much it, double check student info to make sure it'll parse ahd than you are gucci 
    //in science menu, if they start a quiz than hide the animation image (do this in database as well)

    //ok, so you need to do 3 things:
    //take a look at the version code, make sure it gets it from the right file 

    //add animations to each of the panels
    //add a function on wix to save the name 
    //add a function here to save the name they enter 





    public TMPro.TMP_Text divePoints;

    public Button Shop;
    public Button Coins;


    public GameObject sprite1;
    public GameObject sprite2;

    public Button learning;
    public Button aboutMe;

    public GameObject level;
    // public Image image;

    public AudioSource source;
    public AudioClip buttonSound;

    //ok, so the module menu works ok, just mke the gap between cells the same as the cell size
    //get rid of the clicking animtion shit for now 

    public GameObject namePanel;
    public GameObject tutorialPanel;
    public Button nameOk;
    public Button tutorialOk;
    public Button noTutorial;
    public TMPro.TMP_InputField nameField;

    bool doneLoading = false;

    void Start()
    {

        //  Information.username = "test5@email.com";
        //  Information.name = "mark";

        initButtons();





        Information.currentScene = "StudentMenu";
        if (Information.isParent)
        {
            learning.gameObject.SetActive(false);
        }

        if (Information.xmlDoc != null)
        {
            doneLoading = true;
            return; //it was already loaded 
        }

        Debug.LogError("not returning");
        StartCoroutine(handleStreak());
        StartCoroutine(GetRequest());



        //  tempLoad();



        if (Information.name == "none")
        {
            StartCoroutine(panelAniamtion(true, namePanel.transform));
            Information.firstTime = true;
            //   namePanel.SetActive(true);
        }

    }



    void closeName()
    {
        //   StartCoroutine(panelAniamtion(false, namePanel.transform));
        namePanel.SetActive(false);
        Information.name = nameField.text;
        StartCoroutine(saveName());
        tutorialPanel.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = "Hi " + Information.name + "!";
        tutorialPanel.SetActive(true);
        //   StartCoroutine(panelAniamtion(true, tutorialPanel.transform));

    }

    IEnumerator saveName()
    {
        CustomCertificateHandler certHandler = new CustomCertificateHandler();
        Debug.LogError(Information.username + " username " + Information.name + " is the name");

        UnityWebRequest uwr = UnityWebRequest.Get(Information.saveNameUrl + Information.username + "/" + Information.name);
        uwr.chunkedTransfer = false;
        uwr.certificateHandler = certHandler;

        yield return uwr.SendWebRequest();



        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }

        Debug.LogError("saving name output; " + uwr.downloadHandler.text);

        //save the name here 
        yield break;

    }

    void closeTutorial()
    {
        StartCoroutine(panelAniamtion(false, tutorialPanel.transform));
        //tutorialPanel.SetActive(false);
    }

    void openTutorial()
    {
        //do this later 
        GameObject tutorial = GameObject.Find("TutorialCanvas");
        if (tutorial != null)
        {
            tutorial.GetComponent<Tutorial>().takeHelp();
        }
        tutorialPanel.SetActive(false);
    }


    void tempLoad()
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("XML/General/UserData");
        string txt = mytxtData.text;
        Information.xmlDoc = XDocument.Parse(txt);

        mytxtData = (TextAsset)Resources.Load("XML/General/Data");
        txt = mytxtData.text;
        Information.loadDoc = XDocument.Parse(txt);

        Debug.LogError(Information.xmlDoc.ToString());
        Debug.LogError(Information.loadDoc.ToString());
        doneLoading = true;
    }

    //ok, here are the steps:
    //1. change the masterxml doc (data.xml) and the master user xml doc (userdata)
    //2. change this thing to get the individual users xml file 




    IEnumerator GetRequest()
    {
        Debug.LogError("at get request");
        CustomCertificateHandler certHandler = new CustomCertificateHandler();


        UnityWebRequest uwr = UnityWebRequest.Get(Information.loadDocUrl);
        uwr.chunkedTransfer = false;
        uwr.certificateHandler = certHandler;

        yield return uwr.SendWebRequest();

        UnityWebRequest uwr2 = UnityWebRequest.Get(Information.xmlDocUrl + Information.username);//UnityWebRequest.Get(Information.masterXmlDoc);
        uwr2.chunkedTransfer = false;
        uwr2.certificateHandler = certHandler;

        yield return uwr2.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }


        if (uwr2.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr2.downloadHandler.text);
        }
        string xmld = uwr2.downloadHandler.text;
        string ld = uwr.downloadHandler.text;


        Information.loadDoc = XDocument.Parse(ld);

        if (xmld.Length < 10)
        {
            uwr = UnityWebRequest.Get(Information.masterXmlDoc);
            yield return uwr.SendWebRequest();
            Information.xmlDoc = XDocument.Parse(uwr.downloadHandler.text);


        }
        else
        {
            xmld = Regex.Replace(@"" + xmld, @"\\", "");

            if (xmld[xmld.Length - 1] == '\"')
            {
                xmld = xmld.Substring(0, xmld.Length - 1);

            }
            Information.xmlDoc = XDocument.Parse(xmld);//XDocument.Parse(Information.xmlDocUrl + Information.username);
        }


        string version = Information.loadDoc.Root.Element("version").Value;

        string xmlDocVers = Information.xmlDoc.Root.Element("version").Value;

        Debug.LogError("got versions");
        if (xmlDocVers != version)
        {
            uwr = UnityWebRequest.Get(Information.masterXmlDoc);
            yield return uwr.SendWebRequest();
            Information.xmlDoc = XDocument.Parse(uwr.downloadHandler.text);
        }


        loadLast();
    }





    void loadLast()
    {

        XElement lastElement = Information.xmlDoc.Root.Element("past");
        Information.lastGrade = lastElement.Attribute("grade").Value;
        Information.lastSubject = lastElement.Attribute("subject").Value;

        Debug.LogError("done loading");
        doneLoading = true;

        if (Information.xmlDoc.Root.Element("feedback").Attribute("show").Value == "true")
        {
            Information.showPreTest = true;
        }
        else
        {
            Information.showPreTest = false;
        }


        if (Information.xmlDoc.Root.Element("feedback").Attribute("survey").Value == "true")
        {
            Information.shouldShowSurvey = true;
        }
        else
        {
            Information.shouldShowSurvey = false;
        }

        initFunctions();

    }

    int menuIndex = 0;
    void initButtons()
    {
        Shop.onClick.AddListener(delegate { openShop(); });
        Coins.onClick.AddListener(delegate { takeCoins(); });
        coinsPanelButton.onClick.AddListener(delegate { takeCoinsBack(); });


        sprite1.GetComponent<Button>().onClick.AddListener(delegate { takeSprite1(); });
        sprite2.GetComponent<Button>().onClick.AddListener(delegate { takeSprite2(); });

        learning.onClick.AddListener(delegate { takeLearning(); });
        aboutMe.onClick.AddListener(delegate { takeAboutMe(); });

        tutorialOk.onClick.AddListener(delegate { openTutorial(); });
        noTutorial.onClick.AddListener(delegate { closeTutorial(); });
        nameOk.onClick.AddListener(delegate { closeName(); });


    }

    public GameObject coinsPanel;
    public Button coinsPanelButton;
    void takeCoins()
    {
        //here you would show the right info and all that 
        Debug.LogError(Information.totalEarnedPoints + " total earned points student menu");
        coinsPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = Information.totalEarnedPoints.ToString();
        coinsPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = Information.maxDivePoints.ToString();
        StartCoroutine(panelAniamtion(true, coinsPanel.transform));
    }

    void takeCoinsBack()
    {
        StartCoroutine(panelAniamtion(false, coinsPanel.transform));
    }

    public Vector3 coinsStart;
    public Vector3 coinsEnd;
    public Canvas canvas;
    IEnumerator panelAniamtion(bool open, Transform currPanel)
    {
        float count = 0;

        //  coinsEnd.y *= canvas.scaleFactor;
        //  start.y *= canvas.scaleFactor;

        if (open)
        {


            currPanel.gameObject.SetActive(true);
        }


        while (count <= 1)
        {
            count += 0.1f;
            if (open)
            {
                currPanel.transform.localPosition = Vector3.Lerp(coinsStart, coinsEnd, count);
            }
            else
            {
                currPanel.transform.localPosition = Vector3.Lerp(coinsEnd, coinsStart, count);
            }


            yield return new WaitForSeconds(0.02f);
        }

        if (!open)
        {
            currPanel.gameObject.SetActive(false);
        }



    }


    void takeSprite1()
    {
        source.clip = buttonSound;
        source.Play();

        if (menuIndex == 0)
        {
            if (Information.lastSubject != null && Information.lastSubject.Length > 3 && Information.lastGrade != null && Information.lastGrade.Length > 3)
            {
                Information.subject = Information.lastSubject;
                Information.grade = Information.lastGrade;
                SceneManager.LoadScene("ModuleMenu");
            }
            else
            {
                Information.isStudentInfo = false;
                SceneManager.LoadScene("HomeMenu");
            }



        }
        else if (menuIndex == 1)
        {
            Information.isStudentInfo = true;
            SceneManager.LoadScene("HomeMenu");
        }
    }

    void takeSprite2()
    {
        source.clip = buttonSound;
        source.Play();

        if (menuIndex == 0)
        {
            Information.isStudentInfo = false;
            SceneManager.LoadScene("HomeMenu");
        }
        else if (menuIndex == 1)
        {
            //load the acheivment class 
            SceneManager.LoadScene("Acheivments");
        }
    }

    public GameObject secondPanel;
    public Sprite[] learningSprites; //0 is defualt, 1 math 2 science 3 public speaking
    public Sprite[] aboutMeSprites; //0 is a double bar graph, 1 is a trophy or soemthing 
                                    // public Sprite[] tutorialSprites; //0 is a double bar graph, 1 is a trophy or soemthing 


    bool openLearning = false;
    void takeLearning()
    {
        if (!doneLoading)
        {
            Debug.LogError("not done loading yet");
            openLearning = true;
            return;
        }
        source.clip = buttonSound;
        source.Play();

        menuIndex = 0;
        secondPanel.gameObject.SetActive(true);
        //  secondPanel.transform.GetChild(1).GetComponent<Image>().sprite = learningSprites[0];

        Sprite firstSprite = learningSprites[0]; ;
        if (Information.lastSubject == "")
        {
            firstSprite = learningSprites[0];
        }
        else
        {

            switch (Information.lastSubject)
            {
                case "math":
                    firstSprite = learningSprites[1];
                    break;
                case "science":
                    firstSprite = learningSprites[2];
                    break;
                case "public speaking":
                    firstSprite = learningSprites[3];
                    break;
                case "social science":
                    firstSprite = learningSprites[4];
                    break;
            }

        }
        secondPanel.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = firstSprite;
        secondPanel.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = learningSprites[0];


        secondPanel.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Continue last lesson";
        secondPanel.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Learn something new";

        secondPanel.gameObject.SetActive(true);
    }

    bool shouldOpenAboutme = false;
    void takeAboutMe()
    {
        if (!doneLoading)
        {
            shouldOpenAboutme = true;
            return;
        }
        source.clip = buttonSound;
        source.Play();

        menuIndex = 1;
        secondPanel.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = aboutMeSprites[0];
        secondPanel.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = aboutMeSprites[1];

        secondPanel.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "See my results";
        secondPanel.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "See my acheivments";
        secondPanel.gameObject.SetActive(true);
    }


    int streak = 0;

    IEnumerator handleStreak()
    {
        CustomCertificateHandler certHandler = new CustomCertificateHandler();
        UnityWebRequest uwr = UnityWebRequest.Get(Information.sessionsUrl + Information.username);
        uwr.chunkedTransfer = false;
        uwr.certificateHandler = certHandler;

        yield return uwr.SendWebRequest();



        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
        string temp = uwr.downloadHandler.text;
        checkStreak(temp);
    }



    public GameObject streakContainer;
    void checkStreak(string temp)
    {
        Information.wasStreak = true;
        //   string temp = Website.GET(Information.sessionsUrl + Information.username);
        string[] days = temp.Split(',');

        if (days.Length < 2)
        {
            streak = 0;
            return;
        }
        DateTime currentDate = DateTime.Parse(days[days.Length - 1]);
        var diff = DateTime.Today - currentDate;
        if (diff.Days < 1)
        {
            //becaues they already had a streak today
            return;
        }
        DateTime pastDate;

        for (int i = days.Length - 2; i >= 0; i--)
        {
            pastDate = DateTime.Parse(days[i]);
            TimeSpan timeDiff = currentDate - pastDate;
            if (timeDiff.Days <= 1)
            {
                streak++;
                currentDate = pastDate;
            }
            else
            {
                break;
            }
        }
        if (streak > 0)
        {
            Information.socialMediaMessage = "I just studied " + streak + " days in a row on Dive!";
            int points = streak * 10;

            Information.name = streak + " day streak!";
            Information.acheivment = "+" + points + " Dive points";

            streakContainer.gameObject.SetActive(true);

            Information.totalEarnedPoints += points;
            //   Website.GET(Information.setPointsUrl + Information.username + "/" + points);
        }
    }


    void openShop()
    {
        source.clip = buttonSound;
        source.Play();

        SceneManager.LoadScene("Shop");
    }


    void downloadFile(string websitePath, string filePath)
    {

        using (WebClient client = new WebClient())
        {
            client.DownloadFile(new Uri(websitePath), filePath);
        }

    }


    //  string[] levels = new string[] { "Master Diver", "Snorkler", "Junior Diver", "Senior Diver", "Open Water Diver", "Adventure Diver" };
    void initSlider(string output)
    {

        string[] points = output.Split(',');
        float multiplier = 230 / 100;
        float minimum = 13;
        if (points.Length <= 0 || points[0].Contains("error") || points[0] == "")
        {

            return;
        }

        //      int currLevel = int.Parse(points[0]);
        int currPoints = int.Parse(points[1]);

        float amount = Mathf.Max(minimum, currPoints * multiplier);

        level.GetComponent<RectTransform>().sizeDelta = new Vector2(amount, level.GetComponent<RectTransform>().sizeDelta.y);
    }


    //if points is -1, handle that 
    void initFunctions()
    {
        XElement info = Information.xmlDoc.Root.Element("info");
        Information.name = info.Element("name").Value;
        string[] points = info.Element("points").Value.Split(',');
        Debug.LogError(info.Element("points").Value + " points value");
        if (points.Length < 2)
        {
            string date = ParseData.encodeDate();
            string max = Information.loadDoc.Root.Element("points").Value;
            string currPoints = "0";
            Information.xmlDoc.Root.Element("info").Element("points").Value = date + "," + currPoints + "," + max;
            points = new string[] { date, currPoints, max };
        }

        DateTime pastMonth = DateTime.Parse(ParseData.decodeDate(points[0])); //so the first one is the date  //date, user points, max points
        var difference = DateTime.Today - pastMonth;

        Information.totalEarnedPoints = int.Parse(points[1]);
        Information.maxDivePoints = int.Parse(points[2]);
        Information.pastPointsDate = points[0];
        Debug.LogError("date: " + Information.pastPointsDate);
        if (difference.Days >= 30) //30 days is a month
        {
            Information.maxDivePoints += 416;
            Information.pastPointsDate = ParseData.encodeDate();
        }
        Debug.LogError(Information.totalEarnedPoints + " total earned points");
        divePoints.text = Information.totalEarnedPoints.ToString();

    }






    void Update()
    {

        if (openLearning)
        {
            if (doneLoading)
            {
                openLearning = false;
                takeLearning();
            }
        }
        else if (shouldOpenAboutme)
        {
            if (doneLoading)
            {
                shouldOpenAboutme = false;
                takeAboutMe();

            }
        }
        if (streakContainer.activeSelf)
        {
            if (!streakContainer.transform.GetChild(1).gameObject.activeSelf) //which means the certificate container was closed
            {
                streakContainer.gameObject.SetActive(false);
            }

        }


    }

    GameObject currGameObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //here just select the button
        currGameObject = collision.gameObject;
        if (collision.GetComponent<Button>() != null)
        {
            collision.GetComponent<Button>().Select();

        }
    }
}
