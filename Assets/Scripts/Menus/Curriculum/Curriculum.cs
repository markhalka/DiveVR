using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Curriculum : MonoBehaviour
{

    public GameObject currentItem;
    public GameObject toAddItem;
    public Button redo;
    public Button back;
    public TMP_Text title;

    public AudioSource source;
    public AudioClip buttonSound;
    public AudioClip sort;
    public XMLReader reader;

    //two problems with this:
    //after the placment test, have some sort of panel that shays good job or something
    //the first thing is skipped (it never does matter and mass for some reason)



    //fix curriculum
    //1. maeke sure the title works (its tmp text now)
    //2. change the the button in the list itself
    //3. make sure the title is accurate

    LearningPlan learningPlan;



    void Start()
    {

        reader = new XMLReader();
        utility = new utilities();
      
        addListeners();
        ParseData.startXML();
        if (Information.subject == "science")
        {
            ParseData.parseModel();
        }
        getNames();
        getCurrentGradeDefualt();
        learningPlan = new LearningPlan(currGradeDefualt, learningPlanUpdated);
        if (Information.isCurriculum)
        {
            learningPlan.createNewLearningPlan();
            return;
        }


        createCurrent();
        Information.currentScene = "Curriculum";
        title.text = Information.grade + " " + Information.subject + " Learning Plan"; //check to see this works 
        back.onClick.AddListener(delegate { takeBack(); });
        saveOrDont.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { takeSave(); });
        saveOrDont.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { takeDontSave(); });

        if (Information.shouldRedo)
        {

            takeRedo();
        }
    }

    public GameObject learningPlanUpdated;


    public Button learningPlanOk;

    bool wasDragging = false;
    void addListeners()
    {
        currentItem.transform.parent.parent.GetComponent<UnityEngine.UI.Extensions.ReorderableList>().OnElementDropped.AddListener(delegate { cancelClick(); });
        rightPanel.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { saveDifficultyToFile(); });
        learningPlanOk.onClick.AddListener(delegate { takeLearningPlanOk(); });

        redo.onClick.AddListener(delegate { takeRedo(); });
    }

    void takeLearningPlanOk()
    {
        learningPlanUpdated.SetActive(false);
        SceneManager.LoadScene("Curriculum");
    }

    void cancelClick()
    {
        source.clip = sort;
        source.Play();
        wasDragging = true;
    }

    void checkClick(GameObject curr)
    {

        if (!wasDragging)
        {
            takeClick(userLessons[curr]);

        }
        wasDragging = false;
    }


    List<Topic> currGradeDefualt;
    Dictionary<int, string> names;

    void getNames()
    {
        var values = Information.loadDoc.Root.Element("curr").Element(Information.subject).Elements("lesson");
        names = new Dictionary<int, string>();
        foreach (var value in values)
        {
            names.Add(int.Parse(value.Attribute("topics").Value), value.Attribute("name").Value);
        }
    }


    XElement currentElement;
    void getCurrentGradeDefualt()
    {
        var grade = reader.findGrade(Information.loadDoc, Information.grade);
        var subject = reader.findSubject(grade, Information.subject);

        currentElement = subject;

        currGradeDefualt = new List<Topic>();

        ParseData.generateTopicsFromLesson(currentElement, currGradeDefualt);

    }
    public class Lesson
    {
        public Topic topic;
        public int index;
        public bool added;

        public Lesson(Topic t, int i, bool a)
        {
            topic = t;
            index = i;
            added = a;
        }
    }

    public Dictionary<GameObject, Lesson> userLessons;

    void createCurrent()
    {

        userLessons = new Dictionary<GameObject, Lesson>();
        List<string> addedNames = new List<string>();
        for (int i = 0; i < Information.topics.Count; i++)
        {

            if (Information.topics[i].isTest)
            {
                continue; 
            }

            var currTopics = Information.topics[i].topics;
            var currColor = Information.colors[i % Information.colors.Length];

         
            for (int j = 0; j < currTopics.Count; j++)
            {
                if (currTopics[j] < 0)
                {
                    continue; //if the index is less than 0 don't add it 
                }
                GameObject newValue = Instantiate(currentItem, currentItem.transform, true);
                newValue.transform.SetParent(newValue.transform.parent.parent);
                newValue.transform.GetComponent<Image>().color = currColor;
                try
                {
                    newValue.transform.GetComponentInChildren<TMP_Text>().text = names[currTopics[j]];
                }
                catch
                {
                    Debug.LogError(currTopics[j] + " was not found");
                }
                newValue.gameObject.SetActive(true);
                userLessons.Add(newValue, new Lesson(Information.topics[i], j, true));
            }


            foreach (var currDefualt in currGradeDefualt)
            {
                if (currDefualt.name == Information.topics[i].name)
                {
                    //ok, now find all the ones that are not included, and add them
                    addedNames.Add(currDefualt.name);
                    for (int j = 0; j < currDefualt.topics.Count; j++)
                    {
                        if (!currTopics.Contains(currDefualt.topics[j]))
                        {
                            add(currColor, currDefualt, j);
                        }
                    }

                }
            }
        }

        int index = Information.topics.Count;
        foreach (var leftOver in currGradeDefualt)
        {
            if (!addedNames.Contains(leftOver.name))
            {
                if (leftOver.isTest)
                {
                    continue; //again, irnoring the tests
                }

                index++;
                var currColor = Information.colors[index % Information.colors.Length];
                for (int i = 0; i < leftOver.topics.Count; i++)
                {
                    add(currColor, leftOver, i);
                }
            }
        }
    }

    void add(Color currColor, Topic topic, int index)
    {
        GameObject newValue = Instantiate(toAddItem, toAddItem.transform, true);
        newValue.transform.SetParent(newValue.transform.parent.parent);
        newValue.transform.GetComponent<Image>().color = currColor;
        newValue.transform.GetComponentInChildren<TMP_Text>().text = names[topic.topics[index]];
        newValue.gameObject.SetActive(true);
        userLessons.Add(newValue, new Lesson(topic, index, false));

    }


    void saveOrderToFile()
    {
        var grade = reader.findGrade(Information.xmlDoc, Information.grade);
        var currLesson = reader.findSubject(grade, Information.subject);

        currLesson.Add(new XAttribute("name", Information.subject));

        for (int i = 0; i < currentItem.transform.parent.childCount; i++)
        {

            GameObject currItem = currentItem.transform.parent.GetChild(i).gameObject;
            if (!currItem.activeSelf)
                continue;

            int level = (int)(userLessons[currItem].topic.level * 10); //there was an error here, it didfnt find the current item
            if (level <= 0)
            {
                level = 1;
            }
            Lesson tempLesson = userLessons[currItem];
            userLessons[currItem].topic.topicIndex++;
            string name = tempLesson.topic.name;
            if (userLessons[currItem].topic.topicIndex > 0) //then there are multiple 
            {
                name += " (" + userLessons[currItem].topic.topicIndex + ")";
            }
            XElement currTopic = new XElement("lesson", new XAttribute("name", name), new XAttribute("index", tempLesson.topic.index), new XAttribute("topics", tempLesson.topic.topics[userLessons[currItem].topic.topicIndex]),
                new XAttribute("score", ""), new XAttribute("level", level));


            int addedAmount = 0;
            for (int j = i + 1; j < currentItem.transform.parent.childCount; j++)
            {
                if (!currentItem.transform.parent.GetChild(j).gameObject.activeSelf)
                    continue;

                if (userLessons[currentItem.transform.parent.GetChild(j).gameObject].topic.name != userLessons[currItem].topic.name)
                {
                    break;
                }
                addedAmount++;
                Lesson curr = userLessons[currentItem.transform.parent.GetChild(j).gameObject];
                currTopic.Attribute("topics").Value += "," + curr.topic.topics[++userLessons[currentItem.transform.parent.GetChild(j).gameObject].topic.topicIndex]; //maybe?

            }
            currLesson.Add(currTopic);
            i += addedAmount;

        }
        XMLWriter.saveFile();
        Information.grade = "";
        Information.subject = "";
        Information.topics = null; //to avoid that studpid fucking saving shit 
    }


    void saveDifficultyToFile()
    {

        source.clip = buttonSound;
        source.Play();


        if (currentLesson == null || !currentLesson.added)
        {

            return;
        }

        int value = (int)rightPanel.transform.GetChild(2).GetComponent<Slider>().value;
        currentLesson.topic.level = (float)value / 10;
    }


    public GameObject rightPanel;
    utilities utility;
    Lesson currentLesson;

    void takeClick(Lesson lesson)
    {

        source.clip = buttonSound;
        source.Play();

        bool add = lesson.added;
        currentLesson = lesson;
        string name = names[lesson.topic.topics[lesson.index]];

        int difficulty = (int)(lesson.topic.level * 10);

        string sampleProblem = "";
        if (Information.subject == "math")
        {
            utility.getQuestion(lesson.topic.topics[lesson.index].ToString(), 0.5f);
            sampleProblem = differentiator.question[0].question;
        }
        else
        {
            Information.nextScene = lesson.topic.topics[0];
            ParseData.parseModelFromSubject(currentElement);

            Debug.LogError(Information.userModels.Count + " usermodels count");
            List<string> questions = Information.userModels[utility.getRandom(1, Information.userModels.Count - 1)].questions;
            int sanityCheck = 0;
            while (questions.Count < 1)
            {
                sanityCheck++;
                if (sanityCheck > 10)
                {
                    Debug.LogError("NO QUESTIONS FOUND");
                    return;
                }
                questions = Information.userModels[utility.getRandom(1, Information.userModels.Count - 1)].questions;
            }
            sampleProblem = questions[utility.getRandom(0, questions.Count - 1)];


        }

        rightPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = name;
        rightPanel.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = sampleProblem;
        rightPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = name;
        rightPanel.transform.GetChild(2).GetComponent<Slider>().value = difficulty;
        differentiator.question = new List<utilities.Question>();

    }




    void takeRedo()
    {

        source.clip = buttonSound;
        source.Play();

        Information.placmentTest = new List<Topic>();
        Information.placmentScore = new List<float>();
        for (int i = 0; i < currGradeDefualt.Count; i++)
        {
            if (currGradeDefualt[i].isTest)
                continue; //ignore tests

            Information.placmentTest.Add(getRandomTopics(currGradeDefualt[i]));
            Information.placmentScore.Add(0);

        }

        if (Information.subject == "math")
        {
            Information.isCurriculum = true;
            SceneManager.LoadScene("Math");
        }
        else
        {

            Information.isCurriculum = true;
            SceneManager.LoadScene("ScienceTest");
        }
    }

    Topic getRandomTopics(Topic topic)
    {
        Topic output = new Topic();
        output.name = topic.name;
        float miniumum = Mathf.Max(1, topic.topics.Count * 0.5f);
        Topic currTopic = new Topic();
        List<int> included = new List<int>();
        for (int j = 0; j < miniumum && j < topic.topics.Count; j++)
        {

            int next = utility.getRandom(0, topic.topics.Count - 1);
            while (included.Contains(topic.topics[next]))
            {
                next = utility.getRandom(0, topic.topics.Count - 1);

            }
            included.Add(topic.topics[next]);

        }

        included.Shuffle();
        output.topics = included;



        return output;
    }


    public GameObject saveOrDont;
    public void takeBack()
    {
        saveOrDont.SetActive(true);
    }

    void takeSave()
    {

        source.clip = buttonSound;
        source.Play();

        saveOrderToFile();

        SceneManager.LoadScene("StudentMenu");
    }

    void takeDontSave()
    {
        source.clip = buttonSound;
        source.Play();

        SceneManager.LoadScene("StudentMenu");
    }


    void Update()
    {
        if (Information.currentBox != null)
        {
            checkClick(Information.currentBox);
            Information.currentBox = null;
        }
    }
}
