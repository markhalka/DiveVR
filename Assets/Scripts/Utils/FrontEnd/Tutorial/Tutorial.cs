using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{

    //ok, so just have 2 show panels, than you can do an aniamtion for each one
    //add the hilights, and make sure everything works, shouldn't take more than 2 hours 

    //keep showing the hilight until the panel exits
    //use temp load, and put the tutorial from data2 into data 





    public GameObject container;
    public GameObject buttons;
    public GameObject informationPanel;


    GameObject currentPanel;

    bool shouldSwitch = false;


    TutorialScene currentScene;
    void Start()
    {
        //help.onClick.AddListener(delegate { takeHelp(); });
    }

    public void takeHelp()
    {
        Debug.LogError("take help called");
        //  Information.isTutorial = 1;
        Information.isInMenu = true;
        container.gameObject.SetActive(true);
        currentPanel = container.transform.GetChild(0).gameObject;
        informationPanel.transform.parent.gameObject.SetActive(true);
        initButtons();
        getCurrentScnee();
    }


    public void getCurrentScnee()
    {
        if (Information.tutorialScenes == null)
        {
            ParseData.parseTutorial();
        }

        foreach (var scene in Information.tutorialScenes)
        {
            Debug.LogError(Information.currentScene + " " + scene.sceneName);
            if (Information.currentScene == scene.sceneName)
            {
                currentScene = scene;
                return;
            }
        }
    }

    //here add a small growing and shrinking animation to the tutorial



    int growCount = 20;
    float growthAmount = 1.005f;
    int count = 0;

    void updateHighlight()
    {
        if (currHighlight != null)
        {
            if (count < growCount)
            {
                currHighlight.transform.localScale *= growthAmount;
            }
            else if (count < 2 * growCount)
            {
                currHighlight.transform.localScale *= 1 / growthAmount;
            }
            else
            {
                count = 0;
                return;
            }
            count++;
        }
    }

    void Update()
    {

        updateHighlight();
        if (clicked && !informationPanel.activeSelf)
        {
            getNext();
        }
        if (shouldSwitch && !informationPanel.activeSelf)
        {
            Debug.LogError("hereee!");
            shouldSwitch = false;
            currentScene.panelIndex++;
            if (currentScene.panelIndex > currentScene.panels.Count - 1)
            {
                //if you had a main tutorial you would put it here

                currentPanel.SetActive(false);
                container.SetActive(false);
                container.transform.GetChild(0).gameObject.SetActive(true);
                informationPanel.SetActive(false);

                Information.isInMenu = false;
                clicked = false;
                shouldSwitch = false;
                informationPanel.transform.parent.gameObject.SetActive(false);
                //  Information.isTutorial = 2;
                Information.tutorialScenes = null;
                Debug.LogError("parsing model again..");
                ParseData.parseModel(); //this will reparse it 
                                        //here you need to reparse the model


                return;
            }
            else
            {
                currentPanel.gameObject.SetActive(false);
                currentPanel = container.transform.GetChild(currentScene.panelIndex).gameObject;
                currentPanel.gameObject.SetActive(true);
                initButtons();
                currentScene.panels[currentScene.panelIndex].buttonIndex = 0;
            }

            //   takeClick();
        }

    }



    public bool clicked = false;

    public void takeClick()
    {
        clicked = true;
        currHighlight = null;
        Debug.LogError(currentScene.panelIndex + " " + currentScene.panels.Count);
        int buttonIndex = currentScene.panels[currentScene.panelIndex].buttonIndex;

        var thing = currentScene.panels[currentScene.panelIndex];
        Information.tutorialModel = thing.information[buttonIndex];


        if (!informationPanel.activeSelf)
        {
            choosePanel(userButtons[buttonIndex].gameObject);
        }

        informationPanel.transform.parent.GetComponent<InformationPanel>().loadNewModel();


    }

    GameObject currHighlight;

    void getNext()
    {
        Debug.LogError("is in menu");
        Information.isInMenu = true;

        clicked = false;
        int buttonIndex = currentScene.panels[currentScene.panelIndex].buttonIndex;

        userButtons[buttonIndex].gameObject.SetActive(false);
        currHighlight = null;
        count = 0;

        if (buttonIndex + 1 < userButtons.Count)
        {
            userButtons[buttonIndex + 1].gameObject.SetActive(true);
            currHighlight = userButtons[buttonIndex + 1].gameObject;
        }
        else
        {
            shouldSwitch = true;
            return;
        }

        currentScene.panels[currentScene.panelIndex].buttonIndex++;
    }

    //here get the x coordinate of the center of hte outline, and choose the panle that is farther from it, thats pretty much it 
    //ok, so in the show prefab add the other panel?
    //than you can keep information panel 
    //here you would just change which one is in current panel?

    public void choosePanel(GameObject currentButton)
    {
        Information.lableIndex = 0;
        Debug.LogError(currentButton.transform.localPosition.x + " position x");
        if (currentButton.transform.localPosition.x < 0)
        {
            //pick the right panel
            informationPanel.transform.parent.GetComponent<InformationPanel>().locationPanel.setPosition(LocationPanel.MenuPosition.RIGHT);
        }
        else
        {
            //pick the left panel
             informationPanel.transform.parent.GetComponent<InformationPanel>().locationPanel.setPosition(LocationPanel.MenuPosition.LEFT);

        }
        informationPanel.SetActive(true);

    }


    public void getPanelIndexFromScene()
    {

        var temp = currentScene.panels[currentScene.panelIndex];
        var Model = temp.information[temp.buttonIndex];
    }



    List<Button> userButtons;
    void initButtons()
    {
        userButtons = new List<Button>();


        for (int i = 0; i < currentPanel.transform.childCount; i++)
        {

            Button curr = currentPanel.transform.GetChild(i).GetComponent<Button>();
            curr.onClick.AddListener(delegate { takeClick(); });
            curr.gameObject.SetActive(false);
            userButtons.Add(curr);

        }
        userButtons[0].gameObject.SetActive(true);
        currHighlight = userButtons[0].gameObject;
    }
}
