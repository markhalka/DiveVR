using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesSimple : MonoBehaviour
{

    //ok, so here just hide the show panel thing 
    //you're using the wrong show panel here

    public GameObject panel;
    public GameObject quiz;
    public GameObject waveController;
    public Slider amplitude;
    public Slider frequency;
    public Slider wavelength;


    List<int> startPanels = new List<int>(new int[] { 0, 1, 2, 3, 4, 5 });

    int finishStart = 0;

    float time = 0;

    List<UserSlider> sliders;
    class UserSlider
    {
        public List<int> indecies;
        public Slider slider;
        public List<float> mThresh;

        public UserSlider(List<float> mt, List<int> list, Slider s)
        {
            slider = s;
            mThresh = mt;
            indecies = list;
        }
    }


    //you need to parse models?
    void Start()
    {


        Information.panelIndex = -1;
        Information.lableIndex = 0;
        initSliders();
        //  showPanel();
        ParseData.parseModel();
        Information.currentScene = "Waves";

    }
    bool[] toChange = new bool[] { false, false, false };
    void initSliders()
    {
        frequency.onValueChanged.AddListener(delegate { toChange[0] = true; });
        amplitude.onValueChanged.AddListener(delegate { toChange[1] = true; });
        wavelength.onValueChanged.AddListener(delegate { toChange[2] = true; });

        sliders = new List<UserSlider>();
        sliders.Add(new UserSlider(new List<float>(new float[] { 0.2f, 0.5f }), new List<int>(new int[] { 8, 9 }), frequency));
        sliders.Add(new UserSlider(new List<float>(new float[] { 0.2f }), new List<int>(new int[] { 6 }), amplitude));
        sliders.Add(new UserSlider(new List<float>(new float[] { 0.2f }), new List<int>(new int[] { 7 }), wavelength));
    }


    void handleStartPanels()
    {


        if (finishStart == 1 && !panel.transform.parent.GetComponent<InformationPanel>().closeOnEnd)
        {
            Debug.LogError("would close after this");
            panel.transform.parent.GetComponent<InformationPanel>().closeOnEnd = true;
            //currInformationPanel.SetActive(false); //?
        }

        if (Information.panelClosed && finishStart == 0 && !isQuiz)
        {
            if (panel.transform.parent.GetComponent<InformationPanel>().closeOnEnd && startPanels.Count > 1)
            {
                panel.transform.parent.GetComponent<InformationPanel>().closeOnEnd = false;
            }

            if (startPanels.Count > 0)
            {
                if (startPanels[startPanels.Count - 1] > Information.panelIndex) //then not all the start panels have been shown
                {
                    for (int i = 0; i < startPanels.Count; i++)
                    {
                        if (startPanels[i] > Information.panelIndex)
                        {
                            Information.panelIndex = startPanels[i];
                            Information.lableIndex = 0;
                            Information.panelClosed = false;
                            showPanel();
                            if (i == startPanels.Count - 1)
                            {
                                finishStart = 1;
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("{start panels count problem");
                finishStart = 1;
            }
        }
    }

    void changeFreq()
    {

        waveController.GetComponent<Waves>().Octaves[0].speed = new Vector2(0, frequency.value * 10);
    }

    void changeAmp()
    {

        waveController.GetComponent<Waves>().Octaves[0].height = amplitude.value * 8;
    }

    void changeWave()
    {

        waveController.GetComponent<Waves>().Octaves[0].scale = new Vector2(0, wavelength.value * 5);
    }



    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (toChange[0])
            {
                changeFreq();
                toChange[0] = false;
            }
            else if (toChange[1])
            {
                changeAmp();
                toChange[1] = false;
            }
            else if (toChange[2])
            {
                changeWave();
                toChange[2] = false;
            }
        }

        if (!panel.activeSelf)
        {
            checkQuiz();
        }

        handleStartPanels();

        for (int i = 0; i < sliders.Count; i++)
        {
            if (sliders[i].mThresh != null)
            {
                for (int j = 0; j < sliders[i].mThresh.Count; j++)
                {
                    showPanel(sliders[i].mThresh[j], sliders[i].indecies[j], sliders[i].slider.value); //that should take care of the slider values 
                }
            }
        }

    }

    #region panel stuff
    void showPanel(float thresh, int index, float sliderValue) //ok so this will be for the sliders 
    {
        if (sliderValue > thresh)
        {
            if (!Information.userModels[index].wasShown)
            {
                Information.panelIndex = index;
                showPanel();
                Information.userModels[index].wasShown = true;
            }
        }

    }



    #endregion

    void showPanel()
    {
        panel.SetActive(true);
    }


    void startQuiz()
    {
        panel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(false);
        isQuiz = true;
        quiz.gameObject.SetActive(true);
    }

    void endQuiz()
    {
        isQuiz = false;

        Debug.LogError("ended quiz");
        if (Information.wasPreTest)
        {
            Information.panelIndex = -1;
            Information.lableIndex = 0;
            return;
        }


        quiz.GetComponent<QuizMenu>().endQuiz();
        //quiz.gameObject.SetActive(false);
    }

    bool isQuiz = false;
    void checkQuiz()
    {
        if (!isQuiz)
        {
            if (Information.isQuiz == 1)
            {
                startQuiz();
            }

        }
        else
        {
            if (Information.isQuiz == 0)
            {
                endQuiz();
            }
        }
    }

}
