using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeMenu : MonoBehaviour
{
    public GameObject[] arrows;
    public GameObject vrWarning;
    public GameObject subjects;
    public GameObject grades;
    public GameObject newSubjectPanel;

    public Button defualt;
    public Button placementTest;
    public Button back;

    public TMP_Text title;

    public AudioClip buttonSound;
    public AudioSource source;


    void Start()
    {
        vrWarning.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { takeWithVR(); });
        vrWarning.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { takeBack(); });
        vrWarning.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { takeWithoutVR(); });
        vrWarning.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { moreInfo(); });

        defualt.onClick.AddListener(delegate { takeDefault(); });
        placementTest.onClick.AddListener(delegate { takeLearningPlan(); });

        initButtons();
        Information.currentScene = "HomeMenu";

    }
    
    void OnEnable()
    {
        Information.isSelect = false;
        closeMenu = false;

    }

    void initButtons()
    {
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
    }


    #region subjects
    void subjectUpdate()
    {

        if (Information.currentBox != null)
        {
            source.clip = buttonSound;
            source.Play();

            Information.subject = Information.currentBox.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text.ToLower();

            if (Information.subject == "public speaking")
            {
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

    void takeSubjectBack()
    {
        source.clip = buttonSound;
        source.Play();

        SceneManager.LoadScene("StudentMenu");
    }

    #endregion

    #region grades
    void gradeUpdate()
    {
        if (Information.currentBox != null)
        {
            source.clip = buttonSound;
            source.Play();

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
            Information.currentBox = null;
        }
    }

    void takeNewBack()
    {
        title.text = "Choose a grade!";
        grades.SetActive(true);
        newSubjectPanel.SetActive(false);
        back.onClick.AddListener(delegate { takeGradeBack(); });
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

    #endregion


    bool closeMenu = false;
    void takeBack()
    {
        source.clip = buttonSound;
        source.Play();

        closeMenu = true;

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


    void Update()
    {
        if (Information.isInMenu)
        {
            return;
        }

        if (subjects.activeSelf)
        {
            subjectUpdate();
        }
        else
        {
            gradeUpdate();
        }
    }

}

