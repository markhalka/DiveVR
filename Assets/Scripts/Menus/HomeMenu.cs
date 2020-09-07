using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeMenu : MonoBehaviour
{

    GameObject subheader;

    Vector3 offset = new Vector3(0, -30, 0);
    Scroll scroll;

    List<GameObject> buttons;
    List<int> headerIndeciesList;
    List<int> subheaderIndeciesList;
    int[] headerIndecies;
    int[] subheaderIndecies;

    Color greyColor = new Color(194, 194, 194, 180);
    Color greyHighLight = new Color(231, 231, 255);

    public AudioClip buttonSound;
    public AudioSource source;

    public GameObject newSubjectPanel;
    public Button defualt;
    public Button placementTest;

    public GameObject[] arrows;
    public GameObject vrWarning;
    void Start()
    {
        vrWarning.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { takeWithVR(); });
        vrWarning.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { takeBack(); });
        vrWarning.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { takeWithoutVR(); });
        //   vrWarning.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { moreInfo(); });

        defualt.onClick.AddListener(delegate { takeDefault(); });
        placementTest.onClick.AddListener(delegate { takeLearningPlan(); });

        startThing();
        Information.currentScene = "HomeMenu";

    }


    void OnEnable()
    {
        Information.isSelect = false;
        closeMenu = false;

    }

    #region version2



    public GameObject subjects;
    public GameObject grades;
    public TMP_Text title;
    public Button back;


    void subjectUpdate()
    {

        if (Information.currentBox != null)
        {
            source.clip = buttonSound;
            source.Play();

            Information.subject = Information.currentBox.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text.ToLower();
            /*   if(Information.subject != "science")
               {
                   return; //that should work
               } */

            if (Information.subject == "public speaking")
            {
                //ok, then just load open mod
                Information.grade = "Grade Deep";
                if (!closeMenu)
                {
                    vrWarning.gameObject.SetActive(true);
                }
                else
                {
                    vrWarning.gameObject.SetActive(false);
                    Information.subject = "";
                    closeMenu = false;
                    Information.currentBox = null;
                }

                return;
            }

            grades.gameObject.SetActive(true);
            subjects.gameObject.SetActive(false);
            title.text = "Choose a grade!";
            back.onClick.RemoveAllListeners();
            back.onClick.AddListener(delegate { takeGradeBack(); });

            Information.currentBox = null;
        }
    }

    void takeGradeBack()
    {
        source.clip = buttonSound;
        source.Play();

        grades.gameObject.SetActive(false);
        subjects.gameObject.SetActive(true);
        title.text = "Choose a subject!";
        back.onClick.RemoveAllListeners();
        back.onClick.AddListener(delegate { takeSubjectBack(); });

    }
    void startThing()
    {
        //  Information.isSelect = true;
        back.onClick.AddListener(delegate { takeSubjectBack(); });
        List<GameObject> newEntities = new List<GameObject>();
        for (int i = 0; i < subjects.transform.childCount; i++)
        {
            newEntities.Add(subjects.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < grades.transform.childCount; i++)
        {
            newEntities.Add(grades.transform.GetChild(i).gameObject);
        }
        Information.click2d = true;
        Information.updateEntities = newEntities.ToArray();
        // startXML();
    }

    void Update()
    {
        //  Debug.LogError("")
        if (Information.isInMenu)
        {

            return;
        }
        Debug.LogError("not in menu");
        if (subjects.activeSelf)
        {
            subjectUpdate();
        }
        else
        {
            gradeUpdate();
        }
    }
    void takeSubjectBack()
    {
        source.clip = buttonSound;
        source.Play();

        SceneManager.LoadScene("StudentMenu");
    }

    void takeNewBack()
    {
        title.text = "Choose a grade!";
        grades.SetActive(true);
        newSubjectPanel.SetActive(false);
        back.onClick.AddListener(delegate { takeGradeBack(); });
    }


    void gradeUpdate()
    {
        if (Information.currentBox != null)
        {
            source.clip = buttonSound;
            source.Play();

            //   Information.grade = "Grade 5";
            //you need to fix it here, just get the index and then go from there?


            Information.grade = "Grade " + getGradeNumber();


            if (Information.isStudentInfo)
            {
                SceneManager.LoadScene("StudentInfo");
            }
            else
            {

                if (Information.subject == "math" || Information.subject == "science")
                {
                    ParseData.startXML();
                    if (Information.topics == null || Information.topics.Count < 2)
                    {
                        newSubjectPanel.gameObject.SetActive(true);
                        back.onClick.RemoveAllListeners();
                        back.onClick.AddListener(delegate { takeNewBack(); });
                        title.text = "Looks like this is your first time!";
                        grades.gameObject.SetActive(false);

                    }

                }
                if (!newSubjectPanel.activeSelf)
                {
                    SceneManager.LoadScene("ModuleMenu");
                }

            }
            // SceneManager.LoadScene("ModuleMenu");
            Information.currentBox = null;
        }
    }

    int getGradeNumber()
    {
        for (int i = 0; i < grades.transform.childCount; i++)
        {
            if (Information.currentBox.name == grades.transform.GetChild(i).gameObject.name)
            {
                return i + 3;
            }
        }
        return -1;
    }

    public void takeLearningPlan()
    {
        Information.shouldRedo = true;
        SceneManager.LoadScene("Curriculum");
    }


    public void takeDefault()
    {
        ParseData.copySubject();
        ParseData.startXML();

        SceneManager.LoadScene("ModuleMenu");
    }

    void takeWithVR()
    {
        source.clip = buttonSound;
        source.Play();

        Information.isVrMode = true;
        SceneManager.LoadScene("ModuleMenu");
    }

    void moreInfo()
    {
        Application.OpenURL("https://www.divevr.org/post/using-dive-to-master-public-speaking");
    }

    void takeWithoutVR()
    {
        source.clip = buttonSound;
        source.Play();

        Information.isVrMode = false;
        SceneManager.LoadScene("ModuleMenu");

    }

    bool closeMenu = false;
    void takeBack()
    {
        source.clip = buttonSound;
        source.Play();

        closeMenu = true;

    }
    #endregion

}

