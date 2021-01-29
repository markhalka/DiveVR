using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//ok, now make sure reaction and solutions is good 
// here, make sure you call the start function, for a few of them
// also, make sure that the right images are used in the panels... (how to do that with mixtures?)

//todo:
//1. test each one
//2. make sure the start panels pop up
//3. make sure other panels pop up at the right time
//4. done 


// thermal energy pretty much works 

// ok, now lets test mixtures

// mixtures lab doesn't really do anything, just get rid of it
// but keep those two functions, which you can access from the sublabs

// ok, the choose menus seem to be working well, and so are the alloys (except there is not start panel but that's an easy fix)

//for mixtures, call those start panels on top of the choose menu
// then when you click on one of the sub labs, just show the right infopanel

// ok, now mixtures works decently, just fix up all the specifics tommorow 

// ok, so now just one problem with reactions:
// it looks like you never actually set it to current lesson hmm

// YEEEEEEY reactions works now too (just double check it tommorow)


public class MoleculeMenu : MonoBehaviour
{

    public GameObject choosePanel;
    public GameObject chooseSecondPanel;

    public GameObject inBetween;
    public GameObject quiz;

    public Button back;
    List<GameObject> newEntities;
    public TMPro.TMP_Text outputText;
    public Sprite[] reactionInnerSprites;
    public Sprite[] alloySprites;
    public Sprite[] mixtureSprites;

    public ParticleSystem ps;
    public ParticleSystem ps2;
    public ParticleSystem ps3;
    public ParticleSystem ps4;

    public Slider slider;
    public GameObject plainSlider;
    public GameObject mix;

    public GameObject thermalEnergy;
    public GameObject alloySub;
    public GameObject mixtures;
    public GameObject solutionSub;
    public GameObject suspensionColloid;
    public GameObject reaction;
    public Reactions reactionsScript;

    public GameObject currentLab;

 
    public Button start;

    public GameObject infoPanel;


    int[] mixturePanels = new int[] { 3, 8, 4, 9 };


    // 2 heat and thermal energy
    // 3 types of change
    // 4 mixtures and solutions
    // 26 checmical reactions
    private void Start()
    {
      
        ParseData.parseModel();

        Information.panelIndex = -1;
        Information.lableIndex = 0;
        Information.currentScene = "Molecules";


        back.onClick.AddListener(delegate { takeBack(); });
        slider.onValueChanged.AddListener(delegate { sliderValueChanged(); });
        //  start.onClick.AddListener(delegate { takeStart(); }); //YOU NEED TO INIT THESE BUTTONS...
        //  quizButton.onClick.AddListener(delegate { takeQuiz(); });

        callLab();
    }



    void callLab()
    {
        currentLab = null;
        switch (Information.nextScene)
        {
            case 2:
                currentLab = thermalEnergy;
                currentLab.SetActive(true);
                choosePanel.transform.parent.gameObject.SetActive(false);
                break;
            case 4:
                choosePanel.SetActive(true);
                infoPanel.GetComponent<InformationPanel>().initStartPanels();
                mixturesLab();
                break;
            case 26:
                choosePanel.SetActive(true);
                infoPanel.GetComponent<InformationPanel>().initStartPanels();
                reactionsLab();
                break;

        }
    }


    void tempLoad()
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("XML/General/UserData");
        string txt = mytxtData.text;
        Information.xmlDoc = XDocument.Parse(txt);

        mytxtData = (TextAsset)Resources.Load("XML/General/Data");
        txt = mytxtData.text;
        Information.loadDoc = XDocument.Parse(txt);
        Information.name = "none";
        Information.doneLoadingDocuments = true;
        Information.firstTime = true;
    }


    float changeAmount = (360 - 244) / (float)(360 * 100);
    float startAmount = 244 / (float)360;
    void sliderValueChanged()
    {
        slider.transform.GetChild(1).GetComponentInChildren<Image>().color = Color.HSVToRGB(changeAmount * slider.value + startAmount, 1, 1);
    }

    void takeBack()
    {
        if(currentLab!=null)
        currentLab.SetActive(false);

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
        Debug.LogError("ya clicked");
        choosePanel.gameObject.SetActive(false);
        resetAll();
        outputText.gameObject.SetActive(false);
        choosePanelIndex = index;
        switch (Information.nextScene)
        {
            case 4: //mixtures 
               // mix set the info start here
                switch (index)
                {
                    case 0: //mixtures
                        currentLab = solutionSub;
                        currentLab.SetActive(true);         
                        break;
                    case 1: //suspension
                        currentLab = suspensionColloid;
                        currentLab.SetActive(true);
                        currentLab.GetComponent<SuspensionColloidSubLab>().startSuspsension();
                        break;
                    case 2: //alloy
                        createSecondChoosePanel(index);
                        break;
                    case 3: //colloid
                        currentLab = suspensionColloid;
                        currentLab.SetActive(true);
                        currentLab.GetComponent<SuspensionColloidSubLab>().startColloid();
                        break;
                }

                Information.panelIndex = mixturePanels[index];
                infoPanel.GetComponent<InformationPanel>().loadNewModel();
                break;
            case 26: //reactions
                Debug.LogError("here again");
                createSecondChoosePanel(index);
                break;
        }
    }

    void showMixturePanel(int index)
    {
        Information.panelIndex = mixturePanels[index];
        infoPanel.SetActive(true); //YOU SHOULD FIX THIS TOO
    }


    // this creates the choose panel 1
    string[] mixturesDropdown = new string[] { "Solutions", "Suspensions", "Alloys", "Colloids" };
    public void mixturesLab() //4
    {
        newEntities = new List<GameObject>();
        for (int i = 0; i < mixtureSprites.Length; i++)
        {
            choosePanel.transform.GetChild(i).GetComponent<Image>().sprite = mixtureSprites[i];
            choosePanel.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TMP_Text>().text = mixturesDropdown[i];

            newEntities.Add(choosePanel.transform.GetChild(i).gameObject);
        }
        Information.updateEntities = newEntities.ToArray();

     //   outputText.gameObject.SetActive(true);
    }

    public Sprite[] reactionSprites;
    public void reactionsLab()
    {
        newEntities = new List<GameObject>();
        for (int i = 0; i < reactionSprites.Length; i++)
        {
            choosePanel.transform.GetChild(i).GetComponent<Image>().sprite = reactionSprites[i];
            choosePanel.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TMP_Text>().text = Reaction.reaction[i+1];

            newEntities.Add(choosePanel.transform.GetChild(i).gameObject);
        }
        Information.updateEntities = newEntities.ToArray();

    }


 
    void createSecondChoosePanel(int index)
    {
        Debug.LogError("creating reaction second choose panel");
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

                currentText.Add(reactionsScript.reactionNames[index * 3]);
                currentText.Add(reactionsScript.reactionNames[index * 3 + 1]);
                currentText.Add(reactionsScript.reactionNames[index * 3 + 2]);
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

    int reactionIndex;
    void callSecondSubLab(int index)
    {
        chooseSecondPanel.gameObject.SetActive(false);
        switch (Information.nextScene)
        {
            case 4: //mixutres
                //AlloySubLab(index);
                currentLab = alloySub;
                currentLab.SetActive(true);
                currentLab.GetComponent<AlloySubLab>().startLab(index);
                break;
            case 26: //reactions
                currentLab = reaction;
                currentLab.SetActive(true);              
                reactionIndex = choosePanelIndex * 3 + index;
                currentLab.GetComponent<ReactionsLab>().initReactions(reactionIndex);
                Reaction currReaction = reactionsScript.reactions[reactionIndex];
                showReactionPanel(currReaction.name);
                break;
        }

    }

    void showReactionPanel(string name)
    {
        for (int i = 0; i < reactionsScript.reactionNames.Length; i++)
        {
            if (reactionsScript.reactionNames[i] == name)
            {
                Debug.LogError("here, showing reactions panel");
                Information.panelIndex = i + 1; //FIX THIS
                infoPanel.GetComponent<InformationPanel>().loadNewModel();
                return;
            }
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


    private void Update()
    {
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

        if (Information.doneLoading)
        {
            SceneManager.LoadScene("ScienceMain");
        } 
    }


    void resetAll() // make sure you reset the box position as well
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
}

