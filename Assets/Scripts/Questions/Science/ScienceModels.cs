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
    public GameObject upperbound;
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

    public AudioSource source;
    public AudioClip rightAnswer;
    public AudioClip wrongAnswer;
    public AudioClip swipe;

    public Sprite swipeSprite;
    public Sprite clickSprite;

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



    //hard:
    //problem in thermal energy wehre the particles always show up behind the panel (looks to happen when they only move fast, but than permenant?)
    //for the hint button, get rid of some dots, or get rid of some table cells,than, change the hint button to say get the answer '


    public GameObject speechBubble;

    utilities utility;
    void Start()
    {

        utility = new utilities();

        didSwipe = true;
        clickCount = 0;
        neededClicks = 1;

        finishStart = 0;

        UnityEngine.XR.XRSettings.enabled = false;

        Information.isVrMode = false;
        Information.click2d = false;
        Information.isSelect = false;
        Information.isInMenu = false;
        Information.panelClosed = true;

        Information.lowerBoundary = lowerbound;
        Information.upperBoundary = upperbound;


        quiz = new Quiz();

        initModel();

        Information.panelIndex = -1;
        Information.lableIndex = 0;

        hideUnhide.onClick.AddListener(delegate { takeHideclick(); });

        initStartPanels();

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

    //for this one, it hides the correct answer?
    List<int> randomNames;
    bool tookHint = false;
    bool showAnswer = false;
    void takeHint()
    {
        //get the right answer from quiz, and 3 more random dots 
        //you need to tie the dots to answers, and the answers to dots? 
        if (tookHint)
        {
            Debug.LogError("took hint is true");
            showAnswer = true;
        }
        else if (isHorizontalSnap && !inTable)
        {
            showAnswer = true;
        }
        else
        {
            showAnswer = false;
        }



        if (showAnswer)
        {
            takeShowAnswer();
            return;
        }




        tookHint = true;
        //the table one is easier, stat with that 
        randomNames = new List<int>();
        int curr = int.Parse(quiz.getIndex());

        if (inTable) //ok, so that should work
        {

            //now choose 3 more random ones
            int buttonIndex = getButtonIndexFromName(Information.userModels[curr].simpleInfo[0]);
            randomNames.Add(buttonIndex);
            for (int i = 0; i < 3; i++)
            {
                int next = utility.getRandom(1, table.transform.parent.childCount - 1); //not 0, because that is the defualt 
                while (randomNames.Contains(next))
                {
                    next = utility.getRandom(1, table.transform.parent.childCount - 1);
                }
                randomNames.Add(next);
            }



            for (int i = 0; i < table.transform.parent.childCount; i++)
            {
                if (!randomNames.Contains(i))
                {
                    table.transform.parent.GetChild(i).gameObject.SetActive(false);
                }

            }
        }
        else
        {
            int currentIndex = curr - modelOffset;
            Debug.LogError(curr + " current answer index");

            randomNames.Add(currentIndex);

            for (int i = 0; i < 3; i++)
            {
                int next = utility.getRandom(0, entities.Length - 1);
                while (randomNames.Contains(next))
                {
                    next = utility.getRandom(0, entities.Length - 1);
                }
                randomNames.Add(next);
            }

            for (int i = 0; i < entities.Length; i++)
            {
                if (!randomNames.Contains(i))
                {
                    // entities[i].SetActive(false);
                    for (int j = 0; j < entities[i].transform.childCount; j++)
                    {
                        if (entities[i].transform.GetChild(j).gameObject.name.Contains("dot"))
                        {
                            entities[i].transform.GetChild(j).gameObject.SetActive(false); //fuck
                        }
                    }
                }
            }
        }
    }

    public void takeShowAnswer()
    {
        Debug.LogError("at hint");
        if (!isQuiz)
        {
            Debug.LogError("it is not a quiz rn");
            return;
        }

        int index = int.Parse(quiz.lables[quiz.nextId][1]);

        if (isHorizontalSnap)
        {

            horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().ChangePage(index - modelOffset);
        }
        else
        {
            animations.addAnimation(index - modelOffset, Information.animationLength, true, Information.animationGrowth);
        }

        tookHint = false;

        Information.panelIndex = index;
        Information.lableIndex = 0;
        currInformationPanel.SetActive(true);
    }

    int getButtonIndexFromName(string name)
    {
        for (int i = 1; i < table.transform.parent.childCount; i++)
        {
            if (table.transform.parent.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text == name)
            {
                return i;
            }
        }
        return -1;
    }



    void closeHint()
    {
        if (inTable)
        {
            for (int i = 1; i < table.transform.parent.childCount; i++) //the first one is the defualt 
            {
                table.transform.parent.GetChild(i).gameObject.SetActive(true);
            }
            table.transform.parent.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            isHiding = true;
            takeHideclick();
        }
    }

    //this will handle the animation of the thing to the spot in the left, and apply the filer 
    //so, what this will do is:
    //1. if a bool is true, than lerp it to the right spot, keep track of its past location, and hide the model 
    //check the size, and make sure it fits 

    public Vector3 lerpPosition = new Vector3(-350, 0, 300);
    public Vector3 pastPosition;

    //  public GameObject ToMoveObject;

    public GameObject outlinePanel;
    public GameObject currentMoveObjet; //you already have this stored in a different object, but im too lazy to check the name 

    public void fancyAnimation()
    {
        if (finishStart < 1)
        {
            return;
        }

        outlinePanel.SetActive(true);



        StartCoroutine(moveObject(false));
    }

    GameObject currentMoveObject;
    IEnumerator moveObject(bool moveBack)
    {
        Debug.LogError("here");
        if (!moveBack)
        {
            currentMoveObject = Information.currentBox;
        }



        float count = 0;
        Vector3 start, end;
        if (moveBack)
        {

            start = lerpPosition;
            end = pastPosition;
            //  Debug.LogError("moving back " + currentMoveObject.transform.localPosition);

        }
        else
        {
            pastPosition = currentMoveObject.transform.localPosition;
            start = pastPosition;
            end = lerpPosition;
        }
        while (count <= 1.1)
        {
            count += 0.1f;

            currentMoveObject.transform.localPosition = Vector3.Lerp(start, end, count);

            yield return new WaitForSeconds(0.02f);

        }

        /*   if (moveBack)
           {
               currentMoveObject = null;
           } */
    }

    //ye that should work 





    public void takeHelp()
    {
        takeHint();
    }

    public void getAnswer()
    {
        Debug.LogError("at hint");
        if (!isQuiz)
        {
            Debug.LogError("it is not a quiz rn");
            return;
        }

        int index = int.Parse(quiz.lables[quiz.nextId][1]);

        if (isHorizontalSnap)
        {

            horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().ChangePage(index - modelOffset);
        }
        else
        {
            animations.addAnimation(index - modelOffset, Information.animationLength, true, Information.animationGrowth);
        }


        Information.panelIndex = index;
        Information.lableIndex = 0;
        currInformationPanel.SetActive(true);
    }

    void takeShowUnfound()
    {
        showAuroa = !showAuroa;
        if (showAuroa)
        {
            ///showUnfound();
            showUnfoundbutton.transform.GetComponentInChildren<Text>().text = "Hide unfound";
        }
        else
        {
            //  hideUnfound();
            showUnfoundbutton.transform.GetComponentInChildren<Text>().text = "Show unfound";
        }
    }

    void initStartPanels()
    {
        Debug.LogError(Information.userModels.Count + " count");
        startPanels = new List<int>();
        modelOffset = 0;
        for (int i = 0; i < Information.userModels.Count; i++)
        {
            if (Information.userModels[i].section == 1)
            {
                modelOffset++;
                startPanels.Add(i);
            }
        }
    }


    #region table shit
    class TableLayout
    {
        public List<int> rows;
        public TableLayout()
        {
            rows = new List<int>();
        }
    }


    TableLayout initTableRows(int n)
    {
        TableLayout output = new TableLayout();
        if (n <= 5)
        {
            for (int i = 0; i < n; i++)
            {
                output.rows.Add(1);
            }
        }
        if (n == 6 || n == 8)
        {
            int rows = n / 2;
            for (int i = 0; i < rows; i++)
            {
                output.rows.Add(2);
            }
        }
        if (n == 9 || n == 12)
        {
            int rows = n / 3;
            for (int i = 0; i < rows; i++)
            {
                output.rows.Add(3);
            }
        }
        if (n == 16)
        {
            for (int i = 0; i < 4; i++)
            {
                output.rows.Add(4);
            }
        }
        if (n == 10 || n == 14)
        {
            int largetRow = (n + 2) / 4;
            int smallerRow = largetRow - 1;
            for (int i = 0; i < 4; i++)
            {
                if (i % 2 == 0)
                {
                    output.rows.Add(largetRow);
                }
                else
                {
                    output.rows.Add(smallerRow);
                }
            }
        }
        if (n == 7)
        {
            int largetRow = 3;
            int smallerRow = 2;
            for (int i = 0; i < 3; i++)
            {
                if (i % 2 == 0)
                {
                    output.rows.Add(smallerRow);
                }
                else
                {
                    output.rows.Add(largetRow);
                }
            }
        }
        if (n == 11)
        {
            int largetRow = 3;
            int smallerRow = 2;
            for (int i = 0; i < 4; i++)
            {
                if (i >= 2)
                {
                    output.rows.Add(largetRow);
                }
                else
                {
                    output.rows.Add(smallerRow);
                }
            }
        }
        if (n == 13)
        {
            int largetRow = 4;
            int smallerRow = 3;
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || i == 2)
                {
                    output.rows.Add(largetRow);
                }
                else if (i == 1)
                {
                    output.rows.Add(smallerRow);
                }
                else
                {
                    output.rows.Add(smallerRow - 1);
                }
            }
        }
        if (n == 15)
        {
            int largetRow = 4;
            int smallerRow = 3;
            for (int i = 0; i < 4; i++)
            {
                if (i >= 1)
                {
                    output.rows.Add(largetRow);
                }
                else
                {
                    output.rows.Add(smallerRow);
                }

            }
        }
        return output;
    }


    public GameObject table;

    Vector2 boxOffset = new Vector2(200, 50);

    Vector2 startOffset = new Vector2(0, -20);
    bool inTable = false;
    void createTable()
    {

        //   table.transform.parent.GetChild(0).gameObject.SetActive(true); //this is the panel
        showUnfoundbutton.gameObject.SetActive(false);
        speechBubble.SetActive(false);
        TableLayout layout = initTableRows(Information.userModels.Count);

        isHiding = false;
        takeHideclick();

        int rows = layout.rows.Count;


        float yOffset = startOffset.y - (float)(rows - 1) / 2 * boxOffset.y;
        List<GameObject> newButtons = new List<GameObject>();
        int index = 0;

        for (int i = 0; i < rows; i++)
        {
            float xOffset = startOffset.x - (float)(layout.rows[i] - 1) / 2 * boxOffset.x;
            for (int j = 0; j < layout.rows[i]; j++)
            {
                GameObject curr = Instantiate(table, table.transform, true);
                curr.transform.SetParent(curr.transform.parent.parent);
                curr.transform.localPosition = new Vector2(xOffset + boxOffset.x * j, yOffset + boxOffset.y * i);

                curr.GetComponentInChildren<TMPro.TMP_Text>().text = Information.userModels[index++].simpleInfo[0];
                curr.GetComponent<Button>().onClick.AddListener(delegate { takeTableClick(curr); });
                curr.gameObject.SetActive(true);
                newButtons.Add(curr);
            }
        }

        Information.updateEntities = newButtons.ToArray();
        if (isHorizontalSnap) //then you need to hide the horizontal scroll snap shit 
        {
            horizontalSnap.SetActive(false);
        }
        else
        {
            currentModel.SetActive(false);
        }
        inTable = true;

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
    #endregion

    void addRotationAnimation()
    {

        /* if (didSwipe && clickCount >= neededClicks && !outlinePanel.activeSelf || isQuiz)
         {
             StopAllCoroutines();

         }
         */
        if (firstRotate)
            StartCoroutine(rotate());


    }

    Vector3 rotationVector = new Vector3(0, 3, 0);
    Vector3 pastRotationVector = new Vector3(-1, -1, -1);
    bool firstRotate = true;
    IEnumerator rotate()
    {

        while (currentObject != null)
        {

            if (firstRotate)
            {
                pastRotationVector = currentObject.transform.rotation.eulerAngles; //local
                firstRotate = false;
            }

            currentObject.transform.Rotate(rotationVector, Space.Self); //self
            yield return new WaitForSeconds(0.01f);
        }


        int count = 0;
        while (count < 100)
        {
            count++;
            if (Mathf.Abs(pastCurr.transform.rotation.eulerAngles.y - pastRotationVector.y) % 360 == 0)
            {
                break;
            }
            else
            {
                pastCurr.transform.Rotate(rotationVector);
                yield return new WaitForSeconds(0.01f);
            }
        }
        yield break;

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


    void pageChanged()
    {
        Debug.LogError("page changed " + shouldRotate);
        source.clip = swipe;
        source.Play();

        int currentPage = horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().CurrentPage;

        currentObject = horizontalSnap.transform.GetChild(0).GetChild(currentPage).GetChild(0).gameObject;

        if (!isQuiz)
        {
            simple.text = Information.userModels[currentPage + modelOffset].simpleInfo[0]; //-1, because the first page is just a filler value?
        }

        if (finishStart > 1)
        {
            didSwipe = true;
        }

        if (shouldRotate)
        {
            Debug.LogError("adding rotatyion animation");
            addRotationAnimation();
        }
    }


    void handleSwipeUpdate()
    {
        if (!isQuiz && !currInformationPanel.activeSelf && Information.currentBox != null)
        {
            didSwipe = true;
            clickCount = neededClicks + 1;

            showPanel();
            Information.currentBox = null;
        }
    }

    public GameObject horizontalSnap;

    GameObject currentModel;
    public GameObject particleSystem;
    public GameObject page1;
    List<GameObject> cursorInit;
    void initModel()
    {
        Debug.LogError("created new animations");
        animations = new Animations();

        ParseData.parseModel();

        setModelIndex();
        background.GetComponent<Image>().sprite = backgroundDefualt;//backgrounds[currentModelIndex];
        currentModel = models.transform.GetChild(currentModelIndex).gameObject;
        currentModel.SetActive(true);


        cursorInit = new List<GameObject>();
        if (isHorizontalSnap)
        {


            horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().OnSelectionChangeEndEvent.AddListener(delegate { pageChanged(); });
            showIndecies = new bool[currentModel.transform.GetChild(0).childCount];
            int index = 0;
            List<GameObject> toAdd = new List<GameObject>();
            for (int i = 0; i < showIndecies.Length; i++)
            {

                toAdd.Add(currentModel.transform.GetChild(0).GetChild(i).gameObject);


                showIndecies[i] = true;
            }
            if (Information.pretestScore == -1)
            {
                foreach (var child in toAdd)
                {
                    GameObject newPage = Instantiate(page1, page1.transform, false);
                    newPage.transform.SetParent(newPage.transform.parent.parent);
                    child.transform.SetParent(newPage.transform, true);

                    horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().AddChild(newPage, true);
                    cursorInit.Add(child);

                }
            }
            else
            {
                Transform curr = horizontalSnap.transform.GetChild(0);
                for (int i = 0; i < curr.childCount; i++)
                {
                    cursorInit.Add(curr.GetChild(i).GetChild(0).gameObject);
                }
            }

            currentObject = horizontalSnap.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            if (shouldRotate)
            {
                addRotationAnimation();
            }
            simple.text = Information.userModels[modelOffset].simpleInfo[0];
        }
        else
        {
            showIndecies = new bool[currentModel.transform.GetChild(0).childCount];
            for (int i = 0; i < currentModel.transform.GetChild(0).childCount; i++)
            {
                GameObject currentObject = currentModel.transform.GetChild(0).GetChild(i).gameObject;
                cursorInit.Add(currentObject);
                showIndecies[i] = true;
            }
        }

        initDots(currentModel.transform.GetChild(0).gameObject);
        cursorInit.Add(speechBubble);

        Information.updateEntities = cursorInit.ToArray();

        entities = cursorInit.ToArray();
        animations.init(entities);

        labels = new GameObject[0];

        hideAllLables();
    }


    void handleStartPanels()
    {

        if (Information.doneLoading)
        {
            SceneManager.LoadScene("ScienceMain");
        }

        if (isQuiz)
        {
            return;
        }



        if (finishStart == 1 && !currInformationPanel.transform.parent.GetComponent<InformationPanel>().closeOnEnd)
        {
            Debug.LogError("would close after this");
            currInformationPanel.transform.parent.GetComponent<InformationPanel>().closeOnEnd = true;
            //currInformationPanel.SetActive(false); //?
        }

        if (Information.panelClosed && finishStart == 0 && !isQuiz)
        {
            if (currInformationPanel.transform.parent.GetComponent<InformationPanel>().closeOnEnd && startPanels.Count > 1)
            {
                Debug.LogError("closign on end set");
                currInformationPanel.transform.parent.GetComponent<InformationPanel>().closeOnEnd = false;
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
                            simple.text = Information.userModels[startPanels[i]].simpleInfo[0];
                            Information.panelClosed = false;
                            Debug.LogError("set done panel true");
                            showPanel();
                            if (i == startPanels.Count - 1)
                            {
                                finishStart = 1;
                                //    Information.panelClosed = true;
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
                //  Information.panelClosed = true;

            }


        }

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

    public Sprite swipeLeftSprite;
    public bool didSwipe;
    int clickCount = 0;
    int neededClicks = 0;

    public Image animationImage;

    IEnumerator swipeAnimation()
    {

        animationImage.gameObject.SetActive(true);
        animationImage.sprite = swipeLeftSprite;

        int currCount = 0;
        int moveCount = 20;
        bool goingDown = false;
        int animationCount = 0;
        Vector3 translateVector = new Vector3(0.9f, 0, 0);

        while (!didSwipe)
        {
            currCount++;
            if (currCount > moveCount)
            {
                currCount = 0;
                goingDown = !goingDown;
                animationCount++;
            }
            if (goingDown)
            {
                animationImage.transform.Translate(translateVector);
            }
            else
            {
                animationImage.transform.Translate(-translateVector);
            }

            if (!isHorizontalSnap)
            {
                if (animationCount > 5)
                {
                    didSwipe = true;
                }
            }

            yield return new WaitForSeconds(0.05f);
        }

        if (clickCount >= neededClicks)
        {
            animationImage.gameObject.SetActive(false);
        }
        else
        {
            initClickAnimation();
        }
        yield break;
    }

    void initClickAnimation()
    {
        StartCoroutine(clickAnimation(null));
    }

    public Sprite doubleClickSprite;
    public IEnumerator clickAnimation(Transform button)
    {
        if (isHorizontalSnap)
        {
            animationImage.sprite = clickSprite;

        }
        else
        {
            GameObject curr = models.transform.GetChild(currentModelIndex).GetChild(0).GetChild(0).gameObject;
            GameObject dotObject = null;
            for (int i = 0; i < curr.transform.childCount; i++)
            {
                if (curr.transform.GetChild(i).gameObject.name.Contains("dot"))
                {
                    dotObject = curr.transform.GetChild(i).gameObject;
                }
            }
            animationImage.sprite = doubleClickSprite;

            if (dotObject == null)
            {
                Debug.LogError("the dot object was null");
                yield break;
            }
            Vector3 pos = Camera.main.WorldToScreenPoint(dotObject.transform.position); //the first object
                                                                                        //    pos.y += 10;
            pos.y -= 20;
            pos.x += 23;
            animationImage.transform.position = pos;
        }

        float currCount = 0;
        int moveCount = 10;
        bool goingDown = false;
        Vector3 growVector = new Vector3(1.3f, 1.3f, 1.3f);

        while (clickCount < neededClicks)
        {

            currCount++;
            if (currCount > moveCount)
            {
                currCount = 0;
                goingDown = !goingDown;
            }
            if (goingDown)
            {
                animationImage.transform.localScale = animationImage.transform.localScale * 1.06f;
            }
            else
            {
                animationImage.transform.localScale = animationImage.transform.localScale * 1 / 1.06f;
            }

            yield return new WaitForSeconds(0.05f);
        }
        if (shouldRotate)
            addRotationAnimation();

        animationImage.gameObject.SetActive(false);
        yield break;
    }





    void Update()
    {
        if (isHorizontalSnap && !didUpdateShowPanel)
        {
            didUpdateShowPanel = true;
            //  currInformationPanel.transform.parent.GetComponent<InformationPanel>().setCenter(false); 
        }

        if (outlinePanel.activeSelf && !currInformationPanel.activeSelf)
        {
            outlinePanel.SetActive(false);
            StartCoroutine(moveObject(true));
        }

        handleMenu();
        handleStartPanels();

        if (finishStart == 1 && !currInformationPanel.activeSelf && !isQuiz)
        {
            simple.text = Information.userModels[modelOffset].simpleInfo[0]; //thsi part is untested <-
            finishStart++; //that should work 
            currInformationPanel.transform.parent.GetComponent<InformationPanel>().setLeftorRight(true); //maybe??

        }

        if (speechOffset < 41)
        {
            speechOffset++;
        }

        if (isQuiz && Information.isIncorrect || Information.isCorrect)
        {
            updateQuiz(); //?
        }

        if (Information.currentBox != null) //&& !currInformationPanel.activeSelf)
        {

            int i = getIndex(Information.currentBox);
            if (i < 0)
            {

                return;
            }

            if (isHorizontalSnap)
            {
                handleSwipeUpdate();
            }
            else
            {
                handleClickUpdate(i);
            }

            if (isQuiz)
            {

                if (speechBubble.activeSelf && speechOffset < 20)
                {
                    Debug.LogError("returning");
                    return;
                }
                speechOffset = 0;
                if (Information.currentBox == speechBubble)
                {
                    Debug.LogError("speech bubble vibes");

                    i = getIndex(currentDot);
                    Debug.LogError("checking speech bubble... " + Information.userModels[i + modelOffset].simpleInfo[0] + " " + currentDot.name); //ok, you gotta check the dot thing 
                                                                                                                                                  //   speechOffset = 0;


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



                //    quizQuestion(i);
                quiz.check((i + modelOffset).ToString());

            }
            else //its already handled for horizontal snap
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

    /*   void quizQuestion(int i)
       {

           if (quiz.check((i + modelOffset).ToString()))
           {
               if (quiz.getQuestions() > 1 && Information.autoHide && !inTable) //panel is set active when the table is created 
               {

                   createTable();
               }
               if (quiz.getScore() > Information.rightCount)
               {
                   endQuiz();
               }
               //   simple.text = quiz.next();
           }
        //   updateQuiz();
       }*/

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

    // public GameObject rightPanel;


    IEnumerator changeColor(bool right)
    {

        //  rightPanel.gameObject.SetActive(true);
        if (right)
        {
            simple.text = quiz.next();
            source.clip = rightAnswer;
            source.Play();

            background.GetComponent<Image>().color = Information.rightColor;
        }
        else
        {
            if (!Information.notSure)
            {
                source.clip = wrongAnswer;
                source.Play();

                background.GetComponent<Image>().color = Information.wrongColor;
            }
            else
            {
                Debug.LogError("here");
                simple.text = quiz.next();
                Information.notSure = false;
            }

        }

        yield return new WaitForSeconds(1);




        background.GetComponent<Image>().color = Information.defualtColor;

        /*   if(!inTable)
              rightPanel.gameObject.SetActive(false);*/

    }


    //here, you need to get the position of the object, and see if you should show the left or right pnael 

    void handleClickUpdate(int i)
    {

        currentObject = Information.currentBox;


        if (pastCurr != null && !isQuiz && !wasSelected && Information.currentBox.transform == pastCurr.transform)
        {
            didSwipe = true;
            clickCount = neededClicks + 1;

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
            currInformationPanel.transform.parent.GetComponent<InformationPanel>().setLeftorRight(true);
        }
        else
        {
            currInformationPanel.transform.parent.GetComponent<InformationPanel>().setLeftorRight(false);
            //pick the left panel
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
