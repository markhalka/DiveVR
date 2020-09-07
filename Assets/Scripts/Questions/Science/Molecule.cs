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



    public TMPro.TMP_Text outputText;
    bool onSlider = false;


    string[] changeCompounds = new string[] { "nitrogen", "alcohol", "water", "iron" };
    string[,] changeValue = new string[,] { { "-210", "-195" }, { "-115", "79" }, { "0", "100" }, { "1538", "2862" } }; //melting, boiling point

    string[] matterCompounds = new string[] { "air", "alcohol", "water", "toothpaste", "salt" };
    string[] matterValue = new string[] { "0.001293", "0.810", "1", "1.2", "17" };

    string[] reaction = new string[] { "Choose a reaction", "Synthesis", "Decomposition", "Displacment", "Double displacment" };

    string[] ballAndStickCompounds = new string[] { "water", "carbon dioxide", "nitrogen", "oxygen", "methane" };
    string[] ballAndStickFormulas = new string[] { "H20", "C02", "N2", "O2", "CH4" };
    int[] ballAndStickState = new int[] { 1, 2, 2, 2, 2 };

    public Material[] materials;

    public Button start;
    public Button back;
    public Button mix;

    float time;
    bool simulating;

    List<int> panelTimes;

    bool isQuiz;

    List<int> startPanels;

    int modelOffset = 0;

    //ok, so here initially hide the show panel, than in showPanel(), check if it is hidden, and if yes show it
    //add the code from science models to not have it close everytime 
    int finishStart = 0;

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
        back.onClick.AddListener(delegate { takeBack(); });
        mix.onClick.AddListener(delegate { shakeParticles(); });

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


    Dictionary<int, int> sliderPanelValues;
    Dictionary<int, int> sliderTimeValues;


    void loadPanelValues(List<int> time, int[] indecies)
    {
        sliderPanelValues = new Dictionary<int, int>();
        for (int i = 0; i < time.Count; i++)
        {
            sliderPanelValues.Add(time[i], indecies[i] + modelOffset); //? double check that 
        }
    }

    public GameObject choosePanel;
    public GameObject chooseSecondPanel;

    public Sprite[] reactionSprites;
    public Sprite[] mixtuerSprites;
    public Sprite[] reactionInnerSprites;
    public Sprite[] alloySprites;


    void initFunction()
    {
        switch (Information.nextScene)
        {

            case 2: //heat and thermal energy
                thermalEnergyLab();
                //  InformationPanel.SetActive(true);
                break;
            case 4: //mixtures and solutions
                mixturesLab();
                break;
            case 26: //chemical reactions 
                chemicalReactionsLab();
                break;
        }
    }


    void exitPopUp()
    {
        popUp.SetActive(false);
        ps.Play();
    }


    void showPanel(Dictionary<int, int> dict, float sliderValue)
    {
        if (dict != null && !InformationPanel.activeSelf)
        {
            int lowestIndex = 100;
            foreach (var value in dict)
            {


                if (value.Key <= sliderValue && !Information.userModels[value.Value].wasShown)
                {
                    if (value.Value < lowestIndex)
                        lowestIndex = value.Value;

                }

            }

            if (lowestIndex >= 0 && lowestIndex < Information.userModels.Count)
            {
                Information.panelIndex = lowestIndex;
                InformationPanel.SetActive(true);
                Information.userModels[lowestIndex].wasShown = true;
            }
        }
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
    void changeDropdown()
    {

        int curr = dropdown.value;
        //ok so here you can change the max and min values 
        slider.transform.GetChild(3).GetComponent<TMPro.TMP_Text>().text = changeValue[curr, 0].ToString();
        slider.transform.GetChild(4).GetComponent<TMPro.TMP_Text>().text = changeValue[curr, 1].ToString();
        var main = ps.main;
        ps.GetComponent<ParticleSystemRenderer>().material = changeMaterials[curr];
        ps.Play();

    }

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

    void takeBack()
    {
        isReacting = false;
        reactionIndex = -1;
        if (chooseSecondPanel.activeSelf)
        {
            chooseSecondPanel.gameObject.SetActive(false);
            choosePanel.gameObject.SetActive(true);
            newEntities = new List<GameObject>();
            for (int i = 0; i < choosePanel.transform.childCount; i++)
            {
                newEntities.Add(choosePanel.transform.GetChild(i).gameObject);

            }
            Information.updateEntities = newEntities.ToArray();

        }
        else if (choosePanel.activeSelf)
        {
            SceneManager.LoadScene("ModuleMenu");
        }
        else
        {
            if (Information.nextScene == 2) //thermal
            {
                SceneManager.LoadScene("ModuleMenu");
            }
            else if (Information.nextScene == 4) //mixtures
            {
                if (choosePanelIndex == 2)
                {
                    chooseSecondPanel.gameObject.SetActive(true);
                }
                else
                {
                    choosePanel.gameObject.SetActive(true);
                }

            }
            else
            {
                chooseSecondPanel.gameObject.SetActive(true);
            }
        }
    }

    //ok, so just double check the index???
    int choosePanelIndex = 0;
    void callSubLab(int index)
    {
        choosePanel.gameObject.SetActive(false);
        resetAll();
        outputText.gameObject.SetActive(false);
        choosePanelIndex = index;
        switch (Information.nextScene)
        {
            case 4: //mixtures 
                showMixturePanel(index);
                switch (index)
                {
                    case 0: //mixtures
                        SolutionSubLab();
                        Debug.Log("solutions sub lab");
                        break;
                    case 1: //suspension
                        Debug.Log("suspension sub lab");
                        SuspensionSubLab();
                        break;
                    case 2: //alloy
                        createSecondChoosePanel(index);
                        break;
                    case 3: //colloid
                        ColloidsSubLab();
                        break;
                }
                break;
            case 26: //reactions
                createSecondChoosePanel(index);
                break;
        }
    }
    List<GameObject> newEntities;
    void createSecondChoosePanel(int index)
    {
        resetAll();
        newEntities = new List<GameObject>();
        Sprite[] currentSprite = new Sprite[0];
        List<string> currentText = new List<string>();
        switch (Information.nextScene)
        {
            //mixtures
            case 4:
                currentSprite = alloySprites;
                break;
            //reactions
            case 26:
                currentSprite = new Sprite[3];
                currentSprite[0] = reactionInnerSprites[index * 3];
                currentSprite[1] = reactionInnerSprites[index * 3 + 1];
                currentSprite[2] = reactionInnerSprites[index * 3 + 2];

                currentText.Add(reactions[index * 3].name);
                currentText.Add(reactions[index * 3 + 1].name);
                currentText.Add(reactions[index * 3 + 2].name);
                break;
        }
        for (int i = 0; i < currentSprite.Length; i++)
        {
            chooseSecondPanel.transform.GetChild(i).GetComponent<Image>().sprite = currentSprite[i];
            if (i < currentText.Count)
                chooseSecondPanel.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TMP_Text>().text = currentText[i];

            newEntities.Add(chooseSecondPanel.transform.GetChild(i).gameObject);
        }
        Information.updateEntities = newEntities.ToArray();
        chooseSecondPanel.gameObject.SetActive(true);
    }


    void callSecondSubLab(int index)
    {
        chooseSecondPanel.gameObject.SetActive(false);
        switch (Information.nextScene)
        {
            case 4: //mixutres
                AlloySubLab(index);
                break;
            case 26: //reactions
                reactionIndex = choosePanelIndex * 3 + index;
                Reaction currReaction = reactions[reactionIndex];
                showReactionPanel(currReaction.name);
                break;
        }

    }

    float value = 0;
    bool started = false;

    int getIndex()
    {
        for (int i = 0; i < newEntities.Count; i++)
        {
            if (Information.currentBox.transform == newEntities[i].transform)
            {
                return i;
            }
        }
        return -1;
    }




    void handleStartPanels()
    {
        if (Information.doneLoading)
        {
            SceneManager.LoadScene("ScienceMain");
        }
        if (finishStart == 1 && !InformationPanel.transform.parent.GetComponent<InformationPanel>().closeOnEnd)
        {
            Debug.LogError("would close after this");
            InformationPanel.transform.parent.GetComponent<InformationPanel>().closeOnEnd = true;
        }
        if (Information.panelClosed && finishStart == 0 && !isQuiz)
        {
            if (InformationPanel.transform.parent.GetComponent<InformationPanel>().closeOnEnd && startPanels.Count > 1)
            {
                Debug.LogError("closign on end set");
                InformationPanel.transform.parent.GetComponent<InformationPanel>().closeOnEnd = false;
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
                            //    simple.text = Information.userModels[startPanels[i]].simpleInfo[0];
                            Information.panelClosed = false;
                            Debug.LogError("calling: " + Information.panelIndex + " from handle startp panels");
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

        handleStartPanels();

        if (sliderPanelValues != null)
        {
            showPanel(sliderPanelValues, slider.value);
        }

        if (reactionIndex != -1)
        {
            if (!InformationPanel.activeSelf && !isReacting)
            {
                onReationStart();
            }
        }
        //    time += Time.deltaTime;
        //   panelManager();

        if (Information.currentBox != null)
        {
            int index = getIndex();
            if (index < 0)
            {
                Information.currentBox = null;
                return;
            }
            if (choosePanel.activeSelf)
            {
                callSubLab(index);
            }
            else if (chooseSecondPanel.activeSelf)
            {
                callSecondSubLab(index);
            }
            Information.currentBox = null;
        }

        //replace this with handle start psanels

        /*  if (started)
          {
              time += Time.deltaTime;
              if (sliderTimeValues != null)
              {
                  showPanel(sliderTimeValues, time);
              }
          }*/




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


    public void thermalEnergyLab()
    {
        outputText.text = "Set the dropdown to select you material, then move the slider to change the temprature!";
        outputText.gameObject.SetActive(true);

        slider.gameObject.SetActive(true);
        dropdown.gameObject.SetActive(true);
        dropdown.AddOptions(new List<string>(changeCompounds));
        dropdown.onValueChanged.AddListener(delegate { changeDropdown(); });
        changeDropdown();

        var psr = ps.GetComponent<ParticleSystemRenderer>();
        psr.material = changeMaterials[0];
        var col = ps.colorBySpeed;
        col.enabled = false;
        loadPanelValues(new List<int>(new int[] { 5, 15, 25, 35, 45, 55, 65 }), new int[] { 0, 1, 2, 3, 4, 5, 6 }); //this is the thing you need to change <---
    }


    #region mixtures   

    string[] mixturesDropdown = new string[] { "Solutions", "Suspensions", "Alloys", "Colloids" };
    int[] dropDownIndecies = new int[] { 3, 4, 8, 9 };
    int[] startIndecies = new int[] { 0, 1, 2 };

    string[] alloys = new string[] { "Bronze", "Steel", "Solder" };
    int[] alloyIndecies = new int[] { 5, 6, 7 };

    //actually, you can just add it here
    int[] solutionIndecies = new int[] { 10 };


    Dictionary<int, int> alloyPanel;
    Dictionary<int, int> mixturePanel;
    Dictionary<int, int> solutionPanel;
    Dictionary<int, int> reactionPanel;

    string[] reactionNames = new string[] { "Creating water", "Photosynthesis", "Creating salt", "Splitting water", "Decomposing Lithium Carbonate", "Potassium Chloride", "Burning methane", "Zinc in acid",
        "Iron and Copper sulfate",  "neutralizing an acid", "Salt and Silver Nitrate", "Baking soda and Vinegar" };
    int[] reactionIndecies = { 0, 1, 2, 3, 4, 5, 6, 7, };

    public GameObject chooseCanvas;
    public void mixturesLab() //4
    {
        chooseCanvas.SetActive(true);
        choosePanel.SetActive(true);
        newEntities = new List<GameObject>();
        for (int i = 0; i < mixtuerSprites.Length; i++)
        {
            choosePanel.transform.GetChild(i).GetComponent<Image>().sprite = mixtuerSprites[i];
            choosePanel.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TMP_Text>().text = mixturesDropdown[i];

            newEntities.Add(choosePanel.transform.GetChild(i).gameObject);
        }
        Information.updateEntities = newEntities.ToArray();



        outputText.gameObject.SetActive(true);
        var main1 = ps.main;
        main1.startColor = Color.red;
        var main2 = ps2.main;
        main2.startColor = Color.blue;

        var col = ps.colorBySpeed;
        col.enabled = false;

        col = ps2.colorBySpeed;
        col.enabled = false;

        var em = ps2.emission;
        em.enabled = true;
        ps.Stop();
        ps2.Stop();


        alloyPanel = new Dictionary<int, int>();
        for (int i = 0; i < alloys.Length; i++)
        {
            alloyPanel.Add(i, alloyIndecies[i]);
        }

        mixturePanel = new Dictionary<int, int>();
        for (int i = 0; i < startIndecies.Length; i++)
        {
            mixturePanel.Add(i, startIndecies[i]);
        }

        solutionPanel = new Dictionary<int, int>();
        for (int i = 0; i < solutionIndecies.Length; i++)
        {
            solutionPanel.Add(10 * (i + 1), solutionIndecies[i]);
        }

        reactionPanel = new Dictionary<int, int>();
        for (int i = 0; i < reactionIndecies.Length; i++)
        {
            reactionPanel.Add(i, reactionIndecies[i]);
        }

        List<string> mixturesList = new List<string>(mixturesDropdown);
    }

    int[] mixturePanels = new int[] { 3, 8, 4, 9 };
    void showMixturePanel(int index)
    {
        Information.panelIndex = mixturePanels[index];
        InformationPanel.SetActive(true);
    }

    public GameObject mixtureContainer;
    void SolutionSubLab()
    {
        outputText.gameObject.SetActive(true);
        outputText.text = "Use the slider below to set the concentration!";

        plainSlider.gameObject.SetActive(true);
        plainSlider.onValueChanged.AddListener(delegate { solutionSlider(); });

        ps.Play();
        var color = ps.main.startColor;
        color = Color.red;
        color = ps2.main.startColor;
        color = Color.blue;


    }

    string[] mixtures = new string[] { "0.1", "1", "1.5", "2.5" };

    int offset = 20;
    void solutionSlider()
    {
        showPanel(solutionPanel, plainSlider.value);
        int amount = (int)plainSlider.maxValue / mixtures.Length;


        for (int i = 0; i < mixtures.Length; i++)
        {

            if (plainSlider.value < amount * (i + 1))
            {
                outputText.text = "This mixtures has a concentration of: " + mixtures[i] + " mol/L";

                updateAmountOfParticles(ps2, amount * (i)); //was i+1
                ps2.Play();
                break;
            }
        }
        var col = ps2.collision;
        col.enabled = true;
    }

    void updateAmountOfParticles(ParticleSystem p, int amount)
    {

        if (p.particleCount < amount)
        {
            var em = p.emission;
            em.SetBursts(
       new ParticleSystem.Burst[]{
                new ParticleSystem.Burst(0, (amount - p.particleCount))

       });
        }
        else
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[p.particleCount];
            p.GetParticles(particles);
            List<ParticleSystem.Particle> newParticles = new List<ParticleSystem.Particle>(particles);
            for (int i = particles.Length - 1; i > amount; i--)
            {
                newParticles.RemoveAt(i);
            }
            p.SetParticles(newParticles.ToArray());
        }

    }



    void AlloySubLab(int index)
    {
        showPanel(alloyPanel, index);
        var psr = ps.GetComponent<ParticleSystemRenderer>();
        psr.material = alloyMaterials[index * 2];

        var psr2 = ps2.GetComponent<ParticleSystemRenderer>();
        psr2.material = alloyMaterials[index * 2 + 1];


        var color = ps.main.startColor;
        color = Color.white;
        color = ps2.main.startColor;
        color = Color.white;

        var col = ps2.collision;
        col.enabled = true;

        var em = ps.emission;
        em.SetBursts(
new ParticleSystem.Burst[] {
                     new ParticleSystem.Burst(0, 300,0,1,0)
});

        var em2 = ps2.emission;

        em2.SetBursts(
   new ParticleSystem.Burst[] {
                     new ParticleSystem.Burst(0, 30,0,1,0)
    });

        ps.Play();
        ps2.Play();

    }
    public Material defualtMaterial;


    void resetAll()
    {
        ps.Stop();
        ps2.Stop();
        ps3.Stop();
        ps4.Stop();

        ps.Clear();
        ps2.Clear();
        ps3.Clear();
        ps4.Clear();

        var v = ps2.forceOverLifetime;
        v.enabled = false;

        slider.gameObject.SetActive(false);
        plainSlider.gameObject.SetActive(false);
        mix.gameObject.SetActive(false);

    }


    void SuspensionSubLab()
    {

        mix.gameObject.SetActive(true);
        initSuspensionParticles(true);
        var main = ps.main;
    }

    void ColloidsSubLab()
    {
        mix.gameObject.SetActive(true);
        initSuspensionParticles(false);
        var main = ps.main;
    }

    void initSuspensionParticles(bool shouldFloat)
    {
        ps.Stop();
        ps.Clear();
        ps2.Clear();

        var psr = ps.GetComponent<ParticleSystemRenderer>();
        psr.material = defualtMaterial;
        var col = ps.colorBySpeed;
        col.enabled = false;
        var main = ps.main;
        main.startColor = Color.blue;
        main.loop = false;

        var noise = ps.noise;
        noise.frequency = 0.1f;
        noise.positionAmount = 1;
        var em = ps.emission;

        ps2.Stop();
        psr = ps2.GetComponent<ParticleSystemRenderer>();
        psr.material = defualtMaterial;
        col = ps2.colorBySpeed;
        col.enabled = false;
        main = ps2.main;
        main.startColor = Color.red;
        main.loop = false;

        noise = ps2.noise;
        noise.frequency = 0.1f;
        noise.positionAmount = 1;
        em = ps2.emission;


        em.SetBursts(
   new ParticleSystem.Burst[] {
                     new ParticleSystem.Burst(0, 300,300,1,0)
    });


        em.SetBursts(
   new ParticleSystem.Burst[] {
                     new ParticleSystem.Burst(0, 30,30,1,0)
    });
        if (shouldFloat)
        {


            var v = ps2.forceOverLifetime;
            v.enabled = true;
            v.y = -12;

        }
        else
        {
            var v = ps2.forceOverLifetime;
            v.enabled = false;

        }
        ps2.Play();
        ps.Play();


    }

    void shakeParticles()
    {
        outputText.text = "";
        // StartCoroutine(particleAnimation());
        var noise1 = ps.noise;
        var noise2 = ps2.noise;
        noise1.positionAmount = 10;
        noise2.positionAmount = 10;
        StartCoroutine(particleAnimation());
    }



    IEnumerator particleAnimation()
    {
        int shakeCount = 2;
        while (shakeCount > 0)
        {

            shakeCount--;

            yield return new WaitForSeconds(1);
        }
        //just increase the noise of the particles 
        var noise1 = ps.noise;
        var noise2 = ps2.noise;
        noise1.positionAmount = 1;
        noise2.positionAmount = 1;

    }
    #endregion


    List<Reaction> reactions;
    List<Reaction> currentReactions;
    public Dropdown reactionDropdown;

    public class Reaction
    {
        public string name;
        public List<Material> start;
        public List<Material> end;
        public bool exo;

        public float tempChange;
        public string reactionType;

        public Reaction()
        {
            start = new List<Material>();
            end = new List<Material>();
            tempChange = 0;
            reactionType = "";
            exo = false;
        }

    }

    public Button reactionStart;
    public void chemicalReactionsLab()
    {
        reactions = new List<Reaction>();

        Reaction synth2 = new Reaction();
        synth2.name = reactionNames[0]; //creating water
        synth2.start.Add(reactionMaterials[2]); //h2
        synth2.start.Add(reactionMaterials[11]); //02
        synth2.end.Add(reactionMaterials[8]); //h20
        synth2.tempChange = 0;
        synth2.exo = true;
        synth2.reactionType = reaction[1];//reactionTypes.synthesis;
        reactions.Add(synth2);

        Reaction synth1 = new Reaction();
        synth1.name = reactionNames[1]; //photosynthesis
        synth1.start.Add(reactionMaterials[8]); //water
        synth1.start.Add(reactionMaterials[0]); //carbon dioxide 
        synth1.end.Add(reactionMaterials[1]); //glucose 
        synth1.tempChange = 0;
        synth1.exo = false;
        synth1.reactionType = reaction[1];//reactionTypes.synthesis;

        reactions.Add(synth1);

        Reaction synth3 = new Reaction();
        synth3.name = reactionNames[2]; //creating salt
        synth3.start.Add(reactionMaterials[12]); //na
        synth3.start.Add(reactionMaterials[13]); //cl
        synth3.end.Add(reactionMaterials[10]); //na cl 
        synth3.tempChange = 0;
        synth3.exo = false;
        synth3.reactionType = reaction[1];//reactionTypes.synthesis;

        reactions.Add(synth3);

        Reaction decom2 = new Reaction();
        decom2.name = reactionNames[3]; //splitting water
        decom2.start.Add(reactionMaterials[8]); //h20
        decom2.end.Add(reactionMaterials[11]); //02
        decom2.end.Add(reactionMaterials[2]); //h20
        decom2.tempChange = 0;
        decom2.exo = false;
        decom2.reactionType = reaction[2];//reactionTypes.decomposition;
        reactions.Add(decom2);



        //you need to fix these two
        Reaction decom1 = new Reaction();
        decom1.name = reactionNames[4]; //lithium
        decom1.start.Add(reactionMaterials[4]); //Li2CO3
        decom1.end.Add(reactionMaterials[11]); //Li2O not done 
        decom1.end.Add(reactionMaterials[0]); //CO2
        decom1.tempChange = 0;
        decom1.exo = false;
        decom1.reactionType = reaction[2];//reactionTypes.decomposition;
        reactions.Add(decom1);



        Reaction decom3 = new Reaction();
        decom3.name = reactionNames[5]; //pottasium chloride
        decom3.start.Add(reactionMaterials[14]); //potasium chloride
        decom3.end.Add(reactionMaterials[15]); //potassium
        decom3.end.Add(reactionMaterials[16]); //chloride
        decom3.tempChange = 0;
        decom3.exo = false;
        decom3.reactionType = reaction[2];//reactionTypes.decomposition;
        reactions.Add(decom3);

        Reaction combustion = new Reaction();
        combustion.name = reactionNames[6]; //burning methane
        combustion.start.Add(reactionMaterials[5]); //ch4
        combustion.start.Add(reactionMaterials[11]); //02
        combustion.end.Add(reactionMaterials[0]); //co2
        combustion.end.Add(reactionMaterials[8]); //h20
        combustion.exo = true;
        combustion.tempChange = 0;
        combustion.reactionType = reaction[3];//reactionTypes.combustion;
        reactions.Add(combustion);


        Reaction displacment = new Reaction();
        displacment.name = reactionNames[7]; //zinc in acid
        displacment.start.Add(reactionMaterials[7]); //Zn
        displacment.start.Add(reactionMaterials[3]); //HCl
        displacment.end.Add(reactionMaterials[9]); //ZnCl
        displacment.end.Add(reactionMaterials[2]); //h2
        displacment.exo = false;
        displacment.tempChange = 0;
        displacment.reactionType = reaction[3];//reactionTypes.displacment;
        reactions.Add(displacment);

        Reaction displacment2 = new Reaction();
        displacment2.name = reactionNames[8]; //iron and copper sulfate
        displacment2.start.Add(reactionMaterials[16]); //iron suflate
        displacment2.start.Add(reactionMaterials[20]); //copper
        displacment2.end.Add(reactionMaterials[18]); //copper sulfate
        displacment2.end.Add(reactionMaterials[17]); //iron
        displacment2.exo = false;
        displacment2.tempChange = 0;
        displacment2.reactionType = reaction[3];//reactionTypes.displacment;
        reactions.Add(displacment2);


        Reaction doubleDisplacment = new Reaction();
        doubleDisplacment.name = reactionNames[9]; //neutralization
        doubleDisplacment.start.Add(reactionMaterials[2]); //H2SO4
        doubleDisplacment.start.Add(reactionMaterials[11]); //NaOH
        doubleDisplacment.end.Add(reactionMaterials[8]); //h20
        doubleDisplacment.end.Add(reactionMaterials[8]);  // Na2SO4
        doubleDisplacment.tempChange = 0;
        doubleDisplacment.exo = false;
        doubleDisplacment.reactionType = reaction[4];//reactionTypes.neutrilization;
        reactions.Add(doubleDisplacment);

        Reaction saltSilverNitrate = new Reaction();
        saltSilverNitrate.name = reactionNames[10]; //silver nitrate
        saltSilverNitrate.start.Add(reactionMaterials[10]); //nacl
        saltSilverNitrate.start.Add(reactionMaterials[21]); //silver nitrate
        saltSilverNitrate.end.Add(reactionMaterials[22]); //silver chloride
        saltSilverNitrate.end.Add(reactionMaterials[23]);  // sodium nitrate salt
        saltSilverNitrate.tempChange = 0;
        saltSilverNitrate.exo = false;
        doubleDisplacment.reactionType = reaction[4];//reactionTypes.neutrilization;
        reactions.Add(saltSilverNitrate);

        //ok, you need to add the sprites for this one 

        Reaction bakingSodaVinegar = new Reaction();
        bakingSodaVinegar.name = reactionNames[11]; //baking soda and vinear
        bakingSodaVinegar.start.Add(reactionMaterials[10]); //vinegar
        bakingSodaVinegar.start.Add(reactionMaterials[21]); //baking soda
        bakingSodaVinegar.end.Add(reactionMaterials[22]); //carbonic acid 
        bakingSodaVinegar.end.Add(reactionMaterials[23]);  // sodium nitrate salt
        bakingSodaVinegar.tempChange = 0;
        bakingSodaVinegar.exo = false;
        bakingSodaVinegar.reactionType = reaction[4];//reactionTypes.neutrilization;
        reactions.Add(bakingSodaVinegar);

        chooseCanvas.SetActive(true);
        choosePanel.SetActive(true);
        newEntities = new List<GameObject>();
        for (int i = 0; i < reactionSprites.Length; i++)
        {

            choosePanel.transform.GetChild(i).GetComponent<Image>().sprite = reactionSprites[i];
            choosePanel.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TMP_Text>().text = reaction[i + 1];
            newEntities.Add(choosePanel.transform.GetChild(i).gameObject);
        }
        Information.updateEntities = newEntities.ToArray();


        var col = ps.collision;
        var col2 = ps2.collision;

        col.enabled = true;
        col2.enabled = true;

        var main = ps.main;
        var main2 = ps2.main;
        main.loop = false;
        main2.loop = false;

    }


    string reactionType;

    void showReactionPanel(string name)
    {
        for (int i = 0; i < reactionNames.Length; i++)
        {
            if (reactionNames[i] == name)
            {
                Information.panelIndex = i + 1;
                InformationPanel.SetActive(true);
                return;
            }
        }
    }

    IEnumerator thermometerAnimation(bool exo)
    {
        slider.interactable = false;
        int count = 0;
        int reactionLength = 200;
        slider.gameObject.SetActive(true);
        if (exo)
        {
            slider.value = 0;
        }
        else
        {
            slider.value = 100;
        }
        while (count < reactionLength)
        {
            count++;
            if (exo)
            {
                slider.value++;
            }
            else
            {
                slider.value--;
            }

            yield return new WaitForSeconds(0.01f);
        }

    }

    bool isReacting = false;
    int reactionIndex = -1;

    void onReationStart()
    {
        isReacting = true;
        Reaction currReaction = reactions[reactionIndex];
        StartCoroutine(thermometerAnimation(currReaction.exo));
        var psr2 = ps2.GetComponent<ParticleSystemRenderer>();
        var col = ps2.colorBySpeed;
        if (currReaction.reactionType == reaction[2]) //decomp
        {
            reacting = true;

        }
        else
        {
            col.enabled = false;
            if (currReaction.start.Count > 1)
            {
                psr2.material = currReaction.start[1];
                var em = ps2.emission;
                em.SetBursts(
    new ParticleSystem.Burst[] {
               new ParticleSystem.Burst(0, 300)
    });
                ps2.Play();


            }
        }

        reacting = true;

        var psr3 = ps3.GetComponent<ParticleSystemRenderer>();
        col = ps3.colorBySpeed;
        col.enabled = false;
        psr3.material = currReaction.end[0];
        var emission = ps3.emission;
        emission.rateOverTime = 20;
        ps3.Play();

        if (currReaction.reactionType == reaction[2] || currReaction.reactionType == reaction[4])
        {
            var psr4 = ps4.GetComponent<ParticleSystemRenderer>();
            col = ps4.colorBySpeed;
            col.enabled = false;
            psr4.material = currReaction.end[1];
            emission = ps4.emission;
            emission.rateOverTime = 10;
            ps4.Play();
        }

        var psr = ps.GetComponent<ParticleSystemRenderer>();
        psr.material = currReaction.start[0];
        col = ps.colorBySpeed;
        col.enabled = false;

        var emPs = ps.emission;

        emPs.SetBursts(
   new ParticleSystem.Burst[] {
                     new ParticleSystem.Burst(0, 300)
    });

        ps.Play();
        endParticles = 300;
        StartCoroutine(reactionUpdator());
    }

    int reactionSppedOffset = 100;
    bool reacting = false;
    int endParticles;


    IEnumerator reactionUpdator()
    {

        while (reacting)
        {
            if (endParticles < 20)
            {

                reacting = false;
                yield break;
            }


            updateAmountOfParticles(ps, endParticles);
            if (reactionType == reaction[2])
            {

            }
            else
            {
                updateAmountOfParticles(ps2, endParticles);
            }

            endParticles -= 10;
            yield return new WaitForSeconds(1f);
        }

    }

}
