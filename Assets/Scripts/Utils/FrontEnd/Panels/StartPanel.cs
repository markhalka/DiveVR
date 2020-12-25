using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanel : MonoBehaviour
{
    public GameObject outlinePanel;
    public GameObject currentMoveObjet;

    public Vector2 lerpPosition = new Vector3(-350, 0);
    public Vector2 pastPosition;

    List<int> startPanels;
    int finishStart = 0;
    int startOffset = 0;

    public void Start()
    {
        startPanels = new List<int>();
        startOffset = 0;
        for (int i = 0; i < Information.userModels.Count; i++)
        {
            if (Information.userModels[i].section == 1)
            {
                startOffset++;
                startPanels.Add(i);
            }
        }
    }


    #region panel animations
    public void fancyAnimation()
    {
        if (finishStart < 1)
        {
            return;
        }
        outlinePanel.SetActive(true);
        StartCoroutine(moveObject(false));
    }

    GameObject currentMoveObject;
    IEnumerator moveObject(bool moveBack)
    {
        if (!moveBack)
        {
            currentMoveObject = Information.currentBox;
        }

        float count = 0;
        Vector3 start, end;
        if (moveBack)
        {

            start = lerpPosition;
            end = pastPosition;
        }
        else
        {
            pastPosition = currentMoveObject.transform.localPosition;
            start = pastPosition;
            end = lerpPosition;
        }
        while (count <= 1.1)
        {
            count += 0.1f;
            currentMoveObject.transform.localPosition = Vector2.Lerp(start, end, count);
            yield return new WaitForSeconds(0.02f);
        }

    }

    #endregion

    //ye that should work 

    // this should be its own class (just make it a panel class)

    void handleStartPanels()
    {
        if (Information.doneLoading)
        {
            SceneManager.LoadScene("ScienceMain");
        }
        if (finishStart == 1 && !transform.parent.GetComponent<InformationPanel>().closeOnEnd)
        {
            transform.parent.GetComponent<InformationPanel>().closeOnEnd = true;
        }
        if (Information.panelClosed && finishStart == 0 && Information.isQuiz == 0) // this should be information.isquiz or something
        {
            if (transform.parent.GetComponent<InformationPanel>().closeOnEnd && startPanels.Count > 1)
            {
                transform.parent.GetComponent<InformationPanel>().closeOnEnd = false;
            }
            if (startPanels.Count > 0)
            {
                if (startPanels[startPanels.Count - 1] > Information.panelIndex) //then not all the start panels have been shown
                {
                    for (int i = 0; i < startPanels.Count; i++)
                    {
                        if (startPanels[i] > Information.panelIndex)
                        {
                            Information.panelIndex = startPanels[i];
                            Information.lableIndex = 0;
                            //    simple.text = Information.userModels[startPanels[i]].simpleInfo[0];
                            Information.panelClosed = false;

                            showPanel();
                            if (i == startPanels.Count - 1)
                            {
                                finishStart = 1;
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                finishStart = 1;
            }
        }
    }

    void showPanel()
    {
        gameObject.SetActive(true);
    }

    public void Update()
    {
      //  if (outlinePanel.activeSelf && !panel.activeSelf)
        {
            outlinePanel.SetActive(false);
            StartCoroutine(moveObject(true));
        }
    }
}


