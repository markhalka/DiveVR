using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Database : MonoBehaviour
{

    public GameObject quiz;
    public Button button;
    public GameObject[] arrows;
    public GameObject lowerBound;
    public GameObject upperBound;
    public GameObject panel;

    Scroll scroll;

    public Sprite[] scientificNames;
    public Sprite[] ecosystemNames;
    public Sprite[] tectonicNames;

    public Sprite swipeUpSprite;
    public Sprite swipeLeftSprite;
    public Sprite clickSprite;

    public Sprite[] classificationNames;
    int[] classificationIndecies = new int[] { 0, 2, 5, 8, 11, 12, 14, 14 };

    public Sprite[] lifecycleNames;
    int[] lifecycleIndecies = new int[] { 0, 3, 7, 10, 14 };

    public AudioSource source;
    public AudioClip swipe;
    public AudioClip click;

    public bool didSwipe;
    public bool swipeUp;
    public int clickCount;
    public int neededClicks;

    public GameObject verticalScroll;


    void Start()
    {
        quiz.GetComponent<QuizMenu>().startOffset = 1;
        Information.currentScene = "Database";

    }

    void OnEnable()
    {
        panel.transform.parent.gameObject.SetActive(true); // that should work 
        clickCount = 0;
        didSwipe = true;
        swipeUp = true;
        neededClicks = 1;

        Information.lowerBoundary = lowerBound;
        Information.upperBoundary = upperBound;
        scroll = new Scroll();
        scroll.arrows = arrows;
        scroll.lockUp = false;

        finishStart = 0;

        ParseData.parseModel();

        Information.panelIndex = -1;
        Information.lableIndex = 0;

        scrollParent = null;


        initStartPanels();
        initDatabase();
        initButtons();
        initFunctions();
    }

    #region startPanel shit 
    List<int> startPanels;
    int finishStart = 0;
    void initStartPanels()
    {
        startPanels = new List<int>();
        startOffset = 0;
        for (int i = 0; i < Information.userModels.Count; i++)
        {
            if (Information.userModels[i].section == 1)
            {
                Debug.LogError("added start panel");
                startOffset++;
                startPanels.Add(i);
            }
        }
    }

    public Vector2 lerpPosition = new Vector3(-350, 0); //change this 
    public Vector2 pastPosition;

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

            currentMoveObject.transform.localPosition = Vector2.Lerp(start, end, count);

            yield return new WaitForSeconds(0.02f);

        }

    }


    //ye that should work 


    void handleStartPanels()
    {
        if (Information.doneLoading)
        {
            SceneManager.LoadScene("ScienceMain");
        }
        if (finishStart == 1 && !panel.transform.parent.GetComponent<InformationPanel>().closeOnEnd)
        {
            Debug.LogError("would close after this");
            panel.transform.parent.GetComponent<InformationPanel>().closeOnEnd = true;
        }
        if (Information.panelClosed && finishStart == 0 && !isQuiz)
        {
            if (panel.transform.parent.GetComponent<InformationPanel>().closeOnEnd && startPanels.Count > 1)
            {
                Debug.LogError("closign on end set");
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
                            //    simple.text = Information.userModels[startPanels[i]].simpleInfo[0];
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
    #endregion


    Sprite[] currentSprites;
    string[] currentNames;


    bool isHorizontalSnap = false;
    void initDatabase()
    {
        switch (Information.nextScene)
        {
            case 8: //classifcation
                didSwipe = false;
                swipeUp = true;
                neededClicks = 1;
                initClassificationLadder();
                break;
            case 9: //scientific names
                didSwipe = false;
                swipeUp = false;
                neededClicks = 1;
                isHorizontalSnap = true;
                ScientificNamesLadder();
                break;
            case 10:
                didSwipe = false;
                swipeUp = true;
                neededClicks = 2;
                initAnimalLifeCycle();
                neededClicks = 2;
                break;

            case 16: //this is ecosystem,
                didSwipe = false;
                swipeUp = false;
                isHorizontalSnap = true;
                //     ecosystemLadder(); 
                break;
            case 38:
                didSwipe = false;
                swipeUp = false;
                isHorizontalSnap = true;
                tectonicLadder();
                break;

            case 41:
                didSwipe = false;
                swipeUp = false;
                isHorizontalSnap = true;
                identifyEcosystemLadder();
                break;

        }
        StartCoroutine(swipeAnimation());
    }

    List<GameObject> userButtons;

    Vector2 offsetAmount = new Vector2(1.5f, 0);
    public Canvas currentCanvas;

    public GameObject page1;
    public GameObject horizontalSnap;
    public TMP_Text test;

    void createHS()
    {
        horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().OnSelectionChangeEndEvent.AddListener(delegate { pageChanged(); });
        for (int i = 0; i < currentSprites.Length; i++)
        {
            GameObject newPage = Instantiate(page1, page1.transform, false);
            newPage.transform.SetParent(horizontalSnap.transform.GetChild(0));
            newPage.transform.GetChild(0).GetComponent<Image>().sprite = currentSprites[i];
            newPage.gameObject.SetActive(true);
        }

        pageChanged();
    }


    void pageChanged()
    {
        Debug.LogError("here at pange changed");




        source.clip = swipe;
        source.Play();

        int currentPage = horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().CurrentPage;

        if (currentPage > 0)
        {
            Debug.LogError("did swipe...");
            didSwipe = true;
        }


        test.text = Information.userModels[currentPage + startOffset].simpleInfo[0];

    }

    void onHorizontalClick()
    {

        if (Information.currentBox != null)
        {
            source.clip = click;
            source.Play();
            showHSPanel();
            Information.currentBox = null;
        }
    }

    void initQuiz()
    {
        if (currentSprites != null)
        {
            Debug.LogError("added the current sprites ");
            quiz.GetComponent<QuizMenu>().images = currentSprites;
            quiz.GetComponent<QuizMenu>().useImage = true;
        }
        else
        {
            quiz.GetComponent<QuizMenu>().useImage = false;
        }
        quiz.GetComponent<QuizMenu>().startOffset = startOffset;

    }


    void initButtons()
    {

        userButtons = new List<GameObject>();
        Vector2 offset = new Vector2(0, 0);
        if (currentSprites != null || currentNames != null)
        {
            initQuiz();

            if (isHorizontalSnap)
            {
                createHS();
                return;
            }
        }
    }


    void initFunctions()
    {
        switch (Information.nextScene)
        {
            case 8:
                shouldCustomUpdate = true;
                break;
            case 10:
                shouldCustomUpdate = true;
                break;
        }
    }

    int startOffset = -1;
    int currentButtonIndex = -1;
    bool showFromClick = false;
    void takeClick(GameObject curr)
    {
        getIndex(curr);
        if (currentButtonIndex < 0)
            return;
        if (showFromClick)
        {
            Information.panelIndex = currentButtonIndex + startOffset;
            showPanel();
        }

    }

    void getIndex(GameObject curr)
    {
        int i = 0;
        for (i = 0; i < userButtons.Count; i++)
        {
            if (userButtons[i].transform == curr.transform)
            {
                break;
            }
        }
        if (i == userButtons.Count)
        {
            currentButtonIndex = -1;
            return;
        }
        currentButtonIndex = i;

    }

    void showHSPanel()
    {
        clickCount++;

        int currentIndex = -1;
        for (int i = 0; i < horizontalSnap.transform.GetChild(0).childCount; i++)
        {
            if (horizontalSnap.transform.GetChild(0).GetChild(i).GetChild(0).gameObject == Information.currentBox)
            {
                currentIndex = i;
                break;
            }
        }
        if (currentIndex == -1)
        {
            Debug.LogError("could not find the index for: " + Information.currentBox.name);
            return;
        }

        fancyAnimation();

        Information.panelIndex = currentIndex + startOffset;
        showPanel();
    }



    void showPanel()
    {
        if (finishStart > 1)
        {
            didSwipe = true;
            clickCount = neededClicks + 1; //this will get rid of the helping animations 
        }
        else if (finishStart > 0)
        {
            clickCount++;
        }

        Debug.LogError("showing informationp  panel");
        panel.SetActive(true);
    }


    void startQuiz()
    {
        panel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(false);
        animationImage.gameObject.SetActive(false);
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
            panel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(true);
            return;
        }


        quiz.GetComponent<QuizMenu>().endQuiz();
        Debug.LogError("quiz ended");

    }

    public void ClassificationLadder()
    {
        currentNames = new string[7];
        showFromClick = true;
        for (int i = startOffset; i < 9; i++)
        {
            currentNames[i - startOffset] = Information.userModels[i].simpleInfo[0];
        }

    }

    public GameObject vsChild;

    void initClassificationLadder()
    {
        ClassificationLadder();

        for (int i = 0; i < currentNames.Length; i++)
        {

            GameObject currChild = Instantiate(vsChild, vsChild.transform, false);
            currChild.transform.SetParent(verticalScroll.transform.GetChild(0));

            currChild.gameObject.SetActive(true);
            GameObject page = currChild.transform.GetChild(0).GetChild(0).gameObject;
            currChild.transform.GetChild(1).GetComponent<Text>().text = currentNames[i];
            currChild.transform.GetChild(0).GetComponent<Image>().color = Information.colors[i % Information.colors.Length];

            GameObject image = page.transform.GetChild(0).GetChild(0).gameObject;

            for (int j = classificationIndecies[i]; j < classificationNames.Length; j++)
            {
                GameObject currAnimal = Instantiate(image, image.transform, true);
                currAnimal.transform.SetParent(image.transform.parent.parent.GetChild(0));
                currAnimal.GetComponent<Image>().sprite = classificationNames[j];    //ok, that should work 
                currAnimal.gameObject.SetActive(true);
            }
        }
    }



    public void ScientificNamesLadder()
    {
        showFromClick = true;
        currentSprites = scientificNames;

    }


    public void animalLifecycle()
    {
        shouldScroll = false;
        currentSprites = new Sprite[lifecycleIndecies.Length - 1];
        for (int i = 0; i < lifecycleIndecies.Length - 1; i++)
        {
            currentSprites[i] = lifecycleNames[lifecycleIndecies[i]];
        }
    }


    void initAnimalLifeCycle()
    {

        animalLifecycle();
        for (int i = 0; i < currentSprites.Length; i++)
        {
            GameObject currChild = Instantiate(vsChild, vsChild.transform, false);
            currChild.transform.SetParent(verticalScroll.transform.GetChild(0)); //i think that should work
            currChild.gameObject.SetActive(true);
            GameObject page = currChild.transform.GetChild(0).GetChild(0).gameObject;
            currChild.transform.GetChild(0).GetComponent<Image>().sprite = currentSprites[i];

            GameObject image = page.transform.GetChild(0).GetChild(0).gameObject;
            for (int j = lifecycleIndecies[i] + 1; j < lifecycleIndecies[i + 1]; j++)
            {

                GameObject currAnimal = Instantiate(image, image.transform, true);
                currAnimal.transform.SetParent(image.transform.parent.parent.GetChild(0));
                currAnimal.GetComponent<Image>().sprite = lifecycleNames[j];   //ok, that should work               
                currAnimal.gameObject.SetActive(true);

            }
        }
        redoAnimalSprites();
    }

    public void redoAnimalSprites()
    {
        currentSprites = lifecycleNames;
    }


    public void getAllAnimalSprites()
    {
        currentSprites = lifecycleNames;
    }


    void animalPartClick(int mainAnimal, int animalPart)
    {
        Information.panelIndex = lifecycleIndecies[mainAnimal] + animalPart + startOffset;
        showPanel();
        return;

    }



    public void tectonicLadder()
    {
        showFromClick = true;
        currentSprites = tectonicNames;
    }


    public void identifyEcosystemLadder()
    {
        showFromClick = true;
        scroll.lockUp = false;
        currentSprites = ecosystemNames;
    }
    bool shouldScroll = true;
    GameObject scrollParent;

    bool shouldCustomUpdate = false;


    public Image animationImage;

    IEnumerator swipeAnimation()
    {
        animationImage.gameObject.SetActive(true);
        animationImage.sprite = swipeUpSprite;

        int currCount = 0;
        int moveCount = 20;
        bool goingDown = false;
        int animationCount = 0;
        Vector3 translateVector = new Vector3(0, 0.9f, 0);
        if (!swipeUp)
        {
            translateVector = new Vector3(0.9f, 0, 0);
            animationImage.sprite = swipeLeftSprite;
        }

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
        Transform nextClick = null;
        switch (Information.nextScene)
        {
            case 8: //classifcation

                break;
            case 9: //scientific names

                break;
            case 10: //animal life cycle

                break;

            case 16: //this is ecosystem, so teach them aboitic and biotic and shit like that 

                break;
            case 38: //tectonic

                break;

            case 41: //ecosystem ladder
                break;

        }
        StartCoroutine(clickAnimation(nextClick));
    }


    public IEnumerator clickAnimation(Transform button)
    {
        animationImage.sprite = clickSprite;
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

        animationImage.gameObject.SetActive(false);
        yield break;
    }

    void Update()
    {

        if (finishStart == 1 && !panel.activeSelf)
        {
            if (isHorizontalSnap)
            {
                panel.transform.parent.GetComponent<InformationPanel>().setLeftorRight(true); //maybe?? //that should work 
                pageChanged();
            }
            else
            {
                //  panel.transform.parent.GetChild(1).GetComponent<TMPro.TMP_Text>().text = ""; 
            }


            finishStart++;
        }

        if (outlinePanel.activeSelf && !panel.activeSelf)
        {
            outlinePanel.SetActive(false);
            StartCoroutine(moveObject(true));
        }



        if (!panel.activeSelf)
        {
            checkQuiz();
        }

        handleStartPanels();

        if (Information.doneLoading)
        {
            SceneManager.LoadScene("ScienceMain");
        }

        if (shouldCustomUpdate)
        {
            customUpdate();
            return;
        }

        if (isHorizontalSnap)
        {
            onHorizontalClick();
        }

        if (!isQuiz)
        {
            if (!isHorizontalSnap && shouldScroll && scrollParent != null)
            {
                scroll.moveSubButtonsScroll(scrollParent.transform);
            }
        }
    }

    void customUpdate()
    {
        switch (Information.nextScene)
        {
            case 8: //classification
                classificationUpdate();
                break;
            case 10: //animals
                animalUpdate();
                break;
        }
    }

    void classificationUpdate()
    {
        if (Information.currentBox != null)
        {
            for (int i = 0; i < verticalScroll.transform.GetChild(0).childCount; i++)
            {
                Transform image = verticalScroll.transform.GetChild(0).GetChild(i).GetChild(0);
                if (image.gameObject == Information.currentBox)
                {
                    image.GetChild(0).gameObject.SetActive(true);
                    Information.panelIndex = startOffset + i - 1;
                    showPanel();
                }
                else
                {
                    image.GetChild(0).gameObject.SetActive(false);
                }
            }
            Information.currentBox = null;
        }
    }


    void animalUpdate()
    {
        if (Information.currentBox != null)
        {
            for (int i = 0; i < verticalScroll.transform.GetChild(0).childCount; i++)
            {
                GameObject curr = verticalScroll.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(0).gameObject;
                if (curr.gameObject.activeSelf)
                {
                    for (int j = 0; j < curr.transform.GetChild(0).childCount; j++)
                    {
                        if (Information.currentBox == curr.transform.GetChild(0).GetChild(j).gameObject)
                        {
                            animalPartClick(i - 1, j);
                            Information.currentBox = null;
                            return;
                        }
                    }
                }
                Transform image = verticalScroll.transform.GetChild(0).GetChild(i).GetChild(0);
                if (image.gameObject == Information.currentBox)
                {
                    image.GetChild(0).gameObject.SetActive(true);
                    Information.panelIndex = lifecycleIndecies[i - 1] + startOffset;
                    showPanel();
                }
                else
                {
                    image.GetChild(0).gameObject.SetActive(false);
                }
            }

            Information.currentBox = null;
        }
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
