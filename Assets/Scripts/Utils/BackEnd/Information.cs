using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Information
{
    // Start is called before the first frame update
    //this class ony holds the information to be shared between scenes

    public static bool initialized = false;
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
    public static bool isCurriculum;

    public static string tts = "";

    public enum LearningType {VIDEO, MODEL };
    public static LearningType learningType;



    public static float pretestScore = -1;
    public static float minutesInScene = 0;


    public static string pastPointsDate = "";

    public static int totalEarnedPoints = 0;


    public static Model tutorialModel = null;
    public static List<TutorialScene> tutorialScenes;

    public static string name = "";
    public static string acheivment = "";
    public static float score = 0;

    public static string socialMediaMessage = "";


    #region lessons
    public static string grade = "5";
    public static string subject = "math";
    public static string currentScene = "";

    public static string lastGrade; 
    public static string lastSubject;
    public static int previousScene; 

    public static int topicIndex = 0;
    public static int nextScene = 1;

    public static List<Topic> topics;
    public static Topic currentTopic;
    public static List<Model> userModels;

    #endregion




    public static string openDir = "XML/General/Data";

    public static string xmlResourceDir = "XML/General/UserData"; //Application.streamingAssetsPath + "/UserData.xml"; //these guys should be streamingassets, 
    public static string loadResourceDir = "XML/General/data";//Application.streamingAssetsPath + "/data.xml";

    public static string xmlFileDir = Application.persistentDataPath + "/UserData.xml";//"XML/General/Data";//"Assets/Resources/Grades.xml";
    public static string loadDocDir = Application.persistentDataPath + "/data.xml";
    public static XDocument xmlDoc;
    public static XDocument loadDoc;

    public static int isQuiz = 0;

    public static GameObject currentBox;


    public static bool isCorrect = false;
    public static bool isIncorrect = false;

    public static Color rightColor = new Color(0.5606977f, 0.990566f, 0.7740753f, 0.3f);
    public static Color wrongColor = new Color(0.9716981f, 0.5976017f, 0.5683517f, 0.3f);
    public static Color defualtColor = new Color(1, 1, 1, 1);//new Color(0.722f, 1, 1);





    public static bool doneLoadingDocuments = false;
    public static bool doneLoading = false;
    public static bool skip = false;

    public static string currentLevel;


    //--------- url stuff

    #region urls
    public static string loginUrl = "https://www.divevr.org/_functions-dev/checkLogin/";
    public static string parentLoginUrl = "https://www.divevr.org/_functions-dev/checkParentLogin/";
    public static string setPointsMaxUrl = "https://www.divevr.org/_functions-dev/setPoints/"; //this is used for 
    public static string levelUrl = "https://www.divevr.org/_functions-dev/level/";
    public static string shopUrl = "https://www.divevr.org/_functions-dev/shop/";
    public static string buyUrl = "https://www.divevr.org/_functions-dev/buyPoints/";
    public static string levelsUrl = "https://www.divevr.org/_functions-dev/levels/";
    public static string pointsUrl = "https://www.divevr.org/_functions-dev/points/"; //this is used for 
    public static string saveEarnedPointsUrl = "https://www.divevr.org/_functions-dev/setPoints/";
    public static string sessionsUrl = "https://www.divevr.org/_functions-dev/sessions/";
    public static string getNameUrl = "https://www.divevr.org/_functions-dev/name/";
    public static string setNameUrl = "https://www.divevr.org/_functions-dev/setName/";
    public static string saveNameUrl = "https://www.divevr.org/_functions-dev/saveName/";

    public static string loadDocUrl = "https://www.divevr.org/_functions-dev/masterLoadDoc/";
    public static string masterXmlDoc = "https://www.divevr.org/_functions-dev/masterXmlDoc/";
    public static string xmlDocUrl = "https://www.divevr.org/_functions-dev/userXmlDoc/";
    public static string saveFileUrl = "https://www.divevr.org/_functions-dev/saveUserXmlDoc/";
    public static string sendProblemUrl = "https://www.divevr.org/_functions-dev/problem/";


    // add these ones:
    public static string youtubeVideosUrl = "https://www.divevr.org/_functions-dev/youtubeVideo/";
    #endregion

    public static int maxDivePoints = 0;

    public static float animationGrowth = 1.007f;
    public static int animationLength = 100;

    public static string username;


    public static bool isStudentInfo;


    //get better colors 

    public static Color32[] colors = new Color32[] { new Color32(255, 133, 133, 255), new Color32(190, 247, 134, 255), new Color32(255, 202, 133, 255), new Color32(134, 247, 190, 255), new Color32(120, 168, 250,255),
    new Color32(134,238,247,255), new Color32(144,120,250,255),new Color32(236,144,252,255), new Color32(252,144,144,255)}; //have a few good looking colors here 

    public static GameObject[] updateEntities = null;

    public static bool click2d = true;

    public Vector2 cursorPosition;


    public static List<Topic> placmentTest;
    public static List<float> placmentScore;


    public static int lableIndex = 0;
    public static int panelIndex = 0;

    public static int RIGHT_COUNT = 3;

    public static bool retry = false;

    public static string inquire;


}
