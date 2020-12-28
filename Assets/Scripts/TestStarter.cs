using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class TestStarter : MonoBehaviour
{

    //ok, so load documents is called successfully 
    //now you just need to set up the information.
    void Start()
    {
        Information.grade = "3";
        Information.subject = "science";
        tempLoad();
        ParseData.startXML();
        checkTopics();
    }

    void checkTopics()
    {
        foreach(Topic i in Information.topics)
        {
            Debug.LogError(i.name);
        }
    }

    //ok, so now based on the current lesson, find all the start panels and show them 


    void tempLoad()
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("XML/General/UserData");
        string txt = mytxtData.text;
        Information.xmlDoc = XDocument.Parse(txt);

        mytxtData = (TextAsset)Resources.Load("XML/General/Data");
        txt = mytxtData.text;
        Information.loadDoc = XDocument.Parse(txt);

        Debug.LogError(Information.xmlDoc.ToString());
        Debug.LogError(Information.loadDoc.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
