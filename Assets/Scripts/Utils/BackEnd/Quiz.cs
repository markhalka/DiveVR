using System.Collections.Generic;

public class Quiz
{

    public List<string[]> lables;
    public int nextId = 0;
    private int score = 0;
    private int questions = 0;
    private int totalQuestions = 0;
    public bool isQuiz = false;
    public List<string[]> wrongLabels;
    public bool wrongAnswerMode;
    public List<string> pastQuestions;


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

    public int getTotalQuestions()
    {
        return questions + totalQuestions;
    }

    public void end()
    {
        /* if(!Information.isCurriculum && questions > 0)
         {

             Information.score = ((float)score / totalQuestions) * 100;

             Topic.Test currTest = new Topic.Test();
             currTest.date = DateTime.Today.ToString("MM/dd/yyyy");
             currTest.score = Information.score.ToString();
             currTest.time = "0";

             Information.topics[ParseData.getScienceScene()].tests.Add(currTest); //that should save it 
         }

         score = 0;
         questions = 0;
         nextId = 0; */
        isQuiz = false;
    }


}
