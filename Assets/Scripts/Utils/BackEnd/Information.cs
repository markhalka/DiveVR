using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Information
{
    // Start is called before the first frame update
    //this class ony holds the information to be shared between scenes

    public static bool initialized = false;
    public static bool isDeepSea = false; //this value will be setup in initialize
    public static bool isInMenu = false;
    public static bool isGame = false;
    public static bool isVrMode = false;
    public static bool isSelect;
    public static bool interactiveLesson = true;
    public static bool autoHide = true;
    public static bool wasWrongAnswer = false;
    public static bool wasStreak = false;
    public static bool isParent = false;
    public static bool shouldRedo = false;
    public static bool panelClosed = true;
    public static bool menuLoaded = false;
    public static bool firstTime = false;
    public static bool showPreTest = true;
    public static bool wasPreTest = false;
    public static bool shouldShowSurvey = true;
    public static bool notSure = false;

    public static float pretestScore = -1;
    public static float minutesInScene = 0;


    public static string pastPointsDate = "";

    public static int totalEarnedPoints = 0;


    // public static bool isTutorial = false;
    public static Model tutorialModel = null;
    public static List<TutorialScene> tutorialScenes;

    public static string address = "";

    public static string name = "";
    public static string acheivment = "";
    public static string socialMediaName = "Facebook";
    public static string[] socialMediaNames = { "facebook", "twitter", "instagram" }; //keep this in the same order becaause this is the order that it is in for social share 

    public static float score = 0;

    public static string socialMediaMessage = "";

    public static string socialUsername;
    public static string socialPassword;
    public static bool socialLogIn;


    public static string grade = "5";
    public static string subject = "math";

    public static int topicIndex = 0;

    public static string currentScene = "";

    public static string computerIP = "192.168.0.22";
    public static int computerPort = 9009;

    public static string controllerIP = "192.168.0.22"; //im going to keep the same ip, but use a different port for the buttons?? might not need to 
    public static int controllerPort = 8080;


    public static Vector3 currPosition;
    public static Vector3 udpOffset;

    public static GameObject lowerBoundary;
    public static GameObject upperBoundary;

    public static string openDir = "XML/General/Data";

    public static string xmlResourceDir = "XML/General/UserData"; //Application.streamingAssetsPath + "/UserData.xml"; //these guys should be streamingassets, 
    public static string loadResourceDir = "XML/General/data";//Application.streamingAssetsPath + "/data.xml";

    public static string xmlFileDir = Application.persistentDataPath + "/UserData.xml";//"XML/General/Data";//"Assets/Resources/Grades.xml";
    public static string loadDocDir = Application.persistentDataPath + "/data.xml";
    public static XDocument xmlDoc;
    public static XDocument loadDoc;

    public static int isQuiz = 0;

    public static GameObject currentBox;

    public static int numberSinceGame = 0;

    public static int nextScene = 1;

    public static bool isCorrect = false;
    public static bool isIncorrect = false;

    public static Color rightColor = new Color(0.5606977f, 0.990566f, 0.7740753f);
    public static Color wrongColor = new Color(0.9716981f, 0.5976017f, 0.5683517f);
    public static Color defualtColor = new Color(1, 1, 1, 1);//new Color(0.722f, 1, 1);


    public static List<Topic> topics;
    public static Topic currentTopic;
    public static List<Model> userModels;
    //   public static List<Help> helpMath;
    //   public static List<Help> helpScience;

    public static bool doneLoading = false;
    public static bool skip = false;

    public static string currentLevel;


    //--------- url stuff

    public static string loginUrl = "https://www.divevr.org/_functions-dev/checkLogin/";
    public static string parentLoginUrl = "https://www.divevr.org/_functions-dev/checkParentLogin/";


    public static string setPointsMaxUrl = "https://www.divevr.org/_functions-dev/setPoints/"; //this is used for 

    public static string levelUrl = "https://www.divevr.org/_functions-dev/level/";
    // public static string prizeUrl = "https://www.divevr.org/_functions-dev/checkLogin/"; 
    public static string shopUrl = "https://www.divevr.org/_functions-dev/shop/";
    //  public static string resetUrl = "https://www.divevr.org/_functions-dev/checkLogin/"; 
    public static string buyUrl = "https://www.divevr.org/_functions-dev/buyPoints/";
    // public static string getSettingsUrl = "https://www.divevr.org/_functions-dev/getSettings/";
    // public static string putSettingsUrl = "https://www.divevr.org/_functions-dev/putSettings/";
    public static string levelsUrl = "https://www.divevr.org/_functions-dev/levels/";
    // public static string fileUrl = "https://www.divevr.org/_functions-dev/userTopics/";

    public static string pointsUrl = "https://www.divevr.org/_functions-dev/points/"; //this is used for 
                                                                                      //  public static string setPointsUrl = "https://www.divevr.org/_functions-dev/buyPoints/";
    public static string saveEarnedPointsUrl = "https://www.divevr.org/_functions-dev/setPoints/";


    public static string sessionsUrl = "https://www.divevr.org/_functions-dev/sessions/";
    public static string reportUrl = "https://www.divevr.org/_functions-dev/report/";
    //  public static string shopImageUrl = "https://www.divevr.org/_functions-dev/shopImage/";
    public static string addressUrl = "https://www.divevr.org/_functions-dev/address/";
    public static string putAddressUrl = "https://www.divevr.org/_functions-dev/setAddress/";
    public static string getNameUrl = "https://www.divevr.org/_functions-dev/name/";
    public static string setNameUrl = "https://www.divevr.org/_functions-dev/setName/";
    public static string getSpeakingTimeUrl = "https://www.divevr.org/_functions-dev/getTime/";
    public static string setSpeakingTimeUrl = "https://www.divevr.org/_functions-dev/setTime/";
    //  public static string examplesUrl = "https://www.divevr.org/_functions-dev/examples/";
    public static string saveNameUrl = "https://www.divevr.org/_functions-dev/saveName/";


    public static string loadDocUrl = "https://www.divevr.org/_functions-dev/masterLoadDoc/";
    public static string masterXmlDoc = "https://www.divevr.org/_functions-dev/masterXmlDoc/";

    public static string xmlDocUrl = "https://www.divevr.org/_functions-dev/userXmlDoc/";
    public static string saveFileUrl = "https://www.divevr.org/_functions-dev/saveUserXmlDoc/";

    public static string sendProblemUrl = "https://www.divevr.org/_functions-dev/problem/";



    public static int maxSpeakinTime = 30;

    public static int maxDivePoints = 0;

    public static float animationGrowth = 1.007f;
    public static int animationLength = 100;


    //  public static int currentPoints;
    public static int gamePoints;

    public static string username;

    //--------

    public static string lastGrade; //these variables will keep track of what the user did last 
    public static string lastSubject;
    public static int previousScene; //user this for when someone opens the settings menu

    //--------- setting stuff

    public static bool isStudentInfo;
    public static float sensitivity;


    //get better colors 

    public static Color32[] colors = new Color32[] { new Color32(255, 133, 133, 255), new Color32(190, 247, 134, 255), new Color32(255, 202, 133, 255), new Color32(134, 247, 190, 255), new Color32(120, 168, 250,255),
    new Color32(134,238,247,255), new Color32(144,120,250,255),new Color32(236,144,252,255), new Color32(252,144,144,255)}; //have a few good looking colors here 

    public static GameObject[] updateEntities = null;

    public static bool click2d = true;

    public Vector2 cursorPosition;

    public static bool flying;
    public static bool hasFuel;

    public static List<Topic> placmentTest;
    public static List<float> placmentScore;
    //  public static bool isCurriculum;


    public static string lesson; //<- get rid of this after the test 

    public static Scroll scroll;

    public static int lableIndex = 0;
    public static int panelIndex = 0;

    public static int rightCount = 3;

    public static bool retry = false;

    public static string inquire;

    public static bool isCurriculum;

}
