using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LearningPlan
{
    List<Topic> currGradeDefualt;
    GameObject learningPlanUpdated;
    public LearningPlan(List<Topic> topics, GameObject learningPlanUpdated)
    {
        this.learningPlanUpdated = learningPlanUpdated;
        currGradeDefualt = topics;
    }

    public void createNewLearningPlan()
    {
        Information.isCurriculum = false;
        bool isScience = false;
        if (Information.subject == "science")
        {
            isScience = true;
        }
        bool isIncluded = true;

        XElement currLesson = null;
        XElement grade = XMLReader.findGrade(Information.xmlDoc, Information.grade);
        XElement subject = XMLReader.findSubject(grade, Information.subject);
        currLesson = subject;

        if (currLesson == null)
        {
            Debug.LogError("could not find the lesson to save the topics");
            return;
        }

        currLesson.Add(new XAttribute("name", Information.subject));

        for (int i = 0; i < Information.placmentScore.Count; i++)
        {
            isIncluded = true;
            List<int> includedTopics = new List<int>(); //these will be the topics that are included in that section 
            int length = 0;
            var defualtTopics = currGradeDefualt[i].topics;
            //this needs to be a percentage of how many questions are in the thing
            float percentage = (float)Information.placmentScore[i] / Information.placmentTest[i].topics.Count;

            Debug.LogError("percentage " + percentage);

            int level = 0;
            if (percentage < 0.4f)
            {
                includedTopics = defualtTopics;
                length = defualtTopics.Count - 1;
                level = 5;
            }
            else if (percentage < 0.8f)
            {
                length = (int)Mathf.Min(2, defualtTopics.Count * 0.7f);
                level = 7;
            }
            else
            {
                isIncluded = false;
                length = (int)Mathf.Min(1, defualtTopics.Count * 0.2f);
                level = 9;
            }

            XElement newLesson = null;
            if (isScience)
            {
                if (isIncluded)
                {
                    newLesson = new XElement("lesson", new XAttribute("name", Information.placmentTest[i].name), new XAttribute("score", ""), new XAttribute("level", level.ToString()),
                        new XAttribute("index", Information.placmentTest[i].index), new XAttribute("topics", Information.placmentTest[i].topics[0]));
                    currLesson.Add(newLesson);
                }
            }
            else
            {
                newLesson = new XElement("lesson", new XAttribute("name", Information.placmentTest[i].name), new XAttribute("score", ""), new XAttribute("level", level.ToString()),
                    new XAttribute("index", Information.placmentTest[i].index),
           new XAttribute("topics", defualtTopics[0]));// new XAttribute("topics", Information.placmentTest[i].topics[0]));

                for (int j = 1; j <= length && j < defualtTopics.Count; j++)
                {
                    newLesson.Attribute("topics").Value += "," + defualtTopics[j]; //Information.placmentTest[i].topics[j];//userLessons[currentItem.transform.GetChild(j).gameObject].index;
                }
                currLesson.Add(newLesson);
            }
        }

        XMLWriter.saveFile();
        /*   Information.grade = "";
           Information.subject = "";
           Information.topics = null; */

        if (Information.shouldRedo)
        {
            Information.shouldRedo = false;

            SceneManager.LoadScene("ModuleMenu");
        }
        else
        {
            learningPlanUpdated.SetActive(true);
        }
    }
}
