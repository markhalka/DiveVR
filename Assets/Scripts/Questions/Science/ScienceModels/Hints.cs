using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hints : MonoBehaviour
{

    public GameObject table;
    public Quiz quiz;

    //for this one, it hides the correct answer?
    List<int> randomNames;
    bool tookHint = false;
    bool showAnswer = false;
    void takeHint(GameObject[] entities, int index)
    {
        //get the right answer from quiz, and 3 more random dots 
        //you need to tie the dots to answers, and the answers to dots? 


        tookHint = true;
        //the table one is easier, stat with that 
        randomNames = new List<int>();

        randomNames.Add(index);

        for (int i = 0; i < 3; i++)
        {
            int next = Random.Range(0, entities.Length - 1);
            while (randomNames.Contains(next))
            {
                next = Random.Range(0, entities.Length - 1);
            }
            randomNames.Add(next);
        }

        for (int i = 0; i < entities.Length; i++)
        {
            if (!randomNames.Contains(i))
            {
                for (int j = 0; j < entities[i].transform.childCount; j++)
                {
                    if (entities[i].transform.GetChild(j).gameObject.name.Contains("dot"))
                    {
                        entities[i].transform.GetChild(j).gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void checkShowAnswer(bool shouldShow)
    {
        if (tookHint)
        {
            showAnswer = true;
        }
        else if (shouldShow)
        {
            showAnswer = true;
        }
        else
        {
            showAnswer = false;
        }

        if (showAnswer)
        {
            takeShowAnswer();
            return;
        }
    }

    public void tableHint(int index)
    {
        //now choose 3 more random ones
        int buttonIndex = getButtonIndexFromName(Information.userModels[index].simpleInfo[0]);
        randomNames.Add(buttonIndex);
        for (int i = 0; i < 3; i++)
        {
            int next = Random.Range(1, table.transform.parent.childCount - 1); //not 0, because that is the defualt 
            while (randomNames.Contains(next))
            {
                next = Random.Range(1, table.transform.parent.childCount - 1);
            }
            randomNames.Add(next);
        }



        for (int i = 0; i < table.transform.parent.childCount; i++)
        {
            if (!randomNames.Contains(i))
            {
                table.transform.parent.GetChild(i).gameObject.SetActive(false);
            }

        }
    }

    public GameObject horizontalSnap;
    public Animations animations;

    public void takeShowAnswer()
    {

        int index = int.Parse(quiz.lables[quiz.nextId][1]);

        if (horizontalSnap.activeSelf)
        {

        //    horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().ChangePage(index - modelOffset);
        }
        else
        {
         //   animations.addAnimation(index - modelOffset, Information.animationLength, true, Information.animationGrowth);
        }

        tookHint = false;

        Information.panelIndex = index;
        Information.lableIndex = 0;
        //just show here
    }

    int getButtonIndexFromName(string name)
    {
        for (int i = 1; i < table.transform.parent.childCount; i++)
        {
            if (table.transform.parent.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text == name)
            {
                return i;
            }
        }
        return -1;
    }

    void closeHint()
    {
       /* if (inTable)
        {
            for (int i = 1; i < table.transform.parent.childCount; i++) //the first one is the defualt 
            {
                table.transform.parent.GetChild(i).gameObject.SetActive(true);
            }
            table.transform.parent.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            isHiding = true;
            takeHideclick();
        } */
    } 
}
