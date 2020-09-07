using System.Collections.Generic;
using UnityEngine;

public class Personalized : MonoBehaviour
{
    // Start is called before the first frame update



    void Start()
    {

    }

    void adjustMath()
    {
        List<Topic> exams = new List<Topic>();
        for (int i = 0; i < Information.topics.Count; i++)     //just look at the last test, (because this should run everytime they start up, so the other tests have already been compensated for)
        {
            if (Information.topics[i].isTest)
            {
                exams.Add(Information.topics[i]);
                continue;
            }
            float score = float.Parse(Information.topics[i].tests[Information.topics[i].tests.Count - 1].score);
            changeLevel(Information.topics[i], levelChange(score));


        }

        for (int i = 0; i < exams.Count; i++)
        {
            float score = float.Parse(exams[i].tests[0].score);
            changeLevel(exams[i], levelChange(score));

        }
    }

    int levelChange(float score)
    {
        int levelChange = 0;
        if (score >= 95)
        {
            levelChange = 2;
        }
        else if (score > 80)
        {
            levelChange = 1;
        }
        else if (score > 60)
        {
            levelChange = -1;
        }
        else if (score > 40)
        {
            levelChange = -2;
        }
        else
        {
            levelChange = -3;
        }
        return levelChange;

    }

    //this will change the level for that topic in the xml doc, then at the end you need to save it to the website 
    void changeLevel(Topic topic, int level)
    {
        //so to change the level in the xml doc, find the right grade, subject, then the indecies should match up between the topic and the other thing 
        int currLevel = (int)(float.Parse(topic.element.Attribute("level").Value)) * 10;
        currLevel += level;
        if (currLevel < 0)
        {
            currLevel = 0;
        }
        else if (currLevel > 1)
        {
            currLevel = 1;
        }
        topic.element.Attribute("level").Value = "0." + currLevel;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
