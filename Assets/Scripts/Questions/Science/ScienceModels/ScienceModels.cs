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

// ok so pretest doesnt work




public class ScienceModels : MonoBehaviour
{

    public bool[] showIndecies;
    public bool showAuroa = false;

    //   public GameObject panel;
    public static GameObject currentObject;
    public static GameObject currentModel;
    public GameObject models;
    public GameObject currInformationPanel;
    public GameObject speechBubble;
    public GameObject inBetweenPanel;
    public GameObject background;

    public HorizontalModel horizontalModel;

    public Button showUnfoundbutton;
    public Button hideUnhide;

    public Sprite[] backgrounds;
    public Sprite[] hideShow;

    public TMP_Text simple;

    public ModelAnimations modelAnimations;

    public Table table;

    GameObject[] entities;

    Quiz quiz;

    Animations animations;

    bool shouldRotate;
    bool wasSelected = false;
    bool isCircular = false;
    bool shouldGrow = true;
    bool isHorizontalSnap = false;

    float selectionGrowth = 1.03f;
    int selectionLength = 5;
    int currentModelIndex;

    public GameObject outlinePanel;

    List<GameObject> dots;
    public Camera cam;

    public GameObject page1;
    List<GameObject> cursorInit;


    Hints hints;

    //that should work
    bool isHiding = false;

    void Start()
    {

        Information.click2d = false;
        Information.isSelect = false;
        Information.isInMenu = false;
        Information.panelClosed = true;

        quiz = new Quiz();

        initModel();
        if(Information.isQuiz == 0)
        {
            infoPanel.initStartPanels();
        }
        
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
            horizontalModel.gameObject.SetActive(true);
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

            currentObject = horizontalModel.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;

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

            if (isHiding)
            {
                takeHideclick();
            }
           

            Information.updateEntities = cursorInit.ToArray();
        }

        entities = cursorInit.ToArray();
        animations.init(entities);
    }



    void addRotationAnimation()
    {
        StartCoroutine(modelAnimations.rotate()); //? not sure about that
    }

    void createTable()
    {
        if (table.gameObject.activeSelf)
        {
            return;
        }

        table.gameObject.SetActive(true);

        if (isHorizontalSnap) //then you need to hide the horizontal scroll snap shit 
        {
            horizontalModel.gameObject.SetActive(false);
        }
        else
        {
            currentModel.SetActive(false);
            isHiding = false;
            takeHideclick();
        }
        showUnfoundbutton.gameObject.SetActive(false);
        speechBubble.SetActive(false);

        table.createTable(takeTableClick);
    }
    void takeTableClick(GameObject curr)
    {

        if (quiz.checkName(curr.GetComponentInChildren<TMPro.TMP_Text>().text))
        {
            //      simple.text = quiz.next();
            Debug.LogError(quiz.getQuestions() + " questions " + quiz.isQuiz);
            if (quiz.getQuestions() > Information.RIGHT_COUNT)
            {
                Information.isQuiz = 0;
                Debug.LogError("setting quiz to 0");
                //quiz.endQuiz(endQuiz);
            //    endQuiz();
            }
            quiz.getTextQuestion(nextQuestion);
            StartCoroutine(changeColor(true));
        } else
        {
            StartCoroutine(changeColor(false));
        }
     
        //   updateQuiz();

    }

    public void takeHideclick()
    {

        foreach (var g in dots)
        {
            g.SetActive(isHiding);
        }
        isHiding = !isHiding;
        if (isHiding)
        {
            hideUnhide.GetComponent<Image>().sprite = hideShow[0];
            speechBubble.SetActive(false);
        }
        else
        {
            hideUnhide.GetComponent<Image>().sprite = hideShow[1];

        }

    }

    

    //this will handle the animation of the thing to the spot in the left, and apply the filer 
    //so, what this will do is:
    //1. if a bool is true, than lerp it to the right spot, keep track of its past location, and hide the model 
    //check the size, and make sure it fits 



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
                break;
            case 20: //greenhouse
                shouldRotate = false;
                currentModelIndex = 13;
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
        if (!inBetweenPanel.activeSelf) 
        {
            quiz.checkQuiz(startQuiz, endQuiz);
        }

        if (Information.currentBox != null) //&& !currInformationPanel.activeSelf)
        {

            int i = getIndex(Information.currentBox);
            if (i < 0)
            {

                return;
            }

            if(!isHorizontalSnap)
            {
                handleClickUpdate(i);
            } else
            {

            }

            if (quiz.isQuiz && !table.gameObject.activeSelf)
            {
                updateQuiz();
            }  else 
            {
                if (!isHorizontalSnap)
                {
                    Information.lableIndex = 0;
                    simple.text = Information.userModels[i + infoPanel.startOffset].simpleInfo[0];
                }

                pastCurr = Information.currentBox;

            }

            Information.currentBox = null;

        }

        if (!isHorizontalSnap)
            animations.updateAnimations(getCurrentIndex());
    }

    void updateQuiz()
    {
        /*  if (speechOffset < 20)
          {
              speechOffset++;
          }

          if (speechBubble.activeSelf && speechOffset < 20)
          {
              return;
          }
          speechOffset = 0; // put in the onclick method of the speech bubble
        */ 

          if (Information.currentBox.transform.childCount > 0 && !isHorizontalSnap)
          {
                if (currentDot != null && currentDot.transform.parent.gameObject == Information.currentBox || isHiding)
                {
                  quiz.check(getIndex(Information.currentBox));
                }
                else
                {
                    showPopUp();
                    return;
                } // FIGURE OUT WHEN TO SHOW THE POPUP
          }
          else
          {
              speechBubble.SetActive(false);
          }
          Debug.LogError("checking: " + Information.currentBox.name);
          if (quiz.check(getIndex(Information.currentBox)))
          {
              quiz.getTextQuestion(nextQuestion);
              StartCoroutine(changeColor(true));
          } else
          {
              StartCoroutine(changeColor(false));
          }

          if(quiz.currentRightCount > 0)//Information.RIGHT_COUNT)
          {
              createTable();
          }
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

          speechBubble.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = Information.userModels[getIndex(Information.currentBox) + infoPanel.startOffset].simpleInfo[0];  //whatever it is 

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
            Information.currentBox = currentDot.transform.parent.gameObject;
            //int i = getIndex();
            //    quizQuestion(i);
          //  quiz.check(i);
           // quiz.checkName(currentDot.transform.parent.gameObject.name);
            speechOffset = 0;
        }
    }

    // here you gotta move the model, and add the black overlay 
    void handleClickUpdate(int i)
    {
        Debug.LogError("handling click update...");
        currentObject = Information.currentBox;


        if (pastCurr != null && !quiz.isQuiz && !wasSelected && Information.currentBox.transform == pastCurr.transform)
        {
            choosePanel(Information.currentBox);
            showPanel();
            showIndecies[i] = false;

            if (shouldGrow)
                animations.addAnimation2(i, Information.animationLength, false, Information.animationGrowth); //not sure about that one cheif

            if (!isCircular)
                wasSelected = true;
        }
        else
        {
            if (pastCurr != null)
            {
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



    

    public Sprite backgroundDefualt;

    // all you should do here is close the information panel if its open 
    // you also need to make sure that the panel is shown

    // you should also add a custom function for right, wrong, and next question 
    // for right and wrong, just add a change color thing
    // for next question, just show it in the panel 
    void nextQuestion(string currentQuestion)
    {
        infoPanel.justTitle.text = currentQuestion;
    }

    // ok just add this shit

    public GameObject rightPanel;
    public AudioSource source;
    public AudioClip rightAnswer;
    public AudioClip wrongAnswer;

    public IEnumerator changeColor(bool right)
    {
        rightPanel.SetActive(true);
        if (right)
        {
            source.clip = rightAnswer;
            source.Play();
            rightPanel.GetComponent<Image>().color = Information.rightColor;
        }
        else
        {
            source.clip = wrongAnswer;
            source.Play();

            rightPanel.GetComponent<Image>().color = Information.wrongColor;
        }

        yield return new WaitForSeconds(1);
        rightPanel.SetActive(false);
    }
    

// todo now:
// just go through and test every scene, make notes on what to do

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

        infoPanel.justTitle.gameObject.transform.parent.gameObject.SetActive(true); //?
        infoPanel.closePanel();
        quiz.getTextQuestion(nextQuestion);
    }

    public bool inTable = false;
    // in table you should also check to see if its already created
    void endQuiz()
    {
        infoPanel.justTitle.transform.parent.gameObject.SetActive(false);

        if (Information.wasPreTest)
        {
          //  infoPanel.show
        //    infoPanel.initStartPanels();
            initModel();
            // do something 
        } else
        {
            inBetweenPanel.SetActive(true);
        }

        currInformationPanel.transform.parent.GetComponent<InformationPanel>().hintPanel.SetActive(false);
        currInformationPanel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(true);
       
        table.gameObject.SetActive(false);

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
}
