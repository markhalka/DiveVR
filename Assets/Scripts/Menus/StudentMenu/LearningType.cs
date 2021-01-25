using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LearningType : MonoBehaviour
{
    // Start is called before the first frame update

    public Button nextBtn;
    public Button quit;

    Panel panel;

    public TMP_Text title;
    public TMP_Text[] questionText;
    public Toggle[] toggles;

    public Azure azure;

    List<int> responses;

    public GameObject askPanel;
    public GameObject surveyPanel;
    public Button continueSurvey;
    public Button quitSurvey;


    void Start()
    {
        panel = new Panel();
        nextBtn.onClick.AddListener(delegate { nextQuestion(); });
        quit.onClick.AddListener(delegate { close(); });
        continueSurvey.onClick.AddListener(delegate { takeContinue(); });
        quitSurvey.onClick.AddListener(delegate { close(); });
    }

    void takeContinue()
    {
        askPanel.SetActive(false);
        surveyPanel.SetActive(true);
        parseQuestions();
    }


    List<Model> questions;
    void parseQuestions()
    {
        
        questions = new List<Model>();
        foreach(var currQuestion in Information.loadDoc.Root.Element("LearningType").Elements())
        {
            questions.Add(ParseData.getModel(currQuestion));
        }
        responses = new List<int>();
        Debug.LogError(questions.Count + " questions count");
        questionIndex = 0;
        nextQuestion();
    }

    int questionIndex = 0;
    void nextQuestion()
    {
        for(int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                responses.Add(i);
            }
        }

        if(questionIndex >= questions.Count-1)
        {
            close();
        } else
        {

            title.text = questions[questionIndex].simpleInfo[0];
            for(int i = 0; i < questionText.Length; i++)
            {
                questionText[i].text = questions[questionIndex].advancedInfo[i];
            }
        }
        questionIndex++;
    }

    void close()
    {
        StartCoroutine(panel.panelAnimation(false, transform));
        if(responses != null && responses.Count == questions.Count)
        {
           // azure.sendAzureLearningType(); // and ya
        } else
        {
            Debug.LogError("not saving responses");
        }
        Debug.LogError("responses " + responses);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
