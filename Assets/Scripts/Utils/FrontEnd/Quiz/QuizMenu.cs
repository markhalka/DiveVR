using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizMenu : MonoBehaviour
{

    // ok, so this is the quiz that is used in database (and only database I think...?)

    // have this gameobject always active, it should be checking if it should start the quiz or not
    // it can get the images, and the current params from currLesson in database

    // this class is ugly as fuck tho fr, fix it up

    // ok, so get rid of information.right answer and all that shit 

    // ok, now handle how to start a quiz from database, and here

    public GameObject inBetween;
    public GameObject panel;
    public GameObject rightPanel;
    public Database database;

    public Button quizButton;
    public Button notSure;

    public Sprite defualtImage;
    public Sprite[] images;

    public TMP_Text simple;

    public AudioSource source;
    public AudioClip rightAnswer;
    public AudioClip wrongAnswer;

    public int currentRightCount;
    public int currentWrongCount;
    public int imageCount = 3;
    public int wrongAnswerIndex = 0;
    public int startOffset;

    public bool wrongAnswerMode = false;
    public bool useImage = false;
    bool isQuiz = false;

 
    List<string> pastQuestions;
    List<WrongAnswer> wrongAnswers;
    List<Section> section;

    public GameObject[] button;
    string currAnswer;

    utilities utility;

    private void Start()
    {
        utility = new utilities();
        notSure.onClick.AddListener(delegate { takeNotSure(); });

        // YOU NEED TO DELEGATE THE BUTTONS SOMEHOW TO CHECK THE ANSWER
    }


    void startPretest()
    {
        notSure.gameObject.SetActive(true);
    }
    void takeNotSure()
    {
        currentWrongCount++;
        if (currentWrongCount + currentRightCount > Information.rightCount)
        {
            endQuiz();
        }
        StartCoroutine(nextQuestionTimeout());
    }

    /*
    public void takeHelp()
    {
        int currentIndex = findElement();
        if (currentIndex < 0)
        {
            Debug.LogError("could not find next index for help");
            return;
        }

        Information.lableIndex = 0;
        Information.panelIndex = currentIndex;

        panel.SetActive(false);

        panel.SetActive(true); //that should do it 

    }


    int findElement()
    {
        for (int i = 0; i < Information.userModels.Count; i++)
        {
            if (Information.userModels[i].simpleInfo[0] == currAnswer)
            {
                return i;
            }
        }
        return -1;
    }*/

    public bool redoWrongAnswers()
    {
        if (wrongAnswers == null || wrongAnswers.Count == 0)
        {
            Debug.LogError("no wrong ansewrs in quiz menu");
            return false;
        }

        wrongAnswerMode = true;
        startQuiz();
        return true;
    }

    public void startQuiz()
    {
        if (Information.wasPreTest)
        {
            startPretest();
        }

        pastQuestions = new List<string>();
        if (!wrongAnswerMode)
        {
            wrongAnswers = new List<WrongAnswer>();
        }
        Time.timeScale = 1;
        currentRightCount = 0;
        currentWrongCount = 0;

        startOffset = panel.GetComponent<InformationPanel>().startOffset; //double check that, probably not good

        section = ParseData.createSections();
        StartCoroutine(nextQuestionTimeout());

        panel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(false); //again probably the wrong panel
        isQuiz = true;
        Debug.LogError("quiz started");
    }

 
    void Update()
    {
        checkQuiz();   
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

    IEnumerator changeColor(bool right)
    {
        rightPanel.gameObject.SetActive(true);
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
            if (!wrongAnswerMode)
                wrongAnswers.Add(new WrongAnswer(currSection, currQuestion, currQuestionIndex, useImage));
        }

        yield return new WaitForSeconds(1);
        rightPanel.GetComponent<Image>().color = Information.defualtColor;

    }


    public void endQuiz()
    {
        nextIndex = -1;
        wrongAnswerMode = false;

        if (!Information.isCurriculum && Information.topics != null)
        {
            float score = 0;
            if (currentRightCount + currentWrongCount > 0)
            {
                score = ((float)currentRightCount / (currentRightCount + currentWrongCount)) * 100;
                Information.score = score;
                if (Information.wasPreTest)
                {
                    Information.pretestScore = score;
                }
                else
                {
                    /*  Topic.Test currTest = new Topic.Test();
                      currTest.date = DateTime.Today.ToString("MM/dd/yyyy");
                      currTest.score = Information.score.ToString();
                      currTest.time = "0";
                      Information.topics[ParseData.getScienceScene()].tests.Add(currTest); //that should save it */
                    XMLWriter.saveMiniTest(Information.currentTopic, score, 0, false);


                }

            }
        }


        if (Information.wasPreTest)
        {
          //  gameObject.SetActive(false);

            if (quizButton != null)
                quizButton.gameObject.SetActive(true);

            Information.panelIndex = -1;
            Information.lableIndex = 0;
            panel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(true); //probably the wrong panel

            Information.isQuiz = 0;
        }
        {
            inBetween.SetActive(true);
            panel.SetActive(false);
        }
        isQuiz = false;

    }


    void checkAnswer(Button curr)
    {

        for (int i = 0; i < 3; i++)
        {
            button[i].transform.GetChild(1).gameObject.SetActive(false);
        }

        curr.transform.GetChild(1).gameObject.SetActive(true);

        bool correct = false;
        if (useImage)
        {
            correct = checkAnswerImage(curr);
        } else
        {
            correct = checkTextAnswer(curr);
        }

        if (correct)
        {
            currentRightCount++;
            StartCoroutine(changeColor(true));

            if (currentRightCount + currentWrongCount > Information.rightCount && Information.currentScene != "ScienceTest")
            {
                endQuiz();
            } else
            {
                StartCoroutine(nextQuestionTimeout());
            }
        }
        else
        {
            currentWrongCount++;
            StartCoroutine(changeColor(false));
        }
    }

    bool checkTextAnswer(Button button)
    {
        return button.GetComponentInChildren<TMPro.TMP_Text>().text == currAnswer;
    }
    int rightIndex;
    bool checkAnswerImage(Button curr)
    {
        int i = 0;
        for (i = 0; i < button.Length; i++)
        {
            if (button[i].transform == curr.transform)
            {
                break;
            }
        }

        return i == rightIndex;
    }


    // waayyyyy too much next question shit

    // change this so it returns a string, or null, at which point you can deal with the error

    void generateQuestion()
    {
        currSection = utility.getRandom(0, section.Count - 1);
        currQuestion = utility.getRandom(0, section[currSection].questions.Count - 1);
        currQuestionIndex = utility.getRandom(0, section[currSection].questions[currQuestion].questions.Count - 1);

    }
    string getTextQuestion()
    {
        generateQuestion();
        while (section[currSection].questions[currQuestion].questions.Count <= 0)
        {
            generateQuestion();
            errorCount++;
            if (errorCount > 100)
            {
                Debug.LogError("could not find any questions...");
                return null;
            }
        }

        string currentQuestion = section[currSection].questions[currQuestion].questions[currQuestionIndex];

        if (pastQuestions.Contains(currentQuestion))
        {
            getTextQuestion();
            return null;
        }

        return currentQuestion;
    }


    int errorCount = 0;
    public int nextIndex = -1;
    int currSection = 0;
    int currQuestion = 0;
    int currQuestionIndex = 0;


    IEnumerator nextQuestionTimeout()
    {
        if (currentRightCount > 0)
        {
            yield return new WaitForSeconds(1.3f);
        }

        for (int i = 0; i < 3; i++)
        {
            button[i].transform.GetChild(1).gameObject.SetActive(false);
        }

        /*    errorCount = 0;
            choices = new List<string>();
            if (wrongAnswerMode)
            {
                if (wrongAnswerIndex > wrongAnswers.Count - 1)
                {
                    endQuiz();
                    return;
                }
                var curr = wrongAnswers[wrongAnswerIndex++];
                currSection = curr.currSection;
                currQuestion = curr.currQuestion;
                currQuestionIndex = curr.currQuestionIndex;
            }
            else
            { */
        string question = getTextQuestion();
        simple.text = question;
        pastQuestions.Add(question);
        nextTextQuestion(currSection, currQuestion, useImage);
        //        nextIndex = getImageIndex(currSection, currQuestion); put this somewhere
    }

    List<int> generateOptions()
    {
        List<int> included = new List<int>();
        included.Add(currQuestion);
        int questionCount = section[currSection].questions.Count;

        for (int i = 0; i < 2 && i < questionCount; i++)
        {

            int index = utility.getRandom(0, questionCount - 1); //did start at start offset
            while (included.Contains(index))
            {
                index = utility.getRandom(0, questionCount - 1);
            }

            included.Add(index);
        }
        return included;
    }

    void generateTextOptions(List<int> included)
    {
        List<string> choices = new List<string>();

        for (int i = 0; i < included.Count; i++)
        {
            choices.Add(section[currSection].questions[included[i]].simpleInfo[0]);
        }

        choices.Shuffle();
        for (int i = 0; i < button.Length; i++)
        {
            if (i >= choices.Count)
            {
                break;
            }

            button[i].transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = choices[i];
            button[i].transform.GetComponent<Image>().sprite = defualtImage;
        }
    }


    void generateImageOptions(List<int> included)
    {

        for (int i = 0; i < included.Count; i++)
        {

            int index = getImageIndex(currSection, included[i]);
            if (index > images.Length - 1)
            {
                Debug.LogError(images.Length + " " + index + " error at images");
                return;
            }
            if (included[i] == currQuestion)
            {
                rightIndex = i;
            }

            button[i].transform.GetComponent<Image>().sprite = images[index];
        }
    }

    // so here you just generate other random choices, and thats pretty much it 
    // than just get rid of all these uselss comments, and then this class will be ok 
    //oh ya, than add the logic to start the quiz, and get rid of the information shit (iscorrect, incorrect, isquiz etc.)
    // is quiz can stay, but that should be here in the update method 

    public void nextTextQuestion(int currSection, int currQuestion, bool image)
    {
        currAnswer = section[currSection].questions[currQuestion].simpleInfo[0];
  
        var included = generateOptions();

        if (image)
        {
            generateImageOptions(included);
        } else
        {
            generateTextOptions(included);
        }
    }

    int getImageIndex(int currSection, int currQuestion)
    {
        int index = 0;
        for (int i = 0; i < currSection; i++)
        {
            for (int j = 0; j < section[i].questions.Count; j++)
            {
                if (section[i].questions[j].questions.Count > 0)
                {
                    index++;
                }
            }
        }
        for (int i = 0; i <= currQuestion; i++)
        {
            if (section[currSection].questions[i].questions.Count > 0)
            {
                index++;
            }
        }
        return index - startOffset;
    }
}
