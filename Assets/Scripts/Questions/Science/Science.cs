using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YoutubePlayer;

public class Science : MonoBehaviour
{
    // ok, plan:
    // create a seperate gameobject for the quiz, (just have the table and a gameobject for the script)
    // in the script, you just pretty much copy science models (and maybe na.)

    // for video stuff, you need to load user models 


    public GameObject ytEndPanel;
    public GameObject ytStartPanel;
    public GameObject loadingContainer;
    public GameObject inBetwenScene;
    public GameObject VideoPanel;

    public Button nextVideo;
    public Button backVideo;
    public Button exitVideo;
    public Button startRecc;
    public Button startNotRecc;

    public Button modelsEndButton;
    public Button rewatchEndButton;
    public Button quizEndButton;
    public Button exitEndButton;

    public SimpleYoutubeVideo YtVideoPlayer;

    Website website;

    Panel panel;

    class VideoModel
    {
        public int index;
        public bool prefer;
        public int minGrade = 0;
        public VideoModel(int index, bool prefer = false)
        {
            this.index = index;
            this.prefer = prefer;
        }

        public VideoModel(int index, int minGrade)
        {
            this.index = index;
            this.minGrade = minGrade;
            prefer = false;
        }
    }


    int[] good_models = new int[] { 1, 5, 9, 11, 12, 14, 15, 18, 21, 22, 29, 31, 32, 35, 37, 38, 42, 44 };
    int[] ok_models = new int[] { 2, 4, 17, 19, 20, 26, 30, 33, 34, 36, 39, 41, 45 };
    int[] only_videos = new int[] { 3, 6, 7, 8, 10, 23, 24, 27, 28, 40, 43, 47 };

    VideoModel[] video_model = new VideoModel[] {new VideoModel(14,8), new VideoModel(15,8), new VideoModel(41), new VideoModel(36, true), new VideoModel(21), new VideoModel(38, 7),
new VideoModel(1, 8), new VideoModel(26, true), new VideoModel(35, 8),new VideoModel(2, 8), new VideoModel(19, true) };





    void Start()
    {
        panel = new Panel();
        website = new Website();

        startAnimation();
        initVideoButtons();
        initEndButtons();

        openScene();

        //   ParseData.startXML(); 
        Information.currentScene = "OtherScience";
        XMLWriter.savePastSubjectAndGrade();
    }

    void initVideoButtons()
    {
        nextVideo.onClick.AddListener(delegate { takeNextVideo(true); });
        exitVideo.onClick.AddListener(delegate { takeExitVideo(); });
        backVideo.onClick.AddListener(delegate { takeNextVideo(false); });
    }

    void initEndButtons()
    {
        modelsEndButton.onClick.AddListener(delegate { loadEndModel(); });
        exitEndButton.onClick.AddListener(delegate { SceneManager.LoadScene("ModuleMenu"); });
        rewatchEndButton.onClick.AddListener(delegate { replayVideo(); });
        quizEndButton.onClick.AddListener(delegate { checkQuiz(); }); // for this one, you need 

    }

    public GameObject quizGb;

    void checkQuiz()
    {
        string sceneToLoad = getScene();
        if(sceneToLoad == "")
        {
            quizGb.SetActive(true);
            StartCoroutine(panel.panelAnimation(false, ytEndPanel.transform));
            loadingContainer.SetActive(false);
        } else
        {
            Information.isQuiz = 1;
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // check if a model exists, if yes, just show it
    void loadEndModel()
    {
        string sceneToLoad = getScene();
        if (sceneToLoad == "")
        {
            SceneManager.LoadScene("ModuleMenu");
        } else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    bool onlyVideo = false;
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

        // check if it is only model
        if (good_models.Contains(Information.nextScene))
        {
            if (videoModel != null)
            {
                if (videoModel.prefer && videoModel.minGrade <= getGrade()) // figure out how to extract grade 
                {
                    // prefer video, show panel

                    createStartPanel(sceneToLoad, Information.LearningType.VIDEO);
                }
                else
                {
                    // then prefer model, and show panel 
                    createStartPanel(sceneToLoad, Information.LearningType.MODEL);
                }
            }
            else
            {
                // only open model
                Debug.LogError("opening good model");
                takeModel(sceneToLoad);            
            }
        }
        else if (ok_models.Contains(Information.nextScene))
        {
            if (videoModel != null)
            {
                if (videoModel.minGrade <= getGrade())
                {
                    // prefer video, show panel
                    createStartPanel(sceneToLoad, Information.LearningType.VIDEO);
                }
                else
                {
                    //prefer model, show panel
                    createStartPanel(sceneToLoad, Information.LearningType.MODEL);
                }
            }
            else
            {
                // only open model
                Debug.LogError("opening only model");
                takeModel(sceneToLoad);
                
            }
        }
        else if (only_videos.Contains(Information.nextScene))
        {
            // only show video
            Debug.LogError("playing video...");
            onlyVideo = true;
            videoQuizOrLesson();
        }
        else
        {
            // error 
            Debug.LogError("ERROR: COULD NOT FIND WHAT TO DO WITH TOPIC INDEX");
        }
    }

    void videoQuizOrLesson()
    {
        if(Information.isQuiz > 0)
        {
            checkQuiz();
        } else
        {
            playVideo();
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

    void replayVideo()
    {
        StartCoroutine(panel.panelAnimation(false, ytEndPanel.transform));
        videoIndex = 0;
        YtVideoPlayer.videoUrl = videos[0];
        VideoPanel.SetActive(true);
      //  videoPanelObject.set

    }

    void playVideo()
    {
        StartCoroutine(panel.panelAnimation(false, ytStartPanel.transform));
        string url = Information.youtubeVideosUrl + Information.nextScene;
        StartCoroutine(website.GetRequest(url, takeVideo));
    }

    #region video stuff

    // this will handle showing the video and all that

    string[] videos;
    int videoIndex = 0;

    public void takeVideo(string videoUrls)
    {
        Debug.LogError(videoUrls + " before");
        videoUrls = videoUrls.Replace("</p>", "");
        videoUrls = videoUrls.Replace("<p class=\"font_8\">", "");
        Debug.LogError(videoUrls + " after");
        videos = videoUrls.Split(' ');
        videoIndex = 0;
        Debug.LogError(videos[0] + " first url...");

        YtVideoPlayer.videoUrl = videos[0];
          VideoPanel.SetActive(true);
        loadingContainer.SetActive(false);
        //  YtVideoPlayer.gameObject.SetActive(true);
    }

    public GameObject loadingText;
    public GameObject videoPanelObject;
    public UnityEngine.Video.VideoPlayer videoPlayer;

    // you need to check if it is a quiz or na...
    private void Update()
    {

        
        if(loadingText.activeSelf && VideoPanel.activeSelf)
        {
            if (videoPlayer.isPlaying)
            {
                loadingText.SetActive(false);
                videoPanelObject.SetActive(true);
            }
        }
    }

    void takeNextVideo(bool next)
    {
        Debug.LogError("taking next video...");
        if (next)
        {
            videoIndex++;
        } else
        {
            videoIndex--;
        }

        if (videoIndex < 0)
        {
            videoIndex = 0;
        }
        
        if(videoIndex >= videos.Length)
        {
            takeExitVideo();
            return;
        }

        YtVideoPlayer.videoUrl = videos[videoIndex];
    }

    void takeExitVideo()
    {
        Debug.LogError("closing exit video");
        VideoPanel.SetActive(false);
        showEndPanel();
        
    }

    void showEndPanel()
    {
        if (onlyVideo)
        {
            exitEndButton.transform.position = modelsEndButton.transform.position;
            modelsEndButton.gameObject.SetActive(false);
        } 

        StartCoroutine(panel.panelAnimation(true, ytEndPanel.transform));
    }

    #endregion

    void takeModel(string sceneToLoad)
    {
        if (sceneToLoad != "")
        {
            StartCoroutine(LoadScene(sceneToLoad));
        }
    }

    // here, always show that learn panel, but default it to the model if it exists (you can fix this later...)
    // you need to put azure here
    public void openScene()
    {
    //    Information.nextScene = Information.topics[Information.topicIndex++].topics[0];

        Information.doneLoading = false;
  
        //    Information.isQuiz = 0;
        Information.panelIndex = 0;
        Information.lableIndex = 0;
        Information.isSelect = false;

        decideWhatToDo();

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
          
            case 4: //5 mixtures 6 solutions
                sceneToLoad = "Molecules";
                break;
            case 5: //plants  
                sceneToLoad = "Science";
                break;
            case 9: //5 scientific names 6 class and names 
                sceneToLoad = "Database";
                break;
            case 11: //simple machines
                sceneToLoad = "Science";
                break;
            case 12:  //microscope
                sceneToLoad = "Science";
                break;
            case 14: //5 animal cell 6 animal cell
                sceneToLoad = "Science";
                break;
            case 15: //5 plant cell 6 plant cell
                sceneToLoad = "Science";
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
            case 26: //6 Chemical reactions
                sceneToLoad = "Molecules";
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
            case 38: //Plate tectonics
                sceneToLoad = "Database";
                break;
            case 39: //weather and atmosphere  //not done 
                sceneToLoad = "Science";
                break;
            case 41:
                //ecosystem (forest, marine) 
                sceneToLoad = "Database";
                break;
            case 42://5 energy sources, 6 energy sources 7 energy sources 
                sceneToLoad = "Science";
                break;
            case 44: //enviornment //< -------------------------- not done
                sceneToLoad = "Science";
                break;
            case 45: //weathering
                sceneToLoad = "Science";
                break;
        }
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
