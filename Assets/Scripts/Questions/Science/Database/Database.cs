using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Database : MonoBehaviour
{

    public GameObject quiz;
    public GameObject startPanel;
    public GameObject verticalScroll;

    public Sprite[] scientificNamesSprite;
    public Sprite[] ecosystemNames;
    public Sprite[] tectonicNames;

    public AudioSource source;
    public AudioClip swipe;
    public AudioClip click;


    void Start()
    {
        quiz.GetComponent<QuizMenu>().startOffset = 1;
        Information.currentScene = "Database";
    }

    void OnEnable()
    {
        startPanel.transform.parent.gameObject.SetActive(true); // this should handle the start panels

        userButtons = new List<GameObject>();
        Vector2 offset = new Vector2(0, 0);

        //     finishStart = 0;

        ParseData.parseModel();

        Information.panelIndex = -1;
        Information.lableIndex = 0;

        initDatabase();
    }

    //this is the panel gameobject


    string[] currentNames;


    bool isHorizontalSnap = false;

    LessonDb currLesson;

    int tempStart = 0; // figure out how to add this 
    public GameObject InstructionAnimationGb;
    
    void initDatabase()
    {
        switch (Information.nextScene)
        {
            case 8: //classifcation
                currLesson = new ClassificationDb(startOffset);
                break;
            case 9: //scientific names      
                currLesson = new ScientificNamesDb(startOffset);
                break;
            case 10:
                currLesson = new animalLifeCycleDb(startOffset);
                break;
            case 38:
                currLesson = new tectonicDb(startOffset);
                break;

            case 41:
                currLesson = new identifyEcosystemDb(startOffset);
                break;

        }
        var instructionAnimation = InstructionAnimationGb.GetComponent<InstructionAnimation>();
        StartCoroutine(instructionAnimation.swipeAnimation());
    }


    public abstract class LessonDb
    {
        public GameObject vsChild;
        public GameObject verticalScroll;
        public GameObject panelGb;

        public Panel panel;

        public Sprite[] currentSprites;
        public string[] currentNames;

        public int startOffset;


        public LessonDb(int startOffset)
        {
            this.startOffset = startOffset;
        }

        public virtual void initLadder()
        {

        }

        public virtual void update()
        {

        }
    }

    public class ClassificationDb : LessonDb
    {
        public Sprite[] classificationNames;
        int[] classificationIndecies = new int[] { 0, 2, 5, 8, 11, 12, 14, 14 };
        public ClassificationDb(int startOffset) : base(startOffset)
        {
            currentNames = new string[7];
            for (int i = startOffset; i < 9; i++)
            {
                currentNames[i - startOffset] = Information.userModels[i].simpleInfo[0];
            }

            initLadder();
        }

       public override void initLadder()
        {
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

        public override void update()
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
                        panel.showPanel();
                    }
                    else
                    {
                        image.GetChild(0).gameObject.SetActive(false);
                    }
                }
                Information.currentBox = null;
            }
        }
    }

    public class animalLifeCycleDb : LessonDb
    {
       public Sprite[] lifecycleNames;
        int[] lifecycleIndecies = new int[] { 0, 3, 7, 10, 14 };

        public animalLifeCycleDb(int startOffset) : base(startOffset)
        {

        }
        public void animalLifecycle()
        {
            currentSprites = new Sprite[lifecycleIndecies.Length - 1];
            for (int i = 0; i < lifecycleIndecies.Length - 1; i++)
            {
                currentSprites[i] = lifecycleNames[lifecycleIndecies[i]];
            }
        }


        public override void initLadder()
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


        public override void update()
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
                        panel.showPanel();
                    }
                    else
                    {
                        image.GetChild(0).gameObject.SetActive(false);
                    }
                }

                Information.currentBox = null;
            }
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
            panel.showPanel();
            return;

        }

    }

    public class tectonicDb : LessonDb
    {
        Sprite[] tectonicNames; //YOU NEED TO INIT THIS (just make it a general class)
        public tectonicDb(int startOffset) : base(startOffset)
        {
         //   showFromClick = true;
            currentSprites = tectonicNames;
        }


    }

    public class identifyEcosystemDb : LessonDb
    {
        Sprite[] ecosystemNames;
        public identifyEcosystemDb(int startOffset) : base(startOffset)
        {
            currentSprites = ecosystemNames;
        }
    }

    public class ScientificNamesDb : LessonDb
    {
        Sprite[] scientificNamesSprites;
        public ScientificNamesDb(int startOffset) : base(startOffset)
        {
            currentSprites = scientificNamesSprites;
        }

    }

    // this is the horizontal snap gameobject 
    public class HorizontalSnap : MonoBehaviour
    {

        public AudioSource source;
        public AudioClip swipe;
        public AudioClip click;
        public GameObject instructionsGb;
        public TMP_Text test;
        Sprite[] currentSprites; // MAKE SURE YOU INIT THIS 
        public GameObject page1;
        bool didSwipe = false;
        int startOffset;

        public GameObject panelGb;
        public Panel panel;


        public void Start()
        {
            // find a way to init Startoffset
            panel = panelGb.GetComponent<Panel>();
        }
        void createHS()
        {
            GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().OnSelectionChangeEndEvent.AddListener(delegate { pageChanged(); });
            for (int i = 0; i < currentSprites.Length; i++)
            {
                GameObject newPage = Instantiate(page1, page1.transform, false);
                newPage.transform.SetParent(transform.GetChild(0));
                newPage.transform.GetChild(0).GetComponent<Image>().sprite = currentSprites[i];
                newPage.gameObject.SetActive(true);
            }

            pageChanged();
        }

        void pageChanged()
        {
            source.clip = swipe;
            source.Play();

            int currentPage = GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().CurrentPage;

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

        void showHSPanel()
        {
            int currentIndex = -1;
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                if (transform.GetChild(0).GetChild(i).GetChild(0).gameObject == Information.currentBox)
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

           // fancyAnimation();

            Information.panelIndex = currentIndex + startOffset;
            panel.showPanel();
        }

        bool setPanel = false;
        public void Update()
        {
            if (!setPanel)
            {
             //   panel.transform.parent.GetComponent<InformationPanel>().setLeftorRight(true); //maybe?? //that should work 
                pageChanged();
            }
            onHorizontalClick();
        }
    }

    List<GameObject> userButtons;

    Vector2 offsetAmount = new Vector2(1.5f, 0);
    public Canvas currentCanvas;

    public GameObject page1;
    public GameObject horizontalSnap;


    int startOffset = -1;
    int currentButtonIndex = -1;

  /*  void showPanel()
    {

        Debug.LogError("showing informationp  panel");
        panel.SetActive(true);
    }

    #region replace with in quiz
    void startQuiz()
    {
        panel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(false);
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

    void initQuiz(Sprite[] currentSprites)
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

    #endregion



    public GameObject vsChild;



    void Update()
    {
        if (!panel.activeSelf)
        {
            checkQuiz();
        }


        if (Information.doneLoading)
        {
            SceneManager.LoadScene("ScienceMain");
        }

        currLesson.update();
    } */
}
