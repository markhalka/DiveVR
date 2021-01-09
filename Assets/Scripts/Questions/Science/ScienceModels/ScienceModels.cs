using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScienceModels : MonoBehaviour
{

    public bool[] showIndecies;
    public bool showAuroa = false;

    //   public GameObject panel;
    public GameObject models;
    public GameObject currInformationPanel;
    GameObject currentObject;
    public GameObject inBetweenPanel;

    public Button showUnfoundbutton;

    public GameObject lowerbound;
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

    GameObject pastCurr = null;

    bool wasSelected = false;
    int menuCount = 0;
    int menuThresh = 30;
    int finishStart = 0;
    bool shouldScroll = true;
    bool isCircular = false;
    bool didUpdateShowPanel = false;


    //after pretest for horizontal scroll snap, the animation image doesn't change, and you can't click on anythign 


    //remove all uneccesary shit to make it faster (decimate more)



    //what to do now:
    //2. you need to fix up the content
    //3. fix up maht a bit (just make sure everything works and there are no erros)
    //4. get public speakign working well ish (just make sure it doesnt crash, and improve the results a little bit)
    //5. test the fuck of eveything, make sure the file is saved, than build for andriod and ios (this will be the test app)



    //you need to add the construction thing

    //user comments:
    //get rid of the dots, make it pop more, add hilights around the things you should click
    //improve the images (backgorund images, everything in general)
    //you need to redo the material for each grade (for example, for the younger grades, you don't need to include the more advanced topics for younger grades) <-- thats a big one 



    //test construction new module




    //fix up the recycling scene 
    //this one is such a piece of shit 
    //when they click on something, thye should get immediate feedback, maybe hat it show up on the side of the screen, where it tels you that it should be recicled or not
    //it should be more clear if clicking it recycles them, or not 
    //the end panel could use a little bit of work
    //it doesn't speed up as you continue

    //for some reason waves didn't build 


    //redo fossils
    //couldnt fix dam or turbine


    //flight looks weird for the glider 



    //nuclear to the left
    //windmill down
    //dam down



    //try and get the animation working for the oil rig

    //add questions for section 1, or don't include them in the table




    //todo:

    //for water cycle, move it up a bit, and add clouds 

    //make the molecules dropdown look better 



    //67CC92


    // make current object static, and then you can reference that in model animations
    //go througth update and move all horizontal stuff there
    // start going througth and fixing some problems 

    //for this and science test, you should use quiz menu instead, init it, and then access the quiz methods, and the change color shit there



    public GameObject speechBubble;

    ModelAnimations modelAnimations;
    utilities utility;
    void Start()
    {

        utility = new utilities();
        modelAnimations = new ModelAnimations();

        finishStart = 0;


        Information.click2d = false;
        Information.isSelect = false;
        Information.isInMenu = false;
        Information.panelClosed = true;

        quiz = new Quiz();

        initModel();

        Information.panelIndex = -1;
        Information.lableIndex = 0;

        hideUnhide.onClick.AddListener(delegate { takeHideclick(); });

        Information.currentScene = "Models";

        speechBubble.GetComponent<Button>().onClick.AddListener(delegate { takeSpeechBubble(); });
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

    //just import 
    public void fancyAnimation()
    {
        outlinePanel.SetActive(true);
        StartCoroutine(modelAnimations.moveObject(false));
    }

    void addRotationAnimation()
    {
       StartCoroutine(modelAnimations.rotate(currentObject));
    }

    Table table;
    void createTable()
    {
        if (isHorizontalSnap) //then you need to hide the horizontal scroll snap shit 
        {
            horizontalSnap.SetActive(false);
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
                endQuiz();
            }
        }
        //   updateQuiz();

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


    //ye that should work 
    public void takeHelp()
    {
        takeHint();
    }





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
                shouldScroll = false;
                currentModelIndex = 9;
                break;

            case 5: //plants //not done
                Debug.LogError("here at 5");
                shouldRotate = false;
                currentModelIndex = 5;
                shouldScroll = false;
                break;
            case 11: //simple machines  not done
                shouldScroll = false;
                shouldGrow = false;
                isCircular = true;
                shouldRotate = false;
                isHorizontalSnap = true;
                currentModelIndex = 17;
                break;
            case 12: //microscope  not done
                shouldRotate = false;
                shouldScroll = false;
                currentModelIndex = 16;
                break;
            case 14: //animal cell
                shouldRotate = false;
                shouldScroll = false;
                currentModelIndex = 2;
                break;
            case 15: //plant cell
                shouldRotate = false;
                shouldScroll = false;
                currentModelIndex = 3;
                break;
            case 18: //rocks and minerals
                shouldScroll = false;
                shouldGrow = false;
                isCircular = true;
                shouldRotate = true;
                isHorizontalSnap = true;
                currentModelIndex = 0;
                didSwipe = false;
                break;
            case 19: //fossils
                shouldRotate = false;
                isHorizontalSnap = true;
                currentModelIndex = 10;
                modelOffset = 2; //because you have one after that 
                didSwipe = false;
                break;
            case 20: //greenhouse
                shouldRotate = false;
                currentModelIndex = 13;
                modelOffset = 1;
                break;

            case 21: //water cycle
                shouldRotate = false;
                shouldScroll = false;
                currentModelIndex = 7;
                break;
            case 22: //astronomy
                shouldRotate = true;
                isHorizontalSnap = true;
                currentModelIndex = 1;
                didSwipe = false;
                break;
            case 23:     //science tools not done
                isHorizontalSnap = true;
                shouldRotate = false;
                currentModelIndex = 14;
                didSwipe = false;
                break;
            case 30: //biochem
                currentModelIndex = 12;
                isHorizontalSnap = true;
                shouldRotate = false;
                didSwipe = false;
                break;
            case 31: //anatomy
                shouldScroll = false;
                shouldGrow = false;
                isCircular = true;
                shouldRotate = true;
                isHorizontalSnap = true;
                currentModelIndex = 4;
                didSwipe = false;
                break;
            case 32: //flight not done
                shouldRotate = false;
                isHorizontalSnap = true;
                currentModelIndex = 18;
                didSwipe = false;
                break;
            case 34: //climate
                shouldScroll = false;
                shouldRotate = false;
                currentModelIndex = 19;
                break;
            case 35: //photosynthesis
                shouldScroll = false;
                shouldRotate = false;
                currentModelIndex = 6;
                break;
            case 36: //food web
                shouldRotate = false;
                shouldScroll = false;
                currentModelIndex = 11;
                break;
            case 39: //weather and atmosphere 
                shouldScroll = false;
                shouldRotate = false;
                currentModelIndex = 21;
                break;
            case 42: //energy sources 
                shouldScroll = false;
                shouldGrow = false;
                isCircular = true;
                shouldRotate = false;
                isHorizontalSnap = true;
                currentModelIndex = 8;
                didSwipe = false;
                break;

            case 44: //enviornemnt, for now just make it the same as food web
                shouldScroll = false;
                shouldRotate = false;
                currentModelIndex = 20;
                break;

            case 45: //weathering
                shouldScroll = false;
                shouldRotate = false;
                currentModelIndex = 22;
                break;

            case 46: //organ systems 
                shouldScroll = false;
                shouldRotate = false;
                currentModelIndex = 23;
                break;

        }
        StartCoroutine(swipeAnimation());
        /*   didSwipe = true;
           clickCount = 1;
           neededClicks = 0;*/
        //this is all temp
    }


    void handleMenu()
    {
        if (!currInformationPanel.activeSelf && !inBetweenPanel.activeSelf) //that should work now
        {
            checkQuiz();
        }
        else
        {
            if (menuCount < menuThresh)
            {
                menuCount = menuThresh;
            }

        }
        if (menuCount > 0)
        {
            Information.currentBox = null;
            menuCount--;
            return;
        }

        if (currInformationPanel.activeSelf && Information.isInMenu)
        {
            currInformationPanel.SetActive(false);
        }
    }



    void Update()
    {
    

        if (outlinePanel.activeSelf && !currInformationPanel.activeSelf)
        {
            outlinePanel.SetActive(false);
            StartCoroutine(moveObject(true));
        }

        handleMenu();


        if (speechOffset < 41)
        {
            speechOffset++;
        }


        if (Information.currentBox != null) //&& !currInformationPanel.activeSelf)
        {

            int i = getIndex(Information.currentBox);
            if (i < 0)
            {

                return;
            }

            if(!horizontalSnap)
            {
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


    void updateQuiz()
    {

        if (Information.isCorrect)
        {
            Debug.LogError("calling because it is true");
            StartCoroutine(changeColor(true));
            Information.isCorrect = false;

            if (tookHint)
            {
                Debug.LogError("took hint is now false");
                tookHint = false;
                closeHint();
            }
        }
        else if (Information.isIncorrect)
        {
            Debug.LogError("calling because it is false");
            StartCoroutine(changeColor(false));
            Information.isIncorrect = false;
        }

        if (quiz.getQuestions() > 2 && Information.autoHide && !inTable) ///for the skip shit
        {

            createTable();
        }
        else if (quiz.getQuestions() > Information.rightCount)
        {
            endQuiz();
        }
    }


    void handleClickUpdate(int i)
    {

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



    void showPanel()
    {

        if (isHorizontalSnap)
        {
            fancyAnimation();
        }

        if (Information.currentBox != null)
        {
            Information.panelIndex = getIndex(Information.currentBox) + modelOffset;
            if (finishStart > 1)
            {
                clickCount++;
            }

        }

        currInformationPanel.SetActive(true);

    }




    void hidePanel()
    {
        currInformationPanel.SetActive(false);
    }

    public Sprite backgroundDefualt;
    public void startQuiz()
    {
        if (!Information.wasPreTest)
        {
            currInformationPanel.transform.parent.GetComponent<InformationPanel>().hintPanel.SetActive(true);
            currInformationPanel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(false);
        }


        background.GetComponent<Image>().sprite = backgroundDefualt;
        background.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        animationImage.gameObject.SetActive(false);
        //  rightPanel.gameObject.SetActive(true);
        currentModel.SetActive(true);

        //        hideUnfound(); //that should work

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
        simple.text = "Quiz started";
        simple.text = quiz.next();
    }

    public void startWrongQuiz()
    {
        Information.updateEntities = cursorInit.ToArray();

        if (quiz.startWrong())
        {
            startQuiz();
        }

    }




    void endQuiz()
    {
        Debug.LogError("quiz ended");

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
        }
        quiz.end();

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
