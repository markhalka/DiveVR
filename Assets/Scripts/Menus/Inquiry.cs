using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inquiry : MonoBehaviour
{
    public Button submit;
    public Button skip;
    public TMPro.TMP_Text text;
    public TMPro.TMP_InputField inputField;

    void Start()
    {
        initButtons();
        showText();
    }

    void showText()
    {
        text.text = Information.inquire;
    }

    void initButtons()
    {
        submit.onClick.AddListener(delegate { takeSubmit(); });
        skip.onClick.AddListener(delegate { takeSkip(); });

    }
    void takeSubmit()
    {
        foreach (var grade in Information.xmlDoc.Descendants("grade"))
        {
            if ("Grade " + grade.Attribute("number").Value == Information.grade)
            {
                Debug.LogError("found grade");
                foreach (var subject in grade.Elements("subject"))
                {
                    if (subject.Attribute("name").Value.ToLower() == Information.subject)
                    {
                        Debug.LogError("foudn the subject");
                        foreach (var lesson in subject.Elements("lesson"))
                        {

                            if (lesson.Attribute("topics").Value == (Information.nextScene).ToString())
                            {
                                Debug.LogError("added a new element");
                                lesson.Add(new XElement("inquiry", new XAttribute("date", DateTime.Today.ToString("MM/dd/yyyy")), inputField.text));
                                break;
                            }
                        }

                    }
                }
            }
        }

        takeSkip();
    }

    void takeSkip()
    {
        //just close it
        Debug.Log("closing");
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
