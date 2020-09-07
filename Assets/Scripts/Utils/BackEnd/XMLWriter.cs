using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class XMLWriter
{
    //this should instead just the topic itself getting saved 

    public static void saveMiniTest(Topic topic, float score, float time, bool pretest)
    {
        /*  string today = System.DateTime.Today.ToShortDateString();

              string scores = topic.element.Attribute("score").Value;

          if (scores != "")
          {
              scores += " ";
          }
          scores += today + "," + score + "," + time;
          if(Information.pretestScore != -1)
          {
              scores += "," + Information.pretestScore;
          }
          Debug.LogError("added: " + scores);
          topic.element.Attribute("score").Value = scores;*/

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
        Debug.LogError(score + " saved");
        Information.topics[ParseData.getScienceScene()].tests.Add(currTest); //that should save it */

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
                                Debug.LogError("found topic");
                                lesson.Attribute("score").Value += " " + currTest.date + "," + currTest.time + "," + currTest.score;
                                if (Information.pretestScore > 0)
                                {
                                    lesson.Attribute("score").Value += "," + currTest.pretestScore;
                                }
                            }

                        }
                    }
                }
            }
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

    public static void savePastSubjectAndGrae()
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



    /*  private static string header = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>";
      IEnumerator PutRequest(string url, string input)
      {
          byte[] dataToPut = System.Text.Encoding.UTF8.GetBytes(input);

          CustomCertificateHandler certHandler = new CustomCertificateHandler();

          UnityWebRequest uwr = UnityWebRequest.Put(url, dataToPut);
          uwr.chunkedTransfer = false;
          uwr.certificateHandler = certHandler;
          yield return uwr.SendWebRequest();





          if (uwr.isNetworkError)
          {
              Debug.Log("Error While Sending: " + uwr.error);
          }
          else
          {
              Debug.Log("Received: " + uwr.downloadHandler.text);
          }
      }

      */


    //here, just save this to the website as well

    public static void saveFile()
    {
        Debug.LogError("should i save??");
        Information.xmlDoc.Save(Information.xmlFileDir);//Information.xmlFileDir);
    }

    public static void saveLoadFile()
    {
        Information.loadDoc.Save(Information.loadDocDir);
    }

    /*

    public static void saveTopics(string InformationGrade, string InformationSubject)
    {
        if (Information.topics == null || InformationGrade.Length < 2 || InformationSubject.Length < 2)
        {
            Debug.LogError("topics was null, not saving");
            return;
        }

        XElement currLesson = null;
        foreach (var grade in Information.xmlDoc.Descendants("grade"))
        {
            if ("Grade " + grade.Attribute("number").Value == InformationGrade)
            {
                foreach (var subject in grade.Elements())
                {
                    if (subject.Attribute("name").Value == InformationSubject)
                    {
                        subject.RemoveAll();
                        currLesson = subject;
                        break;
                    }
                }
            }
        }
        if (currLesson == null)
        {
            Debug.LogError("could not find current lesson and subject for topics");
            return;
        }

        currLesson.Add(new XAttribute("name", InformationSubject));
        for (int i = 0; i < Information.topics.Count; i++)
        {
            Topic currTopic = Information.topics[i];
            int level = (int)(currTopic.level * 10);

            if (level < 0)
            {
                level = 1;
            } else if(level > 10)
            {
                level = 10;
            }
            

            XElement topicElement = new XElement("lesson", new XAttribute("name", currTopic.name), new XAttribute("index", currTopic.index), new XAttribute("topics", currTopic.topics[0]),
                new XAttribute("score", ""), new XAttribute("level", level));

            for (int j = 1; j < currTopic.topics.Count; j++)
            {

                topicElement.Attribute("topics").Value += "," + currTopic.topics[j];

            }
            for (int j = 0; j < currTopic.tests.Count; j++)
            {
                string tempScore = currTopic.tests[j].score;
                float currScore = 0;
                if (!float.TryParse(tempScore, out currScore))
                {
                    continue;
                }
                currScore = Mathf.Round(currScore);
                //you need to add pretest score shit here
                Debug.LogError("I'm saving rn");
                topicElement.Attribute("score").Value += currTopic.tests[j].date + "," + currTopic.tests[j].time + "," + currScore.ToString();
                if(currTopic.tests[j].pretestScore != null)
                {
                    topicElement.Attribute("score").Value += "," + currTopic.tests[j].pretestScore;
                }
                if (j != currTopic.tests.Count - 1)
                {
                    topicElement.Attribute("score").Value += " ";
                }
            }

            currLesson.Add(topicElement);

        }
        saveFile();
    }
*/
}
