using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrestestPanel : MonoBehaviour
{
    public bool isTutorialPanel = false;

    void Start()
    {
        justTitle.transform.parent.gameObject.SetActive(false);
    }

    void pretestOk()
    {
        Information.isQuiz = 1;
        Information.wasPreTest = true;
        panelContainer.SetActive(false);
        quizPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Not sure";
        StartCoroutine(moveAnimation(false));
    }



    
    void pretestNotOk()
    {
        transform.SetParent(pretestPanel.transform.parent.parent);
        setPosition(currentPosition);
        pretestPanel.SetActive(false);
    }

    void takeDontShowAgain()
    {
        Information.showPreTest = false;
        XMLWriter.savePreTestConfig();
        pretestNotOk();
    }
}


