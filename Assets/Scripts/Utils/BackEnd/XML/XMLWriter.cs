using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class XMLWriter
{
    //this should instead just the topic itself getting saved 

    public static void saveMiniTest(Topic topic, float score, float time, bool pretest)
    {

        Topic.Test currTest = new Topic.Test();
        currTest.date = DateTime.Today.ToString("MM/dd/yyyy");
        currTest.score = Information.score.ToString();
        currTest.time = "69";
        if (Information.pretestScore != -1)
        {
            currTest.pretestScore = Information.pretestScore.ToString();
        }
        else
        {

        }

        Information.topics[ParseData.getScienceScene()].tests.Add(currTest); //that should save it */

        var lesson = XMLReader.findInformationLessonDoc();
        if(lesson.Attribute("score") == null)
        {
            lesson.Add(new XAttribute("score", ""));
        }
        lesson.Attribute("score").Value += " " + currTest.date + "," + currTest.time + "," + currTest.score;
        if (Information.pretestScore > 0)
        {
            lesson.Attribute("score").Value += "," + currTest.pretestScore;
        }

    }




    public static void savePoints()
    {
        Debug.LogError(Information.totalEarnedPoints + " total earned points in xml writer");
        Information.xmlDoc.Root.Element("info").Element("points").Value = Information.pastPointsDate + "," + Information.totalEarnedPoints + "," + Information.maxDivePoints;
    }

    public static void saveCurrentSubjectAndGrade()
    {
        var pastElement = Information.xmlDoc.Root.Element("past");
        pastElement.Attribute("subject").Value = Information.subject;
        pastElement.Attribute("grade").Value = Information.grade;
        Information.xmlDoc.Save(Information.xmlFileDir);
    }

    public static void savePreTestConfig()
    {
        string value = "true";
        if (!Information.showPreTest)
        {
            value = "false";
        }
        Information.xmlDoc.Root.Element("feedback").Attribute("show").Value = value;
    }

    public static void saveSurveyConfig()
    {
        string value = "true";
        if (!Information.showPreTest)
        {
            value = "false";
        }
        Information.xmlDoc.Root.Element("feedback").Attribute("survey").Value = value;
    }

    public static void submitSurvey(int like, int easy, string didlike, string dontLike)
    {
        XElement newFeedback = new XElement("survey", new XAttribute("date", ParseData.encodeDate()), new XAttribute("grade", Information.grade), new XAttribute("subject", Information.subject), new XAttribute("topic", Information.nextScene));
        newFeedback.Add(new XElement("field", new XAttribute("id", "like"), like));
        newFeedback.Add(new XElement("field", new XAttribute("id", "easy"), easy));
        newFeedback.Add(new XElement("field", new XAttribute("id", "didLike"), didlike));
        newFeedback.Add(new XElement("field", new XAttribute("id", "didNotLike"), dontLike));

        Information.xmlDoc.Root.Element("feedback").Add(newFeedback);
        Debug.LogError("submitted: " + newFeedback.ToString());
        Information.totalEarnedPoints += 10;

    }

    public static void savePastSubjectAndGrade()
    {
        var pastElement = Information.xmlDoc.Root.Element("past");
        pastElement.Attribute("subject").Value = Information.lastSubject;
        pastElement.Attribute("grade").Value = Information.lastGrade;
        Information.xmlDoc.Save(Information.xmlFileDir);
    }

    public static void savePresnetation(int index, List<float> userScores) //ok save this one differntly 
    {
        string today = System.DateTime.Today.ToShortDateString();
        foreach (var item in Information.xmlDoc.Descendants("Deep"))
        {
            if (item.Attribute("name").Value == "Public Speaking")
            {
                XElement loc = null;
                int i = 0;
                foreach (var curr in item.Elements("lesson"))
                {
                    loc = curr;
                    i++;
                    if (i == index)
                    {
                        break;
                    }
                }

                string currScore = today;
                foreach (var score in userScores)
                {
                    currScore += "," + score;
                }
                currScore += " ";
                loc.Attribute("score").Value += currScore;
            }
        }
    }

    public static void saveFile()
    {
        Information.xmlDoc.Save(Information.xmlFileDir);
    }
}
