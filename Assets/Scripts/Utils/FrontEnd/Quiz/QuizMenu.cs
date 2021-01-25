using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizMenu : MonoBehaviour
{

    public GameObject inBetween;
    public GameObject panel;
    public GameObject rightPanel;

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
  

    public GameObject[] button;
    string currAnswer;

    Quiz quiz;


    private void Start()
    {
        quiz = new Quiz();

        notSure.onClick.AddListener(delegate { takeNotSure(); });
        quizButton.onClick.AddListener(delegate { quiz.startQuiz(); });

        initAnswerButtons();
    }

    void initAnswerButtons()
    {
        Button first = button[0].GetComponent<Button>();
        first.onClick.AddListener(delegate { checkAnswer(first); });

        Button second = button[0].GetComponent<Button>();
        second.onClick.AddListener(delegate { checkAnswer(second); });

        Button third = button[0].GetComponent<Button>();
        third.onClick.AddListener(delegate { checkAnswer(third); });

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
            quiz.endQuiz();
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
        quiz.startQuiz();
        return true;
    }
    List<Model> questions;

    // these should be in the quiz class

 
    void Update()
    {
        quiz.checkQuiz();   
    }




    //you should save it to file here...

    //


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
            StartCoroutine(quiz.changeColor(true));

            if (currentRightCount + currentWrongCount > Information.rightCount && Information.currentScene != "ScienceTest")
            {
                quiz.endQuiz();
            } else
            {
                StartCoroutine(nextQuestionTimeout());
            }
        }
        else
        {
            currentWrongCount++;
            StartCoroutine(quiz.changeColor(false));
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


    int questionIndex;
    int modelIndex;


    string getTextQuestion()
    {
        string currentQuestion = "";

        modelIndex = Random.Range(0, questions.Count);
        questionIndex = Random.Range(0, questions[modelIndex].questions.Count);

        currentQuestion = questions[modelIndex].questions[questionIndex];

        return currentQuestion;
    }


    // generating text questions should also be generalized and put into the quiz class
    // then you could get rid of the labels shit that you have rn 
    // so just take half this code and put it in quiz
    // the only thing here should be to deal with image questions

    // and then re-add the hints as well



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
        nextTextQuestion();
        yield break;
        //        nextIndex = getImageIndex(currSection, currQuestion); put this somewhere
    }
    List<Model> getSameSection(int section)
    {
        List<Model> output = new List<Model>();
        foreach(var m in questions)
        {
            if(m.section == section)
            {
                output.Add(m);
            }
        }
        return output;
    }
    List<int> generateOptions()
    {
        List<int> included = new List<int>();
        included.Add(modelIndex);
        var sameSection = getSameSection(questions[modelIndex].section);
        for (int i = 0; i < 2 && i < sameSection.Count; i++)
        {

            int index = Random.Range(0, questions.Count); //did start at start offset
            while (included.Contains(index))
            {
                index = Random.Range(0, questions.Count);
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
            choices.Add(questions[included[i]].simpleInfo[0]);
        }

        choices.Shuffle();
        for (int i = 0; i < button.Length; i++)
        {
            if (i >= choices.Count)
            {
                break;
            }

            button[i].transform.GetChild(0).GetComponent<TMP_Text>().text = choices[i];
            button[i].transform.GetComponent<Image>().sprite = defualtImage;
        }
    }


    void generateImageOptions(List<int> included)
    {

        for (int i = 0; i < included.Count; i++)
        {

            int index = getImageIndex();
            if (index > images.Length - 1)
            {
                Debug.LogError(images.Length + " " + index + " error at images");
                return;
            }

            if (included[i] == modelIndex)
            {
                rightIndex = i;
            }

            button[i].transform.GetComponent<Image>().sprite = images[index];
        }
    }

    public void nextTextQuestion()
    {

        currAnswer = questions[modelIndex].simpleInfo[0];
        var included = generateOptions();

        if (useImage)
        {
            generateImageOptions(included);
        } else
        {
            generateTextOptions(included);
        }
    }

    int getImageIndex()
    {
        return questionIndex + modelIndex - startOffset;
    }
}
