using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenModule : MonoBehaviour
{ 

    public GameObject MainButton;
    public GameObject dropdown;
    public GameObject startPanel;
    public GameObject[] arrows;
    List<GameObject> buttons;

    public Button currentButton;
    public Button backButton;

    public ScrollRect scrollRect;

    public AudioSource source;
    public AudioClip scrollSound;
    public AudioClip buttonClick;


    int[] indecies;

    public Sprite[] images;

    bool isPC = true;
    void Start()
    {
     /*   tempLoad();
        Information.grade = "Grade 3";
        Information.subject = "science";

        */
        Information.isQuiz = 0;
        Information.panelClosed = true;

        backButton.onClick.AddListener(delegate { takeBack(); });
        buttons = new List<GameObject>();

        ParseData.startXML();

        startXML();

        indecies = new int[buttons.Count];
        for (int i = 0; i < buttons.Count; i++) //+1 for the back button
        {
            indecies[i] = i;
        }

        Information.updateEntities = buttons.ToArray();
        Information.currentScene = "ModuleMenu";

        Information.lastSubject = Information.subject;
        Information.lastGrade = Information.grade;

        startPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { handleQuiz(); }); //so the first one is the quiz
        startPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { handleLesson(); });

        scrollRect.onValueChanged.AddListener(delegate { takeScroll(); });

    }

    void tempLoad()
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("XML/General/UserData");
        string txt = mytxtData.text;
        Information.xmlDoc = XDocument.Parse(txt);

        mytxtData = (TextAsset)Resources.Load("XML/General/Data");
        txt = mytxtData.text;
        Information.loadDoc = XDocument.Parse(txt);
        Information.name = "none";
        Information.doneLoadingDocuments = true;
        Information.firstTime = true;
        //    Debug.LogError(Information.xmlDoc.ToString());
        //   Debug.LogError(Information.loadDoc.ToString());
    }

    float preValue = 0;
    void takeScroll()
    {
        float curr = scrollRect.transform.GetChild(0).position.y;
        if (Mathf.Abs(preValue - curr) > 10)
        {
            startPanel.SetActive(false);
            preValue = curr;
        }
    }

    void takeBack()
    {
        source.clip = buttonClick;
        source.Play();

        SceneManager.LoadScene("StudentMenu");
    }


    void OnEnable()
    {
        Information.isSelect = true;
        Information.click2d = true;
        Information.inquire = "";
        Time.timeScale = 1; 

        GameObject menu = GameObject.Find("Menu");

    }

    GameObject pastBox = null;

    void OnTriggerStay2D(Collider2D other)
    {
        if (Information.currentBox == null && other.gameObject != pastBox && !isPC)
        {
            source.clip = scrollSound;
            source.Play();

            Information.currentBox = other.gameObject;
        }
    }
    public Sprite[] stars;
    public GameObject grid;
    void createOption(string header, string score, int index, int firstTopic)
    {
        GameObject curr = Instantiate(MainButton, MainButton.transform, true);
        curr.transform.SetParent(grid.transform);
        curr.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = header;

        curr.transform.GetChild(2).GetChild(3).GetComponent<Image>().sprite = images[index];
        if (firstTopic < 0)
        {
            //make it not available, 
            curr.transform.GetChild(2).GetChild(4).gameObject.SetActive(true);
            curr.GetComponent<Button>().interactable = false;
        }

        if (score != "" && score != "-1" && score != "0")
        {
            float currScore = -1;

            if (!float.TryParse(score, out currScore))
            {
                Debug.LogError("could not parse score " + score);
            }
            else
            {
                curr.transform.GetChild(2).GetChild(1).GetComponent<Image>().fillAmount = currScore / 100;
                Image starsImage = curr.transform.GetChild(3).GetComponent<Image>();
                int[] starsAmount = new int[] { 50, 70, 90, 100 };
                for(int i = 0; i < starsAmount.Length; i++)
                {
                    if(currScore <= starsAmount[i])
                    {
                        starsImage.sprite = stars[i];
                    }
                }
            }
        }
        if (index >= 0)
        {
            curr.GetComponent<Button>().onClick.AddListener(delegate { showStartPanel(curr); });
        }

        curr.SetActive(true);
        buttons.Add(curr);
    }

    public GameObject invisible;
    void addInvisible()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject curr = Instantiate(invisible, invisible.transform, true);
            curr.transform.SetParent(grid.transform);
            curr.SetActive(true);
        }
    }

    GameObject currentObject;
    void showStartPanel(GameObject curr)
    {
        if (currentObject == curr)
        {
            startPanel.SetActive(false);
            currentObject = null;
            return;
        }

        startPanel.SetActive(true);
        startPanel.transform.position = curr.transform.position;
        preValue = scrollRect.transform.GetChild(0).position.y;
        currentObject = curr;
    }

    void handleQuiz()
    {
        Information.isInMenu = false;
        Information.isQuiz = 1;
        openScene();
    }

    void handleLesson()
    {
        openScene();
    }
    void openScene()
    {
        GameObject curr = currentObject;
        source.clip = buttonClick;
        source.Play();



        for (int i = 0; i < MainButton.transform.parent.childCount; i++)
        {
            if (MainButton.transform.parent.GetChild(i).transform == curr.transform)
            {
                Information.nextScene = Information.topics[i-1].topics[0];
           /*     if (Information.topics[Information.nextScene].topics.Count < 1 || Information.topics[Information.nextScene].topics[0] == -1)
                {
                    Debug.LogError("could not open anything");
                    return;
                }*/

            //    Information.topicIndex = Information.nextScene;
            //    Debug.LogError(curr.)
                SceneManager.LoadScene("ScienceMain"); ;
            }
        }
    }

    void startXML()
    {
        ParseData.startXML();
        foreach (var topic in Information.topics)
        {
            float max = 0;

            if (topic.tests.Count > 0)
            {
                max = float.Parse(topic.tests[0].score);
                for (int i = 1; i < topic.tests.Count; i++)
                {
                    max = Mathf.Max(max, float.Parse(topic.tests[i].score));
                }

            }
            createOption(topic.name, max.ToString(), topic.index, topic.topics[0]);
        }
        addInvisible();
    }
}


