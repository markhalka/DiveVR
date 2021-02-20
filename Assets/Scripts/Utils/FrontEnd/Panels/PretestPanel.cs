using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PretestPanel : MonoBehaviour
{

    public GameObject panelContainer;
    public GameObject quizPanel;

    public Button pretestNotOkButton;
    public Button pretestOkButton;
    public Button dontShowAgain;


    public bool isTutorialPanel = false;
    public TMP_Text justTitle;

    Panel panel;
    public GameObject pretestPanel;

    void OnEnable()
    {
        panel = new Panel();

        pretestNotOkButton.onClick.AddListener(delegate { pretestNotOk(); });
        pretestOkButton.onClick.AddListener(delegate { pretestOk(); });
        dontShowAgain.onClick.AddListener(delegate { takeDontShowAgain(); });
        StartCoroutine(delayShow());

    }

    IEnumerator delayShow()
    {
        yield return new WaitForSeconds(1);
        Debug.LogError(Information.showPreTest + " " + Information.isQuiz + " " + isTutorialPanel);
        if (Information.showPreTest && !isTutorialPanel && Information.isQuiz == 0)
        {
            Debug.LogError("showing...");
            showPreTest();
        }
    }

    void showPreTest()
    {
        StartCoroutine(panel.panelAnimation(true, pretestPanel.transform));
        justTitle.transform.parent.gameObject.SetActive(false);
    }


    void pretestOk()
    {
        Information.isQuiz = 1;
        Information.wasPreTest = true;
        quizPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Not sure";
        StartCoroutine(panel.panelAnimation(false, transform));
    }



    
    public void pretestNotOk()
    {
        //setPosition(currentPosition);
        StartCoroutine(panel.panelAnimation(false, transform));
    }

    void takeDontShowAgain()
    {
        Information.showPreTest = false;
        XMLWriter.savePreTestConfig();
        pretestNotOk();
    }
}


