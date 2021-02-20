using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoQuiz : MonoBehaviour
{
    // Start is called before the first frame update

    // its just literally the table 

    public GameObject rightPanel;
    public GameObject inBetweenPanel;

    public InformationPanel infoPanel;

    public AudioSource source;
    public AudioClip rightAnswer;
    public AudioClip wrongAnswer;

    public bool inTable = false;

    public Table table;

    Quiz quiz;
    void Start()
    {
        quiz = new Quiz();
        ParseData.parseModel(); // think that should work
        Information.isQuiz = 1;
        createTable();
    }

    // Update is called once per frame

    void createTable()
    {
        Debug.LogError(Information.userModels.Count + " user models count...");

        table.gameObject.SetActive(true);

        table.createTable(takeTableClick);
    }


    void takeTableClick(GameObject curr)
    {

        if (quiz.checkName(curr.GetComponentInChildren<TMPro.TMP_Text>().text))
        {
            if(quiz.totalQuestions() > Information.RIGHT_COUNT)
            {
                Information.isQuiz = 0;
            }
            quiz.getTextQuestion(nextQuestion);
            StartCoroutine(changeColor(true));
        }
        else
        {
            StartCoroutine(changeColor(false));
        }
    }

    public IEnumerator changeColor(bool right)
    {
        rightPanel.SetActive(true);
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
        rightPanel.SetActive(false);
    }


    void startQuiz()
    {
        infoPanel.gameObject.SetActive(true);
        if (!Information.wasPreTest)
        {
            infoPanel.hintPanel.SetActive(true);
            infoPanel.quizPanel.SetActive(false);
        }
        Debug.LogError("showing title...");
        infoPanel.justTitle.gameObject.transform.parent.gameObject.SetActive(true); //?
        infoPanel.closePanel();
        quiz.getTextQuestion(nextQuestion);
    }


    // in table you should also check to see if its already created
    void endQuiz()
    {
        infoPanel.justTitle.transform.parent.gameObject.SetActive(false);

        inBetweenPanel.SetActive(true);

        infoPanel.hintPanel.SetActive(false);
        infoPanel.quizPanel.SetActive(true);

        table.gameObject.SetActive(false);

        inTable = false;
    }



    // just literally include table shit....

    void Update()
    {
        if (!inBetweenPanel.activeSelf)
        {
            quiz.checkQuiz(startQuiz, endQuiz);
        }
    }

    void nextQuestion(string currentQuestion)
    {
        infoPanel.justTitle.text = currentQuestion;
    }


}
