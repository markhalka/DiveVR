using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YoutubePlayer;

public class Science : MonoBehaviour
{

    public GameObject database;
    public GameObject ytEndPanel;
    public GameObject ytStartPanel;
    public GameObject loadingContainer;
    public GameObject inBetwenScene;

    public Button startRecc;
    public Button startNotRecc;

    public Text scoreCount;

    public SimpleYoutubeVideo YtVideoPlayer;

    Website website;

    Panel panel;

    // so first, in student menu get the learning type
    // then here you can access it from information
    // check which one it is, show the choosing panel, let them choose
    // (maybe you can show the estimated time as well)
    // then whicheve is their main option make it big, and make the other one small
    //and done
    

    int[] good_models = new int[] { 1, 5, 9, 12, 14, 15, 16, 18, 21, 22, 29, 31, 35, 37, 38, 42, 44 };

    int[] ok_models = new int[] { 2, 4, 11, 17, 19, 20, 26, 30, 33, 34, 36, 39, 41, 45 };


    int[] only_videos = new int[] { 3, 6, 7, 8, 24, 27, 28, 32, 40, 43, 47 };

    class VideoModel
    {
        public int index;
        public bool prefer;
        public int minGrade = 0;
        public VideoModel(int index, bool prefer = false)
        {

        }

        public VideoModel(int index, int minGrade)
        {

        } 
    }

    VideoModel[] video_model = new VideoModel[] {new VideoModel(14,8), new VideoModel(15,8), new VideoModel(16), new VideoModel(41), new VideoModel(36, true),
new VideoModel(21), new VideoModel(38, 7), new VideoModel(1, 8), new VideoModel(26, true), new VideoModel(35, 8),
new VideoModel(2, 8), new VideoModel(19, true) };

    void decideWhatToDo()
    {
        string sceneToLoad = getScene();
        VideoModel videoModel = null;
        foreach (var model in video_model)
        {
            if (model.index == Information.nextScene)
            {
                videoModel = model;
                break;
            }
        }

        Debug.LogError(Information.nextScene + " " + videoModel);
        // check if it is only model
        if (good_models.Contains(Information.nextScene))
        {        
            if (videoModel != null)
            {
                if(videoModel.prefer && videoModel.minGrade <= getGrade()) // figure out how to extract grade 
                {
                    // prefer video, show panel
                    createStartPanel(sceneToLoad, Information.LearningType.VIDEO);
                } else
                {
                    // then prefer model, and show panel 
                    createStartPanel(sceneToLoad, Information.LearningType.MODEL);
                }
            } else
            {
                // only open model
                StartCoroutine(LoadScene(sceneToLoad));
            }
        } else if (ok_models.Contains(Information.nextScene))
        {
            if (videoModel != null)
            {
                if (videoModel.minGrade <= getGrade())
                {
                    // prefer video, show panel
                    createStartPanel(sceneToLoad, Information.LearningType.VIDEO);
                } else
                {
                    //prefer model, show panel
                    createStartPanel(sceneToLoad, Information.LearningType.MODEL);
                }
            } else
            {
                // only open model
                StartCoroutine(LoadScene(sceneToLoad));
            }
        } else if (only_videos.Contains(Information.nextScene))
        {
            // only show video
            playVideo();
        } else
        {
            // error 
            Debug.LogError("ERROR: COULD NOT FIND WHAT TO DO WITH TOPIC INDEX");
        }
    }

    int getGrade()
    {
        string a = Information.grade;
        string b = string.Empty;
        int val = 0;

        for (int i = 0; i < a.Length; i++)
        {
            if (Char.IsDigit(a[i]))
                b += a[i];
        }

        if (b.Length > 0)
            val = int.Parse(b);

        return val;

    }

        

    void Start()
    {
        panel = new Panel();
        website = new Website();

        startAnimation();

        openScene();

        //   ParseData.startXML(); 
        Information.currentScene = "OtherScience";
        XMLWriter.savePastSubjectAndGrade();
    }  

    void startAnimation()
    {
        int random = UnityEngine.Random.Range(0, loadingContainer.transform.childCount - 1);
        loadingContainer.transform.GetChild(random).gameObject.SetActive(true);

        loadingContainer.gameObject.SetActive(true);
    }


    public void createStartPanel(string sceneToLoad, Information.LearningType learningType)
    {
        // first check if there is an optoin 
        string text = "";
        string reccText = "";
        string notReccText = "";
        if(learningType== Information.LearningType.MODEL)
        {
            text = "We think you'd learn best with our interactive models! Choose the way you'd like to learn:";
            reccText = "Model";
            notReccText = "Video";
            startRecc.onClick.AddListener(delegate { takeModel(sceneToLoad); });
            startNotRecc.onClick.AddListener(delegate { playVideo(); });

        }
        else
        {
            text = "We think you'd learn best with the help of videos! Choose the way you'd like to learn:";
            reccText = "Video";
            notReccText = "Model";
            startRecc.onClick.AddListener(delegate { playVideo(); });
            startNotRecc.onClick.AddListener(delegate { takeModel(sceneToLoad); });
        }
        ytStartPanel.transform.GetChild(0).GetChild(1).GetComponent<TMPro.TMP_Text>().text = text;

        startRecc.GetComponentInChildren<TMPro.TMP_Text>().text = reccText;
        startNotRecc.GetComponentInChildren<TMPro.TMP_Text>().text = notReccText;

        StartCoroutine(panel.panelAnimation(true, ytStartPanel.transform));
    }

    void playVideo()
    {
        StartCoroutine(website.GetRequest(Information.youtubeVideosUrl + "/" + Information.topicIndex, takeVideo));
    }

    #region video stuff

    // this will handle showing the video and all that
    public GameObject VideoPanel; // this is where you can keep the videos
    public Button nextVideo;
    public Button exitVideo;
    string[] videos;
    int videoIndex = 0;

    public void takeVideo(string videoUrls)
    {
        videos = videoUrls.Split(' ');
        videoIndex = 0;

        exitVideo.gameObject.SetActive(true);
        exitVideo.onClick.AddListener(delegate { takeExitVideo(); });
        nextVideo.gameObject.SetActive(true);
        nextVideo.onClick.AddListener(delegate { takeNextVideo(); });

        VideoPanel.SetActive(true);
        YtVideoPlayer.gameObject.SetActive(true);
        YtVideoPlayer.videoUrl = videos[0];
    }

    void takeNextVideo()
    {
        videoIndex++;
        if(videoIndex >= videos.Length)
        {
            takeExitVideo();
            return;
        }
        YtVideoPlayer.videoUrl = videos[videoIndex];
    }

    void takeExitVideo()
    {
        panel.panelAnimation(false, VideoPanel.transform);
        panel.panelAnimation(true, ytEndPanel.transform);
    }

    #endregion

    void takeModel(string sceneToLoad)
    {
        if (sceneToLoad == "Database")
        {
            loadingContainer.gameObject.SetActive(false);
            database.gameObject.SetActive(true);
            return;
        }

        if (sceneToLoad != "")
        {
            StartCoroutine(LoadScene(sceneToLoad));
        }
    }

    // here, always show that learn panel, but default it to the model if it exists (you can fix this later...)
    // you need to put azure here
    public void openScene()
    {
        Information.nextScene = Information.topics[Information.topicIndex++].topics[0];

        Information.doneLoading = false;
        database.SetActive(false);
        //    Information.isQuiz = 0;
        Information.panelIndex = 0;
        Information.lableIndex = 0;
        Information.isSelect = false;
        string sceneToLoad = getScene();

        decideWhatToDo();


    /*    var learningType = Information.LearningType.MODEL;
        if (sceneToLoad == "")
        {
            learningType = Information.LearningType.VIDEO;

        }
        createStartPanel(sceneToLoad, learningType); */
    }

    public string getScene()
    {

        string sceneToLoad = "";
        switch (Information.nextScene)
        {

            case 1: //5 matter and mass 6 matter and mass 
                sceneToLoad = "Science";
                break;
            case 2: //5 heat and thermal 6 thermal energy
                sceneToLoad = "Molecules";
                break;
            case 3: //5 physical and chemical change   //not done
                sceneToLoad = "Molecules"; //temp shit
                Information.nextScene = 4;
                break;
            case 4: //5 mixtures 6 solutions
                sceneToLoad = "Molecules";
                break;
            case 5: //plants   <---------------------------
                sceneToLoad = "Science";
                break;
            case 6: //5 force and motion
                Information.nextScene = 6;
                sceneToLoad = "Rocket"; //ima make this the rocket shit too
                break;
            case 7: //5 magnets   //not done
                    // sceneToLoad = "Physics";
                sceneToLoad = "Physics";
                break;
            case 8: //5 Classification
                sceneToLoad = "Database";
                //  ClassificationLadder();
                break;
            case 9: //5 scientific names 6 class and names 
                sceneToLoad = "Database";
                break;
            case 10: //5 Animals
                     //just make it animal life cylces 
                sceneToLoad = "Database";
                break;
            case 11: //simple machines
                sceneToLoad = "Science";
                break;
            case 12:  //microscope
                sceneToLoad = "Science";
                break;
            case 13: //nothing here
                Information.nextScene = 14; //temp shit
                sceneToLoad = "Science";
                break;
            case 14: //5 animal cell 6 animal cell
                sceneToLoad = "Science";
                break;
            case 15: //5 plant cell 6 plant cell
                sceneToLoad = "Science";
                break;
            case 16: //5 ecology 6 ecology
                sceneToLoad = "Database";
                break;
            case 17: //5 conservation of natural resources 
                //natual, renewable, non renewable energy sources, recycling, not recycling
                //you can make this a gmae 
                sceneToLoad = "Recycling";
                break;
            case 18: //5 rocks and minerals 6 rocks and minerals
                sceneToLoad = "Science";
                break;
            case 19: //5 fossils 6 fossils
                sceneToLoad = "Science";
                break;
            case 20: //greenhouse gasses //5 weather and climate 6 weather climate
                sceneToLoad = "Science";
                break;
            case 21: //5 water cycle 6 watercycle
                sceneToLoad = "Science";
                break;
            case 22: //5 astronomy 6 astronomy
                sceneToLoad = "Science";
                break;
            case 23: //6 Science practices and tools    //not done
                sceneToLoad = "Science"; //ok have the 3d models of the tools, and satey tips 
                break;
            case 24: //6 Designing experiments
                sceneToLoad = "Design";     //not done
                                            //have them design a simple plant growth experiemnt, or maybe have a small collection, have them follow the right process (maybe make this a seperate class)
                break;
            case 25: //6 Engineering practices    //not done
                sceneToLoad = "Molecules";
                Information.nextScene = 26;
                //database 
                break;
            case 26: //6 Chemical reactions
                sceneToLoad = "Molecules";
                break;
            case 27: //6 Velocity, acceleration, and forces
                sceneToLoad = "Rocket"; //this is the rocket shit 
                break;
            case 28: //6 Kinetic and potential energy
                sceneToLoad = "Physics";
                break;
            case 29: //6 Waves
                sceneToLoad = "SampleScene";
                break;
            case 30: //6 Biochemistry
                sceneToLoad = "Science";
                break;
            case 31: //6 anatomy and physi
                sceneToLoad = "Science";
                break;
            case 32:  //flight
                sceneToLoad = "Science";
                break;
            case 33:
                Information.nextScene = 34;
                sceneToLoad = "Science";
                break;

            case 34: //weather and climate 
                sceneToLoad = "Science";
                break;
            case 35: //Photosynthesis    
                sceneToLoad = "Science"; //for these two you can add particle systems too, to simulate everything 
                break;
            case 36: //Ecological interactions
                sceneToLoad = "Science";
                //db food web 
                break;
            case 37:
                sceneToLoad = "Database";
                break;
            case 38: //Plate tectonics
                sceneToLoad = "Database";
                break;
            case 39: //weather and atmosphere  //not done 
                sceneToLoad = "Science";
                break;
            case 40:  // < ------------------------------ not done!  current electricity
                sceneToLoad = "Physics";
                break;
            case 41:
                Information.nextScene = 41;
                //ecosystem (forest, marine) 
                sceneToLoad = "Database";
                break;

            case 42://5 energy sources, 6 energy sources 7 energy sources 
                sceneToLoad = "Science";
                break;

            case 43: //static electricity //<--------------- not done
                sceneToLoad = "Physics";
                break;

            case 44: //enviornment //< -------------------------- not done
                sceneToLoad = "Science";
                break;

            case 45: //weathering
                sceneToLoad = "Science";
                break;

            case 46:
                sceneToLoad = "Science";
                break;


        }
        Debug.LogError(Information.nextScene + " " + sceneToLoad);
        return sceneToLoad;
    }

    private IEnumerator LoadScene(string sceneName)
    {
        var asyncScene = SceneManager.LoadSceneAsync(sceneName);

        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {

           // _loadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100;

            if (asyncScene.progress >= 0.9f)
            {
                asyncScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
