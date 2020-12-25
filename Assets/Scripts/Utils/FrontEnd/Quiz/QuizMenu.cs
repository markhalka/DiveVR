using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizMenu : MonoBehaviour
{

    //here, if it is a pretest, than make sure you just write to the variable
    //have a button at the top for the not sure, make sure that is handled properly
    //in each individual scene you need to handle the end test differently

    /*
     *         <quiz>This is a theory that continents drift and move slowly over time</quiz>
    <quiz>This theory states that the outer layer of Earths crust is broken into plates</quiz>
    <quiz>This theory explains why earthquakes and volcanoes happen</quiz>
    */


    GameObject[] button;
    string currAnswer;
    List<string> choices;
    public int currentRightCount;
    public int currentWrongCount;
    List<Section> section;
    utilities utility;
    public TMP_Text simple;
    public GameObject inBetween;
    public Sprite[] images;
    public Sprite defualtImage;
    public bool useImage = false;
    public int imageCount = 3;
    public int startOffset = 0;
    List<WrongAnswer> wrongAnswers;
    public bool wrongAnswerMode = false;
    public int wrongAnswerIndex = 0;

    public AudioSource source;
    public AudioClip rightAnswer;
    public AudioClip wrongAnswer;
    public Button quizButton;

    public Button notSure;


    List<string> pastQuestions;


    void OnEnable()
    {
        utility = new utilities();
        button = new GameObject[transform.GetChild(1).childCount];
        for (int i = 0; i < button.Length; i++)
        {
            button[i] = transform.GetChild(1).GetChild(i).gameObject;
        }
        notSure.onClick.AddListener(delegate { takeNotSure(); });
        startQuiz();
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
        nextQuestion(); //?
    }



    public GameObject panel;
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
    }


    public class WrongAnswer
    {
        public int currSection;
        public int currQuestion;
        public int currQuestionIndex;
        public bool useImage;

        public WrongAnswer(int cs, int cq, int cqi, bool ui)
        {
            currSection = cs;
            currQuestion = cq;
            currQuestionIndex = cqi;
            useImage = ui;
        }
    }

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

        section = ParseData.createSections();
        createButtons();
        nextQuestion();
        Debug.LogError("quiz started");
    }

    public GameObject rightPanel;
    void Update()
    {
        if (rightPanel == null)
            return;

        if (Information.isCorrect)
        {
            StartCoroutine(changeColor(true));
            Information.isCorrect = false;
        }
        else if (Information.isIncorrect)
        {
            StartCoroutine(changeColor(false));
            Information.isIncorrect = false;
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

        for (int i = 0; i < userButtons.Count; i++)
        {
            userButtons[i].gameObject.SetActive(false);
        }
        Debug.LogError("at closing");
        if (Information.wasPreTest)
        {
            Debug.LogError("at the right closing");
            //  panel.SetActive(false);
            gameObject.SetActive(false);

            if (quizButton != null)
                quizButton.gameObject.SetActive(true);

            Information.isQuiz = 0;
        }
        else if (!Information.isCurriculum)
        {
            Debug.LogError("heree4re??");
            inBetween.SetActive(true);
            panel.SetActive(false);
        }


    }

    List<Button> userButtons;
    void createButtons()
    {
        userButtons = new List<Button>();
        for (int i = 0; i < button.Length; i++)
        {
            button[i].SetActive(true);
            Button curr = button[i].GetComponent<Button>();
            curr.onClick.AddListener(delegate { checkAnswer(curr); });
            userButtons.Add(curr);
        }
    }

    // int delay = 10;
    //here add the hilight
    void checkAnswer(Button curr)
    {
        if (isNextQuestiontimeout)
        {
            Debug.LogError("next question timeout");
            return;
        }
        for (int i = 0; i < 3; i++)
        {
            button[i].transform.GetChild(1).gameObject.SetActive(false);
        }

        curr.transform.GetChild(1).gameObject.SetActive(true);
        Debug.LogError("checking button...");
        if (Information.isCorrect || Information.isIncorrect) //because it hasnt updated yet 
        {
            Debug.LogError("not updated yet " + Information.isCorrect + " " + Information.isIncorrect);
            return;
        }
        if (useImage)
        {
            checkAnswerImage(curr);
            return;
        }
        if (curr.GetComponentInChildren<TMPro.TMP_Text>().text == currAnswer)
        {
            currentRightCount++;
            Information.isCorrect = true;
            Information.isIncorrect = false;
            if (currentRightCount + currentWrongCount > Information.rightCount && Information.currentScene != "ScienceTest")
            {
                endQuiz();
            }
            nextQuestion();
        }
        else
        {
            Information.isCorrect = false;
            Information.isIncorrect = true;
            currentWrongCount++;
        }
    }
    int rightIndex;
    void checkAnswerImage(Button curr)
    {
        int i = 0;
        for (i = 0; i < button.Length; i++)
        {
            if (button[i].transform == curr.transform)
            {
                break;
            }
        }
        //ok, so now you have the index
        if (i == rightIndex)
        {
            Information.isCorrect = true;
            Information.isIncorrect = false;
            currentRightCount++;
            if (currentRightCount + currentWrongCount > Information.rightCount)
            {
                endQuiz();
            }
            nextQuestion();
        }
        else
        {
            Information.isCorrect = false;
            Information.isIncorrect = true;
            currentWrongCount++;
        }
    }

    void getNextQuestion()
    {
        currSection = utility.getRandom(0, section.Count - 1);
        //in ecosystems there is an error here 
        if (section[currSection].questions == null)
        {
            nextQuestion();
            return;
        }
        currQuestion = utility.getRandom(0, section[currSection].questions.Count - 1);
        currQuestionIndex = utility.getRandom(0, section[currSection].questions[currQuestion].questions.Count - 1);
    }


    int errorCount = 0;
    public int nextIndex = -1;
    int currSection = 0;
    int currQuestion = 0;
    int currQuestionIndex = 0;

    public void nextQuestion()
    {
        StartCoroutine(nextQuestionTimeout());
    }
    bool isNextQuestiontimeout = false;
    IEnumerator nextQuestionTimeout()
    {
        isNextQuestiontimeout = true;
        if (currentRightCount > 0)
        {
            yield return new WaitForSeconds(1.3f);
        }

        for (int i = 0; i < 3; i++)
        {
            button[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        isNextQuestiontimeout = false;
        handleNextQuestion();
        yield break;
        //than hide all the hilgihts 

    }
    public void handleNextQuestion()
    {
        errorCount = 0;
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
        {
            getNextQuestion();



            while (section[currSection].questions[currQuestion].questions.Count <= 0)
            {
                getNextQuestion();
                errorCount++;
                if (errorCount > 100)
                {
                    return;
                }
            }

        }



        string currentQuestion = section[currSection].questions[currQuestion].questions[currQuestionIndex];
        if (pastQuestions.Contains(currentQuestion))
        {
            nextQuestion();
            return;
        }
        nextIndex = getImageIndex(currSection, currQuestion);
        simple.text = currentQuestion;
        pastQuestions.Add(currentQuestion);


        if (useImage && currentRightCount < imageCount)
        {
            Debug.LogError("next image question");
            //so at this point use images 
            nextTextQuestion(currSection, currQuestion, true);
        }
        else
        {
            Debug.LogError("next text question");
            //just do what youve been doing 
            useImage = false;
            nextTextQuestion(currSection, currQuestion, false);
        }

    }

    public void nextTextQuestion(int currSection, int currQuestion, bool image)
    {
        currAnswer = section[currSection].questions[currQuestion].simpleInfo[0];
        List<Sprite> imageChoices = new List<Sprite>();


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

        for (int i = 0; i < included.Count; i++)
        {
            if (image)
            {

                int index = getImageIndex(currSection, included[i]);
                if (index > images.Length - 1)
                {
                    Debug.LogError(images.Length + " " + index + " error at images");
                    return;
                }
                imageChoices.Add(images[index]);

            }
            else
            {
                choices.Add(section[currSection].questions[included[i]].simpleInfo[0]);

            }



        }


        if (image)
        {
            Debug.LogError(imageChoices.Count + " total images " + images.Length);
            for (int i = 0; i < button.Length; i++)
            {
                if (i >= imageChoices.Count)
                {
                    break;
                }
                if (included[i] == currQuestion) //so basically you just find which index button has the right answer, then you can check if its right or not 
                {
                    rightIndex = i;
                }
                button[i].transform.GetComponent<Image>().sprite = imageChoices[i];
            }
        }
        else
        {
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
    }

    int getImageIndex(int currSection, int currQuestion)
    {
        Debug.LogError(currSection + " " + currQuestion);
        int index = 0;
        for (int i = 0; i < currSection; i++)
        {
            for (int j = 0; j < section[i].questions.Count; j++)
            {
                Debug.LogError(i + " " + j + " " + section[i].questions[j].questions.Count);
                if (section[i].questions[j].questions.Count > 0)
                {
                    index++;
                }
                //  index += section[i].questions[j].questions.Count;
            }
            //   index += section[i].questions.Count;
        }
        for (int i = 0; i <= currQuestion; i++)
        {
            if (section[currSection].questions[i].questions.Count > 0)
            {
                index++;
            }
            //  index += section[currSection].questions[i].questions.Count;
        }
        //     index += currQuestion;
        return index - startOffset;
    }


}
