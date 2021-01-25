using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// ok, bet so that works
// now just try with a horizontal model

// today todo:
//1. work on this, test a few other things
//2. get the quiz working
//3 go through data, and make sure there is not too much cliking to do
//4. finally add and test the video stuff
//5. add azure stuff

// try and finish this by tommorow, or the day after THE LATEST, honestly, you should be done by now...


// ok, lit now you can work on getting the quiz shit to work again
// work on that for like 20 mins, then just do a few of the assignments, and call it a day



public class ScienceModels : MonoBehaviour
{

    public bool[] showIndecies;
    public bool showAuroa = false;

    //   public GameObject panel;
    public GameObject models;
    public GameObject currInformationPanel;
    public static GameObject currentObject;
    public HorizontalModel horizontalModel;
    public static GameObject currentModel;
    public GameObject inBetweenPanel;

    public Button showUnfoundbutton;

    public GameObject background;



    GameObject[] entities;
    GameObject[] labels;
    AudioClip[] sounds;

    public Sprite[] backgrounds;

    bool isQuiz;
    Quiz quiz;

    public TMP_Text simple;

    string level;
    bool shouldGrow = true;
    Animations animations;

    bool shouldRotate;



    public Sprite[] hideShow;

    List<int> startPanels;

    public Button hideUnhide;

    bool wasSelected = false;
    bool isCircular = false;
 

    public GameObject speechBubble;

    public ModelAnimations modelAnimations;
    
    void Start()
    {

        tempLoad();
        // here you do all the temp stuff
        Information.subject = "science";
        Information.grade = "Grade 7";
        Information.nextScene = 18;
        //

        Information.click2d = false;
        Information.isSelect = false;
        Information.isInMenu = false;
        Information.panelClosed = true;

        quiz = new Quiz();

        initModel();
        infoPanel.initStartPanels(); 

        Information.panelIndex = -1;
        Information.lableIndex = 0;

        hideUnhide.onClick.AddListener(delegate { takeHideclick(); });

        Information.currentScene = "Models";

        speechBubble.GetComponent<Button>().onClick.AddListener(delegate { takeSpeechBubble(); });
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



    List<GameObject> dots;
    public Camera cam;
    void initDots(GameObject selectable)
    {

        dots = new List<GameObject>();

        Debug.LogError("initializing model: " + selectable.transform.childCount);
        //  foreach(var a in )
        for (int i = 0; i < selectable.transform.childCount; i++)
        {
            foreach (var g in selectable.transform.GetChild(i).GetComponentsInChildren<SpriteRenderer>(true))
            {
                if (g.gameObject.name.Contains("dot"))
                {
                    dots.Add(g.gameObject); //maybe?
                }

            }
        }
    }

    public GameObject outlinePanel;

    //just import 
    public void fancyAnimation()
    {
        outlinePanel.SetActive(true);
        StartCoroutine(modelAnimations.moveObject(false));
    }

    public GameObject page1;
    List<GameObject> cursorInit;
    void initModel()
    {
        cursorInit = new List<GameObject>();
        background.GetComponent<Image>().sprite = backgroundDefualt;//backgrounds[currentModelIndex];
        animations = new Animations();

        ParseData.parseModel();

        setModelIndex();

        currentModel = models.transform.GetChild(currentModelIndex).gameObject;
        currentModel.SetActive(true);


        if (isHorizontalSnap)
        {
            bool[] showIndecies = new bool[ScienceModels.currentModel.transform.GetChild(0).childCount];
            List<GameObject> toAdd = new List<GameObject>();
            for (int i = 0; i < showIndecies.Length; i++)
            {
                toAdd.Add(ScienceModels.currentModel.transform.GetChild(0).GetChild(i).gameObject);
                showIndecies[i] = true;
            }
            if (Information.pretestScore == -1)
            {
                foreach (var child in toAdd)
                {
                    GameObject newPage = Instantiate(page1, page1.transform, false);
                    newPage.transform.SetParent(newPage.transform.parent.parent);
                    child.transform.SetParent(newPage.transform, true);

                    horizontalModel.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().AddChild(newPage, true);
                    cursorInit.Add(child);

                }
            }
            else
            {
                Transform curr = horizontalModel.transform.GetChild(0);
                for (int i = 0; i < curr.childCount; i++)
                {
                    cursorInit.Add(curr.GetChild(i).GetChild(0).gameObject);
                }
            }

            ScienceModels.currentObject = horizontalModel.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;

            Information.updateEntities = cursorInit.ToArray();
            infoPanel.showTitle(0);

            if (shouldRotate)
            {
                addRotationAnimation();
            }

            horizontalModel.shouldRotate = shouldRotate;

        } else
        {
            showIndecies = new bool[currentModel.transform.GetChild(0).childCount];
            for (int i = 0; i < currentModel.transform.GetChild(0).childCount; i++)
            {
                GameObject currentObject = currentModel.transform.GetChild(0).GetChild(i).gameObject;
                cursorInit.Add(currentObject);
                showIndecies[i] = true;
            }

            initDots(currentModel.transform.GetChild(0).gameObject);
            cursorInit.Add(speechBubble);

            Information.updateEntities = cursorInit.ToArray();
        }

        entities = cursorInit.ToArray();
        animations.init(entities);

        labels = new GameObject[0];
        hideAllLables();
    }



    void addRotationAnimation()
    {
        StartCoroutine(modelAnimations.rotate()); //? not sure about that
    }

    Table table;
    void createTable()
    {
        if (isHorizontalSnap) //then you need to hide the horizontal scroll snap shit 
        {
            horizontalModel.gameObject.SetActive(false);
        }
        else
        {
            currentModel.SetActive(false);
        }
        showUnfoundbutton.gameObject.SetActive(false);
        speechBubble.SetActive(false);

        isHiding = false;
        takeHideclick();

        table.createTable(takeTableClick);
    }
    void takeTableClick(GameObject curr)
    {

        if (quiz.checkName(curr.GetComponentInChildren<TMPro.TMP_Text>().text))
        {
            //      simple.text = quiz.next();
            if (quiz.getQuestions() > Information.rightCount)
            {
            //    endQuiz();
            }
        }
        //   updateQuiz();

    }

    Hints hints;
    void takeHint()
    {
        hints.checkShowAnswer(isHorizontalSnap && !table);
        if (table)
        {
        //    hints.tableHint();
        }
        else
        {
         //   hints.takeShowAnswer


        int curr = int.Parse(quiz.getIndex());
            int currentIndex = curr - modelOffset;
        }
    }

    //that should work
    bool isHiding = false;
    public void takeHideclick()
    {

        foreach (var g in dots)
        {
            g.SetActive(isHiding);
        }
        isHiding = !isHiding;
        if (isHiding)
        {
            Debug.LogError("hiding dots");
            hideUnhide.GetComponent<Image>().sprite = hideShow[0];
            speechBubble.SetActive(false);
        }
        else
        {
            Debug.LogError("hiding dots");
            hideUnhide.GetComponent<Image>().sprite = hideShow[1];

        }

    }

    

    //this will handle the animation of the thing to the spot in the left, and apply the filer 
    //so, what this will do is:
    //1. if a bool is true, than lerp it to the right spot, keep track of its past location, and hide the model 
    //check the size, and make sure it fits 







    int modelOffset = 0;
    int currentModelIndex;
    bool isHorizontalSnap = false;

    //here you need to add the right ones, as well as the right parameters
    void setModelIndex()
    {
        switch (Information.nextScene)
        {
            case 1: //matter and atoms 
                shouldRotate = false;
                currentModelIndex = 9;
                break;

            case 5: //plants //not done
                Debug.LogError("here at 5");
                shouldRotate = false;
                currentModelIndex = 5;
                break;
            case 11: //simple machines  not done
                shouldGrow = false;
                isCircular = true;
                shouldRotate = false;
                isHorizontalSnap = true;
                currentModelIndex = 17;
                break;
            case 12: //microscope  not done
                shouldRotate = false;
                currentModelIndex = 16;
                break;
            case 14: //animal cell
                shouldRotate = false;
                currentModelIndex = 2;
                break;
            case 15: //plant cell
                shouldRotate = false;
                currentModelIndex = 3;
                break;
            case 18: //rocks and minerals
                shouldGrow = false;
                isCircular = true;
                shouldRotate = true;
                isHorizontalSnap = true;
                currentModelIndex = 0;
                break;
            case 19: //fossils
                shouldRotate = false;
                isHorizontalSnap = true;
                currentModelIndex = 10;
                modelOffset = 2; //because you have one after that 
                break;
            case 20: //greenhouse
                shouldRotate = false;
                currentModelIndex = 13;
                modelOffset = 1;
                break;

            case 21: //water cycle
                shouldRotate = false;
                currentModelIndex = 7;
                break;
            case 22: //astronomy
                shouldRotate = true;
                isHorizontalSnap = true;
                currentModelIndex = 1;
                break;
            case 23:     //science tools not done
                isHorizontalSnap = true;
                shouldRotate = false;
                currentModelIndex = 14;
                break;
            case 30: //biochem
                currentModelIndex = 12;
                isHorizontalSnap = true;
                shouldRotate = false;
                break;
            case 31: //anatomy
                shouldGrow = false;
                isCircular = true;
                shouldRotate = true;
                isHorizontalSnap = true;
                currentModelIndex = 4;
                break;
            case 32: //flight not done
                shouldRotate = false;
                isHorizontalSnap = true;
                currentModelIndex = 18;
                break;
            case 34: //climate
                shouldRotate = false;
                currentModelIndex = 19;
                break;
            case 35: //photosynthesis
                shouldRotate = false;
                currentModelIndex = 6;
                break;
            case 36: //food web
                shouldRotate = false;
                currentModelIndex = 11;
                break;
            case 39: //weather and atmosphere 
                shouldRotate = false;
                currentModelIndex = 21;
                break;
            case 42: //energy sources 
                shouldGrow = false;
                isCircular = true;
                shouldRotate = false;
                isHorizontalSnap = true;
                currentModelIndex = 8;
                break;

            case 44: //enviornemnt, for now just make it the same as food web
                shouldRotate = false;
                currentModelIndex = 20;
                break;

            case 45: //weathering
                shouldRotate = false;
                currentModelIndex = 22;
                break;

            case 46: //organ systems 
                shouldRotate = false;
                currentModelIndex = 23;
                break;

        }
    }



    GameObject pastCurr;
    
    void Update()
    {
        if (!currInformationPanel.activeSelf && !inBetweenPanel.activeSelf) 
        {
            checkQuiz();
        }

        if (speechOffset < 41)
        {
            speechOffset++;
        }

        if (Information.currentBox != null) //&& !currInformationPanel.activeSelf)
        {

            int i = getIndex(Information.currentBox);
            Debug.LogError("current box index: " + i);
            if (i < 0)
            {

                return;
            }

            if(!isHorizontalSnap)
            {
                Debug.LogError("handeling click..");
                handleClickUpdate(i);
            }

            if (isQuiz)
            {

                if (speechBubble.activeSelf && speechOffset < 20)
                {
                    return;
                }
                speechOffset = 0;
                if (Information.currentBox == speechBubble)
                {
                    i = getIndex(currentDot);
                }
                else if (Information.currentBox.transform.childCount > 0 && !isHorizontalSnap)
                {
                    if (currentDot != null && currentDot.transform.parent.gameObject == Information.currentBox || isHiding)
                    {
                        i = getIndex(Information.currentBox);
                    }
                    else
                    {
                        showPopUp();
                        return;
                    }
                }
                else
                {
                    speechBubble.SetActive(false);
                }
                quiz.check((i + modelOffset).ToString());

            }
            else 
            {
                if (!isHorizontalSnap)
                {
                    Information.lableIndex = 0;
                    simple.text = Information.userModels[i + modelOffset].simpleInfo[0];
                }

                pastCurr = Information.currentBox;

            }

            Information.currentBox = null;

        }

        if (!isHorizontalSnap)
            animations.updateAnimations(getCurrentIndex());
    }

    
    GameObject currentDot;
    public Canvas canvas;
    //ok, instead put it on the dot,
    int speechOffset = 0;
    void showPopUp()
    {
        for (int i = 0; i < Information.currentBox.transform.childCount; i++)
        {
            if (Information.currentBox.transform.GetChild(i).gameObject.name.Contains("dot"))
            {
                currentDot = Information.currentBox.transform.GetChild(i).gameObject;
            }
        }
        //  currentDot = Information.currentBox.transform.GetChild(0).gameObject; //??
        speechBubble.SetActive(true);
        Vector3 pos = Camera.main.WorldToScreenPoint(currentDot.transform.position);
        pos.y += 10;

        speechBubble.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = Information.userModels[getIndex(Information.currentBox) + modelOffset].simpleInfo[0];  //whatever it is 

        /*Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1);
        Vector2 tempOut = new Vector2(0, 0);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out tempOut);
        speechBubble.transform.localPosition = new Vector3(tempOut.x, tempOut.y, 0);
        */  
        speechBubble.transform.position = pos;
        speechOffset = 0;
        Information.currentBox = null;


    }

    void takeSpeechBubble()
    {
        if (currentDot != null)
        {

            if (speechOffset < 20)
            {
                Debug.LogError("not yet");
                return;
            }
            int i = getIndex(currentDot.transform.parent.gameObject);
            //    quizQuestion(i);
            quiz.check((i + modelOffset).ToString());

            speechOffset = 0;
        }
    }

    void rightSpecific()
    {
     /*   if (tookHint)
        {
            Debug.LogError("took hint is now false");
            tookHint = false;
            closeHint();
        }

        if (quiz.getQuestions() > 2 && Information.autoHide && !inTable) ///for the skip shit
        {

            createTable();
        }*/
    }


    void handleClickUpdate(int i)
    {
        Debug.LogError("handling click update...");
        currentObject = Information.currentBox;


        if (pastCurr != null && !isQuiz && !wasSelected && Information.currentBox.transform == pastCurr.transform)
        {
            //ok so here you just need to update the text
            //    simple.text = Information.userModels[i].advancedInfo[0];
            //panel.gameObject.SetActive(true);
            choosePanel(Information.currentBox);
            showPanel();
            showIndecies[i] = false;
            if (showAuroa)
            {
                //    showUnfound();
            }

            if (shouldGrow)
                animations.addAnimation2(i, Information.animationLength, false, Information.animationGrowth); //not sure about that one cheif

            if (!isCircular)
                wasSelected = true;
        }
        else
        {
            if (pastCurr != null)
            {
                hideAllLables();
                Information.lableIndex = 0; //this way you get the first one 
                if (shouldGrow && wasSelected)
                {
                    animations.resetSize(getIndex(pastCurr), wasSelected); //that should reset the size 
                    wasSelected = false;
                }
            }
        }


        if (shouldRotate)
        {
            addRotationAnimation();
        }

        if (shouldGrow)
        {
            animations.addAnimation(i, Information.animationLength, true, Information.animationGrowth);
        }
    }

    public void choosePanel(GameObject currentButton)
    {
        Information.lableIndex = 0;
        GameObject dotObject = null;
        for (int i = 0; i < currentButton.transform.childCount; i++)
        {
            if (currentButton.transform.GetChild(i).gameObject.name.Contains("dot"))
            {
                dotObject = currentButton.transform.GetChild(i).gameObject;
                break;
            }
        }
        Vector3 currPosition = Camera.main.WorldToScreenPoint(dotObject.transform.position); //the first object
        if (currPosition.x < Camera.main.pixelWidth / 2)
        {
            //pick the right panel
            // currInformationPanel.transform.parent.GetComponent<InformationPanel>().setLeftorRight(true);
        }
        else
        {
            //pick the left panel
            // currInformationPanel.transform.parent.GetComponent<InformationPanel>().setLeftorRight(false);
        }
        currInformationPanel.SetActive(true);

    }


    public InformationPanel infoPanel;
    void showPanel()
    {
        infoPanel.showPanel(getIndex(Information.currentBox));
    }


    void checkQuiz()
    {
        if (!quiz.isQuiz)
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

    void endQuiz()
    {

       /* Debug.LogError("quiz ended");

        currInformationPanel.transform.parent.GetComponent<InformationPanel>().hintPanel.SetActive(false);
        currInformationPanel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(true);

        background.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        animationImage.gameObject.SetActive(true);
        if (isHorizontalSnap)
        {
            horizontalSnap.SetActive(true);
        }
        currentModel.SetActive(true);

        //table.transform.parent.gameObject.SetActive(false);
        for (int i = 0; i < table.transform.parent.childCount; i++)
        {
            table.transform.parent.GetChild(i).gameObject.SetActive(false);
        }
        inTable = false;


        double time = Time.timeSinceLevelLoad / 60;

        simple.text = "";
        isQuiz = false;
        Information.isQuiz = 0;

        float score = ((float)quiz.getScore() / quiz.getTotalQuestions()) * 100;

        Debug.LogError("score " + score);
        if (Information.wasPreTest)
        {
            isHiding = true;
            takeHideclick(); //that should show the dots 

            Information.pretestScore = score;
            initModel();
        }
        else
        {

            Information.score = score;
            XMLWriter.saveMiniTest(Information.currentTopic, score, 0, false);

            currInformationPanel.transform.parent.gameObject.SetActive(false);
            hideUnhide.gameObject.SetActive(false); //?
            inBetweenPanel.SetActive(true);
        } */
        quiz.end();

    }




    

    public Sprite backgroundDefualt;

    void startQuiz()
    {
        if (!Information.wasPreTest)
        {
            currInformationPanel.transform.parent.GetComponent<InformationPanel>().hintPanel.SetActive(true);
            currInformationPanel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(false);
        }

        if (wasSelected && pastCurr != null)
        {

            animations.resetSize(getIndex(pastCurr), wasSelected);
            wasSelected = false;
        }
        List<string[]> currLables = new List<string[]>();
        for (int i = modelOffset; i < Information.userModels.Count; i++)
        {
            foreach (var question in Information.userModels[i].questions)
            {
                currLables.Add(new string[] { question, i.ToString() });
            }
        }


        isQuiz = true;
        quiz.initQuiz(currLables);

    }

    public bool inTable = false;
    void specificEndQuiz()
    {
        currInformationPanel.transform.parent.GetComponent<InformationPanel>().hintPanel.SetActive(false);
        currInformationPanel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(true);

        if (isHorizontalSnap)
        {
            horizontalModel.gameObject.SetActive(true);
        }
        currentModel.SetActive(true);

        for (int i = 0; i < table.transform.parent.childCount; i++)
        {
            table.transform.parent.GetChild(i).gameObject.SetActive(false);
        }
        inTable = false;


    }


    public int getCurrentIndex()
    {
        if (currentObject == null)
        {
            return -1;
        }
        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i].name == currentObject.name)
            {
                return i;
            }
        }
        return -1;
    }

    int getIndex(GameObject curr)
    {
        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i].name == curr.name)
            {
                return i;
            }
        }
        return -1;
    }





    float selectionGrowth = 1.03f;
    int selectionLength = 5;

    void hideAllLables()
    {
        for (int i = 0; i < labels.Length; i++)
        {
            labels[i].SetActive(false);
        }
    } 
}
