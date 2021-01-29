using AwesomeCharts;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// find a way to locally save goals

public class StudentInfo : MonoBehaviour
{
    public Button back;
    public Button curriculum;
    public GameObject entity;

    public GameObject warningPanel;
    public GameObject learingPlanWarning;

    public GameObject goalContainer;


    GameObject currGameObject;

    public GameObject[] arrows;

    //   public Text outputText;

    int currentBreakdown = -1;

    public Button clearButton;
    public Button saveGoalsButton;

    public TMPro.TMP_Text goalFillerText;

    public AudioSource source;
    public AudioClip buttonSound;
    public AudioClip addGoal;
    public AudioClip completeGoal;

    public TMP_Text defaultText;

    public TMP_Text title;

    void Start()
    {
        title.text = Information.grade + " " + Information.subject + " results";

        ParseData.startXML();
        back.onClick.AddListener(delegate { onBack(); });
        curriculum.onClick.AddListener(delegate { takeCuriculum(); });
        //   ConfigChart();
        loadBarGraph();

        initButtons();

        breakdownWarningOk.onClick.AddListener(delegate { takeBreakdownBack(); });

        initRadialSliders();
        readGoals(); //that should work now...

        Information.currentScene = "StudentInfo";

    }

    void initButtons()
    {
        goalContainer.GetComponent<UnityEngine.UI.Extensions.ReorderableList>().OnElementAdded.AddListener(delegate { takeNewGoal(); });
        goalContainer.GetComponent<UnityEngine.UI.Extensions.ReorderableList>().OnElementRemoved.AddListener(delegate { removeGoal(); });
        clearButton.onClick.AddListener(delegate { clearGoals(); });
        saveGoalsButton.onClick.AddListener(delegate { saveGoals(); });
        unreasonableGoal.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { closeUnreasonableGoal(); });
        learingPlanWarning.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { takeCurriculumOk(); });
        learingPlanWarning.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { takeCurriciulumBack(); });
    }

    void takeBreakdownBack()
    {
        breakdownWarning.SetActive(false);
    }

    void closeUnreasonableGoal()
    {
        source.clip = buttonSound;
        source.Play();

        unreasonableGoal.gameObject.SetActive(false);
    }




    void initRadialSliders()
    {
        for (int i = 0; i < 3; i++)
        {
            goalContainer.transform.GetChild(1 + i).GetChild(0).GetComponent<UnityEngine.UI.Extensions.RadialSlider>().onValueChanged.AddListener(delegate { radialSliderText(); });
        }

    }

    void radialSliderText()
    {
        for (int i = 0; i < 3; i++)
        {
            int value = (int)(100 * goalContainer.transform.GetChild(1 + i).GetChild(0).GetComponent<Image>().fillAmount);
            string percentage = value + "%";
            goalContainer.transform.GetChild(1 + i).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = percentage;

        }
    }


    bool movedFromGoal = false;

    void removeGoal()
    {
        source.clip = buttonSound;
        source.Play();

        movedFromGoal = true;

        for (int i = goalContainer.transform.GetChild(0).childCount - 1; i < 3 && i >= 0; i++)
        {

            goalContainer.transform.GetChild(1 + i).gameObject.SetActive(false);

        }




    }

    void takeNewGoal()
    {
        source.clip = addGoal;
        source.Play();

        goalFillerText.gameObject.SetActive(false);
        movedFromGoal = true;

        for (int i = 0; i < goalContainer.transform.GetChild(0).childCount; i++)
        {
            GameObject curr = goalContainer.transform.GetChild(0).GetChild(i).gameObject;

            if (curr.transform.childCount >= 2)
            {
                updateRadialSlider(curr, i);
            }

        }
        int newCount = 0;
        for (int i = 1; i < goalContainer.transform.childCount; i++)
        {
            if (!goalContainer.transform.GetChild(i).gameObject.activeSelf)
            {
                newCount++;
            }
        }

        if (newCount > 2)
        {

            goalFillerText.gameObject.SetActive(true);
        }

    }

    public TMPro.TMP_Text fadeText;

    void saveGoals()
    {
        source.clip = buttonSound;
        source.Play();

        string today = DateTime.Today.ToString();
        string monthFromToday = DateTime.Today.AddMonths(1).ToString();

        for (int i = 0; i < goalContainer.transform.GetChild(0).childCount && i < 3; i++)
        {
            float goalScore = goalContainer.transform.GetChild(i + 1).GetChild(0).GetComponent<UnityEngine.UI.Extensions.RadialSlider>().Value * 100;
            float currentScore = 0;
            string reasonable = "1";
            if (!float.TryParse(goalContainer.transform.GetChild(0).GetChild(i).GetChild(2).GetComponent<Text>().text, out currentScore)) //which means its na
            {
                if (goalScore < 70)
                {
                    reasonable = "0";
                }
            }
            else
            {
                int newScore = getReasonableGoal(currentScore);
                if (goalScore < newScore)
                {
                    reasonable = "0";
                }
            }

            string name = goalContainer.transform.GetChild(0).GetChild(i).GetChild(1).GetComponent<TMPro.TMP_Text>().text;

            XElement root = Information.xmlDoc.Root.Element("goals");

            Information.xmlDoc.Root.Element("goals").Add(new XElement("goal", new XAttribute("goal", name), new XAttribute("name", Information.subject),
                new XAttribute("number", Information.grade), new XAttribute("score", goalScore), new XAttribute("endDate", monthFromToday), new XAttribute("r", reasonable))); //that looks good 

            Debug.LogError(Information.xmlDoc.Root.Element("goals").ToString() + " saved");
            //radialSlider.transform.GetChild(0).
        }

        fadeText.text = "Goals Saved!";
        fadeText.gameObject.SetActive(true);

        XMLWriter.saveFile();
    }

    public GameObject certificate;
    public GameObject unreasonableGoal;
    bool areGoalsRead = false;
    void readGoals()
    {
        var goals = getGoalElements();
        if (goals == null)
        {
            Debug.LogError("could not read goals");
            return;
        }

        int index = 0;
        int goalCount = 0;

        foreach (var goal in goals)
        {

            string name = goal.Attribute("goal").Value;
            string value = goal.Attribute("score").Value;
            string r = goal.Attribute("r").Value;

            GameObject currScore = getEntityFromName(name);

            if (currScore == null)
            {
                Debug.LogError("could not find score from name");
                continue;
            }

            float currentGrade = 0;
            string temp = currScore.transform.GetChild(2).GetComponent<Text>().text;
            Debug.LogError(currScore.name + " thing name");

            float.TryParse(temp.Substring(0, temp.Length - 1), out currentGrade);


            float targetValue = float.Parse(value);
            Debug.LogError(targetValue + " target value");
            if (currentGrade >= targetValue)
            {

                goal.Remove();

                if (r == "0")
                {
                    Debug.Log("error, not reasonable goal");
                    unreasonableGoal.gameObject.SetActive(true);
                    continue;
                }
                source.clip = completeGoal;
                source.Play();

                Information.acheivment = "completing their monthly goal in " + Information.subject;
                certificate.SetActive(true);

                continue;
            }
            goalCount++;
            GameObject newGrade = Instantiate(currScore, currScore.transform, false);
            newGrade.transform.SetParent(goalContainer.transform.GetChild(0));


            updateRadialSlider(newGrade, index, targetValue);

            index++;
            if (index >= 3)
            {
                break;
            }
        }
        if (goalCount > 0)
        {
            areGoalsRead = true;
            goalContainer.GetComponent<UnityEngine.UI.Extensions.ReorderableList>().IsDraggable = false;
            goalContainer.GetComponent<UnityEngine.UI.Extensions.ReorderableList>().IsDropable = false;
            goalFillerText.gameObject.SetActive(false);
        }
    }



    GameObject getEntityFromName(string name)
    {
        for (int i = 0; i < entity.transform.parent.childCount; i++)
        {
            if (entity.transform.parent.GetChild(i).GetChild(1).GetComponent<TMPro.TMP_Text>().text == name)
            {
                return entity.transform.parent.GetChild(i).gameObject;
            }
        }

        return null;
    }
    public GameObject currentContainer;
    void clearGoals()
    {
        source.clip = buttonSound;
        source.Play();

        var goals = getGoalElements();
        foreach (var g in goals)
        {
            g.Remove();
        }
        goalFillerText.gameObject.SetActive(true);
        goalContainer.GetComponent<UnityEngine.UI.Extensions.ReorderableList>().IsDraggable = true;
        goalContainer.GetComponent<UnityEngine.UI.Extensions.ReorderableList>().IsDropable = true;
        areGoalsRead = false;

        for (int i = goalContainer.transform.GetChild(0).childCount - 1; i >= 0; i--)
        {
            goalContainer.transform.GetChild(0).GetChild(i).SetParent(currentContainer.transform.GetChild(0));
            goalContainer.transform.GetChild(i + 1).gameObject.SetActive(false);
        }
    }

    List<XElement> getGoalElements()
    {
        List<XElement> output = new List<XElement>();
        Debug.LogError(Information.xmlDoc.Descendants("goal").ToString() + " idk reading goals");
        foreach (var element in Information.xmlDoc.Descendants("goal"))
        {
            Debug.LogError(element.Name + " goal name");
            if (element.Attribute("number").Value == Information.grade && element.Attribute("name").Value == Information.subject)
            {

                output.Add(element);
            }
        }

        return output;
    }

    void updateRadialSlider(GameObject droppedObject, int index, float newScore)
    {
        GameObject radialSlider = goalContainer.transform.GetChild(1 + index).gameObject;//droppedObject.transform.GetChild(3).gameObject;
        radialSlider.SetActive(true);

        string currScore = droppedObject.transform.GetChild(2).GetComponent<Text>().text;
        if (currScore.Contains("NA"))
        {
            currScore = "70%";
        }

        Color color = new Color(0.06603771f, 1, 0.1764455f, 1);
        newScore = Mathf.Round(newScore);
        if (newScore < 50)
        {
            color = Color.red;
        }
        else if (newScore < 70)
        {
            color = new Color(1, 0.707648f, 0.1981132f, 1);
        }
        else
        {
            color = new Color(0.06603771f, 1, 0.1764455f, 1);//Color.green;
        }

        radialSlider.transform.GetChild(0).GetComponent<Image>().color = color;
        radialSlider.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = newScore + "%";
        radialSlider.transform.GetChild(0).GetComponent<UnityEngine.UI.Extensions.RadialSlider>().Value = newScore / 100;

        if (goalContainer.transform.GetChild(0).childCount > 3)
        {
            goalContainer.GetComponent<UnityEngine.UI.Extensions.ReorderableList>().IsDropable = false;
        }
        else
        {
            if (!areGoalsRead)
            {
                goalContainer.GetComponent<UnityEngine.UI.Extensions.ReorderableList>().IsDropable = true;
            }
            else
            {
                movedFromGoal = true;
            }

        }
    }

    void updateRadialSlider(GameObject droppedObject, int index)
    {
        if ((1 + index) > goalContainer.transform.childCount)
        {
            return;
        }
        GameObject radialSlider = goalContainer.transform.GetChild(1 + index).gameObject;//droppedObject.transform.GetChild(3).gameObject;
        radialSlider.SetActive(true);

        string currScore = droppedObject.transform.GetChild(2).GetComponent<Text>().text;
        if (currScore.Contains("NA"))
        {
            currScore = "70%";
        }
        float score = float.Parse(currScore.Substring(0, currScore.Length - 1));


        int newScore = getReasonableGoal(score);
        Color color = new Color(0.06603771f, 1, 0.1764455f, 1);

        if (newScore < 50)
        {
            color = Color.red;
        }
        else if (newScore < 70)
        {
            color = new Color(1, 0.707648f, 0.1981132f, 1);
        }
        else
        {
            color = new Color(0.06603771f, 1, 0.1764455f, 1);
        }

        radialSlider.transform.GetChild(0).GetComponent<Image>().color = color;


        radialSlider.transform.GetChild(0).GetComponent<UnityEngine.UI.Extensions.RadialSlider>().Value = (float)newScore / 100;
        radialSlider.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = newScore + "%";


        if (goalContainer.transform.GetChild(0).childCount > 3)
        {
            goalContainer.GetComponent<UnityEngine.UI.Extensions.ReorderableList>().IsDropable = false;
        }
        else
        {
            goalContainer.GetComponent<UnityEngine.UI.Extensions.ReorderableList>().IsDropable = true;
        }
    }

    int getReasonableGoal(float score)
    {
        int newScore = 0;
        if (score >= 90)
        {
            newScore = (int)(score * 1.05f);

        }
        else
        {
            if (score < 50)
            {
                newScore = (int)score + 20;
            }
            else
            {
                newScore = (int)(1.2f * score);
            }

        }

        if (newScore > 100)
        {
            newScore = 100;
        }
        return newScore;
    }

    void takeCuriculum()
    {
        source.clip = buttonSound;
        source.Play();

        learingPlanWarning.gameObject.SetActive(true);
    }

    void takeCurriculumOk()
    {
        source.clip = buttonSound;
        source.Play();

        if (Information.subject != "math" && Information.subject != "science")
        {
            fadeText.text = "Can't change the curriculum for the current subject";
            fadeText.gameObject.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Curriculum");
        }
    }

    void takeCurriciulumBack()
    {
        source.clip = buttonSound;
        source.Play();

        learingPlanWarning.SetActive(false);
    }



    // Update is called once per frame
    int count = 0;
    int waitTime = 5;
    void Update()
    {
        if (Information.currentBox != null) //then it was clicked
        {
            if (count < waitTime)
            {
                count++;

            }

            count = 0;
            if (movedFromGoal)
            {
                movedFromGoal = false;
                Information.currentBox = null;
                return;

            }
            else
            {
                if (userTopics[Information.currentBox].tests.Count > 0)
                    initBreakdown(userTopics[Information.currentBox]);
            }
            Information.currentBox = null;

        }

    }



    int currColor = 0;
    int maxLength = 100;

    GameObject scrollObject = null;


    Dictionary<GameObject, Topic> userTopics;
    void loadBarGraph()
    {
        userTopics = new Dictionary<GameObject, Topic>();

        List<Topic> tests = new List<Topic>();
        for (int i = 0; i < Information.topics.Count; i++)
        {
            if (Information.topics[i].isTest)
            {
                tests.Add(Information.topics[i]);
            }
            createEntity(Information.topics[i], Information.topics[i].tests.Count - 1, Information.topics[i].name);

        }

        if(Information.topics.Count == 0)
        {
            defaultText.gameObject.SetActive(true);
        }
    }

    //ok, here show another line graph, in a lighter color, which will be the pretests 

    public GameObject lineChart;
    public GameObject breakdownWarning;
    public Button breakdownWarningOk;

    void initBreakdown(Topic topic)
    {
        if (topic == null)
        {
            return;
        }

        if (topic.tests.Count < 2)
        {
            breakdownWarning.SetActive(true);
            return;
        }

        //that should work, test this tommorow
        if (Information.subject == "public speaking")
        {
            initAxis(topic);
            return;
        }

        AwesomeCharts.LineChart script = lineChart.GetComponent<AwesomeCharts.LineChart>();
        AwesomeCharts.LineData thing = new AwesomeCharts.LineData();


        LineDataSet set1 = new LineDataSet();
        LineDataSet pretest = new LineDataSet();


        set1.LineColor = new Color32(54, 105, 126, 255);
        set1.FillColor = new Color(0.4f, 1, 0.6313726f, 100);

        pretest.LineColor = new Color32(128, 114, 255, 255);
        pretest.FillColor = new Color32(175, 170, 255, 100);

        List<string> lables = new List<string>();
        int maxLables = 5;
        int lableSkip = (int)Mathf.Floor((float)topic.tests.Count / maxLables) + 1;

        for (int i = 0; i < topic.tests.Count; i++)
        {
            string date = topic.tests[i].date;
            string score = topic.tests[i].score;

            if (i % lableSkip == 0)
            {
                lables.Add(date);
            }

            if (topic.tests[i].pretestScore != null)
            {
                pretest.AddEntry(new LineEntry(i, float.Parse(topic.tests[i].pretestScore)));
            }


            set1.AddEntry(new LineEntry(i, float.Parse(score)));


        }
        script.XAxis.LinesCount = lables.Count;

        thing.CustomLabels = lables;
        thing.DataSets.Add(set1);
        thing.DataSets.Add(pretest);


        script.data = thing;
        script.SetDirty();

        source.clip = buttonSound;
        source.Play();

        lineChart.gameObject.SetActive(true);
    }


    //here set up the legend, and the x axis

    void initAxis(Topic topic)
    {
        AwesomeCharts.LineChart script = lineChart.GetComponent<AwesomeCharts.LineChart>();
        AwesomeCharts.LineData thing = new AwesomeCharts.LineData();

        int maxLables = 5;
        int lableSkip = (int)Mathf.Floor((float)topic.tests.Count / maxLables) + 1;
        List<string> lables = new List<string>();
        List<List<string>> userScores = new List<List<string>>();
        for (int i = 0; i < topic.tests.Count; i++)
        {
            if (i % lableSkip == 0)
            {
                lables.Add(topic.tests[i].date);
            }
            string[] other = topic.tests[i].other.Split('&');
            for (int j = 0; j < other.Length; j++)
            {
                if (userScores.Count < j)
                {
                    userScores.Add(new List<string>());
                }
                userScores[j].Add(other[j]);
            }
        }

        script.XAxis.LinesCount = lables.Count;

        thing.CustomLabels = lables;
        script.data = thing;
        script.SetDirty();
        lineChart.gameObject.SetActive(true);
    }


    private void ConfigChart()
    {
        var chart = lineChart.GetComponent<AwesomeCharts.LineChart>();
        chart.Config.ValueIndicatorSize = 17;

        chart.XAxis.DashedLine = true;
        chart.XAxis.LineThickness = 1;
        chart.XAxis.LabelColor = Color.white;
        chart.XAxis.LabelSize = 18;

        chart.YAxis.DashedLine = true;
        chart.YAxis.LineThickness = 1;
        chart.YAxis.LabelColor = Color.white;
        chart.YAxis.LabelSize = 16;
    }


    void createEntity(Topic topic, int i, string outerText) //the inner text is always the score 
    {

        Topic.Test lastTest;
        bool hasTest = true;
        if (i < 0)
        {
            lastTest = new Topic.Test();
            lastTest.score = "100";
            hasTest = false;
        }
        else
        {
            lastTest = topic.tests[i];
        }

        GameObject currBar = Instantiate(entity.gameObject, entity.transform, true);
        currBar.transform.SetParent(currBar.transform.parent.parent);
        currBar.SetActive(true);
        userTopics.Add(currBar, topic);
        float score = 0;
        Color color = new Color(0.06603771f, 1, 0.1764455f, 1);
        if (float.TryParse(lastTest.score, out score))
        {
            score = Mathf.Round(score);
            if (score < 50)
            {
                color = Color.red;
            }
            else if (score < 70)
            {
                color = new Color(1, 0.707648f, 0.1981132f, 1);
            }
            else
            {
                color = new Color(0.06603771f, 1, 0.1764455f, 1);//Color.green;
            }
            currBar.GetComponent<Image>().color = color;
            currBar.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = outerText;
            currBar.transform.GetChild(2).GetComponent<Text>().color = color;
            currBar.transform.GetChild(2).GetComponent<Text>().text = score.ToString() + "%";

            if (!hasTest)
            {
                currBar.GetComponent<Image>().color = Color.grey;
                currBar.transform.GetChild(2).GetComponent<Text>().text = "NA";
                currBar.transform.GetChild(2).GetComponent<Text>().color = Color.black;
            }

            float percentage = score / 100;
            float height = maxLength * percentage;
            currBar.GetComponent<Image>().fillAmount = percentage;
        }
        else
        {
            Debug.LogError("could not parse score: " + lastTest.score);
        }
    }


    void onBack()
    {
        source.clip = buttonSound;
        source.Play();

        if (lineChart.activeSelf)
        {
            lineChart.gameObject.SetActive(false);

        }
    }

}
