using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationPanel : MonoBehaviour
{
    public TMP_Text justTitle;

    public TMP_Text simple;
    public TMP_Text advanced;

    public GameObject panelContainer;
    public GameObject centerContainer;
    public GameObject leftcontainer;
    public GameObject rightContainer;

    public Vector3 rightStart;
    public Vector3 rightExit;

    public Vector3 leftStart;
    public Vector3 leftExit;

    public Vector3 centerStart;
    public Vector3 centerExit;

    public enum MenuPosition { RIGHT, LEFT, CENTER };
    MenuPosition currentPosition = MenuPosition.CENTER;

    bool wasShowingTitle;
    bool newModelLoaded;


    // Start is called before the first frame update
    void Start()
    {
        setPosition(currentPosition);
    }

    public void setPosition(MenuPosition position)
    {

        currentPosition = position;

        panelContainer.transform.localPosition = new Vector3(0, 0, 0);

        if (position == MenuPosition.CENTER)
        {
            centerContainer.transform.SetParent(panelContainer.transform);

            leftcontainer.transform.SetParent(transform);
            rightContainer.transform.SetParent(transform);

            centerContainer.SetActive(true);

            leftcontainer.SetActive(false);
            rightContainer.SetActive(false);
        }
        else if (position == MenuPosition.LEFT)
        {
            leftcontainer.transform.SetParent(panelContainer.transform);

            centerContainer.transform.SetParent(transform);
            rightContainer.transform.SetParent(transform);

            leftcontainer.SetActive(true);

            centerContainer.SetActive(false);
            rightContainer.SetActive(false);
        }
        else
        {
            rightContainer.transform.SetParent(panelContainer.transform);

            leftcontainer.transform.SetParent(transform);
            centerContainer.transform.SetParent(transform);

            rightContainer.SetActive(true);

            leftcontainer.SetActive(false);
            centerContainer.SetActive(false);
        }

        simple = panelContainer.transform.GetChild(0).GetChild(3).GetComponent<TMPro.TMP_Text>();
        advanced = panelContainer.transform.GetChild(0).GetChild(2).GetComponent<TMPro.TMP_Text>();

        if (centerContainer.activeSelf)
        {
            panelContainer.transform.localPosition = centerExit;
        }
        else if (leftcontainer.activeSelf)
        {
            panelContainer.transform.localPosition = leftExit;
        }
        else if (rightContainer.activeSelf)
        {
            panelContainer.transform.localPosition = rightExit;
        }
    }

    public IEnumerator moveAnimation(bool enter)
    {
        float count = 0;
        Vector3 start, end;
        start = end = new Vector3(0, 0, 0);

        switch (currentPosition)
        {
            case MenuPosition.CENTER:
                end = centerStart;
                start = centerExit;
                break;
            case MenuPosition.LEFT:
                end = leftStart;
                start = leftExit;
                break;
            case MenuPosition.RIGHT:
                end = rightStart;
                start = rightExit;
                break;
        }

        if (!enter)
        {
            var temp = start;
            start = end;
            end = temp;
        }


        while (count <= 1)
        {
            count += 0.1f;
            panelContainer.transform.localPosition = Vector3.Lerp(start, end, count);
            yield return new WaitForSeconds(0.02f);
        }

        if (!enter)
        {
            panelContainer.gameObject.SetActive(false);
            if (Information.isQuiz == 1)
            {
                Debug.LogError("ok now its true");
                wasShowingTitle = true;
            }
            if (wasShowingTitle)
            {

                justTitle.transform.parent.gameObject.SetActive(true);
            }
            newModelLoaded = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
