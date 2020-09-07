using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class MathScript : MonoBehaviour
{

    utilities utility = new utilities();


    public GameObject layoutGameOject;
    public GameObject inBetwenScene;
    public GameObject canvas;
    public GameObject cursor;
    public GameObject points;
    public Text outputText;
    public Button checkAnswer;

    public GameObject helpPanel;

    public Button reportQuestion;
    public Button help;

    public GameObject ltg;
    public List<utilities.Question> wrongAnswers;
    public bool wrongAnswerMode;
    public int wrongAnswerIndex;

    XDocument mathDoc;
    bool isTest = false;

    int currentRightCount = 0;
    int moduleCount = 0;
    public GameObject panel;
    public Text scoreCount;
    int nextSceneAmount = 1;
    int currentWrongCount = 0;
    bool isGame = false;



    private void Start()
    {
        Information.isSelect = false;
        Information.lastSubject = "math";
        Information.lastGrade = Information.grade;


        //   ParseData.checkLoad();
        ParseData.startXML();
        XMLWriter.savePastSubjectAndGrae();

        Information.isVrMode = false;
        reportQuestion.onClick.AddListener(delegate { takeReport(); });
        helpPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { takeContinue(); });
        checkAnswer.onClick.AddListener(delegate { takeCheckAnswer(); });

        if (!Information.isCurriculum)
        {
            nextSceneAmount = Information.topics[Information.nextScene].topics.Count - 1; //?
        }
        else
        {
            Information.nextScene = 0;
            checkAnswer.gameObject.SetActive(true);
            isTest = true;
        }

        StartCoroutine(nextQuestion(0));
        Information.currentScene = "Math";
        wrongAnswers = new List<utilities.Question>();
        wrongAnswerMode = false;
        wrongAnswerIndex = 0;
    }


    public bool redoWrongAnswers()
    {
        if (wrongAnswers.Count == 0)
        {
            return false;
        }
        wrongAnswerMode = true;
        StartCoroutine(startTest());
        return true;
    }


    void takeContinue()
    {
        helpPanel.SetActive(false);
    }



    void takeReport()
    {
        if (differentiator.question.Count > 0)
        {
            string reportInfo = Information.grade + "/" + Information.subject + "/" + Information.nextScene + "/" + differentiator.question[0].question + "/" + differentiator.question[0].stringAnswer + "/" + differentiator.question[0].multipleChoice;
            //   Website.GET(Information.reportUrl + reportInfo);
        }

        Information.isCorrect = true;
    }

    void handleCurriculum()
    {
        Topic currTopic = Information.placmentTest[Information.nextScene];
        currTopic.topicIndex++;

        if (currTopic.topicIndex > Information.placmentTest[Information.nextScene].topics.Count - 1)
        {
            Information.placmentScore[Information.nextScene] = currentRightCount;
            currentRightCount = 0;
            Information.nextScene++;

            if (Information.nextScene > Information.placmentTest.Count - 1)
            {
                SceneManager.LoadScene("Curriculum");
            }
            else
            {
                StartCoroutine(nextQuestion(1));
                return;
            }
        }

        utility.getQuestion(currTopic.topics[currTopic.topicIndex].ToString(), 0.5f);
        checkShape(currTopic);

        return;

    }

    void resetWrongAnswerMode()
    {
        wrongAnswerMode = false;
        wrongAnswers = new List<utilities.Question>();
    }

    void handleWrongAnswer()
    {
        if (wrongAnswerIndex > wrongAnswers.Count - 1)
        {
            wrongAnswerMode = false;
            endTest();
            return;
        }

        differentiator.question.Add(wrongAnswers[wrongAnswerIndex]);
        checkShape();

        return;
    }

    void checkShape(Topic currTopic)
    {
        if (differentiator.question[0].isShape)
        {
            ltg.GetComponent<LTG>().getShapeQuestion(currTopic.topics[currTopic.topicIndex].ToString(), 0.5f);
        }
        else
        {
            layoutGameOject.SetActive(true);
        }
    }

    void checkShape()
    {

        if (differentiator.question[0].isShape)
        {
            int topic = differentiator.question[0].topicIndex;
            ltg.GetComponent<LTG>().getShapeQuestion(topic.ToString(), 0.5f);
            return;
        }
        else
        {
            layoutGameOject.SetActive(true);
        }
    }

    //ok, so the time passed is not good, it should start everytime there is a new question, and it should end when the question is answered 


    public float totalChange = 0;
    public float maxChange = 0.2f;
    public void difficultyAdjustment(bool endSection)
    {
        if (timePassed < 1)
        {
            Debug.LogError("time passed is 0");
            return;
        }

        Debug.LogError("at difficulty adjustment...");
        Debug.LogError("the time passed is: " + timePassed);
        float levelChange = 0;
        if (timePassed > 45)
        {
            levelChange = -0.3f;
        }
        else if (timePassed > 35)
        {
            levelChange = -0.2f;
        }
        else if (timePassed > 25)
        {
            levelChange = -0.1f;
        }

        if (timePassed < 10)
        {
            levelChange = 0.2f;
        }
        else if (timePassed < 20)
        {
            levelChange = 0.1f;
        }

        if (levelChange > 1)
        {

        }
        else if (levelChange < 0)
        {

        }

        timePassed = 0;

        if (Math.Abs(levelChange + totalChange) > maxChange)
        {
            Debug.LogError("the current total change is: " + totalChange + " " + levelChange + " " + maxChange);
            return;
        }

        totalChange += levelChange;
        Information.topics[Information.nextScene].level += levelChange;
        Debug.LogError("chagned the level by: " + levelChange);

        if (!endSection)
        {
            return;
        }
        Debug.LogError("it is end section");

        if (currentRightCount + currentWrongCount > 0)
        {
            float percentageWrong = (float)currentWrongCount / (currentWrongCount + currentRightCount);
            if (percentageWrong > 0.8f)
            {
                levelChange -= 0.3f;
            }
            else if (percentageWrong > 0.7f)
            {
                levelChange -= 0.2f;
            }
            else if (percentageWrong > 0.6f)
            {
                levelChange -= 0.1f;
            }

            if (percentageWrong < 0.1f)
            {
                levelChange += 0.3f;
            }
            else if (percentageWrong < 0.2f)
            {
                levelChange += 0.2f;
            }
            else if (percentageWrong < 0.3f)
            {
                levelChange += 0.1f;
            }

        }
    }

    float timePassed = 0;

    IEnumerator nextQuestion(int wait)
    {
        yield return new WaitForSeconds(wait);
        differentiator.question = new List<utilities.Question>();

        Topic currTopic = null;


        clearPrevious();
        if (wrongAnswerMode)
        {
            handleWrongAnswer();
            yield break;
        }
        if (Information.isCurriculum)
        {

            handleCurriculum();
            yield break;
        }
        else
        {
            currTopic = Information.topics[Information.nextScene];
        }


        scoreCount.text = currentRightCount.ToString() + "/" + (nextSceneAmount + 1);
        if (currTopic.isTest) //you need to double check this part 
        {
            isTest = true;
            StartCoroutine(startTest());
        }

        if (isTest)
        {
            currTopic.topicIndex = utility.getRandom(0, currTopic.topics.Count - 1);
        }
        else
        {
            currTopic.topicIndex++;
            totalChange = 0;
            if (currTopic.topicIndex > currTopic.topics.Count - 1)
            {

                if (Information.nextScene > Information.topics.Count - 1)
                {
                    //you've reached the end of that grade 
                    SceneManager.LoadScene("OpenModule");
                }
                else
                {
                    currTopic.topicIndex = utility.getRandom(0, currTopic.topics.Count - 1);
                    isTest = true;
                    StartCoroutine(startTest());
                }
            }


        }
        difficultyAdjustment(false);
        currentTopic = currTopic.topics[currTopic.topicIndex];
        Debug.LogError(currTopic.level + " curerent level");
        utility.getQuestion(currTopic.topics[currTopic.topicIndex].ToString(), currTopic.level); //not actually 100% sure about this one 
        //i thnk that should work 


        checkShape();

    }
    int currentTopic = 0;

    bool isQuiz = false;
    void checkQuiz()
    {
        if (!isQuiz)
        {
            if (Information.isQuiz == 1)
            {
                isTest = true;
                StartCoroutine(startTest());
                isQuiz = true;
            }

        }
        else
        {
            if (Information.isQuiz == 0)
            {
                endTest();
                isQuiz = false;
            }
        }
    }

    bool checkAnswerClick = false;
    void takeCheckAnswer()
    {
        checkAnswerClick = true;
    }

    private void Update()
    {

        if (!Information.isInMenu)
        {
            checkQuiz();
            timePassed += Time.deltaTime;
        }

        if (Information.isCurriculum)
        {
            if (!checkAnswerClick)
            {
                Debug.Log("returning...");
                return;
            }
            else
            {
                checkAnswerClick = false;
            }

        }

        if (Information.isCorrect)
        {
            currentRightCount++;

            if (!Information.isCurriculum && currentRightCount > nextSceneAmount)
            {

                inBetwenScene.transform.GetChild(7).GetComponent<TMPro.TMP_Text>().text = "The next module is about to start";
                if (isTest)
                {
                    endTest();
                    if (moduleCount > 0)
                    {

                        isGame = true;
                        Information.isGame = true;
                        inBetwenScene.transform.GetChild(7).GetComponent<TMPro.TMP_Text>().text = "Good work! Time for a game";

                    }
                    else
                    {
                        //      inBetwenScene.transform.GetChild(4).GetComponent<Text>().text = "Get ready for the next module!";
                        if (Information.nextScene + 1 < Information.topics.Count)
                        {
                            inBetwenScene.transform.GetChild(7).GetComponent<TMPro.TMP_Text>().text = "Next up: " + Information.topics[Information.nextScene].name;
                        }
                    }
                }
                else
                {
                    if (wrongAnswerMode)
                    {
                        resetWrongAnswerMode();
                        //    inBetwenScene.transform.GetChild(4).GetComponent<Text>().text = "Get ready for the next module!";
                        if (Information.nextScene + 1 < Information.topics.Count)
                        {
                            inBetwenScene.transform.GetChild(7).GetComponent<TMPro.TMP_Text>().text = "Next up: " + Information.topics[Information.nextScene].name;
                        }
                    }
                    else
                    {
                        //   inBetwenScene.transform.GetChild(4).GetComponent<Text>().text = "Get ready for the test!";
                    }
                }


                StartCoroutine(startInBetween());

            }
            else
            {
                StartCoroutine(changeColor(true));
                StartCoroutine(nextQuestion(1));
            }

            Information.isCorrect = false;


        }
        else if (Information.isIncorrect)
        {
            if (!wrongAnswerMode && !wrongAnswers.Contains(differentiator.question[0]))
            {
                wrongAnswers.Add(differentiator.question[0]);
            }

            StartCoroutine(changeColor(false));

            if (!isTest)
            {

            }
            else
            {
                currentWrongCount++;
            }

            if (Information.isCurriculum)
            {

                StartCoroutine(nextQuestion(1));
            }

            Information.isIncorrect = false;
        }

        if (Information.retry)
        {
            if (Information.topics[Information.nextScene].topicIndex == -1)
            {
                Information.nextScene--;
                nextSceneAmount = Information.topics[Information.nextScene].topics.Count - 1;
            }
            currentRightCount = 0;
            Information.topics[Information.nextScene].topicIndex = -1; //?
            inBetwenScene.SetActive(false);
            Information.retry = false;
            Information.doneLoading = false;
            StartCoroutine(nextQuestion(0));
            return;
        }

        if (Information.doneLoading)
        {
            Information.doneLoading = false;
            currentRightCount = 0;
            inBetwenScene.SetActive(false);
            moduleCount++;
            if (isGame && !Information.skip)
            {
                if (utility.toss())
                {
                    SceneManager.LoadScene("DiveGame");//dive game
                }
                else
                {
                    SceneManager.LoadScene("FourOpsGame");
                }

                moduleCount = 0;
                isGame = false;
            }
            else
            {
                StartCoroutine(nextQuestion(0));
                Information.skip = false;
                if (isGame)
                {

                    moduleCount = 0;
                    isGame = false;
                }
            }

        }
    }

    IEnumerator startInBetween()
    {
        StartCoroutine(changeColor(true));
        yield return new WaitForSeconds(1);
        inBetwenScene.SetActive(true);
        yield break;

    }

    public void clearPrevious()
    {

        layoutGameOject.GetComponent<MathLayout>().clearPrevious();
        if (differentiator.question.Count > 0 && differentiator.question[0].isShape)
        {
            ltg.GetComponent<LTG>().clearPrevious();

        }

        layoutGameOject.SetActive(false);
    }


    int time = 300;
    IEnumerator startTest()
    {
        difficultyAdjustment(true);
        time = 300;
        while (time > 0 && isTest)
        {
            time--;
            displayClock(time);
            yield return new WaitForSeconds(1);
        }
        if (isTest)
        {
            scoreCount.text = "Times up!";
            endTest();
        }

    }

    void displayClock(int seconds)
    {
        int minutes = seconds / 60;
        int sec = seconds % 60;
        string secStr = "";
        if (sec < 10)
        {
            secStr = "0";
        }
        secStr += sec;
        scoreCount.text = minutes + ":" + secStr;
    }

    void endTest()
    {
        difficultyAdjustment(true);

        if (!wrongAnswerMode)
        {


            Information.score = (currentRightCount / (float)(currentRightCount + currentWrongCount)) * 100;
            Topic.Test currTest = new Topic.Test();
            currTest.date = DateTime.Today.ToString("MM/dd/yyy");
            currTest.score = Information.score.ToString();
            currTest.time = (300 - time).ToString();

            Information.topics[Information.nextScene].tests.Add(currTest); //that should save it 
        }

        currentWrongCount = 0;
        currentRightCount = 0;

        isTest = false;

        Information.nextScene++;
        nextSceneAmount = Information.topics[Information.nextScene].topics.Count - 1;
    }


    IEnumerator changeColor(bool right)
    {

        if (right)
        {
            panel.GetComponent<Image>().color = Information.rightColor;
        }
        else
        {
            panel.GetComponent<Image>().color = Information.wrongColor;
        }

        yield return new WaitForSeconds(1);


        panel.GetComponent<Image>().color = Information.defualtColor;
    }



}
