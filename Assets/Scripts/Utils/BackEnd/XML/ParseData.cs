using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;


public class ParseData
{
    
    public static string encodeDate()
    {
        string date = DateTime.Today.ToString("MM/dd/yyy");
        date = date.Replace('/', '.');
        return date;
    }

    public static string decodeDate(string input)
    {
        input = input.Replace('.', '/');
        return input;
    }


    public static void copySubject()
    {
        //  checkLoad(); 
        XElement loadDocElement = XMLReader.findSubjectDoc(Information.loadDoc, Information.grade, Information.subject);
        XElement xmlDocElement = XMLReader.findSubjectDoc(Information.xmlDoc, Information.grade, Information.subject);

        foreach (var lesson in loadDocElement.Elements())
        {
            XElement newElement = lesson;
            newElement.Elements().Remove();
            xmlDocElement.Add(newElement);

        }
        XMLWriter.saveFile();
    }


    public static void parseTutorial()
    {
        //checkLoad();
        Information.tutorialScenes = new List<TutorialScene>();
        foreach (var tutorialScene in Information.loadDoc.Root.Element("tutorial").Elements())
        {
            if (tutorialScene.Attribute("name").Value == "simple")
            {
                foreach (var scene in tutorialScene.Elements())
                {
                    TutorialScene curr = new TutorialScene(scene.Attribute("name").Value);

                    foreach (var panel in scene.Elements())
                    {
                        TutorialScene.TutorialPanel currPanel = new TutorialScene.TutorialPanel();
                        foreach (var model in panel.Elements())
                        {
                            currPanel.information.Add(getModel(model));
                        }
                        curr.panels.Add(currPanel);
                    }
                    Information.tutorialScenes.Add(curr);
                }

            }
        }
    }

    public static List<Section> createSections()
    {

        List<Section> section = new List<Section>();

        for (int i = 0; i < Information.userModels.Count; i++)
        {
            int currSection = Information.userModels[i].section;

            if (currSection >= -1)
            {

                int contained = -999;
                for (int j = 0; j < section.Count; j++)
                {
                    if (section[j].section == currSection)
                    {
                        contained = j;
                        break;
                    }
                }

                if (contained < -1)
                {
                    Section newSection = new Section(currSection);
                    newSection.questions.Add(Information.userModels[i]);
                    section.Add(newSection);
                }
                else
                {
                    section[contained].questions.Add(Information.userModels[i]);
                }
            }

        }
        return section;

    }

    public static XElement headerSubject;
    public static void getCurrentHeaderSubject()
    {
        foreach (var subject in Information.loadDoc.Root.Element("headers").Elements("subject"))
        {
            if (subject.Attribute("name").Value == Information.subject)
            {
                headerSubject = subject;
                return;
            }
        }
    }


    public static XElement getBodyFromHeader(XElement lessonName)
    {
        foreach (var lesson in headerSubject.Elements("lesson"))
        {

            if (lesson.Attribute("name").Value == lessonName.Attribute("name").Value)
            {
                return lesson;
            }
        }
        return null;
    }


    //ok, here you need to make this grade 4 rip
    public static bool parseModel()
    {
        getCurrentHeaderSubject();
        if (headerSubject == null)
        {
            Debug.LogError("could not find the header in the load file...");
        }

        if (!checkGradeSubject())
        {
            return false;
        }

        var subject = XMLReader.findSubjectDoc(Information.xmlDoc, Information.grade, Information.subject);

        if(subject == null)
        {
            Debug.LogError("the subject was not found");
            return false;
        }

        if (parseModelFromSubject(subject))
            return true;

        return false;
    }

    public static bool parseModelFromSubject(XElement subject)
    {
        Information.userModels = new List<Model>();
        XElement curr = null;

        foreach (var lesson in subject.Elements())
        {
            if (lesson.Attribute("topics").Value == Information.nextScene.ToString())
            {      
                curr = lesson;
                break;
            }
        }

        if (curr == null)
        {
            Debug.LogError("could not fine: " + Information.grade + " " + Information.subject);
            return false;
        }

        curr = getBodyFromHeader(curr);

        Information.currentTopic = new Topic();
        Information.currentTopic.element = curr;

        foreach (var currModel in curr.Elements())
        {
            Information.userModels.Add(getModel(currModel));
        }
        if (curr.Attribute("inq") != null)
        {
            Information.inquire = curr.Attribute("inq").Value;
        }
        else
        {

        }
        return true;
    }



    public static Model getModel(XElement currModel)
    {
        Model model = new Model();
        foreach (var simple in currModel.Elements("simple"))
        {
            model.simpleInfo.Add(simple.Value);
        }
        foreach (var advanced in currModel.Elements("advanced"))
        {
            model.advancedInfo.Add(advanced.Value);
        }
        foreach (var question in currModel.Elements("quiz"))
        {
            model.questions.Add(question.Value);
        }

        if (currModel.HasAttributes)
        {
            model.section = int.Parse(currModel.Attribute("section").Value);
        }
        else
        {
            model.section = 0;
        }
        return model;
    }


    static bool checkGradeSubject()
    {
        if (Information.grade == null || Information.grade == "" || Information.subject == null || Information.subject == "")
        {
            Debug.LogError("grade or subject is null");
            return false;
        }
        return true;
    }

    public static int getScienceScene()
    {

        for (int i = 0; i < Information.topics.Count; i++)
        {
            if (Information.topics[i].topics[0] == Information.nextScene)
            {
                return i;
            }
        }
        return -1;
    }


    public static void startXML()
    {

        //   checkLoad();
        if (Information.lastSubject == Information.subject && Information.lastGrade == Information.grade && Information.topics != null) //because it is the same thing 
        {
            Debug.LogError("this is the same thing, no need to reaload");
            return;
        }

        Information.topics = new List<Topic>();

     

        if (!checkGradeSubject())
        {
            Debug.LogError("check grade subject failed");
            return;
        }

        var subject = XMLReader.findSubjectDoc(Information.xmlDoc, Information.grade, Information.subject);

        if(subject == null)
        {
            Debug.LogError("the subject was not found");
            return;
        }

        generateTopicsFromLesson(subject, Information.topics);
    }

    public static void generateTopicsFromLesson(XElement subject, List<Topic> topicsToAdd)
    {

        foreach (var lesson in subject.Elements())
        {

            Topic currTopic = new Topic();
            string lessonName = lesson.Name.ToString();
            if (!lessonName.Contains("lesson") && !lessonName.Contains("test"))
            {
                continue;
            }
            currTopic.name = lesson.Attribute("name").Value;
            if (lessonName.Contains("test"))
            {

                currTopic.isTest = true;
            }
            currTopic.element = lesson;
            float currLevel = 0.5f;

            if (float.TryParse(lesson.Attribute("level").Value, out currLevel))
            {
                currLevel /= 10;
            }

            currTopic.level = currLevel;
            foreach (var topic in lesson.Attribute("topics").Value.Split(','))
            {
                if (topic == "" || topic == "-1")
                {
                    currTopic.topics.Add(-1);
                }
                else
                {
                    currTopic.topics.Add(int.Parse(topic));
                }

            }
            string[] scores = new string[0];

            if (lesson.Attribute("score") != null)
            {
                scores = lesson.Attribute("score").Value.Split(' ');
            }


            if (scores.Length > 0)
            {
                for (int i = 0; i < scores.Length; i++)
                {

                    string[] currScore = scores[i].Split(',');
                    if (currScore.Length < 3)
                    {
                        continue;
                    }

                    Topic.Test currTest = new Topic.Test();
                    currTest.date = currScore[0];
                    currTest.time = currScore[1];
                    currTest.score = currScore[2];


                    if (currScore.Length == 4)
                    {
                        currTest.pretestScore = currScore[3];
                    }



                    currTopic.tests.Add(currTest);
                }
            }
            currTopic.index = int.Parse(lesson.Attribute("index").Value);
            topicsToAdd.Add(currTopic);
        }

    }

}
