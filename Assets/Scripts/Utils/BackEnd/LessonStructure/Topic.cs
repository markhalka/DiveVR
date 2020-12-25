using System.Collections.Generic;
using System.Xml.Linq;

public class Topic
{


    public List<int> topics;
    public List<Question> questions;
    public List<Test> tests;

    public float level;
    public int topicIndex;
    public bool isTest;
    public XElement element;
    public string name;
    public int index;


    public Topic()
    {
        topics = new List<int>();
        questions = new List<Question>();
        tests = new List<Test>();
        level = 0;
        topicIndex = -1;
        element = null;
        isTest = false;
        index = 0;
        name = "";

    }

    public class Question
    {
        public string question;
        public string answer; //ok so for 
        public List<string> otherAnswers;
    }


    public class Test
    {
        public string date;
        public int right;
        public int wrong;
        public string score;
        public string time;
        public string other;
        public string pretestScore;

    }





}
