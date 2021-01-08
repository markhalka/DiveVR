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

    LocationPanel panel;

    void Start()
    {
        panel = transform.GetComponent<LocationPanel>();

        pretestNotOkButton.onClick.AddListener(delegate { pretestNotOk(); });
        pretestOkButton.onClick.AddListener(delegate { pretestOk(); });
        dontShowAgain.onClick.AddListener(delegate { takeDontShowAgain(); });

        if (Information.showPreTest && !isTutorialPanel && Information.isQuiz == 0)
        {
            showPreTest();
        }
    }

    void showPreTest()
    {
        justTitle.transform.parent.gameObject.SetActive(false);
    }

    void pretestOk()
    {
        Information.isQuiz = 1;
        Information.wasPreTest = true;
        panelContainer.SetActive(false);
        quizPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Not sure";
        StartCoroutine(panel.moveAnimation(false));
    }



    
    public void pretestNotOk()
    {
        transform.SetParent(transform.parent.parent);
        //setPosition(currentPosition);
        gameObject.SetActive(false);
    }

    void takeDontShowAgain()
    {
        Information.showPreTest = false;
        XMLWriter.savePreTestConfig();
        pretestNotOk();
    }
}


