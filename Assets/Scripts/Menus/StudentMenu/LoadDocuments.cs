using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;

public class LoadDocuments : MonoBehaviour
{
    Website network;
    public void Start()
    {
        network = new Website();
        //  loadDocuments();
        tempLoad();
    }

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

    void loadDocuments()
    {
        StartCoroutine(network.GetRequest(Information.loadDocUrl, loadDoc));
    }


    void loadDoc(string ld)
    {
        Information.loadDoc = XDocument.Parse(ld);

        if (Information.username == null || Information.username == "guest")
        {
            StartCoroutine(network.GetRequest(Information.masterXmlDoc, loadXmlDoc));
        }
        else
        {
            StartCoroutine(network.GetRequest(Information.xmlDocUrl + Information.username, loadXmlDoc));
        }

    }

    void loadMasterXmlDoc(string uwrText)
    {
        Information.xmlDoc = XDocument.Parse(uwrText);
        Information.doneLoadingDocuments = true;
    }

    void loadXmlDoc(string xmld)
    {
        if (xmld.Length < 10)
        {
            Debug.LogError("loaded master");
            StartCoroutine(network.GetRequest(Information.masterXmlDoc, loadMasterXmlDoc));
        }
        else
        {
            xmld = Regex.Replace(@"" + xmld, @"\\", "");

            if (xmld[xmld.Length - 1] == '\"')
            {
                xmld = xmld.Substring(0, xmld.Length - 1);

            }
            Information.xmlDoc = XDocument.Parse(xmld);
        }

        string version = Information.loadDoc.Root.Element("version").Value;
        string xmlDocVers = Information.xmlDoc.Root.Element("version").Value;

        if (xmlDocVers != version)
        {
            Debug.LogError("Incorrect version, using master document...");
            StartCoroutine(network.GetRequest(Information.masterXmlDoc, loadMasterXmlDoc));
        }
        else
        {
            Information.doneLoadingDocuments = true;
        }
    }


    void loadLast()
    {
        XElement lastElement = Information.xmlDoc.Root.Element("past");
        Information.lastGrade = lastElement.Attribute("grade").Value;
        Information.lastSubject = lastElement.Attribute("subject").Value;
    }

    void loadPreferences()
    {
        if (Information.xmlDoc.Root.Element("feedback").Attribute("show").Value == "true")
        {
            Information.showPreTest = true;
        }
        else
        {
            Information.showPreTest = false;
        }

        if (Information.xmlDoc.Root.Element("feedback").Attribute("survey").Value == "true")
        {
            Information.shouldShowSurvey = true;
        }
        else
        {
            Information.shouldShowSurvey = false;
        }
    }


    void loadPoints()
    {
        XElement info = Information.xmlDoc.Root.Element("info");
        Information.name = info.Element("name").Value;
        string[] points = info.Element("points").Value.Split(',');
        if (points.Length < 2)
        {
            string date = ParseData.encodeDate();
            string max = Information.loadDoc.Root.Element("points").Value;
            string currPoints = "0";
            Information.xmlDoc.Root.Element("info").Element("points").Value = date + "," + currPoints + "," + max;
            points = new string[] { date, currPoints, max };
        }

        DateTime pastMonth = DateTime.Parse(ParseData.decodeDate(points[0])); //so the first one is the date  //date, user points, max points
        var difference = DateTime.Today - pastMonth;

        Information.totalEarnedPoints = int.Parse(points[1]);
        Information.maxDivePoints = int.Parse(points[2]);
        Information.pastPointsDate = points[0];
        if (difference.Days >= 30) //30 days is a month
        {
            Information.maxDivePoints += 416;
            Information.pastPointsDate = ParseData.encodeDate();
        }
    }

    bool loadedLast = false;
    private void Update()
    {

        if (Information.doneLoadingDocuments && !loadedLast)
        {
            loadedLast = true;
            loadLast();
            loadPreferences();
            loadPoints();
        }
    }
}
