using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassificationDb : MonoBehaviour
{
    int[] classificationIndecies = new int[] { 0, 2, 5, 8, 11, 12, 14, 14 };
    string[] currentNames;

    public GameObject vsChild;
    public GameObject verticalScroll;
    public Sprite[] classificationSprites;

    public int startOffset;

    public void Start()
    {
        currentNames = new string[7];
        for (int i = startOffset; i < 9; i++)
        {
            currentNames[i - startOffset] = Information.userModels[i].simpleInfo[0];
        }

        initLadder();
        Database.currentSprites = classificationSprites;
    }

    public void initLadder()
    {
        for (int i = 0; i < currentNames.Length; i++)
        {

            GameObject currChild = Instantiate(vsChild, vsChild.transform, false);
            currChild.transform.SetParent(verticalScroll.transform.GetChild(0));

            currChild.gameObject.SetActive(true);
            GameObject page = currChild.transform.GetChild(0).GetChild(0).gameObject;
            currChild.transform.GetChild(1).GetComponent<Text>().text = currentNames[i];
            currChild.transform.GetChild(0).GetComponent<Image>().color = Information.colors[i % Information.colors.Length];

            GameObject image = page.transform.GetChild(0).GetChild(0).gameObject;

            for (int j = classificationIndecies[i]; j < classificationSprites.Length; j++)
            {
                GameObject currAnimal = Instantiate(image, image.transform, true);
                currAnimal.transform.SetParent(image.transform.parent.parent.GetChild(0));
                currAnimal.GetComponent<Image>().sprite = classificationSprites[j];    //ok, that should work 
                currAnimal.gameObject.SetActive(true);
            }
        }
    }

    public void Update()
    {
        if (Information.currentBox != null)
        {
            for (int i = 0; i < verticalScroll.transform.GetChild(0).childCount; i++)
            {
                Transform image = verticalScroll.transform.GetChild(0).GetChild(i).GetChild(0);
                if (image.gameObject == Information.currentBox)
                {
                    image.GetChild(0).gameObject.SetActive(true);
                    Information.panelIndex = startOffset + i - 1;
                  //  panel.showPanel();
                }
                else
                {
                    image.GetChild(0).gameObject.SetActive(false);
                }
            }
            Information.currentBox = null;
        }
    }
}

