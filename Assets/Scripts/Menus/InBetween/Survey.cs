using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// this is the survey gameobject
public class Survey : MonoBehaviour
{
    public Button submitSurvey;
    public Button cancelSurvey;
    public Button dontShowSurvey;
    Panel panel;
    public void Start()
    {
        dontShowSurvey.onClick.AddListener(delegate { takeDontShowSurvey(); });
        cancelSurvey.onClick.AddListener(delegate { closeSurvey(); });
        submitSurvey.onClick.AddListener(delegate { takeSubmitSurvey(); });
        panel = new Panel();
    //    StartCoroutine(openSurvey());
    }


    public void takeSubmitSurvey()
    {
        Transform t = transform.GetChild(0);
        XMLWriter.submitSurvey((int)t.GetChild(0).GetComponent<Slider>().value, (int)t.GetChild(1).GetComponent<Slider>().value, t.GetChild(2).GetComponent<TMP_InputField>().text, t.GetChild(3).GetComponent<TMP_InputField>().text);
        //  StartCoroutine(closeSurvey(true));
        closeSurvey();
    }

    public void closeSurvey()
    {
        StartCoroutine(panel.panelAnimation(false, transform));
      //  StartCoroutine(closeSurvey(false));
    }

    void takeDontShowSurvey()
    {
        Information.shouldShowSurvey = false;
        XMLWriter.saveSurveyConfig();
        //   StartCoroutine(closeSurvey(false));
        closeSurvey();
    }

    Vector2 surveyStart = new Vector2(0, 450.86f);
    Vector2 surveyEnd = new Vector2(0, 0);


 /*   IEnumerator closeSurvey(bool submited)
    {

        float count = 0;
        if (submited)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            while (count < 2)
            {
                count++;
                yield return new WaitForSeconds(0.5f);
            }
        }
        count = 0;

        while (count <= 1)
        {
            count += 0.1f;
            transform.localPosition = Vector3.Lerp(surveyEnd, surveyStart, count);
            //   panelContainer.transform

            yield return new WaitForSeconds(0.02f);
        }

        gameObject.SetActive(false);

    }*/

}


