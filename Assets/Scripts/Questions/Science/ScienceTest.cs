using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScienceTest : MonoBehaviour
{

    public GameObject multiQuiz;
    public GameObject panel;
    public GameObject table;
    public TMPro.TMP_Text simple;
    Quiz tableQuiz;
    int currWrong = 0;
    int currRight = 0;
    QuizMenu script;

    public AudioSource source;
    public AudioClip rightAnswer;
    public AudioClip wrongAnswer;

    int currentIndex = -1;
    void Start()
    {

        tableQuiz = new Quiz();
        script = multiQuiz.transform.GetComponent<QuizMenu>();
        currentIndex = -1;
        nextQuestion();
        Information.currentScene = "ScienceTest";

    }


    #region table shit
    class TableLayout
    {
        public List<int> rows;
        public TableLayout()
        {
            rows = new List<int>();
        }
    }


    TableLayout initTableRows(int n)
    {
        TableLayout output = new TableLayout();
        if (n <= 5)
        {
            for (int i = 0; i < n; i++)
            {
                output.rows.Add(1);
            }
        }
        if (n == 6 || n == 8)
        {
            int rows = n / 2;
            for (int i = 0; i < rows; i++)
            {
                output.rows.Add(2);
            }
        }
        if (n == 9 || n == 12)
        {
            int rows = n / 3;
            for (int i = 0; i < rows; i++)
            {
                output.rows.Add(3);
            }
        }
        if (n == 16)
        {
            for (int i = 0; i < 4; i++)
            {
                output.rows.Add(4);
            }
        }
        if (n == 10 || n == 14)
        {
            int largetRow = (n + 2) / 4;
            int smallerRow = largetRow - 1;
            for (int i = 0; i < 4; i++)
            {
                if (i % 2 == 0)
                {
                    output.rows.Add(largetRow);
                }
                else
                {
                    output.rows.Add(smallerRow);
                }
            }
        }
        if (n == 7)
        {
            int largetRow = 3;
            int smallerRow = 2;
            for (int i = 0; i < 3; i++)
            {
                if (i % 2 == 0)
                {
                    output.rows.Add(largetRow);
                }
                else
                {
                    output.rows.Add(smallerRow);
                }
            }
        }
        if (n == 11)
        {
            int largetRow = 3;
            int smallerRow = 2;
            for (int i = 0; i < 4; i++)
            {
                if (i >= 2)
                {
                    output.rows.Add(largetRow);
                }
                else
                {
                    output.rows.Add(smallerRow);
                }
            }
        }
        if (n == 13)
        {
            int largetRow = 4;
            int smallerRow = 3;
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || i == 2)
                {
                    output.rows.Add(largetRow);
                }
                else if (i == 1)
                {
                    output.rows.Add(smallerRow);
                }
                else
                {
                    output.rows.Add(smallerRow - 1);
                }
            }
        }
        if (n == 15)
        {
            int largetRow = 4;
            int smallerRow = 3;
            for (int i = 0; i < 4; i++)
            {
                if (i >= 1)
                {
                    output.rows.Add(largetRow);
                }
                else
                {
                    output.rows.Add(smallerRow);
                }

            }
        }
        return output;
    }


    Vector2 boxOffset = new Vector2(200, 50);

    Vector2 startOffset = new Vector2(0, -20);


    //here just replace text with tmp_text, and than replace the table in the science test scene s
    void createTable()
    {

        TableLayout layout = initTableRows(Information.userModels.Count);

        int rows = layout.rows.Count;
        //depending on the amont center it in the screen, to center it


        float yOffset = startOffset.y - (float)(rows - 1) / 2 * boxOffset.y;
        List<GameObject> newButtons = new List<GameObject>();
        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            float xOffset = startOffset.x - (float)(layout.rows[i] - 1) / 2 * boxOffset.x;
            for (int j = 0; j < layout.rows[i]; j++)
            {
                GameObject curr = Instantiate(table, table.transform, true); //make a new button
                curr.transform.SetParent(curr.transform.parent.parent);
                curr.transform.localPosition = new Vector2(xOffset + boxOffset.x * j, yOffset + boxOffset.y * i);
                if (index > Information.userModels.Count - 1)
                {
                    Debug.LogError("out of boudns for: " + index + " " + Information.placmentTest[currentIndex].name);
                }
                curr.GetComponentInChildren<TMPro.TMP_Text>().text = Information.userModels[index++].simpleInfo[0];
                curr.GetComponent<Button>().onClick.AddListener(delegate { takeTableClick(curr); });
                curr.gameObject.SetActive(true);
                newButtons.Add(curr);
            }
        }
        Information.updateEntities = newButtons.ToArray();
    }

    void takeTableClick(GameObject curr)
    {
        if (tableQuiz.checkName(curr.GetComponentInChildren<TMPro.TMP_Text>().text))
        {
            currRight++;
            StartCoroutine(changeColor(true));

        }
        else
        {
            currWrong++;
            StartCoroutine(changeColor(false));
        }
        simple.text = tableQuiz.next();
        if (tableQuiz.getQuestions() >= 2)
        {
            endTableQuiz();
        }

    }

    void endTableQuiz()
    {
        for (int i = 1; i < table.transform.parent.childCount; i++)
        {
            table.transform.parent.GetChild(i).gameObject.SetActive(false);
        }


        nextQuestion();

    }
    #endregion



    void nextQuestion()
    {


        currentIndex++;

        if (currentIndex > 0 && currentIndex < Information.placmentScore.Count)
        {
            Debug.LogError(currentIndex + " current index");
            Information.placmentScore[currentIndex - 1] = (float)currRight / (currRight + currWrong);
            Debug.LogError(Information.placmentScore[currentIndex - 1]);

        }
        else
        {
            Debug.LogError(currentIndex + " not in placment score");
        }

        currRight = 0;
        currWrong = 0;

        if (currentIndex > Information.placmentTest.Count - 1)
        {
            SceneManager.LoadScene("Curriculum");
            return;
        }



        Information.nextScene = Information.placmentTest[currentIndex].topics[0];


        //    ParseData.parseModel(Information.loadDoc); //(this will be updated because nextScene changed)
        Debug.LogError(Information.grade + " " + Information.subject + " " + Information.nextScene);
        if (!ParseData.parseModel())
        {
            Debug.LogError("it was null, calling the next question");
            nextQuestion();
        }

        if (isTableQuiz(Information.nextScene))
        {
            List<string[]> currLables = new List<string[]>();

            for (int i = 0; i < Information.userModels.Count; i++)
            {
                foreach (var question in Information.userModels[i].questions)
                {
                    currLables.Add(new string[] { question, i.ToString() });
                }
            }
            tableQuiz.initQuiz(currLables);
            simple.text = tableQuiz.next();
            createTable();

        }
        else
        {
            multiQuiz.gameObject.SetActive(true);
            nextMultiQuiz();
        }
    }

    bool isTableQuiz(int index)
    {
        bool output = false;


        switch (index)
        {
            case 1: //5 matter and mass 6 matter and mass 
                output = true;
                break;
            case 11:
            case 12:
            case 13: //nothing here           
                output = true;
                break;
            case 14: //5 animal cell 6 animal cell
                output = true;
                break;
            case 15: //5 plant cell 6 plant cell
                output = true;
                break;
            case 18: //5 rocks and minerals 6 rocks and minerals
                output = true;
                break;
            case 19: //5 fossils 6 fossils
                output = true;
                break;

            case 21: //5 water cycle 6 watercycle
                output = true;
                break;
            case 22: //5 astronomy 6 astronomy
                output = true;
                break;
            case 23: //6 Science practices and tools    //not done
                output = true;
                break;
            case 30: //6 Biochemistry
                output = true;
                break;
            case 31: //6 anatomy and physi
                output = true;
                break;
            case 32:  //genes and traits 
            case 33:
                Information.nextScene = 34;
                output = true;
                break;
            /*   case 33: //Adaptations and natural selection         //not done
                   break;*/
            case 34: //Plant reproduction    //not done
                output = true;
                break;
            case 35: //Photosynthesis    
                output = true;
                break;
            case 36: //Ecological interactions
                output = true;
                //db food web 
                break;
            case 42://5 energy sources, 6 energy sources 7 energy sources 
                output = true;
                break;

        }
        return output;
    }


    void nextMultiQuiz()
    {
        var script = multiQuiz.GetComponent<QuizMenu>();
        if (script.currentRightCount + script.currentWrongCount >= 2)
        {
            multiQuiz.gameObject.SetActive(false);

            //  Information.placmentScore[currentIndex] = script.currentRightCount;
            nextQuestion();
            return;
        }
        simple.text = script.simple.text;
    }


    void Update()
    {
        if (multiQuiz.activeSelf)
        {

            if (currWrong != script.currentWrongCount)
            {
                Debug.LogError("calling next question");
                currWrong = script.currentWrongCount;
                script.nextQuestion();

            }
            else if (currRight != script.currentRightCount)
            {
                currRight = script.currentRightCount;

            }
            nextMultiQuiz();
        }
    }



    IEnumerator changeColor(bool right)
    {
        Information.isCorrect = false;
        Information.isIncorrect = false;

        if (right)
        {
            source.clip = rightAnswer;
            panel.GetComponent<Image>().color = Information.rightColor;

        }
        else
        {
            source.clip = wrongAnswer;
            panel.GetComponent<Image>().color = Information.wrongColor;
        }
        source.Play();
        yield return new WaitForSeconds(1);


        panel.GetComponent<Image>().color = Information.defualtColor;
    }

}
