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
    public GameObject[] button;
    public GameObject quizObjects;

    public InformationPanel infoPanel;

  //  public Button quizButton;
    public Button notSure;

    public Sprite defualtImage;
    public Sprite[] images;

    public TMP_Text simple;

    public AudioSource source;
    public AudioClip rightAnswer;
    public AudioClip wrongAnswer;

    public int imageCount = 3;
    public int startOffset;

    public bool useImage = false;

    public Quiz quiz;


    private void Start()
    {
        quiz = new Quiz();

        notSure.onClick.AddListener(delegate { takeNotSure(); });
   //     quizButton.onClick.AddListener(delegate { quiz.startQuiz(); });

        initAnswerButtons();

        startOffset = panel.GetComponent<InformationPanel>().startOffset; // you probably need to make sure this is delayed to have it work
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

    void takeNotSure()
    {
        quiz.currentWrongCount++;
        if (quiz.totalQuestions() > Information.RIGHT_COUNT)
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

    void startQuiz()
    {

        if (!Information.wasPreTest)
        {
            infoPanel.hintPanel.SetActive(true);
            infoPanel.quizPanel.SetActive(false);
        }

        infoPanel.justTitle.gameObject.transform.parent.gameObject.SetActive(false);
        infoPanel.closePanel();
        quizObjects.SetActive(true);
        StartCoroutine(nextQuestionTimeout());
    }

    void endQuiz()
    {
       
        infoPanel.justTitle.transform.parent.gameObject.SetActive(false);

        if (Information.wasPreTest)
        {
            //infoPanel.showPostTest();
            quizObjects.SetActive(false);
            Information.wasPreTest = false;
        }
        else
        {
            inBetween.SetActive(true);
        }

        infoPanel.hintPanel.SetActive(false);
        infoPanel.quizPanel.SetActive(true);

    }

    void Update()
    {
        quiz.checkQuiz(startQuiz, endQuiz);   
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


        if (quiz.totalQuestions() >= Information.RIGHT_COUNT && Information.currentScene != "ScienceTest")
        {
            Information.isQuiz = 0;
        }

        if (correct)
        {
            quiz.currentRightCount++;
            StartCoroutine(changeColor(true));
            StartCoroutine(nextQuestionTimeout());

        }
        else
        {
            quiz.currentWrongCount++;
            StartCoroutine(changeColor(false));
        }
    }

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
        }

        yield return new WaitForSeconds(1);
        rightPanel.GetComponent<Image>().color = Information.defualtColor;
    }



    bool checkTextAnswer(Button button)
    {
        return button.GetComponentInChildren<TMPro.TMP_Text>().text == quiz.currAnswer;
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

    IEnumerator nextQuestionTimeout()
    {
        if (quiz.currentRightCount > 0)
        {
            yield return new WaitForSeconds(1.3f);
        }

        for (int i = 0; i < 3; i++)
        {
            button[i].transform.GetChild(1).gameObject.SetActive(false);
        }

        string question = quiz.getTextQuestion();
        simple.text = question;
        nextTextQuestion();
        yield break;
    }

    List<Model> getSameSection(int section)
    {
        List<Model> output = new List<Model>();
        foreach(var m in quiz.questions)
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
        included.Add(quiz.modelIndex);
        var sameSection = getSameSection(quiz.questions[quiz.modelIndex].section);
        for (int i = 0; i < 2 && i < sameSection.Count; i++)
        {

            int index = Random.Range(0, quiz.questions.Count); //did start at start offset
            while (included.Contains(index))
            {
                index = Random.Range(0, quiz.questions.Count);
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
            choices.Add(quiz.questions[included[i]].simpleInfo[0]);
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

            if (included[i] == quiz.modelIndex)
            {
                rightIndex = i;
            }

            button[i].transform.GetComponent<Image>().sprite = images[index];
        }
    }

    public void nextTextQuestion()
    {
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
        return quiz.questionIndex + quiz.modelIndex - startOffset;
    }
}
