using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Molecule : MonoBehaviour
{

    //ok, so there is a few problems with mixtures and with chemical reactions
    //so, with chemical reactions, it should be a little bit more interactive 
    //with mixtures you need to make sure that you reset it everytime,
    //just double check everything 
    //test all the chemical reactions 

    //you could also add, the information panel with the image in it, so you can add more information about that chemical (do that now)

    //time ~ 2 hours

    //ok, so how to deal with the image panel? 
    //so, in the xml doc, maybe have a the name of the image, than you can search for it??
    //actually, what you can do is: have it hard coded in, so you can have kindof a dictionary that includes the panel incies, and the image indecies, then if there exists and image indecie, you can simply update the image in the show panel, and everything else stays the same
    //so this can be used during:
    //chemical reactions 
    //greenhouses gasses
    //alloys

    //so, in mixtures
    //for solutiosn, there are 4 that should pop-up initially 
    //  (0,1,2,3)
    //than there are 3 that should appear as you move the slider 
    //(4,5,6)

    //everything else is more or less the same (but remember, all the other indicies changed 
    //how to do that?
    //have a function, just like what you already have, include the indicies
    //you already have the slider code, just initalize it for mixtures 




    // ok, so create a static struct here too, and initialize it



    public Dictionary<int, Sprite> showPanelImages; //so this will store the index of the show panel, and the image associated with it 


    public ParticleSystem ps;
    public ParticleSystem ps2;
    public ParticleSystem ps3;
    public ParticleSystem ps4;

    public Slider slider;
    public GameObject buttons;
    public Dropdown dropdown;

    public GameObject top;
    public GameObject bottom;
    public GameObject left;
    public GameObject right;
    public GameObject forward;
    public GameObject backward;

    public GameObject inBetween;
    public GameObject quiz;
    public GameObject InformationPanel;
    public GameObject imagePanel;

    public Material[] matterMaterials;
    public Material[] changeMaterials;
    public Material[] reactionMaterials;
    public Material[] alloyMaterials;

    public GameObject cursor;
    public GameObject lowerBound;
    public GameObject upperBound;

    public GameObject mixturesObject;
    public GameObject popUp;

    public Button quizButton;

    public Button mix;



    public TMPro.TMP_Text outputText;
    bool onSlider = false;


    string[] matterCompounds = new string[] { "air", "alcohol", "water", "toothpaste", "salt" };
    string[] matterValue = new string[] { "0.001293", "0.810", "1", "1.2", "17" };

    string[] reaction = new string[] { "Choose a reaction", "Synthesis", "Decomposition", "Displacment", "Double displacment" };

    string[] ballAndStickCompounds = new string[] { "water", "carbon dioxide", "nitrogen", "oxygen", "methane" };
    string[] ballAndStickFormulas = new string[] { "H20", "C02", "N2", "O2", "CH4" };
    int[] ballAndStickState = new int[] { 1, 2, 2, 2, 2 };

    public Material[] materials;

    public Button start;

    float time;
    bool simulating;

    List<int> panelTimes;

    bool isQuiz;

    List<int> startPanels;

    int modelOffset = 0;

    //ok, so here initially hide the show panel, than in showPanel(), check if it is hidden, and if yes show it
    //add the code from science models to not have it close everytime 
    int finishStart = 0;

    //todo here:
    //1. same thing as database, split up each of the different labs
    //2. and split up the initial menu shit 


    void Start()
    {
        Information.isVrMode = false;

        ParseData.parseModel();
        Debug.LogError("model parsed " + Information.userModels.Count);

        time = 0;
        isQuiz = false;

        //  InformationPanel.SetActive(true);
        popUp.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { exitPopUp(); });
        slider.onValueChanged.AddListener(delegate { sliderValueChanged(); });

        start.onClick.AddListener(delegate { takeStart(); });


        //mix.onClick.AddListener(delegate { shakeParticles(); }); add this in the colloid lab shit


        quizButton.onClick.AddListener(delegate { takeQuiz(); });

        time = 0;
        simulating = false;


        Information.panelIndex = -1;
        Information.lableIndex = 0;

        initStartPanels();

        //temp
        outputText.text = "";
        initFunction();

        Information.currentScene = "Molecules";
        //   initPanelTimes();


        finishStart = 0;

    }




    void takeStart()
    {
        outputText.text = "";
        initFunction();
    }

    void takeQuiz()
    {
        Information.isQuiz = 1;

    }

    void startQuiz()
    {

        isQuiz = true;

        if (!quiz.activeSelf)
            quiz.SetActive(true);


        quiz.GetComponent<QuizMenu>().startQuiz();
        quizButton.gameObject.SetActive(false);

    }

    void endQuiz()
    {
        isQuiz = false;
        if (Information.wasPreTest)
            return;


        quiz.GetComponent<QuizMenu>().endQuiz();

        Debug.LogError("quiz ended");
        InformationPanel.transform.parent.gameObject.SetActive(false);
        inBetween.SetActive(true);
    }


    public Sprite[] reactionSprites;
    public Sprite[] mixtuerSprites;
    public Sprite[] reactionInnerSprites;
    public Sprite[] alloySprites;



    void initFunction()
    {

        Lab currentLab;

        switch (Information.nextScene)
        {

            case 2: //heat and thermal energy
                currentLab = new ThermalEnergyLab(outputText, dropdown, slider);
                //  InformationPanel.SetActive(true);
                break;
            case 4: //mixtures and solutions
                currentLab = new MixturesLab(outputText, dropdown, slider);
                break;
            case 26: //chemical reactions 
                currentLab = new ReactionsLab(outputText, dropdown, slider);
                break;
        }

    }


    void exitPopUp()
    {
        popUp.SetActive(false);
        ps.Play();
    }



    float changeAmount = (360 - 244) / (float)(360 * 100);
    float startAmount = 244 / (float)360;
    void sliderValueChanged()
    {
        slider.transform.GetChild(1).GetComponentInChildren<Image>().color = Color.HSVToRGB(changeAmount * slider.value + startAmount, 1, 1);
        switch (Information.nextScene)
        {

            case 2: //heat and thermal energy
                thermalSlider();
                break;
        }
    }


    #region dropdowns


    #endregion

    #region sliders

    float densityOffset = 30;
    float prevValue = 0;
    void changeDensity(float amount)
    {
        if (prevValue == 0)
        {
            prevValue = slider.value / 30;
            return;
        }
        top.transform.Translate(new Vector3(0, 0, (prevValue - amount) * offset));
        bottom.transform.Translate(new Vector3(0, 0, -(prevValue - amount) * offset));

        left.transform.Translate(new Vector3(0, 0, -(prevValue - amount) * offset));
        right.transform.Translate(new Vector3(0, 0, -(prevValue - amount) * offset));


        forward.transform.Translate(new Vector3(0, 0, (prevValue - amount) * offset));
        backward.transform.Translate(new Vector3(0, 0, -(prevValue - amount) * offset));
        prevValue = amount;

    }


    int previousState = 0;
    void thermalSlider()
    {
        var noise = ps.noise;
        float inversePercentage = (1 - slider.value / (slider.maxValue)) + 0.2f; ;

        float percentage = slider.value / slider.maxValue * 10;
        changeDensity(slider.value / 30);

        noise.frequency = inversePercentage * 2;
        noise.positionAmount = percentage * 1.3f;

        if (slider.value < 20)
        {
            outputText.text = "The molecules now form a solid";
            previousState = 0;
            //solid
        }
        else if (slider.value < 30)
        {
            if (previousState == 0)
            {
                //it is melting
                outputText.text = "The molecules are now melting";
            }
            else
            {
                outputText.text = "The molecules are now freezing";
                //it is freezing
            }
            //transition
        }
        else if (slider.value < 40)
        {
            previousState = 1;
            outputText.text = "The molecules now form a liquid";

            //liquid
        }
        else if (slider.value < 50)
        {
            if (previousState == 1)
            {
                //it is evaporating
                outputText.text = "The molecules are now boiling";
            }
            else
            {
                //it is condensating
                outputText.text = "The molecules are now condensating";
            }

        }
        else
        {
            previousState = 2;
            outputText.text = "The molecules now form a gas";
            //gas
        }
    }
    #endregion

    public Slider plainSlider;


    // decouple these two as well...



    void showPanel()
    {
        InformationPanel.SetActive(true);
    }



    void initStartPanels()
    {

        startPanels = new List<int>();
        modelOffset = 0;
        for (int i = 0; i < Information.userModels.Count; i++)
        {
            if (Information.userModels[i].section == 1)
            {
                Debug.LogError("added one ");
                modelOffset++;
                startPanels.Add(i);
            }
        }
    }





    void Update()
    {
        if (!Information.isInMenu)
        {
            checkQuiz();
            outputText.gameObject.SetActive(true);
        }
        if (Information.doneLoading)
        {
            SceneManager.LoadScene("ScienceMain");
        }
    }

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
