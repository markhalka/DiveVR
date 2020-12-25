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
    XMLReader reader;

    void Start()
    {
        initButtons();
        showText();
        reader = new XMLReader();
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
        var lesson = reader.findInformationLessonDoc();
        lesson.Add(new XElement("inquiry", new XAttribute("date", DateTime.Today.ToString("MM/dd/yyyy")), inputField.text));
        takeSkip();
    }

    void takeSkip()
    {
        gameObject.SetActive(false);
    }

}
