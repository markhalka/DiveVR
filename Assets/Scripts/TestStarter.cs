using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class TestStarter : MonoBehaviour
{
    //ok, so now lets get this show on the road
    //objective: get the pretest, and start panels to show up well for a lesson


    void Start()
    {
        tempLoad();
    }



    //ok, so now based on the current lesson, find all the start panels and show them 


    void tempLoad()
    {
        Information.grade = "Grade 3";
        Information.subject = "science";
        Information.nextScene = 38;

      

        TextAsset mytxtData = (TextAsset)Resources.Load("XML/General/UserData");
        string txt = mytxtData.text;
        Information.xmlDoc = XDocument.Parse(txt);

        mytxtData = (TextAsset)Resources.Load("XML/General/Data");
        txt = mytxtData.text;
        Information.loadDoc = XDocument.Parse(txt);

        ParseData.startXML();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
