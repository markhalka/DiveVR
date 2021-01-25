using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YoutubePlayer;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YtVideoPlayer : MonoBehaviour
{

    public Panel panel;
    public Button nextBtn;
    public Button backBtn;
    public Button quizBtn;

    public GameObject endPanel;
    public Button rewatchAllBtn;
    public Button learnWithModelsBtn;
    public Button endQuizBtn;

    public SimpleYoutubeVideo video;

    Website website;

    Science modelChecker;

    string[] urls;
    int videoIndex = 0;

    void Start()
    {
        website = new Website();
        modelChecker = new Science(); // not sure if i can do this...
        initButtons();
        panel.panelAnimation(true, transform);
        takeVideo();
        checkModel();
    }

    void initButtons()
    {
        // check which scene it should be in, and if there is a model available, if not then don't show the model button 
        nextBtn.onClick.AddListener(delegate { takeNext(); });
        backBtn.onClick.AddListener(delegate { takeBack(); });
        quizBtn.onClick.AddListener(delegate { takeQuiz(); });
        rewatchAllBtn.onClick.AddListener(delegate { takeRewatchAll(); });
        learnWithModelsBtn.onClick.AddListener(delegate { takeLearningWithModel(); });
        endQuizBtn.onClick.AddListener(delegate { takeQuiz(); });

    }

    string scene;
    void checkModel()
    {
        scene = modelChecker.getScene();
        if(scene == "")
        {
            learnWithModelsBtn.gameObject.SetActive(false);
        }
    }

    void takeNext()
    {
        videoIndex++;
        if(videoIndex > urls.Length-1)
        {
            panel.panelAnimation(true, endPanel.transform);
            endPanel.SetActive(true);
         //   panel.panelAniamtion(false, transform); // you can't set the gameobject to false
        } else
        {
            playVideo();
        }
    }

    void takeBack()
    {
        videoIndex--;
        if(videoIndex < 0)
        {
            videoIndex = 0;
        }
        playVideo();
    }

    void takeQuiz()
    {
        if(scene != ""){
            Information.isQuiz = 1;
            SceneManager.LoadScene(scene);
        } else
        {
            // figure out what to do
        }
    }

    void takeRewatchAll()
    {
        panel.panelAnimation(false, endPanel.transform);
        videoIndex = 0;
        playVideo();
    }

    void takeLearningWithModel()
    {
        // check if this is available,
    }



    void takeVideo()
    {
        website.GetRequest(Information.youtubeVideosUrl + "/" + Information.topicIndex, getVideo);
    }

    void getVideo(string inputData)
    {
        if (inputData.Length > 5)
        {
            urls = inputData.Split(',');
        }
    }

    void playVideo()
    {
        video.videoUrl = urls[videoIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
