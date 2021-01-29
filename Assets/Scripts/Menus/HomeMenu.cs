using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeMenu : MonoBehaviour
{

    public GameObject grades;

    public TMP_Text title;

    public AudioClip buttonSound;
    public AudioSource source;


    void Start()
    {


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


    void Update()
    {
        if (Information.isInMenu)
        {
            return;
        }

        gradeUpdate();
    }

}

