using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this actually looks pretty good, but you can still clean it up a bit

// get rid of the lables here, and get the questions yourself
// also, add options for section based questions? (na just keep that in quiz menu, but just get rid of the labels i guess?)

// honestly, just take the quiz menu way of coming up with questions...
public class Quiz
{

    public string currAnswer;
    string currQuestion = "";

    public int currentRightCount;
    public int currentWrongCount;
    public int questionIndex;
    public int modelIndex;

    public List<Model> questions;
    public List<string> pastQuestions;



    public bool isQuiz = false;

    public void initQuiz(List<Model> q)
    {
        isQuiz = true;
        pastQuestions = new List<string>();
        questions = q;

        currentRightCount = 0;
        currentWrongCount = 0;

        currQuestion = "";
        currAnswer = "";
    }


    public string getTextQuestion(Action<string> show = null)
    {
        string currentQuestion = "";

        modelIndex = UnityEngine.Random.Range(0, questions.Count);
        questionIndex = UnityEngine.Random.Range(0, questions[modelIndex].questions.Count);

        currentQuestion = questions[modelIndex].questions[questionIndex];

        int count = 0;

        while (pastQuestions.Contains(currentQuestion))
        {
            count++;
            if(count > 10)
            {
                break;
            }
            modelIndex = UnityEngine.Random.Range(0, questions.Count);
            questionIndex = UnityEngine.Random.Range(0, questions[modelIndex].questions.Count);
            currentQuestion = questions[modelIndex].questions[questionIndex];
        }

        pastQuestions.Add(currentQuestion);

        currAnswer = questions[modelIndex].simpleInfo[0];

        show?.Invoke(currentQuestion);
        return currentQuestion;
    }

    public bool check(int index) 
    {
        if (index == modelIndex)
        {
            currentRightCount++;
            Information.isCorrect = true;
            Information.isIncorrect = false;
            return true;
        }

        Information.isCorrect = false;
        Information.isIncorrect = true;
        currentWrongCount++;
        return false;
    } 

    public bool checkName(string name)
    {
        if (name.ToLower().Trim().Equals(currAnswer.ToLower().Trim()))
        {
            currentRightCount++;
            Information.isCorrect = true;
            Information.isIncorrect = false;
            return true;
        }
        Information.isCorrect = false;
        Information.isIncorrect = true;

        currentWrongCount++;
        return false;
    } 

  /*  public int getQuestions()
    {
        return currentRightCount;
    }*/

    public void checkQuiz(Action start = null, Action end = null)
    {
        if (!isQuiz)
        {
            if (Information.isQuiz == 1)
            {
                startQuiz(start);
            }
        }
        else
        {
            if (Information.isQuiz == 0)
            {
                endQuiz(end);
            }
        }
    }

    public void endQuiz(Action end = null)
    {
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
                      Debug.LogError("SAVING QUIZ...");
                      Topic.Test currTest = new Topic.Test();
                      currTest.date = DateTime.Today.ToString("MM/dd/yyyy");
                      currTest.score = Information.score.ToString();
                      currTest.time = "0";
                      Information.topics[ParseData.getScienceScene()].tests.Add(currTest); //that should save it 
                    XMLWriter.saveMiniTest(Information.currentTopic, score, 0, false);


                }

            }
        }

        isQuiz = false;
        Debug.LogError("ok finished, ending quiz...");
        end?.Invoke();

    }

    public int totalQuestions()
    {
        return currentRightCount + currentWrongCount;
    }

    public void startQuiz(Action start = null)
    {
        Debug.LogError("starting quiz");
        pastQuestions = new List<string>();

        currentRightCount = 0;
        currentWrongCount = 0;

        List<Section> section = ParseData.createSections();
        questions = new List<Model>();
        for (int i = 0; i < section.Count; i++)
        {
            if(section[i].section == -1)
            {
                continue;
            }

            for (int j = 0; j < section[i].questions.Count; j++)
            {
                if (section[i].questions[j].questions.Count > 0)
                {
                    questions.Add(section[i].questions[j]);
                }
            }
        }


        initQuiz(questions);


        //  Debug.LogError(questions.Count + " # questions");
        //  StartCoroutine(nextQuestionTimeout());

        //   panel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(false); //again probably the wrong panel
        isQuiz = true;


        start?.Invoke();
        //   transform.GetChild(0).gameObject.SetActive(true);
    }


}
