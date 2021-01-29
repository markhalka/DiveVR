using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hints : MonoBehaviour
{

    public GameObject table;
    public GameObject horizontalSnap;
    public Animations animations;

    public InformationPanel infoPanel;


    public Quiz quiz;

    //for this one, it hides the correct answer?
    List<int> randomNames;
    bool tookHint = false;
    bool showAnswer = false;

    // maybe here you can go for user models 
    void takeHint(GameObject[] entities)
    {
        if (tookHint)
        {
            showAnswer = true;
        } else {
            showAnswer = false;
        }

        if (showAnswer || horizontalSnap.activeSelf)
        {
            takeShowAnswer();
            return;
        } else if (table.activeSelf)
        {
            tableHint();
            return;
        }

        tookHint = true;
        //the table one is easier, stat with that 
        randomNames = new List<int>();

        randomNames.Add(quiz.modelIndex);

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


    public void tableHint()
    {

        //now choose 3 more random ones
        int buttonIndex = getButtonIndexFromName(quiz.currAnswer);
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



    public void takeShowAnswer()
    {

        if (horizontalSnap.activeSelf)
        {

            horizontalSnap.GetComponent<UnityEngine.UI.Extensions.HorizontalScrollSnap>().ChangePage(quiz.modelIndex); // not sure if that'll work tbh
        }
        else
        {
            animations.addAnimation(quiz.modelIndex, Information.animationLength, true, Information.animationGrowth);
        }

        tookHint = false;

        infoPanel.showPanel(quiz.modelIndex);
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
     /*   if (inTable)
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
        }  */
    }

    private void Update()
    {
      /*  if(showAnswer && !infoPanel.panelContainer.activeSelf)
        {
            closeHint();
        } */
    }
}
