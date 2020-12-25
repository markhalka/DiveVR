using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class XMLReader
{

    public XElement findGrade(XDocument doc, string gradeName)
    {
        if(doc == null)
        {
            Debug.LogError("document is null");
            return null;
        }

        foreach (var grade in doc.Descendants("Grade"))
        {
            if ("Grade " + grade.Attribute("number").Value == gradeName)
            {
                return grade;
            }
        }
        return null;
    }

    public XElement findSubject(XElement grade, string subjectName)
    {
        if(grade == null)
        {
            Debug.LogError("grade is null");
            return null;
        }

        foreach (var subject in grade.Elements("subject"))
        {
            if (subject.Attribute("name").Value.ToLower() == subjectName)
            {
                return subject;
            }
        }
        return null;
    }

    public XElement findLesson(XElement subject, string lessonName)
    {
        if(subject == null)
        {
            Debug.LogError("subject is null");
            return null;
        }

        foreach (var lesson in subject.Elements("lesson"))
        {

            if (lesson.Attribute("topics").Value == lessonName)
            {
                return lesson;
            }
        }
        return null;
    }

    public XElement findLessonDoc(XDocument doc, string gradeName, string subjectName, string lessonName)
    {
        var grade = findGrade(doc, gradeName);
        var subject = findSubject(grade, subjectName);
        var lesson = findLesson(subject, lessonName);
        return lesson;
    }

    public XElement findInformationLessonDoc()
    {
        return findLessonDoc(Information.xmlDoc, Information.grade, Information.subject, Information.nextScene.ToString());
    }
}
