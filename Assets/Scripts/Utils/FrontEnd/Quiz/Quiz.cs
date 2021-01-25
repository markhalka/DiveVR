using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this actually looks pretty good, but you can still clean it up a bit

// get rid of the lables here, and get the questions yourself
// also, add options for section based questions? (na just keep that in quiz menu, but just get rid of the labels i guess?)

public class Quiz
{
    public List<string> pastQuestions;
    public List<string[]> wrongLabels;
    public List<string[]> lables;

    public int nextId = 0;
    private int score = 0;
    private int questions = 0;
    private int totalQuestions = 0;

    public bool isQuiz = false;
    public bool wrongAnswerMode;

    public bool startWrong()
    {
        if (wrongLabels == null || wrongLabels.Count == 0)
            return false;

        wrongAnswerMode = true;

        return true;
    }

    public void initQuiz(List<string[]> q)
    {

        isQuiz = true;
        lables = new List<string[]>();
        pastQuestions = new List<string>();
        questions = -1;
        score = 0;

        if (wrongAnswerMode)
        {
            lables = wrongLabels;
        }
        else
        {
            for (int i = 0; i < q.Count; i++)
            {
                lables.Add(q[i]);
            }
            wrongLabels = new List<string[]>();
        }

    }
    string currQuestion = "";

    public bool noRepeat()
    {
        nextId = UnityEngine.Random.Range(0, lables.Count - 1);

        string output = "";
        output = lables[nextId][0];

        if (pastQuestions.Contains(output))
        {
            return false;
        }
        pastQuestions.Add(output);
        questions++;
        currQuestion = output;
        return true;
    }


    public string next()
    {
        while (!noRepeat()) ;

        return currQuestion;

    }

    public string nextWrong(int id)
    {
        nextId = id;
        questions++;
        string output = lables[nextId][0];
        return output;
    }


    public bool check(string name)
    {
        if (name == lables[nextId][1])
        {
            score++;
            Information.isCorrect = true;
            Information.isIncorrect = false;
            return true;
        }


        Information.isCorrect = false;
        Information.isIncorrect = true;
        if (!wrongAnswerMode && !wrongLabels.Contains(lables[nextId]))
            wrongLabels.Add(lables[nextId]);
        totalQuestions++;
        return false;
    }

    public bool checkName(string name)
    {

        if (name == Information.userModels[int.Parse(lables[nextId][1])].simpleInfo[0])
        {

            score++;
            Information.isCorrect = true;
            Information.isIncorrect = false;
            return true;
        }

        Information.isCorrect = false;
        Information.isIncorrect = true;

        if (!wrongAnswerMode && !wrongLabels.Contains(lables[nextId]))
            wrongLabels.Add(lables[nextId]);

        totalQuestions++;
        return false;
    }

    public string getIndex()
    {
        return lables[nextId][1];
    }

    public int getScore()
    {
        return score;
    }

    public int getQuestions()
    {
        return questions;
    }


    // util functions 
    public IEnumerator changeColor(bool right)
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
                wrongAnswers.Add(new WrongAnswer(modelIndex, questionIndex, useImage));
        }

        yield return new WaitForSeconds(1);
        rightPanel.GetComponent<Image>().color = Information.defualtColor;
    }



    // starting things
    public void checkQuiz()
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

    public void endQuiz()
    {
        //    nextIndex = -1;
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
            //     panel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(true); //probably the wrong panel

            Information.isQuiz = 0;
        }
        {
            inBetween.SetActive(true);
            panel.SetActive(false);
        }
        isQuiz = false;

        transform.GetChild(0).gameObject.SetActive(false);
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

        currentRightCount = 0;
        currentWrongCount = 0;

        startOffset = panel.GetComponent<InformationPanel>().startOffset; //double check that, probably not good

        List<Section> section = ParseData.createSections();
        questions = new List<Model>();
        for (int i = section.Count - 1; i >= 0; i--)
        {
            for (int j = section[i].questions.Count - 1; j >= 0; j--)
            {
                if (section[i].questions[j].questions.Count > 0)
                {
                    questions.Add(section[i].questions[j]);
                }
            }
        }
        Debug.LogError(questions.Count + " # questions");
        StartCoroutine(nextQuestionTimeout());

        //   panel.transform.parent.GetComponent<InformationPanel>().quizPanel.SetActive(false); //again probably the wrong panel
        isQuiz = true;
        Debug.LogError("quiz started");
        transform.GetChild(0).gameObject.SetActive(true);
    }


}
