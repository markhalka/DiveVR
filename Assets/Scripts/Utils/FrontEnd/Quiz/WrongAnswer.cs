using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongAnswer
{
    public int currSection;
    public int currQuestion;
    public int currQuestionIndex;
    public bool useImage;

    public WrongAnswer(int cs, int cq, int cqi, bool ui)
    {
        currSection = cs;
        currQuestion = cq;
        currQuestionIndex = cqi;
        useImage = ui;
    }

    public int modelIndex;
    public int questionIndex;
    public WrongAnswer(int modelIndex, int questionIndex, bool ui)
    {
        this.modelIndex = modelIndex;
        this.questionIndex = questionIndex;
        useImage = ui;
    }
}
