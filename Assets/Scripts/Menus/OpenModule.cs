using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenModule : MonoBehaviour
{

    //if the topics is -1 thaan DONT OPEN IT
    //if the index is -1, than add a banner (just add that ot the button, and set it equal to true
    //and disable the button 



    //ok, os you need to work on this code a bit 
    //first things first;

    //3. make sure the clicking still works,  and for now don't include the sprites, just have the name 

    //ok, just add the stars, and the start panel

    //ok, now add the hilight and the selected color and you should be gucci for now 



    public Button currentButton;
    public Button backButton;

    public ScrollRect scrollRect;

    public GameObject MainButton;
    public GameObject dropdown;
    public GameObject[] arrows;

    public GameObject lowerbound;
    public GameObject upperbound;

    public AudioSource source;
    public AudioClip scrollSound;
    public AudioClip buttonClick;


    Vector3 offset = new Vector3(0, -30, 0);

    List<GameObject> buttons;
    int[] indecies;

    public Sprite[] images;

    public GameObject startPanel;

    bool isPC = false;
    void Start()
    {

#if UNITY_ANDROID || UNITY_IOS
        isPC = false;
#else
        isPC = true;
#endif


        Information.upperBoundary = upperbound;
        Information.lowerBoundary = lowerbound;

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

        startPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { onButtonClick(true); }); //so the first one is the quiz
        startPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { onButtonClick(false); });

        scrollRect.onValueChanged.AddListener(delegate { takeScroll(); });

    }

    float preValue = 0;
    float currValue = 0;
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
        Time.timeScale = 1; //reset that 
        //and reset the button thing 
        GameObject menu = GameObject.Find("Menu");
        /*     if(menu != null)
             {
                 menu.GetComponent<MenuBar>().resetQuiz(); //k that should work
             } else
             {
                 Debug.LogError("menu was null");
             }*/

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
    void createButton2(string header, string score, int index, int firstTopic)
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


                //here, just get the mofuckin radial amount 
                curr.transform.GetChild(2).GetChild(1).GetComponent<Image>().fillAmount = currScore / 100;
                Image starsImage = curr.transform.GetChild(3).GetComponent<Image>();
                if (currScore > 90)
                {
                    starsImage.sprite = stars[3]; //3 stars, 2, 1, 0
                }
                else if (currScore > 70)
                {
                    starsImage.sprite = stars[2];
                }
                else if (currScore > 50)
                {
                    starsImage.sprite = stars[1];
                }
                else
                {
                    starsImage.sprite = stars[0];
                }

            }


        }
        if (index >= 0)
        {
            Debug.Log("idnex assigned");

            curr.GetComponent<Button>().onClick.AddListener(delegate { showStartPanel(curr); });
            //    curr.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { onButtonClick(curr, true); });

        }

        curr.SetActive(true);
        buttons.Add(curr);

    }


    void onBackClick()
    {
        source.clip = buttonClick;
        source.Play();
        SceneManager.LoadScene("HomeMenu");
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
        //  curr.transform.GetChild(4).gameObject.SetActive(true);
        startPanel.SetActive(true);
        startPanel.transform.position = curr.transform.position;

        preValue = scrollRect.transform.GetChild(0).position.y;
        //     startPanel.transform.SetParent(curr.transform);
        //  startPanel.transform.

        currentObject = curr;
    }


    void onButtonClick(bool isQuiz)
    {
        GameObject curr = currentObject;
        source.clip = buttonClick;
        source.Play();

        for (int i = 0; i < MainButton.transform.parent.childCount; i++)
        {
            // Debug.LogError(MainButton.transform.GetChild(i).transform + " " + curr.transform);
            if (MainButton.transform.parent.GetChild(i).transform == curr.transform)
            {
                if (isQuiz)
                {
                    Information.isInMenu = false;
                    Information.isQuiz = 1;

                }

                Information.nextScene = i - 1;
                if (Information.topics[Information.nextScene].topics.Count < 1 || Information.topics[Information.nextScene].topics[0] == -1)
                {
                    Debug.LogError("could not open anything");
                    return;
                }

                if (Information.subject == "math")
                {
                    Debug.LogError("openning scene");
                    SceneManager.LoadScene("Math");
                }
                else if (Information.subject == "science")
                {
                    //  Information.nextScene--;  //to accomodate science
                    Information.topicIndex = Information.nextScene;
                    SceneManager.LoadScene("ScienceMain");

                }
                else if (Information.subject == "social science")
                {
                    Information.topicIndex = Information.nextScene;
                    SceneManager.LoadScene("ScienceMain");
                }
                else if (Information.subject == "public speaking")
                {
                    SceneManager.LoadScene("Presentation");
                }
            }
        }

    }


    XDocument xmlDoc;
    IEnumerable<XElement> items;

    string charText, dialogueText;

    void startXML()
    {
        foreach (var topic in Information.topics)
        {
            float max = 0;

            Debug.LogError(topic.name + " " + topic.tests.Count);
            if (topic.tests.Count > 0)
            {
                max = float.Parse(topic.tests[0].score);
                for (int i = 1; i < topic.tests.Count; i++)
                {
                    max = Mathf.Max(max, float.Parse(topic.tests[i].score));
                }

            }
            createButton2(topic.name, max.ToString(), topic.index, topic.topics[0]);
        }


    }
}


