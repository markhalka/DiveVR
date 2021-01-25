using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeMenu : MonoBehaviour
{

    public GameObject grades;

    public Button defualt;
    public Button placementTest;
    public Button back;

    public TMP_Text title;

    public AudioClip buttonSound;
    public AudioSource source;


    void Start()
    {

        defualt.onClick.AddListener(delegate { takeDefault(); });
        placementTest.onClick.AddListener(delegate { takeLearningPlan(); });

        initButtons();
        Information.currentScene = "HomeMenu";
        Information.subject = "science";
    }

    void OnEnable()
    {
        Information.isSelect = false;
    }

    void initButtons()
    {
        back.onClick.AddListener(delegate { takeGradeBack(); });
        List<GameObject> newEntities = new List<GameObject>();

        for (int i = 0; i < grades.transform.childCount; i++)
        {
            newEntities.Add(grades.transform.GetChild(i).gameObject);
        }
        Information.click2d = true;
        Information.updateEntities = newEntities.ToArray();
    }


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
                SceneManager.LoadScene("ModuleMenu");
            }
        }
        Information.currentBox = null;
    }


    void takeGradeBack()
    {
        source.clip = buttonSound;
        source.Play();

        SceneManager.LoadScene("StudentMenu");
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


    void Update()
    {
        if (Information.isInMenu)
        {
            return;
        }

        gradeUpdate();
    }

}

