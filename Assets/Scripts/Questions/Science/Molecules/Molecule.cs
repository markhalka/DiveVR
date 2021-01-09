using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Molecule : MonoBehaviour
{
    //1. start with the molecule menu
    //

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




    //ok, new plan, just get rid of lab, you might be able to make a few util classes from there, and just make each 
    //class inherit from monobehaviour, and import its own stuff, and then create gameobjects for them


    // ok, so you can probably get rid of molecule menu, and put it in here, and you can get rid of 90% of the code currenlty in this class

    // in molecule menu, they both have suspensioncolloid, find a way to only init the right one... (maybe make two seperate classes?)

    // ok, so right now its pretty good, next steps:
    //1. fix all errors in each class
    //2. add gameobjects to scene, and add scripts
    //3. get rid of this class, and add what you need in molecule menu
    //4. create a utils class, add shit there, and get rid of lab class
    //5. start by testing the molecule menu, and starter panels 


    public Dictionary<int, Sprite> showPanelImages; //so this will store the index of the show panel, and the image associated with it 


    public ParticleSystem ps;
    public ParticleSystem ps2;
    public ParticleSystem ps3;
    public ParticleSystem ps4;

    public Slider slider;
    public GameObject buttons;
    public Dropdown dropdown;



    public GameObject inBetween;
    public GameObject quiz;
    public GameObject InformationPanel;
    public GameObject imagePanel;

    public Material[] matterMaterials;




    public GameObject cursor;
    public GameObject lowerBound;
    public GameObject upperBound;

    public GameObject mixturesObject;
    public GameObject popUp;

    public Button quizButton;





    public TMPro.TMP_Text outputText;
    bool onSlider = false;


    string[] matterCompounds = new string[] { "air", "alcohol", "water", "toothpaste", "salt" };
    string[] matterValue = new string[] { "0.001293", "0.810", "1", "1.2", "17" };



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

        Information.currentScene = "Molecules";
        //   initPanelTimes();


        finishStart = 0;

    }

    void exitPopUp()
    {
        popUp.SetActive(false);
        ps.Play();
    }


    // this needs to be included wherever there is a temperature slider

    float changeAmount = (360 - 244) / (float)(360 * 100);
    float startAmount = 244 / (float)360;
    void sliderValueChanged()
    {
        slider.transform.GetChild(1).GetComponentInChildren<Image>().color = Color.HSVToRGB(changeAmount * slider.value + startAmount, 1, 1);
    }




    void Update()
    {
        if (Information.doneLoading)
        {
            SceneManager.LoadScene("ScienceMain");
        }
    }
}
